using System;
using System.Diagnostics;
using JAM8.Algorithms.Numerics;
using JAM8.Utilities;

namespace JAM8.Algorithms.Geometry
{
    public class Snesim
    {
        int global_progress = 0; //全局进度(多重网格使用)

        //int cd_max;//Maximum number of conditional data 条件数据最大数量
        //float servo_system;//correction factor 校正系数

        //simulation path 模拟路径
        public SimulationPath path { get; internal set; }

        private Snesim()
        {
        }

        /// <summary>
        /// Snesim's multi-grid simulation method
        /// Snesim的多重网格模拟方法
        /// </summary>
        /// <param name="random_seed">Random seed
        /// </param>
        /// <param name="multigrid_count">Total number of multi-grids
        /// </param>
        /// <param name="max_number">Total number of nodes in the actual template
        /// </param>
        /// <param name="template">Template
        /// </param>
        /// <param name="TI">Training image
        /// </param>
        /// <param name="cd">Conditional data
        /// </param>
        /// <param name="gs_re">Size of the simulation grid structure
        /// </param>
        /// <param name="progress_for_retrieve_inverse">In the simulation progress, the proportion of reverse query 
        /// in the previous simulation progress, default is 0
        /// </param>
        /// <returns></returns>
        public (Grid, double time) simulate_multigrid(int random_seed, int multigrid_count, int max_number,
            (int rx, int ry, int rz) template, GridProperty TI, CData cd, GridStructure gs_re,
            int progress_for_retrieve_inverse = 0)
        {
            Stopwatch sw = new(); //检测模拟时间
            sw.Start();

            Grid g = Grid.create(gs_re);

            var current_cd = cd?.deep_clone();

            if (current_cd != null)
            {
                var (coarsened_cd, coarsened_grid) = current_cd.coarsened(gs_re);
                g.add_gridProperty("cd", coarsened_grid.first_gridProperty());
            }

            for (int multi_grid = multigrid_count; multi_grid >= 1; multi_grid--)
            {
                var mould = gs_re.dim == Dimension.D2
                    ? Mould.create_by_ellipse(template.rx, template.ry, multi_grid)
                    : Mould.create_by_ellipse(template.rx, template.ry, template.rz, multi_grid);

                mould = Mould.create_by_front_section(mould, max_number);

                var (re_mg, time_) = simulate_single_grid(TI, current_cd, gs_re, random_seed, mould, multi_grid,
                    progress_for_retrieve_inverse);

                g.add_gridProperty($"{multi_grid}", re_mg[0]);

                current_cd = CData.create_from_gridProperty(re_mg[0], "re", CompareType.NotEqual, null);

                MyConsoleHelper.write_string_to_console("时间", time_.ToString());

                if (multi_grid == 1)
                {
                    sw.Stop();
                    g.showGrid_win();
                    return (re_mg, sw.ElapsedMilliseconds);
                }
            }

            return (null, 0);
        }

