using JAM8.Utilities;

namespace JAM8.Algorithms.Geometry
{
    /// <summary>
    /// 表示用于多点模拟（如 SimPat/SNESIM）的模拟路径控制器，支持冻结、打乱、按路径逐个访问空间节点。
    /// 
    /// 模拟路径是一种基于随机路径（Random Path）的方法，对待模拟的网格节点进行随机排序、访问和控制，
    /// 常用于多重网格建模、条件模拟等过程。
    /// 
    /// <code>
    /// 【主要功能】
    /// - 支持从二维或三维网格生成路径；
    /// - 路径节点随机打乱，可指定自定义随机数生成器；
    /// - 支持逐节点访问（visit_next）；
    /// - 支持冻结节点（freeze），即跳过模拟或保留已有结果；
    /// - 支持按多重网格等级 coarse grid 自动生成路径；
    /// - 可追踪访问进度（progress）；
    /// 
    /// 【常见用途】
    /// - 多点地质建模；
    /// - 多重网格下的模拟路径控制；
    /// - 可扩展为条件模拟路径，冻结已有条件点；
    /// 
    /// 【使用示例】
    /// using JAM8.Algorithms.Geometry;
    /// using JAM8.Utilities;
    /// 
    /// // 1. 创建一个简单网格结构（200x200）
    /// var gs = GridStructure.CreateSimple(200, 200);
    /// 
    /// // 2. 使用多重网格等级为 3，构建模拟路径（采样点变稀疏）
    /// var mt = new MersenneTwister(12345);  // 自定义梅森旋转随机数
    /// var path = SimulationPath.create(gs, multi_grid_m: 3, mt);
    /// 
    /// // 3. 模拟循环，逐点访问并处理
    /// while (!path.is_visit_over())
    /// {
    ///     var si = path.visit_next();
    ///     if (si == null) break;
    ///     
    ///     // 对当前 SpatialIndex 节点执行模拟操作
    ///     // ... 模拟逻辑 ...
    /// }
    /// 
    /// // 4. 查询模拟进度
    /// Console.WriteLine($"模拟完成度：{path.progress}%");
    /// </code>
    /// </summary>
    public class SimulationPath
    {
        /// <summary>
        /// 访问节点，可冻结
        /// </summary>
        private class path_node
        {
            /// <summary>
            /// 冻结状态
            /// </summary>
            public bool freezed = false;

            /// <summary>
            /// 节点的SpatialIndex
            /// </summary>
            public SpatialIndex spatialIndex;

            public override string ToString()
            {
                return $"[{freezed}]{spatialIndex}";
            }
        }

        /// <summary>
        /// 模拟路径包含的节点集
        /// </summary>
        public List<SpatialIndex> spatialIndexes { get; internal set; }

        private MersenneTwister mt; //随机数生成器
        private int flag_forward = -1; //向前访问的位置

        private List<path_node> path_nodes; //
        private Dictionary<string, int> spatialIndex_MapTo_randomIndex; //

        private SimulationPath()
        {
        }

        //总数
        public int N
        {
            get { return spatialIndexes.Count; }
        }

        //累积冻结的数量
        private int N_freezed = 0;

        //进度
        public double progress
        {
            get { return Math.Round(100.0 * N_freezed / N, 2); }
        }

        private void init()
        {
            path_nodes = new();
            spatialIndex_MapTo_randomIndex = new();
            for (int n = 0; n < spatialIndexes.Count; n++)
            {
                path_nodes.Add(new path_node() { spatialIndex = spatialIndexes[n] });
            }

            path_nodes = MyShuffleHelper.fisher_yates_shuffle(path_nodes, mt).shuffled;
            for (int i = 0; i < path_nodes.Count; i++)
                spatialIndex_MapTo_randomIndex.Add(path_nodes[i].spatialIndex.view_text(), i);
        }

