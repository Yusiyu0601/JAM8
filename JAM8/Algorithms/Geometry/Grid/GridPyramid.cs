using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Accord.IO;
using JAM8.Utilities;

namespace JAM8.Algorithms.Geometry
{
    /// <summary>
    /// 网格金字塔（Grid Pyramid）类，提供多分辨率网格处理方法。
    /// </summary>
    public class GridPyramid
    {
        /// <summary>
        /// 将当前 GridProperty 扩展为最近的 2 的整数次幂尺寸（例如 101×101 → 128×128），
        /// 保留原始数据，超出部分补为指定哨兵值（OutsideValue）。适用于多分辨率模拟预处理。
        /// </summary>
        /// <param name="outsideValue">
        /// 域外填充值（哨兵值），用于区分 ROI 外区域；建议选择不会出现在真实数据中的值。
        /// </param>
        /// <returns>扩展后的 GridProperty（尺寸为 2^N × 2^N × 2^N）</returns>
        public static GridProperty expand_to_pow2(GridProperty grid_property, float outsideValue = -99f)
        {
            int next_pow2(int n)
            {
                int p = 1;
                while (p < n) p <<= 1;
                return p;
            }

            var gs = grid_property.grid_structure;
            var dim = gs.dim;

            int nx_old = gs.nx, ny_old = gs.ny, nz_old = gs.nz;

            int nx_new = next_pow2(nx_old);
            int ny_new = next_pow2(ny_old);
            int nz_new = dim == Dimension.D3 ? next_pow2(nz_old) : 1;

            // 若尺寸已是 2^N 则无需扩展
            if (nx_new == nx_old && ny_new == ny_old && nz_new == nz_old)
                return grid_property.deep_clone();

            // 创建新的 GridStructure
            var gs_new = GridStructure.create_with_old_size_origin(nx_new, ny_new, nz_new, gs);
            var gp_new = GridProperty.create(gs_new);

            // 全部先填充为哨兵值
            gp_new.set_value(outsideValue);

            // 拷贝原值（null 保留）
            for (int i = 0; i < nx_old; i++)
            for (int j = 0; j < ny_old; j++)
            for (int k = 0; k < (dim == Dimension.D3 ? nz_old : 1); k++)
            {
                var si = dim == Dimension.D2
                    ? SpatialIndex.create(i, j)
                    : SpatialIndex.create(i, j, k);

                var value = grid_property.get_value(si);
                if (value != null)
                    gp_new.set_value(si, value.Value);
                else
                    gp_new.set_value(si, null); // ROI 内但未赋值
            }

            return gp_new;
        }

        /// <summary>
        /// 将当前粗网格的 GridProperty 按指定缩放因子稀疏上采样，
        /// 自动生成 finer_gs（放大网格结构），仅将值映射到中心点，其余为空。
        /// </summary>
        /// <param name="scale_x">X方向缩放因子（>1表示放大）</param>
        /// <param name="scale_y">Y方向缩放因子</param>
        /// <param name="scale_z">Z方向缩放因子（2D时不生效）</param>
        /// <returns>稀疏上采样后的 GridProperty（仅中心格点有值）</returns>
        public static GridProperty pyramid_upsample_sparse(GridProperty grid_property, int scale_x, int scale_y,
            int scale_z = 1)
        {
            if (scale_x <= 0 || scale_y <= 0 || scale_z <= 0)
                throw new ArgumentException("缩放因子必须为正整数");

            var coarse_gs = grid_property.grid_structure;
            var dim = coarse_gs.dim;

            int finer_nx = coarse_gs.nx * scale_x;
            int finer_ny = coarse_gs.ny * scale_y;
            int finer_nz = dim == Dimension.D3 ? coarse_gs.nz * scale_z : 1;

            // 基于当前 coarse 的结构，创建 finer 的结构
            var finer_gs = GridStructure.create_with_old_size_origin(finer_nx, finer_ny, finer_nz, coarse_gs);

            // 创建 finer 属性，并进行稀疏中心点映射
            GridProperty finer = GridProperty.create(finer_gs);

            for (int i = 0; i < coarse_gs.N; i++)
            {
                var si_coarse = coarse_gs.get_spatial_index(i);
                var value = grid_property.get_value(si_coarse);
                if (value == null) continue;

                int fine_ix = si_coarse.ix * scale_x + scale_x / 2;
                int fine_iy = si_coarse.iy * scale_y + scale_y / 2;
                int fine_iz = dim == Dimension.D3 ? si_coarse.iz * scale_z + scale_z / 2 : 0;

                if (fine_ix >= finer_gs.nx || fine_iy >= finer_gs.ny ||
                    (dim == Dimension.D3 && fine_iz >= finer_gs.nz))
                    continue;

                var si_fine = dim == Dimension.D2
                    ? SpatialIndex.create(fine_ix, fine_iy)
                    : SpatialIndex.create(fine_ix, fine_iy, fine_iz);

                finer.set_value(si_fine, value);
            }

            return finer;
        }

