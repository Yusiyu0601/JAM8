using System.Drawing;
using System.Runtime.CompilerServices;

namespace JAM8.Algorithms.Geometry
{
    /// <summary>
    /// 网格结构类
    /// 其中有2种索引，spatialIndex和arrayIndex
    /// </summary>
    public class GridStructure
    {
        public const string Exception_NotEquals = "two gridStructure are not equals";

        #region 属性

        /// <summary>
        /// 获取网格的维度（通过 nz 判断）
        /// </summary>
        public Dimension dim => nz == 1 ? Dimension.D2 : Dimension.D3;

        /// <summary>
        /// 网格单元的总数
        /// </summary>
        public int N { get; internal set; } = 0;

        /// <summary>
        /// x方向网格单元数量
        /// </summary>
        public int nx { get; internal set; } = 1;

        /// <summary>
        /// y方向网格单元数量
        /// </summary>
        public int ny { get; internal set; } = 1;

        /// <summary>
        /// z方向网格单元数量
        /// </summary>
        public int nz { get; internal set; } = 1;

        /// <summary>
        /// x方向网格单元尺寸，默认等于1
        /// </summary>
        public float xsiz { get; internal set; } = 1;

        /// <summary>
        /// y方向网格单元尺寸，默认等于1
        /// </summary>
        public float ysiz { get; internal set; } = 1;

        /// <summary>
        /// z方向网格单元尺寸，默认等于1
        /// </summary>
        public float zsiz { get; internal set; } = 1;

        /// <summary>
        /// x方向长度，等于nx*xsiz
        /// </summary>
        public float xextent { get; internal set; } = 1;

        /// <summary>
        /// y方向长度，等于ny*ysiz
        /// </summary>
        public float yextent { get; internal set; } = 1;

        /// <summary>
        /// z方向长度，等于nz*zsiz
        /// </summary>
        public float zextent { get; internal set; } = 1;

        /// <summary>
        /// x方向的网格点起始点，默认等于xsiz的一半
        /// </summary>
        public float xmn { get; internal set; } = 0.5f;

        /// <summary>
        /// y方向的网格点起始点，默认等于ysiz的一半
        /// </summary>
        public float ymn { get; internal set; } = 0.5f;

        /// <summary>
        /// z方向的网格点起始点，默认等于zsiz的一半
        /// </summary>
        public float zmn { get; internal set; } = 0.5f;

        /// <summary>
        /// 索引映射(arrayIndex -> spatialIndex)
        /// 注意：在千万数量级网格时，会消耗大量内存
        /// </summary>
        public List<SpatialIndex> index_mapper { get; internal set; } = null;

        #endregion

        // 缓存,避免重复计算
        private int ny_mul_nx = 0;

        #region 实例函数

        /// <summary>
        /// 根据spatial_index计算array_index，ix、iy、iz从0开始，到N=nx*ny*nz-1结束
        /// </summary>
        /// <param name="ix"></param>
        /// <param name="iy"></param>
        /// <param name="iz"></param>
        /// <returns> 如果 根据spatial_index计算array_index(ix,iy,iz)不在GridStructure范围内，则返回-1 </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int get_array_index(int ix, int iy, int iz = 0)
        {
            if ((uint)ix >= (uint)nx || (uint)iy >= (uint)ny || (uint)iz >= (uint)nz)
            {
                return -1; // 索引超出范围
            }

            return iz * ny_mul_nx + iy * nx + ix;
        }

        /// <summary>
        /// 根据spatialIndex计算arrayIndex，spatialIndex的ix、iy、iz从0开始，到N=nx*ny*nz-1结束
        /// </summary>
        /// <param name="si"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int get_array_index(SpatialIndex si)
        {
            return get_array_index(si.ix, si.iy, si.iz);
        }

        /// <summary>
        /// 根据array索引计算spatial索引，arrayIndex从0开始
        /// </summary>
        /// <param name="array_index"></param>
        /// <returns></returns>
        public SpatialIndex get_spatial_index(int array_index)
        {
            return index_mapper[array_index];
        }

        /// <summary>
        /// 对角线距离
        /// </summary>
        /// <returns></returns>
        public double diagonal_distance()
        {
            if (dim == Dimension.D2)
                return Math.Sqrt(nx * nx + ny * ny);
            if (dim == Dimension.D3)
                return Math.Sqrt(nx * nx + ny * ny + nz * nz);
            return -1;
        }

