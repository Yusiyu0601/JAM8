using System.Diagnostics;
using System.Numerics;
using JAM8.Algorithms;
using JAM8.Algorithms.Geometry;
using JAM8.Algorithms.Numerics;
using JAM8.Utilities;
using MathNet.Numerics;

namespace JAM8.SpecificApps.建模方法
{
    public class Model_GRFS
    {
        public static void grfs_run()
        {
            Form_GRFS frm = new();
            frm.Show();
        }

        public static void GRFS_win2()
        {
            Random rnd = new();
            GridStructure gs = GridStructure.create_win();
            Variogram vm = Variogram.create(VariogramType.Spherical, 0, 1, 20);
            CData cd = CData.read_from_gslib_win().cdata;
            GRFS grfs = GRFS.create(gs, vm, cd, cd.select_by_property_name_win());
            var rot_mat = new double[] { 0, 0, 0, 20, 20, 2 };
            var (result, time) = grfs.run2(30, rot_mat, 20, rnd.Next());
            result.showGrid_win();
            Console.WriteLine($@"计算时间:{time}秒");
        }

        public static void GRFS_win1()
        {
            Random rnd = new();
            GridStructure gs = GridStructure.create_win();
            Variogram vm = Variogram.create(VariogramType.Spherical, 0, 1, 20);
            CData cd = CData.read_from_gslib_win().cdata;
            GRFS grfs = GRFS.create(gs, vm, cd, cd.select_by_property_name_win());
            var rot_mat = new double[] { 0, 0, 0, 20, 20, 2 };
            var (result, time) = grfs.run(30, rot_mat, 20, rnd.Next());
            result.showGrid_win();
            Console.WriteLine($@"计算时间:{time}秒");
        }

        public static void GRFS1()
        {
            GridStructure gs = GridStructure.create_win();
            Grid g = Grid.create(gs, "GRFS_work");
            g.add_gridProperty("GRF");
            GridStructure gs_extent =
                GridStructure.create_simple(gs.nx + 50, gs.ny + 50, gs.dim == Dimension.D2 ? 1 : gs.nz + 10);

            Grid g_extent = Grid.create(gs_extent);
            g_extent.add_gridProperty("origin");
            g_extent.add_gridProperty("gauss");
            g_extent["origin"].set_values_gaussian(0, 1, new Random());
            Mould mould = (gs.dim == Dimension.D2)
                ? Mould.create_by_ellipse(25, 25, 1)
                : Mould.create_by_ellipse(5, 5, 5, 1);
            int flag = 1;
            Parallel.For(0, gs_extent.N, i =>
            {
                MyConsoleProgress.Print(flag++, gs_extent.N, "gauss random field: ");
                var mouldInstance =
                    MouldInstance.create_from_gridProperty(mould, gs_extent.get_spatial_index(i), g_extent["origin"]);
                if (mouldInstance.neighbor_nulls_ids.Count == 0)
                    g_extent["gauss"].set_value(i, mouldInstance.neighbor_values.Average(a => a.Value));
            });
            List<float?> t1 = new();
            for (int i = 0; i < gs_extent.N; i++)
                if (g_extent["gauss"].get_value(i) != null)
                    t1.Add(g_extent["gauss"].get_value(i));
            for (int i = 0; i < gs.N; i++)
                g["GRF"].set_value(i, t1[i]);

            g.showGrid_win();

            var (cd, _) = CData.read_from_gslib_win();
            g.add_gridProperty("cd", cd.coarsened(gs).coarsened_grid.first_gridProperty());

            //cd quantile
            Quantile quantile_cd =
                Quantile.create(g["cd"].buffer.Where(a => a != null).Select(a => (double)a).ToList());
            Form_QuickChart.ScatterPlot(quantile_cd.quantile_values, quantile_cd.cumulative_probabilities, null,
                "quantile", "cdf", "cd quantile");
            //GRF quantile
            Quantile quantile_gauss =
                Quantile.create(g["GRF"].buffer.Where(a => a != null).Select(a => (double)a).ToList());
            Form_QuickChart.ScatterPlot(quantile_gauss.quantile_values, quantile_gauss.cumulative_probabilities, null,
                "quantile", "cdf", "guass quantile");
            //return;
            //正态得分变换(GRF->cd)
            g.add_gridProperty("GRF_transformed");
            for (int n = 0; n < gs.N; n++)
            {
                var value_GRF = g["GRF"].get_value(n);
                if (value_GRF != null)
                {
                    var p_gauss = quantile_gauss.get_cumulativeProbabilities(value_GRF.Value);
                    var value_cd = quantile_cd.get_quantileValue(p_gauss);
                    if (value_cd >= 0)
                        g["GRF_transformed"].set_value(n, (float)value_cd);
                    else
                        continue;
                }
            }

            //计算偏差
            g.add_gridProperty("error");
            for (int n = 0; n < gs.N; n++)
                if (g["cd"].get_value(n) != null)
                    g["error"].set_value(n, g["cd"].get_value(n) - g["GRF_transformed"].get_value(n));
            //对偏差进行OK计算
            Variogram vg = Variogram.create(VariogramType.Spherical, 0, 1, 100);
            CData cd1 = CData.create_from_gridProperty(g["error"], "error", CompareType.NotEqual, null);
            var rot_mat = new double[] { 0, 0, 0, 20, 20, 2 };
            g.add_gridProperty("estimate", OK.Run(gs, vg, cd1, "error", 50, rot_mat, 3).result[1]);
            //将OK结果与随机场进行叠加
            g.add_gridProperty("result", g["estimate"] + g["GRF_transformed"]);
            g.showGrid_win();
        }