        /// <summary>
        /// 将当前 GridProperty 进行平滑下采样，生成更粗分辨率的网格（如 128→64），
        /// 对每个 block 内有效值求平均值。
        /// </summary>
        /// <param name="factor_x">X方向缩小倍数</param>
        /// <param name="factor_y">Y方向缩小倍数</param>
        /// <param name="factor_z">Z方向缩小倍数（2D时不生效）</param>
        /// <returns>下采样后的 GridProperty</returns>
        public static GridProperty pyramid_downsample_smooth(GridProperty grid_property, int factor_x, int factor_y,
            int factor_z = 1)
        {
            if (factor_x <= 0 || factor_y <= 0 || factor_z <= 0)
                throw new ArgumentException("缩放因子必须为正整数");

            var gs = grid_property.grid_structure;
            var dim = gs.dim;

            int coarse_nx = (int)Math.Ceiling(gs.nx / (double)factor_x);
            int coarse_ny = (int)Math.Ceiling(gs.ny / (double)factor_y);
            int coarse_nz = dim == Dimension.D3 ? (int)Math.Ceiling(gs.nz / (double)factor_z) : 1;

            var coarse_gs = GridStructure.create_with_old_size_origin(coarse_nx, coarse_ny, coarse_nz, gs);
            var coarse_gp = GridProperty.create(coarse_gs);

            for (int iz = 0; iz < coarse_nz; iz++)
            for (int iy = 0; iy < coarse_ny; iy++)
            for (int ix = 0; ix < coarse_nx; ix++)
            {
                List<float> valid_values = new();

                for (int dz = 0; dz < (dim == Dimension.D3 ? factor_z : 1); dz++)
                for (int dy = 0; dy < factor_y; dy++)
                for (int dx = 0; dx < factor_x; dx++)
                {
                    int x = ix * factor_x + dx;
                    int y = iy * factor_y + dy;
                    int z = iz * factor_z + dz;

                    if (x >= gs.nx || y >= gs.ny || (dim == Dimension.D3 && z >= gs.nz))
                        continue;

                    var si = dim == Dimension.D2
                        ? SpatialIndex.create(x, y)
                        : SpatialIndex.create(x, y, z);

                    var value = grid_property.get_value(si);

                    valid_values.Add(value.Value);
                }

                var si_coarse = dim == Dimension.D2
                    ? SpatialIndex.create(ix, iy)
                    : SpatialIndex.create(ix, iy, iz);

                if (valid_values.Count == 0)
                {
                    coarse_gp.set_value(si_coarse, null);
                }
                else
                {
                    float average =
                        MyArrayHelper.find_all_modes<float>(valid_values.ToArray()).modes[0]; // 你也可以改为 Median
                    coarse_gp.set_value(si_coarse, average);
                }
            }

            return coarse_gp;
        }

