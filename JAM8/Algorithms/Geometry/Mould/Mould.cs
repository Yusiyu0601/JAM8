using Easy.Common.Extensions;
using JAM8.Algorithms.Numerics;
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
        private Mould()
        {
        }

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
        /// 根据中心点 core 与邻居点 neighbors 构建 Mould 模板。
        /// 所有邻居位置将统一以 core 为参考点进行偏移，并按偏移后的空间距离从近到远排序（与 C++ 行为完全一致）。
        /// 
        /// 【注意】
        /// - 该方法排除重复邻居（根据坐标判断）；
        /// - 排除与 core 相同位置的点（距离为0）；
        /// - 返回的 neighbor_spiral_mapper 不包含 core 自身；
        /// - 所有节点相对于 core 的坐标将进行偏移处理，构建一个以 (0,0[,0]) 为中心的模板。
        /// 
        /// 【等效行为】
        /// 与 C++ 中 geometry::Mould::create_by_location(const SpatialIndex& core, const std::vector<SpatialIndex>& neighbors) 完全一致。
        /// 
        /// 【示例】
        /// <code>
        /// var core = SpatialIndex.create(5, 5);
        /// var neighbors = new List&lt;SpatialIndex&gt;
        /// {
        ///     SpatialIndex.create(6, 5),
        ///     SpatialIndex.create(4, 5),
        ///     SpatialIndex.create(5, 6),
        ///     SpatialIndex.create(5, 4),
        /// };
        /// var mould = Mould.create_by_location(core, neighbors);
        /// </code>
        /// </summary>
        /// <param name="core">核心点 SpatialIndex，模板中心</param>
        /// <param name="neighbors">模板邻居点集合，未偏移，允许任意顺序</param>
        /// <returns>一个新的 Mould 对象，内部邻居列表已偏移并按距离排序</returns>
        /// <exception cref="ArgumentNullException">core 为 null</exception>
        /// <exception cref="ArgumentException">neighbors 为 null 或空</exception>
        public static Mould create_by_location(SpatialIndex core, List<SpatialIndex> neighbors)
        {
            if (core == null)
                throw new ArgumentNullException(nameof(core), "Core cannot be null.");
            if (neighbors == null || neighbors.Count == 0)
                throw new ArgumentException("Neighbors cannot be null or empty.", nameof(neighbors));

            // 【步骤1】计算偏移量，使 core 变为原点
            var offset = core.dim == Dimension.D2
                ? SpatialIndex.create(-core.ix, -core.iy)
                : SpatialIndex.create(-core.ix, -core.iy, -core.iz);

            // 【步骤2】去重 → 应用偏移 → 排除中心点 → 计算距离 → 排序
            var neighbor_spiral_mapper = neighbors
                .DistinctBy(n => new { n.dim, n.ix, n.iy, n.iz }) // 去重
                .Select(n => n.offset(offset)) // 将 neighbor 偏移，使 core → 原点
                .Where(n => SpatialIndex.calc_dist_to_origin(n) > 0) // 排除偏移后变为原点的点
                .Select(n => (
                    distance: SpatialIndex.calc_dist_to_origin(n),
                    spatial_index: n
                ))
                .OrderBy(t => t.distance) // 主排序：距离升序
                .ThenBy(t => t.spatial_index.ix) // Tie-break 1
                .ThenBy(t => t.spatial_index.iy) // Tie-break 2
                .ThenBy(t => t.spatial_index.iz) // Tie-break 3
                .ToList();

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
        public static Mould create_by_front_section(Mould mould, int k_first_neighbors)
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
        public static Mould create_by_front_section(Mould mould, double first_ratio)
        {
            return create_by_front_section(mould, (int)(first_ratio * mould.neighbors_number));
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
            List<SpatialIndex> neighbors = new((2 * radius_x + 1) * (2 * radius_y + 1)); // 初始化大小

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
            List<SpatialIndex> neighbors = new((2 * radius_x + 1) * (2 * radius_y + 1) * (2 * radius_z + 1)); // 初始化容量

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
        /// Create a new Mould according to the radius size of the Ellipse template, 
        /// including the center core of the template itself
        /// 根据椭圆Ellipse模板半径尺寸新建Mould，包含模板中心core自身
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
                    if (b <= 1) //在椭圆内
                    {
                        int ix_mg = (int)(ix * Pow(2, multi_grid - 1));
                        int iy_mg = (int)(iy * Pow(2, multi_grid - 1));
                        neighbors.Add(SpatialIndex.create(ix_mg, iy_mg)); //根据多重网格，计算实际网格节点位置
                    }
                }
            }

            return create_by_location(core, neighbors);
        }

        public static Mould create_by_anis_ellipse(
            double aLong, // 长轴半径（格点数）
            double aShort, // 短轴半径（格点数）
            int multi_grid = 1, // 多重网格层级（>=1）
            double angleDeg = 0.0 // 椭圆长轴相对 +x 轴的角度（度）
        )
        {
            if (aLong <= 0 || aShort <= 0) throw new ArgumentException("Radii must be > 0.");
            if (multi_grid < 1) throw new ArgumentException("multiGridLevel must be >= 1.");

            var core = SpatialIndex.create(0, 0);
            var neighbors = new List<SpatialIndex>();

            // 旋转到椭圆自身坐标（长轴对齐 u 轴）：[u,v] = R(-θ) * [x,y]
            double theta = angleDeg * Math.PI / 180.0;
            double c = Math.Cos(theta), s = Math.Sin(theta);

            // 搜索边界盒（取长短轴中较大者）
            int r = (int)Math.Ceiling(Math.Max(aLong, aShort));

            // 多重网格缩放系数（1,2,4,8,...）
            int scale = 1 << (multi_grid - 1);

            double aLong2 = aLong * aLong;
            double aShort2 = aShort * aShort;
            const double eps = 1e-9;

            for (int iy = -r; iy <= r; iy++)
            {
                for (int ix = -r; ix <= r; ix++)
                {
                    if (ix == 0 && iy == 0) continue; // 不把 core 自己放进邻居

                    // 旋转到椭圆坐标系：R(-θ) = [[c, s],[-s, c]]
                    double u = ix * c + iy * s; // 沿长轴
                    double v = -ix * s + iy * c; // 沿短轴

                    // 椭圆判定：(u/aLong)^2 + (v/aShort)^2 <= 1
                    double val = (u * u) / aLong2 + (v * v) / aShort2;
                    if (val <= 1.0 + eps)
                    {
                        int ix_mg = ix * scale;
                        int iy_mg = iy * scale;
                        neighbors.Add(SpatialIndex.create(ix_mg, iy_mg));
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
                        double b = Sqrt(Pow(ix / (double)radius_x, 2) + Pow(iy / (double)radius_y, 2) +
                                        Pow(iz / (double)radius_z, 2));
                        if (b <= 1) //在椭圆内
                        {
                            int ix_mg = (int)(ix * Pow(2, multi_grid - 1));
                            int iy_mg = (int)(iy * Pow(2, multi_grid - 1));
                            int iz_mg = (int)(iz * Pow(2, multi_grid - 1));
                            neighbors.Add(SpatialIndex.create(ix_mg, iy_mg, iz_mg)); //根据多重网格，计算实际网格节点的索引
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
            GridStructure gs = gp.grid_structure;
            if (gs.dim != core_in_gridProperty.dim)
                return null;
            var (index_not_equal_null, _) = gp.get_values_by_condition(null, CompareType.NotEqual);
            var neighbors = index_not_equal_null
                .Select(index => gs.get_spatial_index(index))
                .ToList();
            return create_by_location(core_in_gridProperty, neighbors);
        }

        /// <summary>
        /// 显示2d的IrregularMould
        /// </summary>
        /// <param name="Title"></param>
        public void Show2d(string title)
        {
            if (dim != Dimension.D2)
            {
                Console.WriteLine("⚠️  This mould is not 2D. Show2d is only for 2D templates.");
                return;
            }

            if (neighbor_spiral_mapper == null || neighbor_spiral_mapper.Count == 0)
            {
                Console.WriteLine("⚠️  No neighbors to show.");
                return;
            }

            // 获取所有邻居坐标（偏移后的）
            var all = neighbor_spiral_mapper.Select(t => t.spatial_index).ToList();

            // 包括中心 (0,0)
            all.Add(SpatialIndex.create(0, 0));

            int minX = all.Min(p => p.ix);
            int maxX = all.Max(p => p.ix);
            int minY = all.Min(p => p.iy);
            int maxY = all.Max(p => p.iy);

            Console.WriteLine($"\n📐 {title}");
            Console.WriteLine($"范围：X=[{minX},{maxX}], Y=[{minY},{maxY}]\n");

            // y 从 max 到 min（上到下），x 从 min 到 max（左到右）
            for (int y = maxY; y >= minY; y--)
            {
                for (int x = minX; x <= maxX; x++)
                {
                    if (x == 0 && y == 0)
                        Console.Write(" O"); // 中心点
                    else if (neighbor_spiral_mapper.Any(t => t.spatial_index.ix == x && t.spatial_index.iy == y))
                        Console.Write(" *"); // 邻居点
                    else
                        Console.Write("  "); // 空白
                }
                Console.WriteLine();
            }

            Console.WriteLine();
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
        /// 无需构造 MouldInstance，直接提取邻居值列表并进行有效性检测
        /// </summary>
        public static bool TryGetNeighborValues(
            Mould mould,
            SpatialIndex core,
            GridProperty gp,
            out List<float?> neighbor_values,
            out float? core_value)
        {
            neighbor_values = new List<float?>(mould.neighbors_number);
            core_value = gp.get_value(core);
            int valid_count = 0;

            int core_x = core.ix;
            int core_y = core.iy;
            int core_z = core.iz;

            for (int i = 0; i < mould.neighbors_number; i++)
            {
                var offset = mould.neighbor_spiral_mapper[i].spatial_index;

                int neighbor_x = core_x + offset.ix;
                int neighbor_y = core_y + offset.iy;
                int neighbor_z = core_z + offset.iz;

                var value = gp.get_value(neighbor_x, neighbor_y, neighbor_z);

                neighbor_values.Add(value);
                if (value != null)
                    valid_count++;
            }

            // 是否为“完整样式”
            return valid_count == mould.neighbors_number;
        }

        /// <summary>
        /// 深度复制
        /// </summary>
        /// <returns></returns>
        private Mould clone()
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