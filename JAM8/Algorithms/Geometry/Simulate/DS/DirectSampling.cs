using System.Collections.Concurrent;
using System.Diagnostics;
using System.Security.Policy;
using JAM8.Utilities;

namespace JAM8.Algorithms.Geometry
{
    /// <summary>
    /// 直接抽样法
    /// </summary>
    public class DirectSampling
    {
        private DirectSampling()
        {
        }

        //realizations
        private Grid realizations { get; set; }

        //training image
        private GridProperty ti { get; set; }

        /// <summary>
        /// 条件数据的属性名称，例如"岩相类型"
        /// </summary>
        public string propertyName { get; internal set; }

        /// <summary>
        /// 创建ds对象
        /// </summary>
        /// <param name="ti"></param>
        /// <param name="cd"></param>
        /// <param name="gs"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static DirectSampling create(GridStructure gs, GridProperty ti, CData cd = null,
            string propertyName = null)
        {
            DirectSampling ds = new();

            if (cd != null)
            {
                var gp_cd = cd.coarsened(gs).coarsened_grid[propertyName];
                ds.realizations = Grid.create(gs);
                ds.realizations.add_gridProperty("cd", gp_cd); //将cd赋值给grid
                ds.realizations.add_gridProperty("re", gp_cd.deep_clone()); //将cd赋值给grid
            }
            else
            {
                ds.realizations = Grid.create(gs);
                ds.realizations.add_gridProperty("re"); //将cd赋值给grid
            }

            ds.ti = ti;

            return ds;
        }