        /// <summary>
        /// 将 fine 网格中的硬数据（非空值）投影到 coarse 网格，按 coarse cell 聚合（默认均值），
        /// 返回 coarse 网格结构对应的 GridProperty。
        /// </summary>
        /// <param name="fine_gp">fine 网格属性（非 null 表示硬数据）</param>
        /// <param name="coarse_gs">目标 coarse 网格结构</param>
        /// <param name="is_discrete">是否为离散变量（true = 众数，false = 均值）</param>
        /// <returns>coarse 层的 GridProperty（仅聚合后的 cell 有值，其余为 null）</returns>
        public static GridProperty project_hard_data_to_coarse(GridProperty fine_gp, GridStructure coarse_gs,
            bool is_discrete = false)
        {
            var fine_gs = fine_gp.grid_structure;
            var dim = fine_gs.dim;

            var result = new Dictionary<SpatialIndex, List<float>>();

            for (int n = 0; n < fine_gs.N; n++)
            {
                var si_fine = fine_gs.get_spatial_index(n);
                var value = fine_gp.get_value(si_fine);

                if (value == null) continue;

                // 坐标映射
                var coord = fine_gs.spatial_index_to_coord(si_fine);
                var si_coarse = coarse_gs.coord_to_spatial_index(coord);

                if (!result.ContainsKey(si_coarse))
                    result[si_coarse] = [];

                result[si_coarse].Add(value.Value);
            }

            // 构造 coarse 层 GridProperty
            var coarse_gp = GridProperty.create(coarse_gs);

            foreach (var (key, values) in result)
            {
                float aggregated = is_discrete
                    ? MyArrayHelper.find_all_modes<float>(values.ToArray()).modes[0]
                    : values.Average();

                coarse_gp.set_value(key, aggregated);
            }

            return coarse_gp;
        }

        /// <summary>
        /// 将 coarse 层的 GridProperty 稀疏映射到 fine 网格结构，
        /// 仅将值投影到对应 fine 网格中每个 coarse block 的中心点，
        /// 并仅在 fine 上原本为 null 的位置赋值（保留硬数据）。
        /// </summary>
        /// <param name="coarse_gp">coarse 层属性（模拟结果）</param>
        /// <param name="fine_gp">fine 层原始属性（含硬数据）</param>
        /// <returns>映射后的 fine 层副本，仅中心点被赋值</returns>
        public static GridProperty project_hard_data_to_fine(GridProperty coarse_gp, GridProperty fine_gp)
        {
            var coarse_gs = coarse_gp.grid_structure;
            var fine_gs = fine_gp.grid_structure;
            var dim = fine_gs.dim;

            int scale_x = fine_gs.nx / coarse_gs.nx;
            int scale_y = fine_gs.ny / coarse_gs.ny;
            int scale_z = dim == Dimension.D3 ? fine_gs.nz / coarse_gs.nz : 1;

            if (fine_gs.nx % coarse_gs.nx != 0 || fine_gs.ny % coarse_gs.ny != 0 ||
                (dim == Dimension.D3 && fine_gs.nz % coarse_gs.nz != 0))
                throw new InvalidOperationException("fine 网格尺寸必须是 coarse 网格的整数倍");

            var fine_result = fine_gp.deep_clone();

            for (int n = 0; n < coarse_gs.N; n++)
            {
                var si_coarse = coarse_gs.get_spatial_index(n);
                var value = coarse_gp.get_value(si_coarse);
                if (value == null) continue;

                int fine_ix = si_coarse.ix * scale_x + scale_x / 2;
                int fine_iy = si_coarse.iy * scale_y + scale_y / 2;
                int fine_iz = dim == Dimension.D3 ? si_coarse.iz * scale_z + scale_z / 2 : 0;

                if (fine_ix >= fine_gs.nx || fine_iy >= fine_gs.ny || (dim == Dimension.D3 && fine_iz >= fine_gs.nz))
                    continue;

                var si_fine = dim == Dimension.D2
                    ? SpatialIndex.create(fine_ix, fine_iy)
                    : SpatialIndex.create(fine_ix, fine_iy, fine_iz);

                // 仅在原始 fine 上该位置为 null 时赋值（跳过硬数据）
                if (fine_gp.get_value(si_fine) == null)
                    fine_result.set_value(si_fine, value.Value);
            }

            return fine_result;
        }
    }
}