        /// <summary>
        /// Simulate the nodes of the specified grid
        /// 模拟指定网格级别的节点
        /// </summary>
        /// <param name="TI">Training image
        /// </param>
        /// <param name="cd">Conditional data
        /// </param>
        /// <param name="gs_re">Simulation grid structure
        /// </param>
        /// <param name="random_seed">Random seed
        /// </param>
        /// <param name="mould">Simulation template
        /// </param>
        /// <param name="multi_grid">Multi-grid level, default is 1
        /// </param>
        /// <param name="progress_for_retrieve_inverse">In the simulation progress, the proportion of reverse query 
        /// in the previous simulation progress, default is 0
        /// </param>
        /// <returns></returns>
        public (Grid re, double time) simulate_single_grid(GridProperty TI, CData cd, GridStructure gs_re, int random_seed, Mould mould,
            int multigrid_level = 1, int progress_for_retrieve_inverse = 0)
        {
            MersenneTwister mt = new((uint)random_seed);

            Grid result = Grid.create(gs_re); //Create a grid based on the gs_re. 根据gs_re创建grid工区 

            //Assign the value of cd to the model. 把cd赋值到模型中
            if (cd != null)
            {
                var (coarsened_cd, coarsened_grid) = cd.coarsened(gs_re);
                result.add_gridProperty("re", coarsened_grid.first_gridProperty());
            }
            else
            {
                result.add_gridProperty("re");
            }

            STree tree = STree.create(mould, TI);
            if (tree == null)
                return (null, 0.0);

            Dictionary<int, int> nod_cut = [];
            Dictionary<int, double> pdf = []; //global facies probability 全局相概率
            Dictionary<int, double> cpdf = []; //conditional constraint probability 条件约束相概率
            List<float?> categories = []; //The value range of discrete variables 离散变量的取值范围

            var category_freq = TI.discrete_category_freq(false);
            for (int i = 0; i < category_freq.Count; i++)
            {
                nod_cut.Add((int)category_freq[i].value, 0);
                pdf.Add((int)category_freq[i].value, category_freq[i].freq);
                categories.Add(category_freq[i].value);
            }

            path = SimulationPath.create(gs_re, multigrid_level, mt);

            //Only the simulation time is recorded (excluding building the search tree).
            Stopwatch sw = new();
            sw.Start();

            MyDataFrame df_time = MyDataFrame.create(["progress", "ElapsedMilliseconds", "totalElapsedTime"]);
            long totalElapsedTime = 0;
            double progress_preview = -1;
            while (path.is_visit_over() == false)
            {
                global_progress++; //全局进度(多重网格使用)
                if (global_progress == (int)(gs_re.N * 0.2))
                {
                    // result.showGrid_win("20%");
                }

                if (path.progress % 1 == 0 && path.progress != progress_preview)
                {
                    progress_preview = path.progress;
                    sw.Stop();
                    // 将 tick 转换为毫秒
                    double elapsedMicroseconds = (sw.ElapsedTicks / (double)Stopwatch.Frequency) * 1_000;
                    totalElapsedTime += (long)elapsedMicroseconds; // 累加时长
                    df_time.add_record([path.progress, elapsedMicroseconds, totalElapsedTime]);
                    sw.Restart();
                }

                MyConsoleProgress.Print(path.progress, $"snesim multigrid_level{multigrid_level}");
                var si = path.visit_next();
                if (gs_re.get_array_index(si) == 6219)
                {
                    MyConsoleHelper.write_string_to_console("发现异常");
                }

                var value_si = result["re"].get_value(si);
                if (value_si == null)
                {
                    var dataEvent = MouldInstance.create_from_gridProperty(mould, si, result["re"]);
                    cpdf = get_cpdf(dataEvent, tree, path.progress, progress_for_retrieve_inverse);
                    cpdf ??= pdf;
                    var value = SamplingHelper.sample<int>(cpdf.Select(kv => (kv.Key, kv.Value)), mt.NextDouble());
                    result["re"].set_value(si, value);
                    // nod_cut[value]++;
                }
            }

            sw.Stop();
            return (result, totalElapsedTime);
        }

        private Dictionary<int, double> get_cpdf(MouldInstance dataEvent, STree tree, double progress,
            int progress_for_retrieve_inverse = 0)
        {
            var cpdf = new Dictionary<int, double>();

            //In the case of conditional data, a cpdf of the conditional data is
            //retrieved from the search tree.
            //有条件数据的情况,从搜索树取回条件数据的cpdf
            if (dataEvent.neighbor_not_nulls_ids.Count != 0)
            {
                Dictionary<int, int> core_values;
                if (progress <= progress_for_retrieve_inverse)
                    core_values = tree.retrieve_inverse(dataEvent, 1);
                else
                    core_values = tree.retrieve(dataEvent, 1);
                // core_values = tree.retrieve_recursive(dataEvent, 1);

                //There is the number of retrieved duplicates, and the conditional probability
                //is calculated
                //有取回重复数，计算条件概率
                if (core_values != null)
                {
                    int sumrepl = 0; //Total number of repetitions 重复数总数
                    sumrepl = core_values.Sum(a => a.Value);

                    //Calculate cpdf
                    foreach (var category in tree.categories)
                    {
                        cpdf.Add(category, core_values[category] / (float)sumrepl);
                    }

                    return cpdf;
                }
            }

            return null;
        }

        public static Snesim create()
        {
            return new Snesim();
        }
    }
}