        public Grid run(int search_radius = 10, int maximum_number = 30,
            double maximum_fraction = 0.3, double distance_threshold = 0.01, int random_seed = 123123)
        {
            MersenneTwister mt = new((uint)random_seed);

            Stopwatch sw = new();
            sw.Start();

            GridStructure gs_model = realizations.gridStructure;

            //扫描模型的模板
            Mould mo_scan_model = gs_model.dim == Dimension.D2
                ? Mould.create_by_ellipse(search_radius, search_radius, 1)
                : Mould.create_by_ellipse(search_radius, search_radius, 2, 1);

            var range1 = MyGenerator.range(0, gs_model.N);
            var model_random_idxes = MyShuffleHelper.fisher_yates_shuffle(range1, mt).shuffled; //乱序

            var range2 = MyGenerator.range(0, ti.grid_structure.N); //训练图像
            var ti_random_idxes = MyShuffleHelper.fisher_yates_shuffle(range2, mt).shuffled; //乱序

            int progress = 0;
            Grid state = Grid.create(gs_model);

            state.add_gridProperty($"{progress}", realizations["re"].deep_clone());
            MyConsoleHelper.write_string_to_console($"{progress}", DateTime.Now.ToString());
            foreach (var model_random_idx in model_random_idxes) //计算工区网格的所有节点
            {
                progress++;
                if (progress % 1000 == 0)
                {
                    //state.add_gridProperty($"{progress}", model["re"].deep_clone());
                    MyConsoleHelper.write_string_to_console($"{progress}", DateTime.Now.ToString());
                }

                SpatialIndex si_model = gs_model.get_spatial_index(model_random_idx); //下一个待模拟点

                if (realizations["re"].get_value(si_model) == null) //如果某个点没有数据，则需要插值
                {
                    //MyConsoleProgress.Print(progress, gs_model.N, "DS");
                    var mi_model = MouldInstance.create_from_gridProperty(mo_scan_model, si_model,
                        realizations["re"]); //根据模板mo获取si位置的模板实例mi
                    List<SpatialIndex> sis_not_null = []; //根据模板实例mi，提取有值的部分
                    List<float?> data = []; //不包含core
                    for (int i = 0; i < maximum_number; i++) //设置最大条件点数
                    {
                        if (i >= mi_model.neighbor_not_nulls_ids.Count) //实际cd数少于最大给定数，则跳过
                            break;
                        int idx_not_null = mi_model.neighbor_not_nulls_ids[i];
                        sis_not_null.Add(mo_scan_model.neighbor_spiral_mapper[idx_not_null].spatial_index);
                        data.Add(mi_model[idx_not_null]);
                    }

                    if (data.Count == 0)
                    {
                        var ti_array_index = mt.Next(0, ti.grid_structure.N);
                        var select_value = ti.get_value(ti_array_index);
                        realizations["re"].set_value(model_random_idx, select_value);
                        //Console.WriteLine(model_random_idx + " no_cd " + select_value);
                        continue;
                    }


                    //扫描ti的模板
                    SpatialIndex core_ti = gs_model.dim == Dimension.D2
                        ? SpatialIndex.create(0, 0)
                        : SpatialIndex.create(0, 0, 0);
                    Mould mo_scan_ti = Mould.create_by_location(core_ti, sis_not_null);
                    double min_dist = double.MaxValue;
                    float? best_value = null;

                    #region 并行

                    // int M = (int)(maximum_fraction * ti_random_idxes.Count);
                    //
                    //
                    // var bag = new ConcurrentBag<(double hsim, float? value)>();
                    //
                    // //从ti_random_idxes中随机抽取M个索引进行并行计算
                    // var ti_random_idxes_subset = ti_random_idxes.Take(M).ToList();
                    //
                    // Parallel.ForEach(ti_random_idxes_subset, ti_random_idx =>
                    // {
                    //     var si_ti = ti.grid_structure.get_spatial_index(ti_random_idx);
                    //     var mi_ti = MouldInstance.create_from_gridProperty(mo_scan_ti, si_ti, ti);
                    //     if (mi_ti.neighbor_not_nulls_ids.Count == mi_ti.mould.neighbors_number)
                    //     {
                    //         double hsim = calc_hsim(mi_ti.neighbor_values, data);
                    //         float? value = ti.get_value(si_ti);
                    //
                    //         bag.Add((hsim, value)); // 放入线程安全容器
                    //     }
                    // });
                    // // 选出 hsim 最大的项（即相似度最高的）
                    // var best = bag.MaxBy(x => x.hsim);
                    //
                    // min_dist = best.hsim;
                    // best_value = best.value;

                    #endregion

                    #region 并行

                    // int M = (int)(maximum_fraction * ti_random_idxes.Count);
                    // var bag = new ConcurrentBag<(double hsim, float? value)>();
                    //
                    // // 从 ti_random_idxes 中随机抽取前 M 个索引
                    // var ti_random_idxes_subset = ti_random_idxes.Take(M).ToList();
                    //
                    // Parallel.ForEach(ti_random_idxes_subset, ti_random_idx =>
                    // {
                    //     var si_ti = ti.grid_structure.get_spatial_index(ti_random_idx);
                    //
                    //     // 不再构造 MouldInstance，直接尝试提取邻居值和中心值
                    //     if (Mould.TryGetNeighborValues(mo_scan_ti, si_ti, ti, out var neighbor_values,
                    //             out var center_value))
                    //     {
                    //         double hsim = calc_hsim(neighbor_values, data);
                    //         bag.Add((hsim, center_value)); // 放入线程安全容器
                    //     }
                    // });
                    //
                    // // 选出相似度最高的项（hsim 最大）
                    // if (!bag.IsEmpty)
                    // {
                    //     var best = bag.MaxBy(x => x.hsim);
                    //     min_dist = best.hsim;
                    //     best_value = best.value;
                    // }
                    // else
                    // {
                    //     // 若 bag 全为空，说明没有找到匹配，设置默认值
                    //     min_dist = double.MaxValue;
                    //     best_value = null;
                    // }

                    #endregion

                    #region 串行代码

                    // int M = (int)(maximum_fraction * ti_random_idxes.Count);
                    // int m = 0;
                    // MouldInstance mi_ti = MouldInstance.create(mo_scan_ti);
                    // foreach (var ti_random_idx in ti_random_idxes)
                    // {
                    //     m++;
                    //     if (m == M)
                    //         break;
                    //     var si_ti = ti.grid_structure.get_spatial_index(ti_random_idx);
                    //     mi_ti.update_from_gridProperty(si_ti, ti);
                    //     if (mi_ti.neighbor_not_nulls_ids.Count == mi_ti.mould.neighbors_number)
                    //     {
                    //         var hsim = 1 - calc_hsim(mi_ti.neighbor_values, data);
                    //         //正确代码
                    //         if (hsim < min_dist)
                    //         {
                    //             min_dist = hsim;
                    //             best_value = ti.get_value(si_ti);
                    //         }
                    //
                    //         if (min_dist < distance_threshold)
                    //             break;
                    //     }
                    // }

                    int M = (int)(maximum_fraction * ti_random_idxes.Count);
                    int m = 0;
                    
                    foreach (var ti_random_idx in ti_random_idxes)
                    {
                        m++;
                        if (m == M)
                            break;
                    
                        var si_ti = ti.grid_structure.get_spatial_index(ti_random_idx);
                    
                        if (Mould.TryGetNeighborValues(mo_scan_ti, si_ti, ti, out var neighbor_values,
                                out var center_value))
                        {
                            var hsim = 1 - calc_hsim(neighbor_values, data);
                    
                            if (hsim < min_dist)
                            {
                                min_dist = hsim;
                                best_value = center_value;
                            }
                    
                            if (min_dist < distance_threshold)
                                break;
                        }
                    }

                    #endregion

                    realizations["re"].set_value(model_random_idx, best_value);
                    //Console.WriteLine(model_random_idx + " hsim:" + min_dist + " " + best_value);
                }
            }

            sw.Stop();
            Console.WriteLine("运行时间:" + sw.ElapsedMilliseconds / 1000.0 + "秒");
            state.showGrid_win();
            return realizations;
        }

        public static double calc_hsim(IList<float?> vector1, IList<float?> vector2)
        {
            double _d = 0; //测度值

            //点对点计算两个矢量之间的测度
            for (int i = 0; i < vector1.Count; i++)
                _d += 1.0 / (1.0 + Math.Abs(vector1[i].Value - vector2[i].Value));
            double _s = _d / vector1.Count; //转换称为相似度
            return _s;
        }
    }
}