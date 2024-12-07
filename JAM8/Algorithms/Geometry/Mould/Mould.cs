using Easy.Common.Extensions;
using JAM8.Utilities;
using static System.Math;

namespace JAM8.Algorithms.Geometry
{
    /// <summary>
    /// 模板类,由中心点core和邻居neighbor组成,其中core在二维是(0,0),三维是(0,0,0)
    /// </summary>
    public class Mould
    {
        /// <summary>
        /// 私有构造函数
        /// </summary>
        private Mould() { }

        /// <summary>
        /// 邻居点的数量
        /// </summary>
        public int neighbors_number
        {
            get { return neighbor_spiral_mapper.Count; }
        }

        /// <summary>
        /// 邻居节点的位置索引(由中心螺旋向外,距离逐渐增加),注意不包含中心节点core
        /// </summary>
        public List<(float distance, SpatialIndex spatial_index)> neighbor_spiral_mapper { get; internal set; } = null;

        /// <summary>
        /// 维度
        /// </summary>
        public Dimension dim { get; internal set; }

        /// <summary>
        /// 根据 core 与 neighbors 创建 Mould
        /// </summary>
        /// <param name="core">核心点</param>
        /// <param name="neighbors">相邻点列表</param>
        /// <returns>生成的 Mould 对象</returns>
        public static Mould create_by_location(SpatialIndex core, List<SpatialIndex> neighbors)
        {
            // 参数检查
            if (core == null)
                throw new ArgumentNullException(nameof(core), "Core cannot be null.");
            if (neighbors == null || neighbors.Count == 0)
                throw new ArgumentException("Neighbors cannot be null or empty.", nameof(neighbors));

            // 1. 对 neighbors 去重并过滤距离为 0 的点，同时计算距离
            var offset = core.dim == Dimension.D2
                ? SpatialIndex.create(-core.ix, -core.iy)
                : SpatialIndex.create(-core.ix, -core.iy, -core.iz);

            // 计算与核心点的距离，过滤距离为 0 的点，并排序
            var neighbor_spiral_mapper = neighbors
                .DistinctBy(n => new { n.dim, n.ix, n.iy, n.iz }) // 去重
                .Select(n => (distance: SpatialIndex.calc_dist(core, n), neighbor: n)) // 计算距离
                .Where(t => t.distance > 0) // 过滤距离为 0 的点
                .OrderBy(t => t.distance) // 按距离排序
                .Select(t => (t.distance, t.neighbor.offset(offset))) // 应用偏移量
                .ToList();

            // 2. 构建 Mould 对象
            return new Mould
            {
                dim = core.dim,
                neighbor_spiral_mapper = neighbor_spiral_mapper
            };
        }

        /// <summary>
        /// 提取前k个邻居节点，组成新Mould
        /// </summary>
        /// <param name="mould"></param>
        /// <param name="first_k"></param>
        /// <returns></returns>
        public static Mould create_by_mould(Mould mould, int k_first_neighbors)
        {
            // 参数检查
            if (k_first_neighbors > mould.neighbors_number)
                throw new Exception("k_first_neighbors > mould.N");

            // 使用 LINQ 的 Take 方法简化代码
            var mould_first_k = new Mould
            {
                dim = mould.dim,
                // 提取前k个元素
                neighbor_spiral_mapper = mould.neighbor_spiral_mapper.Take(k_first_neighbors).ToList()
            };

            return mould_first_k;
        }

        /// <summary>
        /// 提取前ratio百分比的节点，组成新Mould
        /// </summary>
        /// <param name="mould"></param>
        /// <param name="first_ratio"></param>
        /// <returns></returns>
        public static Mould create_by_mould(Mould mould, double first_ratio)
        {
            return create_by_mould(mould, (int)(first_ratio * mould.neighbors_number));
        }

