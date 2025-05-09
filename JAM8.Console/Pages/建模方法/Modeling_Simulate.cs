using System.Diagnostics;
using System.Numerics;
using EasyConsole;
using JAM8.Algorithms;
using JAM8.Algorithms.Geometry;
using JAM8.Algorithms.Numerics;
using JAM8.SpecificApps.建模方法.Forms;
using MathNet.Numerics;

namespace JAM8.Console.Pages
{
    public class Modeling_Simulate : Page
    {
        public Modeling_Simulate(EasyConsole.Program program) : base("Modeling_Simulate", program)
        {
        }

        public override void Display()
        {
            base.Display();

            Output.WriteLine(ConsoleColor.Green, "Modeling_Simulate 功能：");

            Perform();

            System.Console.WriteLine();
            EasyConsole.Output.WriteLine(ConsoleColor.Green, "按任意键返回");
            System.Console.ReadKey();

            Program.NavigateBack();
        }

        private void Perform()
        {
            var menu = new EasyConsole.Menu()

           .Add("Back", CommonFunctions.Cancel)
            .Add("MPS模拟", MPS模拟)
            .Add("GRFS模拟", GRFS模拟)
           ;

            menu.Display();
        }

        private void GRFS模拟()
        {
            EasyConsole.Output.WriteLine(ConsoleColor.Green, "GRFS模拟");

            var menu = new EasyConsole.Menu()

  .Add("Back", CommonFunctions.Cancel)
   .Add("基于FFT的GRFS(2d 非条件测试)", Modeling_GRFS_FFT2d)
   .Add("基于FFT的GRFS(3d 非条件测试)", Modeling_GRFS_FFT3d)
   .Add("基于FFT的GRFS(条件模拟方案1)", Modeling_GRFS_withCData_Method1)
   .Add("基于FFT的GRFS(条件模拟方案2)", Modeling_GRFS_withCData_Method2)
  ;

            menu.Display();
        }

        private void Modeling_GRFS_FFT2d()
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
                var c = gs.arrayIndex_to_coord(n);
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

            System.Console.WriteLine(@"模拟完成,按任意键返回");
            System.Console.ReadKey();

        }

        private void Modeling_GRFS_FFT3d()
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
            System.Console.WriteLine($@"阶段1,time={sw.ElapsedMilliseconds}");
            sw.Reset();
            sw.Restart();
            for (int n = 0; n < gs.N; n++)
            {
                var c = gs.arrayIndex_to_coord(n);
                var offset = Coord.create(c.x - (rx + 1), c.y - (ry + 1), c.z - (rz + 1));
                var dist = Math.Sqrt(Math.Pow(offset.x / scale_x, 2) + Math.Pow(offset.y / scale_y, 2) + Math.Pow(offset.z / scale_z, 2));
                var semiv = stdev * stdev - Math.Pow(stdev, 2) * (1 - Math.Exp(-Math.Pow(dist, 2)));
                gp.set_value(n, (float?)semiv);
            }
            sw.Stop();
            System.Console.WriteLine($@"阶段2,time={sw.ElapsedMilliseconds}");
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
            System.Console.WriteLine($@"阶段3,time={sw.ElapsedMilliseconds}");
            sw.Reset();
            sw.Restart();
            //Form_QuickChart.ArrayPlot2(cov, 0);
            var cov_shift = MyFFT.fftshift3(cov);
            sw.Stop();
            System.Console.WriteLine($@"阶段4,time={sw.ElapsedMilliseconds}");
            sw.Reset();
            sw.Restart();
            //Form_QuickChart.ArrayPlot2(cov_shift, 0);
            var fftC = MyFFT.fft3(cov_shift);
            sw.Stop();
            System.Console.WriteLine($@"阶段5,time={sw.ElapsedMilliseconds}");
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
            System.Console.WriteLine($@"阶段6,time={sw.ElapsedMilliseconds}");
            sw.Reset();
            sw.Restart();
            //Form_QuickChart.ArrayPlot2(z_rand, 0);
            z_rand = MyFFT.fft3(z_rand);
            sw.Stop();
            System.Console.WriteLine($@"阶段7,time={sw.ElapsedMilliseconds}");
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
            System.Console.WriteLine($@"模拟完成,time={sw.ElapsedMilliseconds}");
            Form_QuickChart.ArrayPlot2(random_field, 0);

            System.Console.WriteLine(@"模拟完成,按任意键返回");
            System.Console.ReadKey();

        }

        private void Modeling_GRFS_withCData_Method1()
        {
            Random rnd = new();
            GridStructure gs = GridStructure.create_win();
            Variogram vm = Variogram.create(VariogramType.Spherical, 0, 1, 20);
            CData cd = CData.read_from_gslibwin().cdata;
            GRFS grfs = GRFS.create(gs, vm, cd, cd.select_cd_propertyName_win());
            var rot_mat = new double[] { 0, 0, 0, 20, 20, 2 };
            var (result, time) = grfs.run(30, rot_mat, 20, rnd.Next());
            result.showGrid_win();
            System.Console.WriteLine($@"计算时间:{time}秒");
        }

        private void Modeling_GRFS_withCData_Method2()
        {
            Random rnd = new();
            GridStructure gs = GridStructure.create_win();
            Variogram vm = Variogram.create(VariogramType.Spherical, 0, 1, 20);
            CData cd = CData.read_from_gslibwin().cdata;
            GRFS grfs = GRFS.create(gs, vm, cd, cd.select_cd_propertyName_win());
            var rot_mat = new double[] { 0, 0, 0, 20, 20, 2 };
            var (result, time) = grfs.run2(30, rot_mat, 20, rnd.Next());
            result.showGrid_win();
            System.Console.WriteLine($@"计算时间:{time}秒");
        }

        private void MPS模拟()
        {
            Form_MPS frm = new();
            if (frm.ShowDialog() != DialogResult.OK)
                return;
        }
    }
}
