using System.Diagnostics;
using System.Numerics;
using JAM8.Algorithms.Numerics;
using JAM8.Utilities;
using MathNet.Numerics;

namespace JAM8.Algorithms.Geometry
{
    /// <summary>
    /// 非条件高斯随机场
    /// </summary>
    public class FFT_MA
    {
        /// <summary>
        /// 二维的非条件高斯随机场
        /// </summary>
        /// <param name="gs"></param>
        /// <param name="scale_x"></param>
        /// <param name="scale_y"></param>
        /// <param name="random_seed"></param>
        /// <returns></returns>
        public static (GridProperty gp, long time) fft_move_average_2d(GridStructure gs, int scale_x = 50, int scale_y = 50, int random_seed = 123123)
        {
            MersenneTwister mt = new((uint)random_seed);

            Stopwatch sw = new();
            sw.Start();

            int nx = (int)(gs.nx == 0 ? 1 : Math.Pow(2, Math.Ceiling(Math.Log2(gs.nx))));
            int ny = (int)(gs.ny == 0 ? 1 : Math.Pow(2, Math.Ceiling(Math.Log2(gs.ny))));
            int rx = (int)Math.Ceiling(nx / 2.0);
            int ry = (int)Math.Ceiling(ny / 2.0);
            double stdev = 1;
            GridStructure gs_fft = GridStructure.create_simple(nx, ny, 1);
            GridProperty gp_fft = GridProperty.create(gs_fft);
            for (int n = 0; n < gs_fft.N; n++)
            {
                var c = gs_fft.array_index_to_coord(n);
                var offset = Coord.create(c.x - (rx + 1), c.y - (ry + 1));
                var dist = Math.Sqrt(Math.Pow(offset.x / scale_x, 2) + Math.Pow(offset.y / scale_y, 2));
                var semiv = stdev * stdev - Math.Pow(stdev, 2) * (1 - Math.Exp(-Math.Pow(dist, 2)));
                gp_fft.set_value(n, (float?)semiv);
            }

            Complex[,] cov = new Complex[gs_fft.nx, gs_fft.ny];
            for (int iy = 0; iy < gs_fft.ny; iy++)
                for (int ix = 0; ix < gs_fft.nx; ix++)
                    cov[ix, iy] = new Complex(gp_fft.get_value(ix, iy).Value, 0);
            var cov_shift = MyFFT.fftshift2(cov);
            var fftC = MyFFT.fft2(cov_shift);
            var z_rand = new Complex[gs_fft.nx, gs_fft.ny];
            Gaussian gau = new();
            for (int j = 0; j < ny; j++)
                for (int i = 0; i < nx; i++)
                    z_rand[i, j] = new Complex(gau.Sample(mt), 0);
            z_rand = MyFFT.fft2(z_rand);
            var random_field = new Complex[gs_fft.nx, gs_fft.ny];
            for (int j = 0; j < ny; j++)
                for (int i = 0; i < nx; i++)
                    random_field[i, j] = fftC[i, j].SquareRoot() * z_rand[i, j];
            random_field = MyFFT.ifft2(random_field);

            GridProperty gp = GridProperty.create(gs);
            for (int iy = 0; iy < gs.ny; iy++)
                for (int ix = 0; ix < gs.nx; ix++)
                    gp.set_value(ix, iy, (float?)random_field[ix, iy].Real);

            sw.Stop();

            return (gp, sw.ElapsedMilliseconds);
        }

        /// <summary>
        /// 三维的非条件高斯随机场
        /// </summary>
        /// <param name="gs"></param>
        /// <param name="scale_x"></param>
        /// <param name="scale_y"></param>
        /// <param name="scale_z"></param>
        /// <param name="random_seed"></param>
        /// <returns></returns>
        public static (GridProperty gp, long time) fft_move_average_3d(GridStructure gs, int scale_x = 20, int scale_y = 20, int scale_z = 20, int random_seed = 123123)
        {
            MersenneTwister mt = new((uint)random_seed);

            Stopwatch sw = new();
            sw.Start();

            int nx = (int)(gs.nx == 0 ? 1 : Math.Pow(2, Math.Ceiling(Math.Log2(gs.nx))));
            int ny = (int)(gs.ny == 0 ? 1 : Math.Pow(2, Math.Ceiling(Math.Log2(gs.ny))));
            int nz = (int)(gs.ny == 0 ? 1 : Math.Pow(2, Math.Ceiling(Math.Log2(gs.nz))));
            int rx = (int)Math.Ceiling(nx / 2.0);
            int ry = (int)Math.Ceiling(ny / 2.0);
            int rz = (int)Math.Ceiling(nz / 2.0);
            double stdev = 1;

            GridStructure gs_fft = GridStructure.create_simple(nx, ny, nz);
            GridProperty gp_fft = GridProperty.create(gs_fft);
            for (int n = 0; n < gs_fft.N; n++)
            {
                var c = gs_fft.array_index_to_coord(n);
                var offset = Coord.create(c.x - (rx + 1), c.y - (ry + 1), c.z - (rz + 1));
                var dist = Math.Sqrt(Math.Pow(offset.x / scale_x, 2) + Math.Pow(offset.y / scale_y, 2) + Math.Pow(offset.z / scale_z, 2));
                var semiv = stdev * stdev - Math.Pow(stdev, 2) * (1 - Math.Exp(-Math.Pow(dist, 2)));
                gp_fft.set_value(n, (float?)semiv);
            }

            Complex[,,] cov = new Complex[gs_fft.nx, gs_fft.ny, gs_fft.nz];
            for (int iz = 0; iz < gs_fft.nz; iz++)
                for (int iy = 0; iy < gs_fft.ny; iy++)
                    for (int ix = 0; ix < gs_fft.nx; ix++)
                        cov[ix, iy, iz] = new Complex(gp_fft.get_value(ix, iy, iz).Value, 0);

            var cov_shift = MyFFT.fftshift3(cov);
            var fftC = MyFFT.fft3(cov_shift);
            var z_rand = new Complex[gs_fft.nx, gs_fft.ny, gs_fft.nz];
            Gaussian gau = new();
            for (int k = 0; k < nz; k++)
                for (int j = 0; j < ny; j++)
                    for (int i = 0; i < nx; i++)
                        z_rand[i, j, k] = new Complex(gau.Sample(mt), 0);

            z_rand = MyFFT.fft3(z_rand);
            var random_field = new Complex[gs_fft.nx, gs_fft.ny, gs_fft.nz];
            for (int k = 0; k < nz; k++)
                for (int j = 0; j < ny; j++)
                    for (int i = 0; i < nx; i++)
                        random_field[i, j, k] = fftC[i, j, k].SquareRoot() * z_rand[i, j, k];

            random_field = MyFFT.ifft3(random_field);

            GridProperty gp = GridProperty.create(gs);
            for (int iz = 0; iz < gs.nz; iz++)
                for (int iy = 0; iy < gs.ny; iy++)
                    for (int ix = 0; ix < gs.nx; ix++)
                        gp.set_value(ix, iy, iz, (float?)random_field[ix, iy, iz].Real);

            sw.Stop();

            return (gp, sw.ElapsedMilliseconds);
        }