        /// <summary>
        /// 根据矩形Rectangle模板的半径尺寸新建IrregularMould(默认包含模板中心core自身)
        /// 注意事项：
        /// NormalMultiGrid:表示是否正常的多重网格模式
        /// 1.如果MultiGridLevel>=2 同时NormalMultiGrid==true，
        ///     节点范围的半径Radius随着MultiGridLevel变大，
        ///     多网格节点数量与单网格节点相同
        /// 2.如果MultiGridLevel>=2 同时NormalMultiGrid==false，
        ///     节点范围的半径Radius不变，所有节点包含于IRadius、JRadius内部，
        ///     多网格节点大部分被截掉
        /// </summary>
        /// <returns></returns>
        public static Mould create_by_rectangle(int radius_x, int radius_y, int multi_grid)
        {
            var core = SpatialIndex.create(0, 0);
            List<SpatialIndex> neighbors = new((2 * radius_x + 1) * (2 * radius_y + 1));  // 初始化大小

            // 计算多重网格缩放因子，避免多次计算
            int gridFactor = (int)Math.Pow(2, multi_grid - 1);

            // 遍历矩形范围，计算每个邻居的索引
            for (int iy = -radius_y; iy <= radius_y; iy++)
            {
                for (int ix = -radius_x; ix <= radius_x; ix++)
                {
                    // 根据多重网格，计算实际网格节点的索引
                    int ix_mg = ix * gridFactor;
                    int iy_mg = iy * gridFactor;

                    neighbors.Add(SpatialIndex.create(ix_mg, iy_mg));
                }
            }

            return create_by_location(core, neighbors);
        }

        /// <summary>
        /// 根据矩形体Rectangle模板的半径尺寸新建IrregularMould(默认包含模板中心core自身)
        /// 注意事项：
        /// NormalMultiGrid:表示是否正常的多重网格模式
        /// 1.如果MultiGridLevel>=2 同时NormalMultiGrid==true，
        ///     节点范围的半径Radius随着MultiGridLevel变大，
        ///     多网格节点数量与单网格节点相同
        /// 2.如果MultiGridLevel>=2 同时NormalMultiGrid==false，
        ///     节点范围的半径Radius不变，所有节点包含于IRadius、JRadius内部，
        ///     多网格节点大部分被截掉
        /// </summary>
        /// <param name="IRadius"></param>
        /// <param name="JRadius"></param>
        /// <param name="KRadius"></param>
        /// <param name="MultiGridLevel"></param>
        /// <param name="NormalMultiGrid"></param>
        /// <returns></returns>
        public static Mould create_by_rectangle(int radius_x, int radius_y, int radius_z, int multi_grid)
        {
            var core = SpatialIndex.create(0, 0, 0);
            List<SpatialIndex> neighbors = new((2 * radius_x + 1) * (2 * radius_y + 1) * (2 * radius_z + 1));  // 初始化容量

            // 计算多重网格缩放因子，避免多次计算
            int gridFactor = (int)Math.Pow(2, multi_grid - 1);

            // 遍历三维矩形范围，计算每个邻居的索引
            for (int iz = -radius_z; iz <= radius_z; iz++)
            {
                for (int iy = -radius_y; iy <= radius_y; iy++)
                {
                    for (int ix = -radius_x; ix <= radius_x; ix++)
                    {
                        // 根据多重网格，计算实际网格节点的索引
                        int ix_mg = ix * gridFactor;
                        int iy_mg = iy * gridFactor;
                        int iz_mg = iz * gridFactor;

                        neighbors.Add(SpatialIndex.create(ix_mg, iy_mg, iz_mg));
                    }
                }
            }

            return create_by_location(core, neighbors);
        }

