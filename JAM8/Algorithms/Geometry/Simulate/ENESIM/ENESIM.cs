using System.Collections.Concurrent;
using System.Diagnostics;
using JAM8.Algorithms.Numerics;
using JAM8.Utilities;

namespace JAM8.Algorithms.Geometry
{
    public class ENESIM
    {
        private ENESIM()
        {
        }

        private Grid model { get; set; }
        private GridProperty ti { get; set; }

        public static ENESIM create(Grid sim_grid, GridProperty ti)
        {
            ENESIM ds = new();
            ds.model = sim_grid;
            ds.ti = ti;

            return ds;
        }

        public Grid run(int search_radius = 20, int maximum_number = 10)
        {
            MersenneTwister mt = new(1111); //随机数生成器
            Stopwatch sw = new();
            sw.Start();

            GridStructure gs_re = model.gridStructure; //模拟实现尺寸
            Grid g_result = Grid.create(gs_re); //模拟网格

            //用于扫描模拟网格的模板
            Mould scan_re_mould = gs_re.dim == Dimension.D2
                ? Mould.create_by_ellipse(search_radius, search_radius, 1)
                : Mould.create_by_ellipse(search_radius, search_radius, 2, 1);

            //随机访问模拟网格的索引
            var visit_re_idxes = MyShuffleHelper.fisher_yates_shuffle(MyGenerator.range(0, gs_re.N), mt).shuffled; //乱序

            //进度
            int progress = 0;

            g_result.add_gridProperty($"re", model["re"].deep_clone());

            MyConsoleHelper.write_string_to_console($"{progress}", DateTime.Now.ToString());

            Dictionary<float?, float> pdf = []; //边缘概率（训练图像中各种相类型的比例）

            List<float?> categories = []; //离散变量的取值范围

            var category_freq = ti.discrete_category_freq(false);
            for (int i = 0; i < category_freq.Count; i++)
            {
                pdf.Add(category_freq[i].value, category_freq[i].freq);
                categories.Add(category_freq[i].value);
            }

            foreach (var model_random_idx in visit_re_idxes) //随机访问模拟网格的所有节点
            {
                progress++;
                MyConsoleProgress.Print(progress, visit_re_idxes.Count, "ENESIM");

                //下一个待模拟点
                SpatialIndex visit_re_next_si = gs_re.get_spatial_index(model_random_idx);

                //如果某个点没有数据，则需要插值
                if (g_result["re"].get_value(visit_re_next_si) == null)
                {
                    //根据模板获取当前模拟位置的模板实例
                    var scan_re_mi =
                        MouldInstance.create_from_gridProperty(scan_re_mould, visit_re_next_si, g_result["re"]);

                    //如果模板实例没有任何条件数据，则从训练图像里随机抽取一个值赋给模拟网格（ds相似度的做法，后期可能改成snesim的概率抽样）
                    if (scan_re_mi.neighbor_not_nulls_ids.Count == 0)
                    {
                        var ti_array_index = mt.Next(0, ti.grid_structure.N);
                        var select_value = ti.get_value(ti_array_index);
                        g_result["re"].set_value(model_random_idx, select_value);
                    }
                    else
                    {
                        List<float?> neighbor_values = [.. scan_re_mi.neighbor_values];
                        int count = 0; // 记录已保留的非null数量
                        //根据设定最大条件点数，对数据事件的模板实例进行处理，只保留要求的最大数量的条件点，超出范围的设置为null
                        for (int i = 0; i < scan_re_mi.mould.neighbors_number; i++)
                        {
                            if (scan_re_mi.neighbor_values[i].HasValue) //实际cd数少于最大给定数，则跳过
                            {
                                count++;
                                if (count > maximum_number)
                                    neighbor_values[i] = null; // 超过K个的非null元素置为null
                            }
                        }

                        scan_re_mi.update(scan_re_mi.core_value, scan_re_mi.core_arrayIndex, [.. neighbor_values]);

                        //条件约束相概率
                        var cpdf = get_cpdf_parallel(scan_re_mi, ti, categories);

                        //抽样
                        var value = cdf_sampler.sample(cpdf, (float)mt.NextDouble());

                        //赋值
                        g_result["re"].set_value(model_random_idx, value);
                    }
                }
            }

            return g_result;
        }