        /// <summary>
        /// 打印
        /// </summary>
        /// <returns></returns>
        public string to_string()
        {
            return $"\n\tGridStructure {dim}_[{nx}_{ny}_{nz}]_[{xsiz}_{ysiz}_{zsiz}]_[{xmn}_{ymn}_{zmn}]\n";
        }

        /// <summary>
        /// spatial_index 转换为 coord
        /// </summary>
        /// <param name="si"></param>
        /// <returns></returns>
        public Coord spatial_index_to_coord(SpatialIndex si)
        {
            if (si.dim != dim)
                return null;

            Coord coord = null;

            if (dim == Dimension.D2)
            {
                if (si.ix >= 0 && si.ix < nx && si.iy >= 0 && si.iy < ny)
                {
                    float x = si.ix * xsiz + xmn;
                    float y = si.iy * ysiz + ymn;
                    coord = Coord.create(x, y);
                }
            }

            if (dim == Dimension.D3)
            {
                if (si.ix >= 0 && si.ix < nx && si.iy >= 0 && si.iy < ny && si.iz >= 0 && si.iz < nz)
                {
                    float x = si.ix * xsiz + xmn;
                    float y = si.iy * ysiz + ymn;
                    float z = si.iz * zsiz + zmn;
                    coord = Coord.create(x, y, z);
                }
            }

            return coord;
        }

        /// <summary>
        /// coord 转换为 spatialIndex
        /// </summary>
        /// <param name="coord"></param>
        /// <returns></returns>
        public SpatialIndex coord_to_spatial_index(Coord coord)
        {
            if (dim != coord.dim)
                return null;

            SpatialIndex si = null;
            if (coord.y == 99.5)
                Console.WriteLine();
            if (dim == Dimension.D2)
            {
                int ix = (int)((coord.x - xmn + 0.5 * xsiz) / xsiz);
                int iy = (int)((coord.y - ymn + 0.5 * ysiz) / ysiz);
                if (ix >= 0 && ix < nx && iy >= 0 && iy < ny)
                    si = SpatialIndex.create(ix, iy);
            }

            if (dim == Dimension.D3)
            {
                int ix = (int)((coord.x - xmn + 0.5 * xsiz) / xsiz);
                int iy = (int)((coord.y - ymn + 0.5 * ysiz) / ysiz);
                int iz = (int)((coord.z - zmn + 0.5 * zsiz) / zsiz);
                if (ix >= 0 && ix < nx && iy >= 0 && iy < ny && iz >= 0 && iz < nz)
                    si = SpatialIndex.create(ix, iy, iz);
            }

            return si;
        }


        /// <summary>
        /// arrayIndex 转换为 coord
        /// </summary>
        /// <param name="array_index"></param>
        /// <returns></returns>
        public Coord array_index_to_coord(int array_index)
        {
            if (array_index < 0 || array_index >= N) //不在0~N-1范围内
                return null;

            SpatialIndex si = get_spatial_index(array_index);

            Coord c = null;

            if (dim == Dimension.D2)
            {
                if (si.ix >= 0 && si.ix < nx && si.iy >= 0 && si.iy < ny)
                {
                    float x = si.ix * xsiz + xmn;
                    float y = si.iy * ysiz + ymn;
                    c = Coord.create(x, y);
                }
            }

            if (dim == Dimension.D3)
            {
                if (si.ix >= 0 && si.ix < nx && si.iy >= 0 && si.iy < ny && si.iz >= 0 && si.iz < nz)
                {
                    float x = si.ix * xsiz + xmn;
                    float y = si.iy * ysiz + ymn;
                    float z = si.iz * zsiz + zmn;
                    c = Coord.create(x, y, z);
                }
            }

            return c;
        }

        /// <summary>
        /// coord 转换为 array_index
        /// </summary>
        /// <param name="coord"></param>
        /// <returns></returns>
        public int coord_to_array_index(Coord coord)
        {
            if (dim != coord.dim)
                return -1;

            SpatialIndex si = null;

            if (dim == Dimension.D2)
            {
                int ix = (int)((coord.x - xmn + 0.5 * xsiz) / xsiz);
                int iy = (int)((coord.y - ymn + 0.5 * ysiz) / ysiz);
                if (ix >= 0 && ix < nx && iy >= 0 && iy < ny)
                    si = SpatialIndex.create(ix, iy);
            }

            if (dim == Dimension.D3)
            {
                int ix = (int)((coord.x - xmn + 0.5 * xsiz) / xsiz);
                int iy = (int)((coord.y - ymn + 0.5 * ysiz) / ysiz);
                int iz = (int)((coord.z - zmn + 0.5 * zsiz) / zsiz);
                if (ix >= 0 && ix < nx && iy >= 0 && iy < ny && iz >= 0 && iz < nz)
                    si = SpatialIndex.create(ix, iy, iz);
            }

            if (si == null) //coord转为array_index时，可能发生超出范围的情况，此时返回-1
                return -1;

            return get_array_index(si);
        }