        /// <summary>
        /// 冻结指定spatialIndex
        /// </summary>
        /// <param name="spatialIndex"></param>
        public void freeze(SpatialIndex spatialIndex)
        {
            string viewText_si = spatialIndex.view_text();
            if (spatialIndex_MapTo_randomIndex.ContainsKey(viewText_si))
            {
                int random_index = spatialIndex_MapTo_randomIndex[viewText_si];
                if (path_nodes[random_index].freezed == false)
                {
                    path_nodes[random_index].freezed = true;
                    N_freezed++;
                }
            }
        }

        /// <summary>
        /// 冻结指定spatialIndexes
        /// </summary>
        /// <param name="spatialIndexes"></param>
        public void freeze(List<SpatialIndex> spatialIndexes)
        {
            for (int i = 0; i < spatialIndexes.Count; i++)
            {
                freeze(spatialIndexes[i]);
            }
        }

        /// <summary>
        /// 访问next，并冻结该节点。全部访问，则返回null
        /// </summary>
        /// <returns></returns>
        public SpatialIndex visit_next()
        {
            while (true)
            {
                if (flag_forward >= path_nodes.Count - 1)
                    return null;
                flag_forward++;
                var path_node = path_nodes[flag_forward];
                //访问并冻结
                if (path_node.freezed == false)
                {
                    path_node.freezed = true;
                    N_freezed++;
                    return path_node.spatialIndex;
                }
            }
        }

        /// <summary>
        /// 访问结束
        /// </summary>
        /// <returns></returns>
        public bool is_visit_over()
        {
            return N_freezed == N;
        }

        /// <summary>
        /// 创建SimulationPath对象
        /// </summary>
        /// <param name="spatialIndexes"></param>
        /// <param name="rnd"></param>
        /// <returns></returns>
        public static SimulationPath create(List<SpatialIndex> spatialIndexes, MersenneTwister mt)
        {
            SimulationPath path = new()
            {
                spatialIndexes = spatialIndexes,
                mt = mt,
            };
            path.init();
            return path;
        }

        /// <summary>
        /// 创建SimulationPath对象
        /// </summary>
        /// <param name="gs"></param>
        /// <param name="multi_grid_m"></param>
        /// <param name="rnd"></param>
        /// <returns></returns>
        public static SimulationPath create(GridStructure gs, int multi_grid_m, MersenneTwister mt)
        {
            List<SpatialIndex> coords_m = [];
            if (gs.dim == Dimension.D2)
            {
                for (int iy = 0; iy < gs.ny; iy++)
                {
                    for (int ix = 0; ix < gs.nx; ix++)
                    {
                        int ix_m = (int)(ix * Math.Pow(2, multi_grid_m - 1));
                        int iy_m = (int)(iy * Math.Pow(2, multi_grid_m - 1));
                        if (ix_m < 0 || ix_m >= gs.nx ||
                            iy_m < 0 || iy_m >= gs.ny)
                            continue;
                        coords_m.Add(SpatialIndex.create(ix_m, iy_m));
                    }
                }
            }

            if (gs.dim == Dimension.D3)
            {
                for (int iz = 0; iz < gs.nz; iz++)
                {
                    for (int iy = 0; iy < gs.ny; iy++)
                    {
                        for (int ix = 0; ix < gs.nx; ix++)
                        {
                            int ix_m = (int)(ix * Math.Pow(2, multi_grid_m - 1));
                            int iy_m = (int)(iy * Math.Pow(2, multi_grid_m - 1));
                            int iz_m = (int)(iz * Math.Pow(2, multi_grid_m - 1));
                            if (ix_m < 0 || ix_m >= gs.nx ||
                                iy_m < 0 || iy_m >= gs.ny ||
                                iz_m < 0 || iz_m >= gs.nz
                               )
                                continue;
                            coords_m.Add(SpatialIndex.create(ix_m, iy_m, iz_m));
                        }
                    }
                }
            }

            return create(coords_m, mt);
        }
    }
}