        public static void GRFS2d_FFT()
        {
            int nx = 1000;
            int ny = 1000;
            nx = (int)(nx == 0 ? 1 : Math.Pow(2, Math.Ceiling(Math.Log2(nx))));
            ny = (int)(ny == 0 ? 1 : Math.Pow(2, Math.Ceiling(Math.Log2(ny))));

            int rx = (int)Math.Ceiling(nx / 2.0);
            int ry = (int)Math.Ceiling(ny / 2.0);
            int scale_x = 50;
            int scale_y = 50;
            double stdev = 1;
            GridStructure gs = GridStructure.create_simple(nx, ny, 1);
            GridProperty gp = GridProperty.create(gs);
            for (int n = 0; n < gs.N; n++)
            {
                var c = gs.array_index_to_coord(n);
                var offset = Coord.create(c.x - (rx + 1), c.y - (ry + 1));
                var dist = Math.Sqrt(Math.Pow(offset.x / scale_x, 2) + Math.Pow(offset.y / scale_y, 2));
                var semiv = stdev * stdev - Math.Pow(stdev, 2) * (1 - Math.Exp(-Math.Pow(dist, 2)));
                gp.set_value(n, (float?)semiv);
            }
            //gp.show_win();

            Complex[,] cov = new Complex[gs.nx, gs.ny];
            for (int iy = 0; iy < gs.ny; iy++)
            {
                for (int ix = 0; ix < gs.nx; ix++)
                {
                    cov[ix, iy] = new Complex(gp.get_value(ix, iy).Value, 0);
                }
            }

            //Form_QuickChart.ArrayPlot2(cov, 0);
            var cov_shift = MyFFT.fftshift2(cov);
            //Form_QuickChart.ArrayPlot2(cov_shift, 0);
            var fftC = MyFFT.fft2(cov_shift);
            //Form_QuickChart.ArrayPlot2(fftC, 2);

            var z_rand = new Complex[gs.nx, gs.ny];
            //z_rand = new Complex[100, 100];
            Gaussian g = new();
            for (int j = 0; j < ny; j++)
            {
                for (int i = 0; i < nx; i++)
                {
                    z_rand[i, j] = new Complex(g.Sample(), 0);
                }
            }

            Form_QuickChart.ArrayPlot2(z_rand, 0);
            z_rand = MyFFT.fft2(z_rand);
            //Form_QuickChart.ArrayPlot2(z_rand, 0);
            var random_field = new Complex[gs.nx, gs.ny];
            for (int j = 0; j < ny; j++)
            {
                for (int i = 0; i < nx; i++)
                {
                    random_field[i, j] = fftC[i, j].SquareRoot() * z_rand[i, j];
                }
            }

            random_field = MyFFT.ifft2(random_field);
            Form_QuickChart.ArrayPlot2(random_field, 0);
        }