        /// <summary>
        /// 批量 spatial_index 转换为 coord
        /// </summary>
        public IList<Coord> spatial_indexes_to_coords(IList<SpatialIndex> sis)
        {
            var result = new List<Coord>();
            if (sis == null)
                return result;

            foreach (var si in sis)
            {
                var coord = spatial_index_to_coord(si);
                if (coord != null)
                    result.Add(coord);
            }

            return result;
        }

        /// <summary>
        /// 批量 coord 转换为 spatialIndex
        /// </summary>
        public IList<SpatialIndex> coords_to_spatial_indexes(IList<Coord> coords)
        {
            var result = new List<SpatialIndex>();
            if (coords == null)
                return result;

            foreach (var coord in coords)
            {
                var si = coord_to_spatial_index(coord);
                if (si != null)
                    result.Add(si);
            }

            return result;
        }

        /// <summary>
        /// 批量 arrayIndex 转换为 coord
        /// </summary>
        public IList<Coord> array_indexes_to_coords(IList<int> array_indexes)
        {
            var result = new List<Coord>();
            if (array_indexes == null)
                return result;

            foreach (var ai in array_indexes)
            {
                var coord = array_index_to_coord(ai);
                if (coord != null)
                    result.Add(coord);
            }

            return result;
        }

        /// <summary>
        /// 批量 coord 转换为 array_index
        /// </summary>
        public IList<int> coords_to_array_indexes(IList<Coord> coords)
        {
            var result = new List<int>();
            if (coords == null)
                return result;

            foreach (var coord in coords)
            {
                int ai = coord_to_array_index(coord);
                if (ai >= 0)
                    result.Add(ai);
            }

            return result;
        }

        /// <summary>
        /// 将当前网格结构粗化（或细化）到指定的网格数量（nx, ny, nz），
        /// 保持原始空间范围（extent）不变，
        /// 自动计算新的网格单元尺寸（xsiz 等）与起点坐标（xmn 等）。
        /// </summary>
        /// <param name="nx_coarsed">目标 X 向网格数量</param>
        /// <param name="ny_coarsed">目标 Y 向网格数量</param>
        /// <param name="nz_coarsed">目标 Z 向网格数量（2D 时填 1）</param>
        /// <returns>新的 GridStructure 对象，起点为 coarse 后第一个单元格的中心坐标</returns>
        public GridStructure coarse(int nx_coarsed, int ny_coarsed, int nz_coarsed)
        {
            if ((dim == Dimension.D2 && nz_coarsed > 1) || (dim == Dimension.D3 && nz_coarsed < 1))
            {
                Console.WriteLine(MyExceptions.Geometry_IndexException);
                return null;
            }

            // 原始最小边界（左下角/底部）
            float x_boundary_min = xmn - 0.5f * xsiz;
            float y_boundary_min = ymn - 0.5f * ysiz;
            float z_boundary_min = zmn - 0.5f * zsiz;

            // 新的 cell size
            float xsiz_coarsed = xextent / nx_coarsed;
            float ysiz_coarsed = yextent / ny_coarsed;
            float zsiz_coarsed = zextent / nz_coarsed;

            // 新的起点（格子中心）
            float xmn_coarsed = x_boundary_min + 0.5f * xsiz_coarsed;
            float ymn_coarsed = y_boundary_min + 0.5f * ysiz_coarsed;
            float zmn_coarsed = z_boundary_min + 0.5f * zsiz_coarsed;

            return init(nx_coarsed, ny_coarsed, nz_coarsed,
                xsiz_coarsed, ysiz_coarsed, zsiz_coarsed,
                xmn_coarsed, ymn_coarsed, zmn_coarsed);
        }

