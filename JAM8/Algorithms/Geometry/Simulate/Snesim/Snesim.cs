﻿using System;
using System.Diagnostics;
using JAM8.Algorithms.Numerics;
using JAM8.Utilities;

namespace JAM8.Algorithms.Geometry
{
    public class Snesim
    {
        //int cd_max;//条件数据最大数量
        //float servo_system;//校正系数

        public SimulationPath path { get; internal set; }

        private Snesim() { }

        public (Grid, double time) run(int random_seed, int multigrid_count, int max_number,
            (int rx, int ry, int rz) template, GridProperty TI, CData cd, GridStructure gs_re,
            int progress_for_retrieve_inverse = 0)
        {
            Stopwatch sw = new();//模拟时间
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

        public (Grid re, double time) run(GridProperty TI, CData cd, GridStructure gs_re, int random_seed,
            Mould mould, int multi_grid = 1, int progress_for_retrieve_inverse = 0)
        {
            Random rnd = new(random_seed);
            Grid g = Grid.create(gs_re);//根据gs_model创建grid工区

            //把cd赋值到模型中
            if (cd == null)
                g.add_gridProperty("模型");
            //把cd赋值到模型中
            else
                g.add_gridProperty("模型", cd.assign_to_grid(gs_re).grid_assigned[0]);

            STree tree = STree.create(mould, TI);
            if (tree == null)
                return (null, 0.0);

            Dictionary<float?, int> nod_cut = [];
            Dictionary<float?, float> pdf = [];//全局相概率
            Dictionary<float?, float> cpdf = [];//条件约束相概率
            List<float?> categories = [];//离散变量的取值范围

            var category_freq = TI.discrete_category_freq(false);
            for (int i = 0; i < category_freq.Count; i++)
            {
                nod_cut.Add(category_freq[i].value, 0);
                pdf.Add(category_freq[i].value, category_freq[i].freq);
                categories.Add(category_freq[i].value);
            }

            path = SimulationPath.create(gs_re, multi_grid, rnd);

            Stopwatch sw = new();//只记录模拟时间（不包括构建搜索树）
            sw.Start();

            MyDataFrame df_time = MyDataFrame.create(["progress", "ElapsedMilliseconds", "totalElapsedTime"]);
            // 累计时长变量（单位：毫秒）
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
            if (dataEvent.neighbor_not_nulls_ids.Count != 0)//有条件数据的情况,从搜索树取回条件数据的cpdf
            {
                Dictionary<float?, int> core_values;
                if (progress <= progress_for_retrieve_inverse)
                    core_values = tree.retrieve_inverse(dataEvent, 1);
                else
                    core_values = tree.retrieve(dataEvent, 1);
                if (core_values != null)//有取回重复数，计算条件概率
                {
                    int sumrepl = 0;//重复数总数
                    sumrepl = core_values.Sum(a => a.Value);//重复数满足条件，则跳出
                    foreach (var category in tree.categories)//计算cpdf
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
