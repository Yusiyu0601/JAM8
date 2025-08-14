using System.Collections.Concurrent;
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
        private DirectSampling()
        {
        }

        /// <summary>
        /// 使用 Direct Sampling 方法对 re 网格进行模拟填充（单变量）
        /// </summary>
        /// <param name="re">待模拟的目标属性网格（null 表示需模拟）</param>
        /// <param name="ti">训练图像属性网格（完整的模式参考）</param>
        /// <param name="search_radius">扫描模板的半径（控制邻居空间范围）</param>
        /// <param name="maximum_number">最大可用邻居点数</param>
        /// <param name="maximum_fraction">用于匹配的 TI 中最多采样比例（避免全搜索）</param>
        /// <param name="distance_threshold">相似度阈值（用于早停）</param>
        /// <param name="random_seed">随机种子，控制可重复性</param>
        /// <returns>填充后的目标属性网格（re）</returns>
        public static GridProperty run(GridProperty re, GridProperty ti, int search_radius = 10,
            int maximum_number = 30, double maximum_fraction = 0.3, double distance_threshold = 0.01,
            int random_seed = 123123)
        {
            double calc_hsim(IList<float?> vector1, IList<float?> vector2)
            {
                double _d = 0; //测度值

                //点对点计算两个矢量之间的测度
                for (int i = 0; i < vector1.Count; i++)
                    _d += 1.0 / (1.0 + Math.Abs(vector1[i].Value - vector2[i].Value));
                double _s = _d / vector1.Count; //转换称为相似度
                return _s;
            }

            //获取建模的维度
            var dim = re.grid_structure.dim;
            //随机数生成器
            MersenneTwister mt = new((uint)random_seed);

            Stopwatch sw = new();
            sw.Start();

            //根据维度创建扫描模型的模板
            Mould mo_scan_re = dim == Dimension.D2
                ? Mould.create_by_ellipse(search_radius, search_radius, 1)
                : Mould.create_by_ellipse(search_radius, search_radius, 2, 1);

            var range1 = MyGenerator.range(0, re.grid_structure.N); //模拟工区网格
            var re_random_idxes = MyShuffleHelper.fisher_yates_shuffle(range1, mt).shuffled; //乱序

            var range2 = MyGenerator.range(0, ti.grid_structure.N); //训练图像
            var ti_random_idxes = MyShuffleHelper.fisher_yates_shuffle(range2, mt).shuffled; //乱序

            int progress = 0;

            MyConsoleHelper.write_string_to_console($"{progress}", DateTime.Now.ToString());

            foreach (var re_random_idx in re_random_idxes) //计算工区网格的所有节点
            {
                progress++;
                MyConsoleProgress.Print(progress, re.grid_structure.N, "DS");

                SpatialIndex si_model = re.grid_structure.get_spatial_index(re_random_idx); //下一个待模拟点

                //如果某个点没有数据，则需要插值
                if (re.get_value(si_model) == null)
                {
                    //根据模板mo获取si位置的模板实例mi
                    var mi_model = MouldInstance.create_from_gridProperty(mo_scan_re, si_model, re);

                    //根据模板实例mi，提取有值的部分
                    List<SpatialIndex> sis_not_null = [];

                    //不包含core
                    List<float?> data = [];

                    //设置最大条件点数
                    for (int i = 0; i < maximum_number; i++)
                    {
                        //实际cd数少于最大给定数，则跳过
                        if (i >= mi_model.neighbor_not_nulls_ids.Count)
                            break;
                        int idx_not_null = mi_model.neighbor_not_nulls_ids[i];
                        sis_not_null.Add(mo_scan_re.neighbor_spiral_mapper[idx_not_null].spatial_index);
                        data.Add(mi_model[idx_not_null]);
                    }

                    if (data.Count == 0)
                    {
                        var ti_array_index = mt.Next(0, ti.grid_structure.N);
                        var select_value = ti.get_value(ti_array_index);
                        re.set_value(re_random_idx, select_value);
                        continue;
                    }


                    SpatialIndex core_ti = SpatialIndex.create_in_origin(dim);

                    //扫描ti的模板
                    Mould mo_scan_ti = Mould.create_by_location(core_ti, sis_not_null);

                    double min_dist = double.MaxValue;

                    float? best_value = null;

                    #region 串行代码

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

                    re.set_value(re_random_idx, best_value);
                }
            }

            sw.Stop();
            Console.WriteLine("运行时间:" + sw.ElapsedMilliseconds / 1000.0 + "秒");
            return re;
        }

        /// <summary>
        /// 基于多分辨率的 Direct Sampling 模拟方法（允许每层设置不同参数）
        /// </summary>
        /// <param name="re">模拟目标属性网格</param>
        /// <param name="ti">训练图像属性网格</param>
        /// <param name="ds_parameters">
        /// 每层的模拟参数元组列表，格式为：
        /// (search_radius, maximum_number, maximum_fraction, distance_threshold)
        /// 最粗层在 index 0，最细层在最后
        /// </param>
        /// <param name="random_seed">随机种子</param>
        /// <returns>模拟后的高分辨率 GridProperty</returns>
        public static GridProperty run_multiresolution(
            GridProperty re, GridProperty ti,
            List<(int radius, int max_number, double max_fraction, double threshold)> ds_parameters,
            int random_seed = 123123)
        {
            int N_pyramid = ds_parameters.Count;
            int factor = 2;

            // 构建 TI 金字塔
            List<GridProperty> ti_pyramid = [];
            GridProperty current_ti = ti;
            for (int i = 0; i < N_pyramid - 1; i++)
            {
                current_ti = current_ti.pyramid_downsample_smooth(factor, factor, factor);
                ti_pyramid.Insert(0, current_ti); // 最粗在最前
            }
            ti_pyramid.Add(ti); // 最细在最后

            // 构建 RE 金字塔
            List<GridProperty> re_pyramid = [];
            GridProperty current_re = re;
            for (int i = 0; i < N_pyramid - 1; i++)
            {
                current_re = current_re.resize_nearest_by_scale(1.0 / factor, 1.0 / factor, 1.0 / factor);
                re_pyramid.Insert(0, current_re);
            }
            re_pyramid.Add(re);

            // 多分辨率模拟
            for (int i = 0; i < N_pyramid; i++)
            {
                var (radius, max_num, max_frac, threshold) = ds_parameters[i];

                if (i == 0)
                {
                    // 最粗层：无条件数据
                    current_re = DirectSampling.run(
                        re_pyramid[i], ti_pyramid[i],
                        radius, max_num, max_frac, threshold, random_seed);
                }
                else
                {
                    // 上一层插值作为当前层的条件
                    current_ti = ti_pyramid[i];
                    current_re = re_pyramid[i - 1].pyramid_upsample_sparse(factor, factor);
                    current_re = DirectSampling.run(
                        current_re, current_ti,
                        radius, max_num, max_frac, threshold, random_seed);
                }

                re_pyramid[i] = current_re;
                current_re.deep_clone().show_win($"{i}"); // 可选调试展示
            }

            return re_pyramid.Last(); // 返回最高分辨率结果
        }

    }
}