        public static void GRFS3d_FFT()
        {
            int nx = 500;
            int ny = 500;
            int nz = 100;
            nx = (int)(nx == 0 ? 1 : Math.Pow(2, Math.Ceiling(Math.Log2(nx))));
            ny = (int)(ny == 0 ? 1 : Math.Pow(2, Math.Ceiling(Math.Log2(ny))));
            nz = (int)(ny == 0 ? 1 : Math.Pow(2, Math.Ceiling(Math.Log2(nz))));
            int rx = (int)Math.Ceiling(nx / 2.0);
            int ry = (int)Math.Ceiling(ny / 2.0);
            int rz = (int)Math.Ceiling(nz / 2.0);
            int scale_x = 20;
            int scale_y = 20;
            int scale_z = 20;
            double stdev = 1;

            Stopwatch sw = new();
            sw.Start();
            GridStructure gs = GridStructure.create_simple(nx, ny, nz);
            GridProperty gp = GridProperty.create(gs);
            sw.Stop();
            Console.WriteLine($@"阶段1,time={sw.ElapsedMilliseconds}");
            sw.Reset();
            sw.Restart();
            for (int n = 0; n < gs.N; n++)
            {
                var c = gs.array_index_to_coord(n);
                var offset = Coord.create(c.x - (rx + 1), c.y - (ry + 1), c.z - (rz + 1));
                var dist = Math.Sqrt(Math.Pow(offset.x / scale_x, 2) + Math.Pow(offset.y / scale_y, 2) +
                                     Math.Pow(offset.z / scale_z, 2));
                var semiv = stdev * stdev - Math.Pow(stdev, 2) * (1 - Math.Exp(-Math.Pow(dist, 2)));
                gp.set_value(n, (float?)semiv);
            }

            sw.Stop();
            Console.WriteLine($@"阶段2,time={sw.ElapsedMilliseconds}");
            sw.Reset();
            sw.Restart();
            //gp.show_win();

            Complex[,,] cov = new Complex[gs.nx, gs.ny, gs.nz];
            for (int iz = 0; iz < gs.nz; iz++)
            {
                for (int iy = 0; iy < gs.ny; iy++)
                {
                    for (int ix = 0; ix < gs.nx; ix++)
                    {
                        cov[ix, iy, iz] = new Complex(gp.get_value(ix, iy, iz).Value, 0);
                    }
                }
            }

            sw.Stop();
            Console.WriteLine($@"阶段3,time={sw.ElapsedMilliseconds}");
            sw.Reset();
            sw.Restart();
            //Form_QuickChart.ArrayPlot2(cov, 0);
            var cov_shift = MyFFT.fftshift3(cov);
            sw.Stop();
            Console.WriteLine($@"阶段4,time={sw.ElapsedMilliseconds}");
            sw.Reset();
            sw.Restart();
            //Form_QuickChart.ArrayPlot2(cov_shift, 0);
            var fftC = MyFFT.fft3(cov_shift);
            sw.Stop();
            Console.WriteLine($@"阶段5,time={sw.ElapsedMilliseconds}");
            sw.Reset();
            sw.Restart();
            //Form_QuickChart.ArrayPlot2(fftC, 2);

            var z_rand = new Complex[gs.nx, gs.ny, gs.nz];
            //z_rand = new Complex[100, 100];
            Gaussian g = new();
            for (int k = 0; k < nz; k++)
            {
                for (int j = 0; j < ny; j++)
                {
                    for (int i = 0; i < nx; i++)
                    {
                        z_rand[i, j, k] = new Complex(g.Sample(), 0);
                    }
                }
            }

            sw.Stop();
            Console.WriteLine($@"阶段6,time={sw.ElapsedMilliseconds}");
            sw.Reset();
            sw.Restart();
            //Form_QuickChart.ArrayPlot2(z_rand, 0);
            z_rand = MyFFT.fft3(z_rand);
            sw.Stop();
            Console.WriteLine($@"阶段7,time={sw.ElapsedMilliseconds}");
            sw.Reset();
            sw.Restart();
            //Form_QuickChart.ArrayPlot2(z_rand, 0);
            var random_field = new Complex[gs.nx, gs.ny, gs.nz];
            for (int k = 0; k < nz; k++)
            {
                for (int j = 0; j < ny; j++)
                {
                    for (int i = 0; i < nx; i++)
                    {
                        random_field[i, j, k] = fftC[i, j, k].SquareRoot() * z_rand[i, j, k];
                    }
                }
            }

            random_field = MyFFT.ifft3(random_field);
            sw.Stop();
            Console.WriteLine($@"模拟完成,time={sw.ElapsedMilliseconds}");
            Form_QuickChart.ArrayPlot2(random_field, 0);
        }
    }
}