        /// <summary>
        /// 三维的非条件高斯随机场（并行化计算）
        /// </summary>
        /// <param name="gs"></param>
        /// <param name="scale_x"></param>
        /// <param name="scale_y"></param>
        /// <param name="scale_z"></param>
        /// <param name="random_seed"></param>
        /// <returns></returns>
        public static (GridProperty gp, long time) fft_move_average_3d_parallel(GridStructure gs, int scale_x = 20, int scale_y = 20, int scale_z = 20, int random_seed = 123123)
        {
            MersenneTwister mt = new((uint)random_seed);

            Stopwatch sw = new();
            sw.Start();

            int nx = (int)(gs.nx == 0 ? 1 : Math.Pow(2, Math.Ceiling(Math.Log2(gs.nx))));
            int ny = (int)(gs.ny == 0 ? 1 : Math.Pow(2, Math.Ceiling(Math.Log2(gs.ny))));
            int nz = (int)(gs.ny == 0 ? 1 : Math.Pow(2, Math.Ceiling(Math.Log2(gs.nz))));
            int rx = (int)Math.Ceiling(nx / 2.0);
            int ry = (int)Math.Ceiling(ny / 2.0);
            int rz = (int)Math.Ceiling(nz / 2.0);
            double stdev = 1;

            GridStructure gs_fft = GridStructure.create_simple(nx, ny, nz);
            GridProperty gp_fft = GridProperty.create(gs_fft);
            Parallel.For(0, gs_fft.N, n =>
            {
                var c = gs_fft.array_index_to_coord(n);
                var offset = Coord.create(c.x - (rx + 1), c.y - (ry + 1), c.z - (rz + 1));
                var dist = Math.Sqrt(Math.Pow(offset.x / scale_x, 2) + Math.Pow(offset.y / scale_y, 2) + Math.Pow(offset.z / scale_z, 2));
                var semiv = stdev * stdev - Math.Pow(stdev, 2) * (1 - Math.Exp(-Math.Pow(dist, 2)));
                gp_fft.set_value(n, (float?)semiv);
            });

            Complex[,,] cov = new Complex[gs_fft.nx, gs_fft.ny, gs_fft.nz];
            for (int iz = 0; iz < gs_fft.nz; iz++)
                for (int iy = 0; iy < gs_fft.ny; iy++)
                    for (int ix = 0; ix < gs_fft.nx; ix++)
                        cov[ix, iy, iz] = new Complex(gp_fft.get_value(ix, iy, iz).Value, 0);

            var cov_shift = MyFFT.fftshift3(cov);
            var fftC = MyFFT.fft3(cov_shift);
            var z_rand = new Complex[gs_fft.nx, gs_fft.ny, gs_fft.nz];
            Gaussian gau = new();
            for (int k = 0; k < nz; k++)
                for (int j = 0; j < ny; j++)
                    for (int i = 0; i < nx; i++)
                        z_rand[i, j, k] = new Complex(gau.Sample(mt), 0);

            z_rand = MyFFT.fft3(z_rand);
            var random_field = new Complex[gs_fft.nx, gs_fft.ny, gs_fft.nz];
            for (int k = 0; k < nz; k++)
                for (int j = 0; j < ny; j++)
                    for (int i = 0; i < nx; i++)
                        random_field[i, j, k] = fftC[i, j, k].SquareRoot() * z_rand[i, j, k];

            random_field = MyFFT.ifft3(random_field);

            GridProperty gp = GridProperty.create(gs);
            for (int iz = 0; iz < gs.nz; iz++)
                for (int iy = 0; iy < gs.ny; iy++)
                    for (int ix = 0; ix < gs.nx; ix++)
                        gp.set_value(ix, iy, iz, (float?)random_field[ix, iy, iz].Real);

            sw.Stop();

            return (gp, sw.ElapsedMilliseconds);
        }

    }
}
