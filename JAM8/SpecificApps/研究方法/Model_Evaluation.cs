using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Easy.Common.Extensions;
using JAM8.Algorithms;
using JAM8.Algorithms.Geometry;
using JAM8.Algorithms.Numerics;
using JAM8.Utilities;

namespace JAM8.SpecificApps.研究方法
{
    public class Model_Evaluation
    {
        public static void cd在随机实现中表现为噪点的比例_批量计算()
        {
            OpenFileDialog ofd = new();
            ofd.Multiselect = true;
            if (ofd.ShowDialog() != DialogResult.OK)
                return;
            var file_names = ofd.FileNames;
            GridStructure gs = GridStructure.create_win();

            MyDataFrame df = MyDataFrame.create(new string[] { "file_name", "噪点数" });
            foreach (var file_name in file_names)
            {
                Grid g = Grid.create(gs);
                g.read_from_gslib(file_name, 1, -99);
                //g.showGrid_win();

                int All = 0;
                int N_is_噪点 = 0;
                Mould m = Mould.create_by_rectangle(1, 1, 1);
                for (int n = 0; n < g.gridStructure.N; n++)
                {
                    if (g["cd"].get_value(n) != null)
                    {
                        All++;
                        SpatialIndex si_core = g.gridStructure.get_spatial_index(n);
                        var mi = MouldInstance.create_from_gridProperty(m, si_core, g["re"]);
                        int count_不相同 = 0;
                        for (int i = 1; i < mi.neighbor_values.Count; i++)//统计不相同的节点总数
                        {
                            if (mi[0] != mi[i])
                            {
                                count_不相同++;
                            }
                        }
                        if (count_不相同 == mi.neighbor_values.Count - 1)
                            N_is_噪点++;
                    }
                }

                var record = df.new_record(new object[] { FileHelper.GetFileName(file_name, false), N_is_噪点 });
                df.add_record(record);
                Console.WriteLine();
                Console.WriteLine($@"cd节点总数=[{All}]     cd节点表现为噪点数=[{N_is_噪点}]");
            }
            df.show_win();
        }

        public static void cd在随机实现中表现为噪点的比例()
        {
            Console.WriteLine(@"输入[1]查看demo数据; 输入[2]打开文件");
            string input = Console.ReadLine();
            if (input == "1")
            {
                string embeddedFilePath = "JAM8.资源文件.A(ti)_A(cd)_re.out";
                string txt = MyEmbeddedFileHelper.read_embedded_txt(embeddedFilePath);
                Form_showTxt frm = new(txt);
                frm.Show();
            }
            if (input == "2")
            {
                Grid g = Grid.create_from_gslibwin().grid;
                g.showGrid_win();

                int All = 0;
                int N_is_噪点 = 0;
                Mould m = Mould.create_by_rectangle(1, 1, 1);
                for (int n = 0; n < g.gridStructure.N; n++)
                {
                    if (g["cd"].get_value(n) != null)
                    {
                        All++;
                        SpatialIndex si_core = g.gridStructure.get_spatial_index(n);
                        var mi = MouldInstance.create_from_gridProperty(m, si_core, g["re"]);
                        int count_不相同 = 0;
                        for (int i = 1; i < mi.neighbor_values.Count; i++)//统计不相同的节点总数
                        {
                            if (mi[0] != mi[i])
                            {
                                count_不相同++;
                            }
                        }
                        if (count_不相同 == mi.neighbor_values.Count - 1)
                            N_is_噪点++;
                    }
                }
                Console.WriteLine();
                Console.WriteLine($@"cd节点总数=[{All}]");
                Console.WriteLine($@"cd节点表现为噪点数=[{N_is_噪点}]");
            }
            else
                Console.WriteLine(@"输入错误");

        }

        public static void 随机实现与cd匹配率()
        {
            Console.WriteLine(@"输入[1]查看demo数据; 输入[2]打开文件");
            string input = Console.ReadLine();
            if (input == "1")
            {
                string embeddedFilePath = "JAM8.资源文件.A(ti)_A(cd)_re.out";
                string txt = MyEmbeddedFileHelper.read_embedded_txt(embeddedFilePath);
                Form_showTxt frm = new(txt);
                frm.Show();
            }
            if (input == "2")
            {
                Grid g = Grid.create_from_gslibwin().grid;
                g.showGrid_win();

                int All = 0;
                int matched = 0;
                for (int n = 0; n < g.gridStructure.N; n++)
                {
                    if (g["cd"].get_value(n) != null)
                    {
                        All++;
                        if (g["cd"].get_value(n) == g["re"].get_value(n))
                        {
                            matched++;
                        }
                    }
                }
                Console.WriteLine();
                Console.WriteLine($@"cd节点总数=[{All}]");
                Console.WriteLine($@"cd节点匹配数=[{matched}]");
            }
            else
                Console.WriteLine(@"输入错误");
        }

