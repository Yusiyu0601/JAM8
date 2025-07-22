using Accord.IO;

namespace JAM8.Algorithms.Geometry
{
    public class IDW_Interpolation
    {
        private IDW_Interpolation()
        {
        }

        /// <summary>
        /// 主程序
        /// </summary>
        /// <param name="cd"></param>
        /// <param name="radius">根据数据密度设置搜索半径</param>
        /// <param name="k_cdi">4~8之间，不要低于3</param>
        /// <param name="gs"></param>
        /// <param name="propertyName"></param>
        /// <returns>模型和估计方差</returns>
        public static GridProperty Run(GridStructure gs, CData cd, string propertyName, int radius, int k_cdi)
        {
            //首先将条件数据进行粗化到工区网格，后续的插值都基于粗化后的条件数据
            var (coarsened_cdata, coarsened_grid) = cd.coarsened(gs);
            //基于粗化后的条件数据（已经与工区网格对齐，因此能通过网格单元的索引查询）创建查询类
            var cd_finder = CDataNearestFinder_kdtree.create(coarsened_cdata);
            //复制
            var re = coarsened_grid.first_gridProperty().deep_clone();
            //计算工区网格的所有节点
            for (int n = 0; n < gs.N; n++)
            {
                //根据n获取网格单元的空间索引
                SpatialIndex si = gs.get_spatial_index(n);

                Coord coord = gs.dim == Dimension.D2
                    ? Coord.create(si.ix, si.iy)
                    : Coord.create(si.ix, si.iy, si.iz);

                //如果某个网格单元没有数据，则需要插值
                if (re.get_value(n) == null)
                {
                    var founds = cd_finder.find(coord, k_cdi);
                    if (founds.Count == 0)
                        continue;
                    //MyConsoleProgress.Print(n, gs.N, "Inverse Distance Weighting Interpolation", cd_founds.Count.ToString());
                    int k = founds.Count;
                    float estimate = 0; //计算待估值
                    List<double> weights = [];
                    for (int i = 0; i < k; i++)
                    {
                        var distance = founds[i].distance;
                        double weight = Math.Pow(1 / distance, 2) / founds.Sum(a => Math.Pow(1 / a.distance, 2));
                        weights.Add(weight);
                        estimate += founds[i].attrs[propertyName].Value * (float)weight;

                        //if (si.iy == 28)//观察权重
                        //    Console.WriteLine($"{cd_founds[i].si} {weights[i]} {cd_founds[i].distance}");
                        if (si.ix == 40 && si.iy == 40) //观察权重
                            Console.WriteLine($@"{founds[i].coord} {weights[i]} {founds[i].distance}");
                    }

                    //if (k > 0 && si.iy == 28)
                    if (si.ix == 40 && si.iy == 40)
                    {
                        Console.WriteLine(si);
                        Console.WriteLine();
                        Console.WriteLine();
                    }

                    re.set_value(n, estimate);
                }
            }

            return re;
        }
    }
}