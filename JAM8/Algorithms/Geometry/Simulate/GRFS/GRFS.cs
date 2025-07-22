using System.Diagnostics;
using System.Net.Http.Headers;
using Easy.Common.Extensions;
using JAM8.Algorithms.Numerics;
using JAM8.Utilities;

namespace JAM8.Algorithms.Geometry
{
    /// <summary>
    /// 高斯随机场，基于ok实现条件化
    /// </summary>
    public class GRFS
    {
        private GRFS()
        {
        }

        public GridStructure gs { get; internal set; }
        public Variogram vm { get; internal set; }
        public CData cd { get; internal set; }

        /// <summary>
        /// 条件数据的属性名称，例如"孔隙度"
        /// </summary>
        public string property_name { get; internal set; }

        /// <summary>
        /// 创建GRFS对象
        /// </summary>
        /// <param name="vm"></param>
        /// <param name="cd"></param>
        /// <param name="gs"></param>
        /// <returns></returns>
        public static GRFS create(GridStructure gs, Variogram vm, CData cd, string property_name)
        {
            GRFS grfs = new()
            {
                gs = gs,
                vm = vm,
                cd = cd,
                property_name = property_name
            };
            return grfs;
        }

        /// <summary>
        /// 基于ok与高斯随机场的随机模拟，返回模型与计算时间(毫秒)
        /// </summary>
        /// <param name="search_radius"></param>
        /// <param name="k_cdi"></param>
        /// <param name="random_seed"></param>
        /// <returns></returns>
        public (Grid result, long milliseconds) run(int search_radius, double[] rot_mat, int k_cdi,
            int random_seed = 12312)
        {
            Stopwatch sw = new();
            sw.Start();

            Grid g = Grid.create(gs); //工区网格

            //条件数据网格化
            g.add_gridProperty("cd_assign_to_grid", cd.coarsened(gs).coarsened_grid[property_name]);

            //计算条件数据的分位数
            Quantile quantile_cd =
                Quantile.create(g["cd_assign_to_grid"].buffer.Where(a => a != null).Select(a => (double)a).ToList());

            //生成高斯随机场
            g.add_gridProperty("grf",
                gs.dim == Dimension.D2
                    ? FFT_MA.fft_move_average_2d(gs, 10, 10, random_seed).gp
                    : FFT_MA.fft_move_average_3d_parallel(gs, 20, 20, 5, random_seed).gp);

            //对高斯随机场数据进行排序
            var data_grf = g["grf"].buffer.OrderBy(a => a).Select(a => (double)a).ToList();
            //由于数量过大，从原始数据里提取一部分
            var indx = MyGenerator.linespace(0, data_grf.Count - 1, 500);
            List<double> data_grf_sampling = [];
            foreach (var item in indx)
                data_grf_sampling.Add(data_grf[(int)item]);
            //计算高斯随机场的分位数
            Quantile quantile_grf = Quantile.create(data_grf_sampling);

            //正态得分变换，将grf映射到cd的分布
            g.add_gridProperty("grf_transformed");
            for (int n = 0; n < gs.N; n++)
            {
                var value_grf = g["grf"].get_value(n);
                if (value_grf != null)
                {
                    var p_grf = quantile_grf.get_cumulativeProbabilities(value_grf.Value);
                    var value_cd = quantile_cd.get_quantileValue(p_grf);
                    if (value_cd >= 0)
                        g["grf_transformed"].set_value(n, (float)value_cd);
                    else
                        continue;
                }
            }

            //计算损失值
            g.add_gridProperty("loss");
            for (int n = 0; n < gs.N; n++)
                if (g["cd_assign_to_grid"].get_value(n) != null)
                    g["loss"].set_value(n, g["cd_assign_to_grid"].get_value(n) - g["grf_transformed"].get_value(n));
            CData cd_loss = CData.create_from_gridProperty(g["loss"], "loss", CompareType.NotEqual, null);

            //条件化
            var (g_ok, time) = OK.Run(gs, vm, cd_loss, "loss", search_radius, rot_mat, k_cdi);
            g.add_gridProperty("loss_estimate_ok", g_ok[1]);
            g.add_gridProperty("loss_var_ok", g_ok[2]);
            g.add_gridProperty("cd_assign_to_grid2", g_ok[0]);
            //将OK结果与随机场进行叠加
            g.add_gridProperty("result", g["loss_estimate_ok"] + g["grf_transformed"]);
            sw.Stop();

            //检验
            Console.WriteLine($@"条件数据匹配错误的数量：{find_mistakes(g, "cd_assign_to_grid", "result")}");

            return (g, sw.ElapsedMilliseconds);
        }