        /// <summary>
        /// 用来自模拟网格的数据事件，创建扫描训练图像的模板。然后用该模板扫描训练图像，
        /// 要求实现一次扫描，将由近到远的条件点重复情况都记录下来
        /// </summary>
        /// <param name="scan_re_mi">数据事件</param>
        /// <param name="ti">训练图像</param>
        /// <param name="categories"></param>
        /// <returns></returns>
        private Dictionary<float?, float> get_cpdf(MouldInstance scan_re_mi, GridProperty ti, List<float?> categories)
        {
            //根据数据事件创建扫描训练图像的模板
            var trim_scan_re_mi = scan_re_mi.trim(CompareType.NotEqual, null);


            //<条件数据的排序,<取值,重复数>>
            Dictionary<int, Dictionary<float?, int>> temp_repls = [];
            //初始化
            for (int i = 0; i < trim_scan_re_mi.mould.neighbors_number; i++)
            {
                Dictionary<float?, int> repls = [];
                foreach (var category in categories)
                {
                    repls.Add(category, 0);
                }

                temp_repls.Add(i, repls);
            }

            //新建一个用于扫描训练图像的模板实例（后续以更新方式获取训练图像数据）
            MouldInstance scan_ti_mi = MouldInstance.create(trim_scan_re_mi.mould);


            //串行扫描训练图像
            for (int visit_ti_idx = 0; visit_ti_idx < ti.grid_structure.N; visit_ti_idx++)
            {
                //扫描训练图像的下个位置
                var visit_ti_next_si = ti.grid_structure.get_spatial_index(visit_ti_idx);

                //扫描训练图像，获取模板实例
                scan_ti_mi.update_from_gridProperty(visit_ti_next_si, ti);

                //只计算充满条件数据的模板实例
                if (scan_ti_mi.neighbor_nulls_ids.Count == 0)
                {
                    //由近到远，判断条件点是否相同
                    for (int i = 0; i < scan_ti_mi.neighbor_values.Count; i++)
                    {
                        //判断第i个条件数据是否相等，相同则记录加1，否则后续也无需判断
                        if (scan_ti_mi.neighbor_values[i] == trim_scan_re_mi.neighbor_values[i])
                            temp_repls[i][scan_ti_mi.core_value]++;
                        else
                            break;
                    }
                }
            }

            //返回重复数之和大于CMin的情况
            int CMin = 1;
            var cpdf = new Dictionary<float?, float>();
            for (int i = trim_scan_re_mi.mould.neighbors_number - 1; i >= 0; i--)
            {
                int sum_repls = 0; //重复数总数
                sum_repls = temp_repls[i].Sum(a => a.Value); //重复数满足条件，则跳出
                if (sum_repls >= CMin)
                {
                    foreach (var category in categories) //计算cpdf
                    {
                        cpdf.Add(category, temp_repls[i][category] / (float)sum_repls);
                    }

                    return cpdf;
                }
            }

            //没有满足条件的情况，返回null，当然一般不会发生
            return null;
        }

        /// <summary>
        /// 并行扫描训练图像，获取条件概率密度函数（CPDF）。
        /// 效果明显，速度提升明显。
        /// </summary>
        /// <param name="scan_re_mi"></param>
        /// <param name="ti"></param>
        /// <param name="categories"></param>
        /// <returns></returns>
        private Dictionary<float?, float> get_cpdf_parallel(MouldInstance scan_re_mi, GridProperty ti,
            List<float?> categories)
        {
            var trim_scan_re_mi = scan_re_mi.trim(CompareType.NotEqual, null);
            int neighborsCount = trim_scan_re_mi.mould.neighbors_number;

            // 使用数组代替字典，减少锁竞争
            var tempCounts = new int[neighborsCount, categories.Count];

            // 创建类别索引映射
            var categoryIndices = categories
                .Select((c, i) => new { Category = c, Index = i })
                .ToDictionary(x => x.Category, x => x.Index);

            // 并行扫描训练图像
            Parallel.For(0, ti.grid_structure.N, () =>
                {
                    // 每个线程有自己的模板实例和本地计数器
                    return new
                    {
                        mi = MouldInstance.create(trim_scan_re_mi.mould),
                        localCounts = new int[neighborsCount, categories.Count]
                    };
                },
                (visit_ti_idx, loopState, localData) =>
                {
                    // 扫描训练图像的下个位置
                    var visit_ti_next_si = ti.grid_structure.get_spatial_index(visit_ti_idx);

                    // 更新模板实例
                    localData.mi.update_from_gridProperty(visit_ti_next_si, ti);

                    // 只处理完全填充的模板实例
                    if (localData.mi.neighbor_nulls_ids.Count == 0)
                    {
                        for (int i = 0; i < localData.mi.neighbor_values.Count; i++)
                        {
                            if (localData.mi.neighbor_values[i] == trim_scan_re_mi.neighbor_values[i])
                            {
                                if (categoryIndices.TryGetValue(localData.mi.core_value, out int catIndex))
                                {
                                    localData.localCounts[i, catIndex]++;
                                }
                            }
                            else
                            {
                                break;
                            }
                        }
                    }

                    return localData;
                },
                localData =>
                {
                    // 合并线程本地计数到全局结果
                    lock (tempCounts)
                    {
                        for (int i = 0; i < neighborsCount; i++)
                        {
                            for (int j = 0; j < categories.Count; j++)
                            {
                                tempCounts[i, j] += localData.localCounts[i, j];
                            }
                        }
                    }
                });

            // 查找满足条件的结果
            int CMin = 1;
            for (int i = neighborsCount - 1; i >= 0; i--)
            {
                int sum = 0;
                for (int j = 0; j < categories.Count; j++)
                {
                    sum += tempCounts[i, j];
                }

                if (sum >= CMin)
                {
                    var cpdf = new Dictionary<float?, float>();
                    for (int j = 0; j < categories.Count; j++)
                    {
                        cpdf.Add(categories[j], tempCounts[i, j] / (float)sum);
                    }

                    return cpdf;
                }
            }

            return null;
        }
    }
}