        /// <summary>
        /// 按给定的缩放倍数（factor）对当前网格进行粗化或细化，
        /// 保持空间范围（extent）不变，自动计算新的网格数量和起点坐标。
        /// </summary>
        /// <param name="factor_x">X 向缩放倍数（>1 为粗化，<1 为细化）</param>
        /// <param name="factor_y">Y 向缩放倍数</param>
        /// <param name="factor_z">Z 向缩放倍数（2D 时填 1）</param>
        /// <returns>新的 GridStructure 对象，起点为缩放后第一个单元格的中心坐标</returns>
        public GridStructure coarse_by_factor(double factor_x, double factor_y, double factor_z)
        {
            if ((dim == Dimension.D2 && factor_z != 1) || (dim == Dimension.D3 && factor_z <= 0))
            {
                Console.WriteLine(MyExceptions.Geometry_IndexException);
                return null;
            }

            // 计算缩放后的格子数（向上取整，防止缩小后格子数为 0）
            int nx_new = Math.Max(1, (int)Math.Round(nx / factor_x));
            int ny_new = Math.Max(1, (int)Math.Round(ny / factor_y));
            int nz_new = dim == Dimension.D2 ? 1 : Math.Max(1, (int)Math.Round(nz / factor_z));

            return coarse(nx_new, ny_new, nz_new);
        }

        #endregion

        #region 静态方法

        /// <summary>
        /// 私有构造函数，避免直接实例化
        /// </summary>
        private GridStructure()
        {
        }

        /// <summary>
        /// 通用方法：根据给定参数初始化 GridStructure。
        /// </summary>
        private static GridStructure init(
            int nx, int ny, int nz,
            float xsiz, float ysiz, float zsiz,
            float xmn, float ymn, float zmn)
        {
            GridStructure gs = new()
            {
                nx = nx,
                ny = ny,
                nz = nz,
                xsiz = xsiz,
                ysiz = ysiz,
                zsiz = zsiz,
                xmn = xmn,
                ymn = ymn,
                zmn = zmn,
                xextent = nx * xsiz,
                yextent = ny * ysiz,
                zextent = nz * zsiz,
                N = nx * ny * nz,
                ny_mul_nx = ny * nx
            };
            gs.index_mapper = get_index_mapper(gs);
            return gs;
        }

        /// <summary>
        /// 创建 GridStructure，当 nz 等于 1 时，是 2D 网格结构，否则是 3D 网格结构。
        /// </summary>
        public static GridStructure create_simple(int nx, int ny, int nz)
        {
            return init(nx, ny, nz, 1.0f, 1.0f, 1.0f, 0.5f, 0.5f, 0.5f);
        }

        /// <summary>
        /// 创建 GridStructure，当 nz 等于 1 时，是 2D 网格结构，否则是 3D 网格结构。
        /// </summary>
        public static GridStructure create(int nx, int ny, int nz, float xsiz, float ysiz, float zsiz, float xmn,
            float ymn, float zmn)
        {
            return init(nx, ny, nz, xsiz, ysiz, zsiz, xmn, ymn, zmn);
        }

        /// <summary>
        /// 使用旧的尺寸和起点创建 GridStructure。
        /// </summary>
        public static GridStructure create_with_old_size_origin(int nx, int ny, int nz, GridStructure gs_old)
        {
            if ((nz == 1 && gs_old.dim == Dimension.D3) || (nz > 1 && gs_old.dim == Dimension.D2))
            {
                Console.WriteLine(MyExceptions.Geometry_IndexException);
                return null;
            }

            return init(nx, ny, nz, gs_old.xsiz, gs_old.ysiz, gs_old.zsiz, gs_old.xmn, gs_old.ymn,
                gs_old.zmn);
        }

        /// <summary>
        /// 基于已有的 GridStructure 创建一个新的 GridStructure。
        /// </summary>
        public static GridStructure create(GridStructure gs)
        {
            return init(gs.nx, gs.ny, gs.nz, gs.xsiz, gs.ysiz, gs.zsiz, gs.xmn, gs.ymn, gs.zmn);
        }

