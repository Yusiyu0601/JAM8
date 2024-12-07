using JAM8.Utilities;

namespace JAM8.Algorithms.Geometry
{
    // Simpat的模拟路径
    // ********** Demo **********
    //    GridStructure gs = GridStructure.CreateSimple(200, 200);
    //    SimulationPath vp = new SimulationPath(gs, 3, 200);
    //    Random rnd = new Random();
    //        while (true)
    //        {
    //            var sample = vp.Sample(rnd);
    //            vp.Remove(sample.Item1);
    //            if (vp.IsEmpty)
    //                break;
    //        }
    public class SimulationPath
    {
        /// <summary>
        /// 访问节点，可冻结
        /// </summary>
        class path_node
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
        Random rnd;
        int flag_forward = -1;//向前访问的位置

        List<path_node> path_nodes;//
        Dictionary<string, int> spatialIndex_MapTo_randomIndex;//

        private SimulationPath() { }

        //总数
        public int N
        {
            get
            {
                return spatialIndexes.Count;
            }
        }
        //累积冻结的数量
        int N_freezed = 0;
        //进度
        public double progress
        {
            get { return Math.Round(100.0 * N_freezed / N, 2); }
        }

        void init()
        {
            path_nodes = new();
            spatialIndex_MapTo_randomIndex = new();
            for (int n = 0; n < spatialIndexes.Count; n++)
            {
                path_nodes.Add(new path_node() { spatialIndex = spatialIndexes[n] });
            }
            path_nodes = SortHelper.RandomSort(path_nodes, rnd).sorted;
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
        public static SimulationPath create(List<SpatialIndex> spatialIndexes, Random rnd)
        {
            SimulationPath path = new()
            {
                spatialIndexes = spatialIndexes,
                rnd = rnd,
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
        public static SimulationPath create(GridStructure gs, int multi_grid_m, Random rnd)
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
            return create(coords_m, rnd);
        }
    }
}
