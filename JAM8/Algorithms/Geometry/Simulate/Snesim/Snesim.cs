using System;
using System.Diagnostics;
using JAM8.Algorithms.Numerics;
using JAM8.Utilities;

namespace JAM8.Algorithms.Geometry
{
    public class Snesim
    {
        //int cd_max;//Maximum number of conditional data 条件数据最大数量
        //float servo_system;//correction factor 校正系数

        //simulation path 模拟路径
        public SimulationPath path { get; internal set; }

        private Snesim() { }

        /// <summary>
        /// snesim的多重网格模拟方法
        /// SNESIM's multi-grid simulation method
        /// </summary>
        /// <param name="random_seed">随机种子
        /// Random seed
        /// </param>
        /// <param name="multigrid_count">多重网格总数
        /// Total number of multi-grids
        /// </param>
        /// <param name="max_number">实际样板的节点总数
        /// Total number of nodes in the actual template
        /// </param>
        /// <param name="template">样板
        /// Template
        /// </param>
        /// <param name="TI">训练图像
        /// Training image
        /// </param>
        /// <param name="cd">条件数据
        /// Conditional data
        /// </param>
        /// <param name="gs_re">模拟网格结构尺寸
        /// Size of the simulation grid structure
        /// </param>
        /// <param name="progress_for_retrieve_inverse">模拟进度中，反向查询在前段模拟进度的比例，默认为0
        /// In the simulation progress, the proportion of reverse query in the previous simulation progress, default is 0
        /// </param>
        /// <returns></returns>
        public (Grid, double time) run(int random_seed, int multigrid_count, int max_number,
            (int rx, int ry, int rz) template, GridProperty TI, CData cd, GridStructure gs_re,
            int progress_for_retrieve_inverse = 0)
        {
            Stopwatch sw = new();//检测模拟时间
            sw.Start();

            var cd1 = cd?.deep_clone();
            for (int multi_grid = multigrid_count; multi_grid >= 1; multi_grid--)
            {
                var mould = gs_re.dim == Dimension.D2 ?
                    Mould.create_by_ellipse(template.rx, template.ry, multi_grid) :
                    Mould.create_by_ellipse(template.rx, template.ry, template.rz, multi_grid);
                mould = Mould.create_by_mould(mould, max_number);
                var (re_mg, time_) = run(TI, cd1, gs_re, random_seed, mould, multi_grid, progress_for_retrieve_inverse);
                re_mg.showGrid_win();
                cd1 = CData.create_from_gridProperty(re_mg, "模型", null, false);
                MyConsoleHelper.write_string_to_console("时间", time_.ToString());
                if (multi_grid == 1)
                {
                    sw.Stop();
                    return (re_mg, sw.ElapsedMilliseconds);
                }
            }

            return (null, 0);
        }

        /// <summary>
        /// 模拟指定网格级别的节点
        /// Simulate the nodes of the specified grid
        /// </summary>
        /// <param name="TI">训练图像
        /// Training image
        /// </param>
        /// <param name="cd">条件数据
        /// Conditional data
        /// </param>
        /// <param name="gs_re">模拟网格结构
        /// Simulation grid structure
        /// </param>
        /// <param name="random_seed">随机种子
        /// Random seed
        /// </param>
        /// <param name="mould">模拟样板
        /// Simulation template
        /// </param>
        /// <param name="multi_grid">多重网格级别，默认为1
        /// Multi-grid level, default is 1
        /// </param>
        /// <param name="progress_for_retrieve_inverse">模拟进度中，反向查询在前段模拟进度的比例，默认为0
        /// In the simulation progress, the proportion of reverse query in the previous simulation progress, default is 0
        /// </param>
        /// <returns></returns>
        public (Grid re, double time) run(GridProperty TI, CData cd, GridStructure gs_re, int random_seed,
            Mould mould, int multi_grid = 1, int progress_for_retrieve_inverse = 0)
        {
            Random rnd = new(random_seed);
            Grid g = Grid.create(gs_re);//Create a grid based on the gs_re. 根据gs_re创建grid工区 

            //Assign the value of cd to the model. 把cd赋值到模型中
            if (cd == null)
                g.add_gridProperty("模型");
            else
                g.add_gridProperty("模型", cd.assign_to_grid(gs_re).grid_assigned[0]);

            STree tree = STree.create(mould, TI);
            if (tree == null)
                return (null, 0.0);

            Dictionary<float?, int> nod_cut = [];
            Dictionary<float?, float> pdf = [];//global facies probability 全局相概率
            Dictionary<float?, float> cpdf = [];//conditional constraint probability 条件约束相概率
            List<float?> categories = [];//The value range of discrete variables 离散变量的取值范围

            var category_freq = TI.discrete_category_freq(false);
            for (int i = 0; i < category_freq.Count; i++)
            {
                nod_cut.Add(category_freq[i].value, 0);
                pdf.Add(category_freq[i].value, category_freq[i].freq);
                categories.Add(category_freq[i].value);
            }

            path = SimulationPath.create(gs_re, multi_grid, rnd);

            //Only the simulation time is recorded (excluding building the search tree).
            //只记录模拟时间（不包括构建搜索树）
            Stopwatch sw = new();
            sw.Start();

            MyDataFrame df_time = MyDataFrame.create(["progress", "ElapsedMilliseconds", "totalElapsedTime"]);
            //累计时长变量（单位：毫秒）
            long totalElapsedTime = 0;
            //避免重复进度
            double progress_preview = -1;
            while (path.is_visit_over() == false)
            {
                if (path.progress % 1 == 0 && path.progress != progress_preview)
                {
                    progress_preview = path.progress;
                    sw.Stop();
                    // 将 tick 转换为毫秒
                    double elapsedMicroseconds = (sw.ElapsedTicks / (double)Stopwatch.Frequency) * 1_000;
                    totalElapsedTime += (long)elapsedMicroseconds;  // 累加时长
                    df_time.add_record([path.progress, elapsedMicroseconds, totalElapsedTime]);
                    sw.Restart();
                }
                MyConsoleProgress.Print(path.progress, "snesim");
                var si = path.visit_next();
                var value_si = g["模型"].get_value(si);
                if (value_si == null)
                {
                    var dataEvent = MouldInstance.create_from_gridProperty(mould, si, g["模型"]);
                    cpdf = get_cpdf(dataEvent, tree, path.progress, progress_for_retrieve_inverse);
                    cpdf ??= pdf;
                    var value = cdf_sampler.sample(cpdf, (float)rnd.NextDouble());
                    g["模型"].set_value(si, value);
                    nod_cut[value]++;
                }
            }
            sw.Stop();
            return (g, totalElapsedTime);
        }

        Dictionary<float?, float> get_cpdf(MouldInstance dataEvent, STree tree, double progress,
            int progress_for_retrieve_inverse = 0)
        {
            var cpdf = new Dictionary<float?, float>();

            //In the case of conditional data, a cpdf of the conditional data is
            //retrieved from the search tree.
            //有条件数据的情况,从搜索树取回条件数据的cpdf
            if (dataEvent.neighbor_not_nulls_ids.Count != 0)
            {
                Dictionary<float?, int> core_values;
                if (progress <= progress_for_retrieve_inverse)
                    core_values = tree.retrieve_inverse(dataEvent, 1);
                else
                    core_values = tree.retrieve(dataEvent, 1);

                //There is the number of retrieved duplicates, and the conditional probability
                //is calculated
                //有取回重复数，计算条件概率
                if (core_values != null)
                {
                    int sumrepl = 0;//Total number of repetitions 重复数总数
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
