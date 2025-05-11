using System.Diagnostics;
using System.Security.Policy;
using JAM8.Algorithms.Numerics;
using JAM8.Utilities;

namespace JAM8.Algorithms.Geometry
{
    /// <summary>
    /// 直接抽样法
    /// </summary>
    public class DirectSampling
    {
        private DirectSampling() { }

        //GridProperty model { get; set; }
        private Grid model { get; set; }

        private GridProperty ti { get; set; }

        /// <summary>
        /// 条件数据的属性名称，例如"岩相类型"
        /// </summary>
        public string propertyName { get; internal set; }

        /// <summary>
        /// 创建ds对象
        /// </summary>
        /// <param name="vm"></param>
        /// <param name="cd"></param>
        /// <param name="gs"></param>
        /// <returns></returns>
        public static DirectSampling create(GridStructure gs, GridProperty ti, CData cd = null, string propertyName = null)
        {
            DirectSampling ds = new();

            if (cd != null)
            {
                var gp_cd = cd.assign_to_grid(gs).grid_assigned[propertyName];
                ds.model = Grid.create(gs);
                ds.model.add_gridProperty("cd", gp_cd);//将cd赋值给grid
                ds.model.add_gridProperty("re", gp_cd.deep_clone());//将cd赋值给grid
            }
            else
            {
                ds.model = Grid.create(gs);
                ds.model.add_gridProperty("re");//将cd赋值给grid
            }

            ds.ti = ti;

            return ds;
        }

