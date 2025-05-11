using System.Collections.Concurrent;
using System.Diagnostics;
using JAM8.Utilities;

namespace JAM8.Algorithms.Geometry
{
    public class Simpat
    {
        private Simpat() { }

        private int random_seed = 1;
        private int multi_grid = 1;
        private GridProperty train_image;
        private GridStructure gs_re;
        private CoarsenedCData ccd;
        private Mould mould;
        private (int rx, int ry, int rz) template_rx_ry_rz;
        private int N = 1;//建模个数

        private Dimension dim
        {
            get
            {
                return train_image.grid_structure.dim;
            }
        }

        //样式库
        public Dictionary<int, (Mould mould, Patterns patterns)> pats_mg
        { get; internal set; }

        /// <summary>
        /// 初始化，创建模式库
        /// </summary>
        private void init()
        {
            pats_mg = [];
            for (int m = 1; m <= multi_grid; m++)//多重网格模拟
            {
                if (dim == Dimension.D2)
                    mould = Mould.create_by_rectangle(template_rx_ry_rz.rx, template_rx_ry_rz.ry, m);
                if (dim == Dimension.D3)
                    mould = Mould.create_by_rectangle(template_rx_ry_rz.rx, template_rx_ry_rz.ry, template_rx_ry_rz.rz, m);
                Patterns pats = Patterns.create(mould, train_image);//提取模式
                if (pats.Count > 0)
                {
                    pats_mg.Add(m, (mould, pats));
                    Console.WriteLine(pats.Count);
                }
            }

        }

        /// <summary>
        /// 创建simpat对象
        /// </summary>
        /// <param name="random_seed"></param>
        /// <param name="multi_grid"></param>
        /// <param name="template_rx_ry_rz"></param>
        /// <param name="train_image"></param>
        /// <param name="gs_re"></param>
        /// <param name="N"></param>
        /// <returns></returns>
        public static Simpat create(int random_seed, int multi_grid, (int, int, int) template_rx_ry_rz, GridProperty train_image, CData cd, GridStructure gs_re, int N)
        {
            Simpat simpat = new()
            {
                random_seed = random_seed,
                multi_grid = multi_grid,
                train_image = train_image,
                gs_re = gs_re,
                N = N,
                template_rx_ry_rz = template_rx_ry_rz,
                ccd = cd != null ? CoarsenedCData.create(gs_re, cd).ccd : null
            };
            simpat.init();
            return simpat;
        }