        /// <summary>
        /// 基于OK条件化的GRFS模拟
        /// 参考文献:Conditional simulation for characterising the spatial variability of sand state
        /// </summary>
        /// <param name="search_radius"></param>
        /// <param name="k_cdi"></param>
        /// <param name="random_seed"></param>
        /// <returns></returns>
        public (Grid result, long milliseconds) run2(int search_radius, double[] rot_mat, int k_cdi,
            int random_seed = 123123)
        {
            Stopwatch sw = new();
            sw.Start();

            Grid g = Grid.create(gs); //工区网格
            MyConsoleHelper.write_string_to_console("基于fft的grfs模拟...");
            //先创建非条件高斯随机场
            g.add_gridProperty("grf",
                gs.dim == Dimension.D2
                    ? FFT_MA.fft_move_average_2d(gs, 10, 10, random_seed).gp
                    : FFT_MA.fft_move_average_3d(gs, 20, 20, 5, random_seed).gp);

            MyConsoleHelper.write_string_to_console("第1次OK计算...");
            //条件数据网格化
            g.add_gridProperty("Z_0_a", cd.coarsened(gs).coarsened_grid[property_name]);
            CData cd_Z_0_a = CData.create_from_gridProperty(g["Z_0_a"], "Z_0_a", CompareType.NotEqual, null);
            g.add_gridProperty("Z_0_ok", OK.Run(gs, vm, cd_Z_0_a, "Z_0_a", search_radius, rot_mat, k_cdi).result[1]);

            //cd quantile 计算分位数
            Quantile quantile_cd =
                Quantile.create(g["Z_0_a"].buffer.Where(a => a != null).Select(a => (double)a).ToList());

            //GRF quantile 计算分位数
            var data_gauss = g["grf"].buffer.OrderBy(a => a).Select(a => (double)a).ToList();
            //防止数量过大，从原始数据里提取一部分
            var indx = MyGenerator.linespace(0, data_gauss.Count - 1, 1000);
            List<double> data_gauss_sampling = [];
            foreach (var item in indx)
            {
                data_gauss_sampling.Add(data_gauss[(int)item]);
            }

            Quantile quantile_gauss = Quantile.create(data_gauss_sampling);

            //正态得分变换(GRF->cd)
            g.add_gridProperty("Z_s");
            for (int n = 0; n < gs.N; n++)
            {
                var value_GRF = g["grf"].get_value(n);
                if (value_GRF != null)
                {
                    var p_gauss = quantile_gauss.get_cumulativeProbabilities(value_GRF.Value);
                    var value_cd = quantile_cd.get_quantileValue(p_gauss);
                    if (value_cd >= 0)
                        g["Z_s"].set_value(n, (float)value_cd);
                    else
                        continue;
                }
            }

            //从高斯随机场中提取cd位置的值
            g.add_gridProperty("Z_S_a");
            for (int n = 0; n < gs.N; n++)
                if (g["Z_0_a"].get_value(n) != null)
                    g["Z_S_a"].set_value(n, g["Z_s"].get_value(n));

            MyConsoleHelper.write_string_to_console("第2次OK计算...");
            CData cd_Z_S_a = CData.create_from_gridProperty(g["Z_S_a"], "Z_S_a", CompareType.NotEqual, null);
            g.add_gridProperty("Z_S_ok", OK.Run(gs, vm, cd_Z_S_a, "Z_S_a", search_radius, rot_mat, k_cdi).result[1]);
            g.add_gridProperty("Z_CS", g["Z_s"] - g["Z_S_ok"] + g["Z_0_ok"]);

            sw.Stop();

            //检验
            Console.WriteLine($@"条件数据匹配错误的数量：{find_mistakes(g, "Z_0_a", "Z_CS")}");

            return (g, sw.ElapsedMilliseconds);
        }

        private int find_mistakes(Grid g, string property_name1, string property_name2)
        {
            int count = 0;
            for (int n = 0; n < g.gridStructure.N; n++)
            {
                if (g[property_name1].get_value(n) != null)
                {
                    if (g[property_name1].get_value(n) != g[property_name2].get_value(n))
                    {
                        count++;
                    }
                }
            }

            return count;
        }
    }
}