        /// <summary>
        /// 根据椭圆Ellipse模板半径尺寸新建Mould，包含模板中心core自身)
        /// </summary>
        /// <param name="radius_x"></param>
        /// <param name="radius_y"></param>
        /// <param name="multi_grid"></param>
        /// <returns></returns>
        public static Mould create_by_ellipse(int radius_x, int radius_y, int multi_grid)
        {
            var core = SpatialIndex.create(0, 0);
            List<SpatialIndex> neighbors = [];

            for (int iy = -radius_y; iy <= radius_y; iy++)
            {
                for (int ix = -radius_x; ix <= radius_x; ix++)
                {
                    double b = Sqrt(Pow(ix / (double)radius_x, 2) + Pow(iy / (double)radius_y, 2));
                    if (b <= 1)//在椭圆内
                    {
                        int ix_mg = (int)(ix * Pow(2, multi_grid - 1));
                        int iy_mg = (int)(iy * Pow(2, multi_grid - 1));
                        neighbors.Add(SpatialIndex.create(ix_mg, iy_mg));//根据多重网格，计算实际网格节点位置
                    }
                }
            }
            return create_by_location(core, neighbors);
        }

        /// <summary>
        /// 根据椭球体Ellipse模板的半径尺寸新建IrregularMould(默认包含模板中心core自身)
        /// </summary>
        /// <returns></returns>
        public static Mould create_by_ellipse(int radius_x, int radius_y, int radius_z, int multi_grid)
        {
            var core = SpatialIndex.create(0, 0, 0);
            List<SpatialIndex> neighbors = [];
            for (int iz = -radius_z; iz <= radius_z; iz++)
            {
                for (int iy = -radius_y; iy <= radius_y; iy++)
                {
                    for (int ix = -radius_x; ix <= radius_x; ix++)
                    {
                        double b = Sqrt(Pow(ix / (double)radius_x, 2) + Pow(iy / (double)radius_y, 2) + Pow(iz / (double)radius_z, 2));
                        if (b <= 1)//在椭圆内
                        {
                            int ix_mg = (int)(ix * Pow(2, multi_grid - 1));
                            int iy_mg = (int)(iy * Pow(2, multi_grid - 1));
                            int iz_mg = (int)(iz * Pow(2, multi_grid - 1));
                            neighbors.Add(SpatialIndex.create(ix_mg, iy_mg, iz_mg));//根据多重网格，计算实际网格节点的索引
                        }
                    }
                }
            }
            return create_by_location(core, neighbors);
        }

        /// <summary>
        /// 根据Grid尺寸新建IrregularMould(默认包括CoreLoc自身)
        /// 注意：Core是模板的中心（设定的中心，不一定是实际中心）
        /// </summary>
        /// <param name="core_in_gridProperty">Core在Grid里的位置</param>
        /// <param name="gp">Grid包含 Null 和 非Null的节点，Mould只记录非Null的节点位置</param>
        /// <returns></returns>
        public static Mould create_by_gridProperty(SpatialIndex core_in_gridProperty, GridProperty gp)
        {
            GridStructure gs = gp.gridStructure;
            if (gs.dim != core_in_gridProperty.dim)
                return null;
            return create_by_location(core_in_gridProperty, gp.get_spatialIndex_ne_null());
        }

        /// <summary>
        /// 显示2d的IrregularMould
        /// </summary>
        /// <param name="Title"></param>
        public void Show2d(string Title)
        {

        }

        public override string ToString()
        {
            return $"[dim:{dim} N:{neighbors_number}]";
        }

        /// <summary>
        /// 计算2个样式的距离
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static float get_distance(MouldInstance left, MouldInstance right)
        {
            float distance = 0f;
            for (int n = 0; n < left.mould.neighbors_number; n++)
            {
                if (left[n] != null && right[n] != null)
                {
                    distance += Abs(left[n].Value - right[n].Value);
                }
            }
            return distance;
        }

        /// <summary>
        /// 深度复制
        /// </summary>
        /// <returns></returns>
        Mould clone()
        {
            Mould mould = new()
            {
                dim = dim,
                neighbor_spiral_mapper = []
            };
            for (int i = 0; i < neighbor_spiral_mapper.Count; i++)
            {
                float distance = neighbor_spiral_mapper[i].distance;
                SpatialIndex si = neighbor_spiral_mapper[i].spatial_index.clone();
                mould.neighbor_spiral_mapper.Add((distance, si));
            }

            return mould;
        }
    }
}