        public Grid run(int index)
        {
            random_seed += index;

            Stopwatch sw = new();

            Grid g = Grid.create(gs_re);

            for (int m = multi_grid; m >= 1; m--)//模拟第m重网格
            {
                sw.Start();

                var mould_m = pats_mg[m].mould;//第m重网格的模板
                var pats_m = pats_mg[m].patterns;//第m重网格的样式库
                //第m重网格的随机路径
                SimulationPath path_m = SimulationPath.create(gs_re, m, new Random(random_seed));


                //将ccd赋值到第m重网格节点上
                g.add_gridProperty($"re_{m}");//第m重网格的模拟实现
                g.add_gridProperty($"hd_{m}");//第m重网格的硬数据

                if (g.ContainsKey($"re_{m + 1}"))//如果有上一层粗网格，则将粗网格模拟结果赋值到本层网格
                {
                    for (int n = 0; n < gs_re.N; n++)
                    {
                        g[$"re_{m}"].set_value(n, g[$"re_{m + 1}"].get_value(n));
                    }
                }

                if (ccd != null)//导入了条件数据，则需要采用条件数据约束
                {
                    List<SpatialIndex> locs_m = path_m.spatialIndexes;//获取第m重路径的所有节点

                    //逐个条件数据点赋值到第m重网格上，具体操作是将硬数据赋值到最近的多重网格节点上
                    foreach (var (array_index, ccdi) in ccd)
                    {
                        var loc_cd = gs_re.get_spatial_index(array_index);
                        SpatialIndex loc_nearest = null;
                        var min_dist = float.MaxValue;
                        foreach (var loc_m in locs_m)
                        {
                            var distance = SpatialIndex.calc_dist(loc_m, loc_cd);
                            if (distance <= min_dist)
                            {
                                min_dist = distance;
                                loc_nearest = loc_m;//记录离硬数据最近的点
                            }
                        }
                        g[$"hd_{m}"].set_value(loc_nearest, ccdi[ccd.PropertyNames[0]]);
                        g[$"re_{m}"].set_value(loc_nearest, ccdi[ccd.PropertyNames[0]]);
                    }
                }


                //模拟第m重网格的非硬数据点
                while (true != path_m.is_visit_over())
                {
                    MyConsoleProgress.Print(path_m.progress, $"{m}");
                    var si_m = path_m.visit_next();//访问网格点
                    //数据事件
                    MouldInstance dataEvent_m = MouldInstance.create_from_gridProperty(mould_m, si_m, g[$"re_{m}"]);
                    //硬数据事件(仅包含硬数据)
                    MouldInstance hd_m = MouldInstance.create_from_gridProperty(mould_m, si_m, g[$"hd_{m}"]);
                    //最佳匹配模式，满足两个条件（1）如果有硬数据，则必须完全匹配硬数据，（2）相似度最大
                    MouldInstance bestPat_m;

                    //dataEvent_m为空，则hd_m必然也为空
                    if (dataEvent_m.neighbor_not_nulls_ids.Count == 0)
                        bestPat_m = pats_m[pats_m.random_select(new Random(random_seed))];
                    else//数据事件有数据值
                    {
                        ConcurrentBag<(int index, float distance)> temp = [];
                        //没有发现硬数据
                        if (hd_m.neighbor_not_nulls_ids.Count == 0)
                        {
                            Parallel.For(0, pats_m.Count, i =>
                            {
                                var distance = Mould.get_distance(dataEvent_m, pats_m.get_by_index(i));
                                temp.Add((i, distance));
                            });
                        }
                        else//如果有硬数据，pat必须完全匹配硬数据
                        {
                            for (int i = 0; i < pats_m.Count; i++)
                            {
                                bool is_match_all_hd = true;
                                var indexes_is_not_null = hd_m.neighbor_not_nulls_ids;
                                foreach (var item in indexes_is_not_null)
                                {
                                    if (pats_m.get_by_index(i)[item] != hd_m[item])
                                    {
                                        is_match_all_hd = false;//只要有一个hd不匹配，该样式就不能用
                                    }
                                }
                                if (is_match_all_hd)//当hd全部匹配
                                {
                                    var distance = Mould.get_distance(dataEvent_m, pats_m.get_by_index(i));
                                    temp.Add((i, distance));
                                }
                            }
                            if (temp.Count == 0)
                            {
                                Console.WriteLine(@"有hd，但是没有一个pat匹配所有hd节点");
                            }
                        }
                        var ordered = temp.OrderBy(a => a.distance).ToList();

                        bestPat_m = pats_m.get_by_index(ordered[0].index);
                    }
                    var neighbors = bestPat_m.paste_to_gridProperty(mould_m, si_m, g[$"re_{m}"]);
                    path_m.freeze(neighbors);//冻结样式所有节点，后面路径不访问，但是可以改变值
                }

                #region 检查模拟re是否匹配hd

                //检查模拟re是否匹配hd
                int N_not_match = 0;
                for (int n = 0; n < gs_re.N; n++)
                {
                    if (g[$"hd_{m}"] != null && g[$"hd_{m}"] == g[$"re_{m}"])
                    {
                        N_not_match++;
                    }
                }
                MyConsoleHelper.write_string_to_console("不匹配数量", N_not_match.ToString());

                //g.showGrid_win();
                //return null;

                #endregion

                sw.Stop();
                Console.WriteLine(sw.ElapsedMilliseconds);
            }


            #region 检查模拟re是否匹配hd

            if (ccd != null)
            {   //检查模拟re是否匹配hd
                int N_not_match_result = 0;
                //逐个条件数据点赋值到第m重网格上，具体操作是将硬数据赋值到最近的多重网格节点上
                foreach (var (array_index, ccdi) in ccd)
                {
                    var value_cd = ccdi[ccd.PropertyNames[0]];
                    if (value_cd != g[$"re_{1}"].get_value(array_index))
                    {
                        N_not_match_result++;
                    }
                }
                MyConsoleHelper.write_string_to_console("不匹配数量", N_not_match_result.ToString());
            }

            #endregion

            return g;
        }
    }
}
