using System;
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

        public Grid run(int random_seed, int multigrid, int max_number, (int rx, int ry, int rz) template, GridProperty TI, CData cd, GridStructure gs_model)
        {
            var cd1 = cd.deep_clone();
            for (int multi_grid = multigrid; multi_grid >= 1; multi_grid--)
            {
                var mould = gs_model.dim == Dimension.D2 ?
                    Mould.create_by_ellipse(template.rx, template.ry, multi_grid) :
                    Mould.create_by_ellipse(template.rx, template.ry, template.rz, multi_grid);
                mould = Mould.create_by_mould(mould, max_number);
                var (re_mg, time_) = run(TI, cd1, gs_model, random_seed, mould, multi_grid);
                re_mg.showGrid_win();
                cd1 = CData.create_from_gridProperty(re_mg, "模型", null, false);
                MyConsoleHelper.write_string_to_console("时间", time_.ToString());
                if (multi_grid == 1)
                    return re_mg;
            }
            return null;
        }

        public (Grid re, double time) run(GridProperty ti, CData cd, GridStructure gs_re, int seed,
            Mould mould, int multi_grid = 1, int progress_for_retrieve_inverse = 0)
        {
            Random rnd = new(seed);
            Grid g = Grid.create(gs_re);//根据gs_model创建grid工区

            //把cd赋值到模型中
            if (cd == null)
                g.add_gridProperty("模型");
            //把cd赋值到模型中
            else
                g.add_gridProperty("模型", cd.assign_to_grid(gs_re).grid_assigned[0]);

            STree tree = STree.create(mould, ti);
            if (tree == null)
                return (null, 0.0);

            Dictionary<float?, int> nod_cut = [];
            Dictionary<float?, float> pdf = [];//全局相概率
            Dictionary<float?, float> cpdf = [];//条件约束相概率
            List<float?> categories = [];//离散变量的取值范围

            var category_freq = ti.discrete_category_freq(false);
            for (int i = 0; i < category_freq.Count; i++)
            {
                nod_cut.Add(category_freq[i].value, 0);
                pdf.Add(category_freq[i].value, category_freq[i].freq);
                categories.Add(category_freq[i].value);
            }

            path = SimulationPath.create(gs_re, multi_grid, rnd);

            Stopwatch sw = new();//只记录模拟时间（不包括构建搜索树）
            sw.Start();
            while (path.is_visit_over() == false)
            {
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
                if (path.progress % 20 == 0)
                    g["模型"].deep_clone().show_win($"{path.progress}");
            }
            sw.Stop();
            tree.df.show_win("访问节点总数", true);
            return (g, sw.ElapsedMilliseconds);
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
