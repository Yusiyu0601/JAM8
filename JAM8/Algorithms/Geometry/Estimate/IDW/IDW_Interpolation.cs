namespace JAM8.Algorithms.Geometry
{
    public class IDW_Interpolation
    {
        private IDW_Interpolation() { }
        public GridStructure gs { get; internal set; }
        public CData cd { get; internal set; }
        /// <summary>
        /// 条件数据的属性名称，例如"孔隙度"
        /// </summary>
        public string propertyName { get; internal set; }

        /// <summary>
        /// 创建OK对象
        /// </summary>
        /// <param name="vm"></param>
        /// <param name="cd"></param>
        /// <param name="gs"></param>
        /// <returns></returns>
        public static IDW_Interpolation create(GridStructure gs, CData cd, string propertyName)
        {
            IDW_Interpolation ok = new()
            {
                gs = gs,
                cd = cd,
                propertyName = propertyName
            };
            return ok;
        }

        /// <summary>
        /// 主程序
        /// </summary>
        /// <param name="radius">根据数据密度设置搜索半径</param>
        /// <param name="k_cdi">4~8之间，不要低于3</param>
        /// <returns>模型和估计方差</returns>
        public GridProperty Run(int radius, int k_cdi)
        {
            Grid g = Grid.create(gs);//根据gridStructure创建新grid
            g.add_gridProperty("模型", cd.assign_to_grid(gs).grid_assigned[propertyName]);
            //var cd_finder = CDataNearestFinder_based_kdtree.create(gs, cd);
            //Parallel.For(1, gs.N + 1, n =>
            //{
            //    SpatialIndex si = gs.get_spatialIndex(n);
            //    if (g["模型"].get_value(n) == null)//如果某个点没有数据，则需要插值
            //    {
            //        var cd_founds = cd_finder.find(si, radius, k_cdi);
            //        MyConsoleProgress.Print(n, gs.N, "Inverse Distance Weighting Interpolation", cd_founds.Count.ToString());
            //        int k = cd_founds.Count;
            //        float estimate = 0;//计算待估值
            //        for (int i = 0; i < k; i++)
            //        {
            //            var distance = cd_founds[i].distance;
            //            double weight = Math.Pow(1 / distance, 2) / cd_founds.Sum(a => Math.Pow(1 / a.distance, 2));
            //            estimate += cd_founds[i].cdi[propertyName].Value * (float)weight;
            //        }
            //        g["模型"].set_value(n, estimate);
            //    }
            //});
            var cd_finder = CDataNearestFinder_kdtree4_anisotropy.create(gs, cd);
            for (int n = 0; n < gs.N; n++)//计算工区网格的所有节点
            {
                SpatialIndex si = gs.get_spatial_index(n);
                if (g["模型"].get_value(n) == null)//如果某个点没有数据，则需要插值
                {
                    var cd_founds = cd_finder.find(si, k_cdi);
                    if (cd_founds.Count == 0)
                        continue;
                    //MyConsoleProgress.Print(n, gs.N, "Inverse Distance Weighting Interpolation", cd_founds.Count.ToString());
                    int k = cd_founds.Count;
                    float estimate = 0;//计算待估值
                    List<double> weights = new();
                    for (int i = 0; i < k; i++)
                    {
                        var distance = cd_founds[i].distance;
                        double weight = Math.Pow(1 / distance, 2) / cd_founds.Sum(a => Math.Pow(1 / a.distance, 2));
                        weights.Add(weight);
                        estimate += cd_founds[i].cdi[propertyName].Value * (float)weight;

                        //if (si.iy == 28)//观察权重
                        //    Console.WriteLine($"{cd_founds[i].si} {weights[i]} {cd_founds[i].distance}");
                        if (si.ix == 40 && si.iy == 40)//观察权重
                            Console.WriteLine($@"{cd_founds[i].si} {weights[i]} {cd_founds[i].distance}");
                    }
                    //if (k > 0 && si.iy == 28)
                    if (si.ix == 40 && si.iy == 40)
                    {
                        Console.WriteLine(si);
                        Console.WriteLine();
                        Console.WriteLine();
                    }

                    g["模型"].set_value(n, estimate);
                }
            }
            return g["模型"];
        }

    }
}