        /// <summary>
        /// 从字符串解析并创建 GridStructure。
        /// </summary>
        public static GridStructure create(string gs_viewText)
        {
            List<string> s1 = [];
            string s2 = "";
            bool b = false;
            foreach (var item in gs_viewText)
            {
                if (item == '[')
                {
                    b = true;
                    s2 = "";
                    s2 += item.ToString();
                    continue;
                }

                if (item == ']')
                {
                    b = false;
                    s2 += item.ToString();
                    s1.Add(s2.Trim('[', ']'));
                }

                if (b)
                {
                    s2 += item.ToString();
                }
            }

            var nx_ny_nz = s1[0].Split('_');
            int nx = int.Parse(nx_ny_nz[0]);
            int ny = int.Parse(nx_ny_nz[1]);
            int nz = int.Parse(nx_ny_nz[2]);
            var xsiz_ysiz_zsiz = s1[1].Split('_');
            float xsiz = float.Parse(xsiz_ysiz_zsiz[0]);
            float ysiz = float.Parse(xsiz_ysiz_zsiz[1]);
            float zsiz = float.Parse(xsiz_ysiz_zsiz[2]);
            var xmn_ymn_zmn = s1[2].Split('_');
            float xmn = float.Parse(xmn_ymn_zmn[0]);
            float ymn = float.Parse(xmn_ymn_zmn[1]);
            float zmn = float.Parse(xmn_ymn_zmn[2]);

            return init(nx, ny, nz, xsiz, ysiz, zsiz, xmn, ymn, zmn);
        }

        /// <summary>
        /// 从窗体获取参数并创建 GridStructure。
        /// </summary>
        public static GridStructure create_win(GridStructure gs = null, string title = null)
        {
            Form_GridStructure frm = new(gs, title);
            if (frm.ShowDialog() != DialogResult.OK)
                return null;
            var paras = frm.paras;
            int nx = int.Parse(paras[0]);
            int ny = int.Parse(paras[1]);
            int nz = int.Parse(paras[2]);
            float xsiz = float.Parse(paras[3]);
            float ysiz = float.Parse(paras[4]);
            float zsiz = float.Parse(paras[5]);
            float xmn = float.Parse(paras[6]);
            float ymn = float.Parse(paras[7]);
            float zmn = float.Parse(paras[8]);
            string dim = paras[9];
            if (dim == "D2")
                return create(nx, ny, 1, xsiz, ysiz, zsiz, xmn, ymn, zmn);
            if (dim == "D3")
                return create(nx, ny, nz, xsiz, ysiz, zsiz, xmn, ymn, zmn);
            return null;
        }

        /// <summary>
        /// 索引映射(arrayIndex -> spatialIndex),作用是根据arrayIndex获取spatialIndex
        /// </summary>
        /// <param name="gs"></param>
        /// <returns></returns>
        private static List<SpatialIndex> get_index_mapper(GridStructure gs)
        {
            List<SpatialIndex> indexMapper = new()
            {
                Capacity = gs.N
            };
            for (int iz = 0; iz < gs.nz; iz++)
            for (int iy = 0; iy < gs.ny; iy++)
            for (int ix = 0; ix < gs.nx; ix++)
            {
                indexMapper.Add(gs.dim == Dimension.D2 ? SpatialIndex.create(ix, iy) : SpatialIndex.create(ix, iy, iz));
            }

            return indexMapper;
        }

        /// <summary>
        /// 判断两个GridStructure是否相同，如果其中一个是null，都是不相同的
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator ==(GridStructure left, GridStructure right)
        {
            //return true;
            if (Equals(left, null))
            {
                return Equals(right, null) ? true : false;
            }
            else
            {
                return left.Equals(right);
            }
        }

        //判断!=
        public static bool operator !=(GridStructure left, GridStructure right)
        {
            return !(left == right);
        }


        #region 判断相等

        /// <summary>
        /// 判断两个GridStructure是否相等
        /// </summary>
        /// <param name="gs"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (obj is not GridStructure)
            {
                return false;
            }

            GridStructure _gs = (GridStructure)obj;
            return dim == _gs.dim && N == _gs.N
                                  && nx == _gs.nx && ny == _gs.ny && nz == _gs.nz
                                  && xsiz == _gs.xsiz && ysiz == _gs.ysiz && zsiz == _gs.zsiz
                                  && xmn == _gs.xmn && ymn == _gs.ymn && zmn == _gs.zmn;
        }

        public override int GetHashCode()
        {
            return dim.GetHashCode() ^ N.GetHashCode()
                                     ^ nx.GetHashCode() ^ ny.GetHashCode() ^ nz.GetHashCode()
                                     ^ xsiz.GetHashCode() ^ ysiz.GetHashCode() ^ zsiz.GetHashCode()
                                     ^ xmn.GetHashCode() ^ ymn.GetHashCode() ^ zmn.GetHashCode();
        }

        #endregion

        #endregion
    }
}