        public static void 随机实现与TI的实验变差函数比较()
        {
            var (gp_ti, name_ti) = Grid.create_from_gslibwin("打开TI文件")
                .grid
                .select_gridProperty_win("选择TI");

            var g_re = Grid.create_from_gslibwin("打开Re文件").grid;

            int N_lag = gp_ti.grid_structure.nx / 3;

            List<double> lags_ti = new();
            lags_ti.AddRange(Variogram.calc_variogram_from_grid_2d(gp_ti, 0, N_lag, 1).gamma);
            lags_ti.AddRange(Variogram.calc_variogram_from_grid_2d(gp_ti, 45, N_lag, 1).gamma);
            lags_ti.AddRange(Variogram.calc_variogram_from_grid_2d(gp_ti, 90, N_lag, 1).gamma);
            lags_ti.AddRange(Variogram.calc_variogram_from_grid_2d(gp_ti, 135, N_lag, 1).gamma);

            List<double> differences = new();
            for (int i = 0; i < g_re.propertyNames.Count; i++)
            {
                var gp_re = g_re[i];
                List<double> lags_re = new();
                lags_re.AddRange(Variogram.calc_variogram_from_grid_2d(gp_re, 0, N_lag, 1).gamma);
                lags_re.AddRange(Variogram.calc_variogram_from_grid_2d(gp_re, 45, N_lag, 1).gamma);
                lags_re.AddRange(Variogram.calc_variogram_from_grid_2d(gp_re, 90, N_lag, 1).gamma);
                lags_re.AddRange(Variogram.calc_variogram_from_grid_2d(gp_re, 135, N_lag, 1).gamma);

                differences.Add(MyDistance.calc_hsim(lags_ti.ToArray(), lags_re.ToArray()));
            }
            Console.WriteLine($@"differences={differences.Average()}");



        }

        public static void 随机实现与TI的理论变差函数比较()
        {
            var (gp_ti, name_ti) = Grid.create_from_gslibwin("打开TI文件")
                .grid
                .select_gridProperty_win("选择TI");

            int N_lag = gp_ti.grid_structure.nx / 3;

            List<double> lags_ti = new();

            var (h_0, gamma_0, N_0) = Variogram.calc_variogram_from_grid_2d(gp_ti, 0, N_lag, 1);
            var (fit_0, _) = Variogram.variogramFit(VariogramType.Spherical, h_0, gamma_0, N_0);
            lags_ti.Add(fit_0.range);
            //lags_ti.Add(fit_0.sill);
            //lags_ti.Add(fit_0.nugget);

            var (h_45, gamma_45, N_45) = Variogram.calc_variogram_from_grid_2d(gp_ti, 45, N_lag, 1);
            var (fit_45, _) = Variogram.variogramFit(VariogramType.Spherical, h_45, gamma_45, N_45);
            lags_ti.Add(fit_45.range);
            //lags_ti.Add(fit_45.sill);
            //lags_ti.Add(fit_45.nugget);

            var (h_90, gamma_90, N_90) = Variogram.calc_variogram_from_grid_2d(gp_ti, 90, N_lag, 1);
            var (fit_90, _) = Variogram.variogramFit(VariogramType.Spherical, h_90, gamma_90, N_90);
            lags_ti.Add(fit_90.range);
            //lags_ti.Add(fit_90.sill);
            //lags_ti.Add(fit_90.nugget);

            var (h_135, gamma_135, N_135) = Variogram.calc_variogram_from_grid_2d(gp_ti, 135, N_lag, 1);
            var (fit_135, _) = Variogram.variogramFit(VariogramType.Spherical, h_135, gamma_135, N_135);
            lags_ti.Add(fit_135.range);
            //lags_ti.Add(fit_135.sill);
            //lags_ti.Add(fit_135.nugget);

            MyDataFrame df = MyDataFrame.create(new string[] { "realization_name", "hsim" });
            while (true)
            {
                string option = MyConsoleHelper.read_string_from_console("是否退出", "Y(y):退出;其他:继续");
                if (option.ToLower() == "y")
                    break;

                var (g_re, file_name) = Grid.create_from_gslibwin("打开Re文件");

                List<double> differences = new();
                for (int i = 0; i < g_re.propertyNames.Count; i++)
                {
                    var gp_re = g_re[i];
                    List<double> lags_re = new();

                    (h_0, gamma_0, N_0) = Variogram.calc_variogram_from_grid_2d(gp_re, 0, N_lag, 1);
                    (fit_0, _) = Variogram.variogramFit(VariogramType.Spherical, h_0, gamma_0, N_0);
                    lags_re.Add(fit_0.range);
                    //lags_re.Add(fit_0.sill);
                    //lags_re.Add(fit_0.nugget);

                    (h_45, gamma_45, N_45) = Variogram.calc_variogram_from_grid_2d(gp_re, 45, N_lag, 1);
                    (fit_45, _) = Variogram.variogramFit(VariogramType.Spherical, h_45, gamma_45, N_45);
                    lags_re.Add(fit_45.range);
                    //lags_re.Add(fit_45.sill);
                    //lags_re.Add(fit_45.nugget);

                    (h_90, gamma_90, N_90) = Variogram.calc_variogram_from_grid_2d(gp_re, 90, N_lag, 1);
                    (fit_90, _) = Variogram.variogramFit(VariogramType.Spherical, h_90, gamma_90, N_90);
                    lags_re.Add(fit_90.range);
                    //lags_re.Add(fit_90.sill);
                    //lags_re.Add(fit_90.nugget);

                    (h_135, gamma_135, N_135) = Variogram.calc_variogram_from_grid_2d(gp_re, 135, N_lag, 1);
                    (fit_135, _) = Variogram.variogramFit(VariogramType.Spherical, h_135, gamma_135, N_135);
                    lags_re.Add(fit_135.range);
                    //lags_re.Add(fit_135.sill);
                    //lags_re.Add(fit_135.nugget);

                    differences.Add(MyDistance.calc_hsim(lags_ti.ToArray(), lags_re.ToArray()));
                }
                Console.WriteLine($@"differences={differences.Average()}");

                var record = df.new_record();
                record["realization_name"] = FileHelper.GetFileName(file_name, false);
                record["hsim"] = differences.Average();
                df.add_record(record);
            }
            df.show_win();
        }