        public Grid run(int search_radius = 10,
            int maximum_number = 30,
            double maximum_fraction = 0.3,
            double distance_threshold = 0.01, int random_seed = 123123)
        {
            MersenneTwister mt = new(1111);

            Stopwatch sw = new();
            sw.Start();

            GridStructure gs_model = model.gridStructure;

            Mould mo_scan_model = gs_model.dim == Dimension.D2 ?//扫描模型的模板
                Mould.create_by_ellipse(search_radius, search_radius, 1) :
                Mould.create_by_ellipse(search_radius, search_radius, 2, 1);

            var range1 = MyGenerator.range(0, gs_model.N - 1, 1, true);
            var model_random_idxes = SortHelper.FisherYatesShuffle(range1, mt);//乱序

            var range2 = MyGenerator.range(0, ti.grid_structure.N - 1, 1, true);//训练图像
            var ti_random_idxes = SortHelper.FisherYatesShuffle(range2, mt);//乱序

            int progress = 0;
            Grid state = Grid.create(gs_model);

            state.add_gridProperty($"{progress}", model["re"].deep_clone());
            MyConsoleHelper.write_string_to_console($"{progress}", DateTime.Now.ToString());
            foreach (var model_random_idx in model_random_idxes)//计算工区网格的所有节点
            {
                progress++;
                if (progress % 1000 == 0)
                {
                    //state.add_gridProperty($"{progress}", model["re"].deep_clone());
                    MyConsoleHelper.write_string_to_console($"{progress}", DateTime.Now.ToString());
                }
                SpatialIndex si_model = gs_model.get_spatial_index(model_random_idx);//下一个待模拟点

                if (model["re"].get_value(si_model) == null)//如果某个点没有数据，则需要插值
                {
                    //MyConsoleProgress.Print(progress, gs_model.N, "DS");
                    var mi_model = MouldInstance.create_from_gridProperty(mo_scan_model, si_model, model["re"]);//根据模板mo获取si位置的模板实例mi
                    List<SpatialIndex> sis_not_null = [];//根据模板实例mi，提取有值的部分
                    List<float?> data = [];//不包含core
                    for (int i = 0; i < maximum_number; i++)//设置最大条件点数
                    {
                        if (i >= mi_model.neighbor_not_nulls_ids.Count)//实际cd数少于最大给定数，则跳过
                            break;
                        int idx_not_null = mi_model.neighbor_not_nulls_ids[i];
                        sis_not_null.Add(mo_scan_model.neighbor_spiral_mapper[idx_not_null].spatial_index);
                        data.Add(mi_model[idx_not_null]);
                    }
                    if (data.Count == 0)
                    {
                        var ti_array_index = mt.Next(0, ti.grid_structure.N);
                        var select_value = ti.get_value(ti_array_index);
                        model["re"].set_value(model_random_idx, select_value);
                        //Console.WriteLine(model_random_idx + " no_cd " + select_value);
                        continue;
                    }


                    //扫描ti的模板
                    SpatialIndex core_ti = gs_model.dim == Dimension.D2 ?
                        SpatialIndex.create(0, 0) : SpatialIndex.create(0, 0, 0);
                    Mould mo_scan_ti = Mould.create_by_location(core_ti, sis_not_null);
                    double min_dist = double.MaxValue;
                    float? best_value = null;

                    #region 并行

                    //int M = (int)(maximum_fraction * ti_random_idxes.Count);
                    //int m = 0;
                    ////扫描ti
                    //Parallel.For(0, ti_random_idxes.Count, i =>
                    //{
                    //    m++;
                    //    if (m == M)
                    //        return;
                    //    var si_ti = ti.gridStructure.get_spatialIndex(ti_random_idxes[i]);
                    //    MouldInstance mi_ti = MouldInstance.create_from_gridProperty(mo_scan_ti, si_ti, ti);

                    //    if (mi_ti.is_full)
                    //    {
                    //        var dist = 1 - calc_hsim(mi_ti.buffer[1..], data.ToArray());
                    //        if (dist < min_dist)
                    //        {
                    //            min_dist = dist;
                    //            best_value = ti.get_value(si_ti);
                    //        }
                    //        if (min_dist < distance_threshold)
                    //            return;
                    //    }
                    //});

                    #endregion

                    #region 串行代码

                    int M = (int)(maximum_fraction * ti_random_idxes.Count);
                    int m = 0;
                    MouldInstance mi_ti = MouldInstance.create(mo_scan_ti);
                    foreach (var ti_random_idx in ti_random_idxes)
                    {
                        m++;
                        if (m == M)
                            break;
                        var si_ti = ti.grid_structure.get_spatial_index(ti_random_idx);
                        mi_ti.update_from_gridProperty(si_ti, ti);
                        if (mi_ti.neighbor_not_nulls_ids.Count == mi_ti.mould.neighbors_number)
                        {
                            var hsim = 1 - calc_hsim(mi_ti.neighbor_values, data);
                            //正确代码
                            if (hsim < min_dist)
                            {
                                min_dist = hsim;
                                best_value = ti.get_value(si_ti);
                            }
                            if (min_dist < distance_threshold)
                                break;
                        }
                    }

                    #endregion

                    model["re"].set_value(model_random_idx, best_value);
                    //Console.WriteLine(model_random_idx + " hsim:" + min_dist + " " + best_value);
                }
            }
            sw.Stop();
            Console.WriteLine("运行时间:" + sw.ElapsedMilliseconds / 1000.0 + "秒");
            state.showGrid_win();
            return model;
        }

        public static double calc_hsim(float?[] vector1, float?[] vector2)
        {
            double _d = 0;//测度值

            //点对点计算两个矢量之间的测度
            for (int i = 0; i < vector1.Length; i++)
                _d += 1.0 / (1.0 + Math.Abs(vector1[i].Value - vector2[i].Value));
            double _s = _d / vector1.Length;//转换称为相似度
            return _s;
        }

        public static double calc_hsim(List<float?> vector1, List<float?> vector2)
        {
            double _d = 0;//测度值

            //点对点计算两个矢量之间的测度
            for (int i = 0; i < vector1.Count; i++)
                _d += 1.0 / (1.0 + Math.Abs(vector1[i].Value - vector2[i].Value));
            double _s = _d / vector1.Count;//转换称为相似度
            return _s;
        }
    }
}