        /// <summary>
        /// 根据模式平均熵，比较随机实现与训练图像差异性
        /// </summary>
        public static void Re_TI_entropy_of_pattern_size()
        {
            //1 打开TI
            var ti = Grid.create_from_gslibwin().grid.select_gridProperty_win().grid_property;
            var entropy_ti = get_entropy_of_pattern_size(ti);

            List<double> hsim = new();

            //2 打开基于TI的Re集合
            var re_list = Grid.create_from_gslibwin().grid;
            foreach (var (gp_name, gp) in re_list)
            {
                MyConsoleHelper.write_string_to_console($"********************{gp_name}*****************");
                var entropy_gp = get_entropy_of_pattern_size(gp);

                var vector_ti = entropy_ti.Select(a => a.entropy).ToList();
                var vector_gp = entropy_gp.Select(a => a.entropy).ToList();

                hsim.Add(MyDistance.calc_hsim(vector_ti, vector_gp));
            }

            var hsim_mean = hsim.Average();
            MyConsoleHelper.write_string_to_console(hsim_mean.ToString());
        }

        #region 模型评价1:计算单个模型的模式平均熵

        /// <summary>
        /// 计算单个模型（不同尺寸模式分解方式）的平均熵序列
        /// </summary>
        /// <returns></returns>
        public static List<(int radius, double entropy)> get_entropy_of_pattern_size(GridProperty gp)
        {
            List<(int radius, double entropy)> result = new();
            List<int> radius_list = MyGenerator.range(1, 20, 1);
            for (int i = 0; i < radius_list.Count; i++)
            {
                int radius = radius_list[i];
                var mould = Mould.create_by_ellipse(radius, radius, 1);
                var pats = Patterns.create(mould, gp);
                double entropy = entropy_pattern(pats);

                result.Add((radius, entropy));
            }
            return result;

            //根据Pattern模式库计算模型Model（某个模板尺寸分解）的平均熵
            static double entropy_pattern(Patterns pats)
            {
                double entropy_mean = 0;
                foreach (var (_, mouldInstance) in pats)
                {
                    double entropy = 0.0;
                    var distinct = mouldInstance.neighbor_values.Distinct().ToList();
                    Dictionary<double?, int> temp = new();
                    for (int i = 0; i < distinct.Count; i++)
                    {
                        temp.Add(distinct[i], 0);
                    }

                    for (int i = 0; i < mouldInstance.mould.neighbors_number; i++)
                    {
                        double? value = mouldInstance[i];
                        for (int j = 0; j < distinct.Count; j++)
                        {
                            double? key = temp.Keys.ToList()[j];
                            if (value == key)
                            {
                                temp[key]++;
                            }
                        }
                    }
                    for (int i = 0; i < distinct.Count; i++)
                    {
                        double? key = temp.Keys.ToList()[i];
                        int n = temp[key];
                        double p = (double)n / mouldInstance.mould.neighbors_number;//概率
                        entropy += -p * Math.Log(p, 2);//典型熵的定义的底数为2
                    }
                    entropy_mean += entropy;
                }
                entropy_mean /= pats.Count;//取均值
                return entropy_mean;
            }
        }

        #endregion

    }
}
