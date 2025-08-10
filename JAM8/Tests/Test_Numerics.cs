using System.Diagnostics;
using System.Numerics;
using DataStructures.ViliWonka.KDTree;
using HNSW.Net;
using JAM8.Algorithms;
using JAM8.Algorithms.Geometry;
using JAM8.Algorithms.Images;
using JAM8.Algorithms.Numerics;
using JAM8.Utilities;
using MathNet.Numerics;
using MathNet.Numerics.IntegralTransforms;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using MathNet.Numerics.Optimization;
using Vector_double = MathNet.Numerics.LinearAlgebra.Vector<double>;
using Vector3 = DataStructures.ViliWonka.KDTree.Vector3;

namespace JAM8.Tests
{
    public class Test_Numerics
    {
        public static void LevenbergMarquardtSolver_Test()
        {
            // 定义我们的函数
            // f(x; A1, B1,x0)=A1*e^(B1(x-x0))
            Vector_double MyModel(Vector_double p, Vector_double x)
            {
                var y = CreateVector.Dense<double>(x.Count);
                var A1 = p[0];
                var B1 = p[1];
                var x0 = p[2];
                for (int i = 0; i < x.Count; i++)
                {
                    y[i] = A1 * Math.Exp(B1 * (x[i] - x0));
                }

                return y;
            }

            //derivatives:求导数
            //df/A1 = e^(B1*x-x0)
            //df/B1 = A1*e^(B1*x-x0)*(x-x0)
            //df/x0 = A1*e^(B1*x-x0)*(-B1)
            Matrix<double> MyPrime(Vector_double p, Vector_double x)
            {
                var prime = Matrix<double>.Build.Dense(x.Count, p.Count);
                var A1 = p[0];
                var B1 = p[1];
                var x0 = p[2];
                for (int i = 0; i < x.Count; i++)
                {
                    prime[i, 0] = Math.Exp(B1 * (x[i] - x0));
                    prime[i, 1] = A1 * Math.Exp(B1 * (x[i] - x0)) * (x[i] - x0);
                    prime[i, 2] = A1 * Math.Exp(B1 * (x[i] - x0)) * (-B1);
                }

                return prime;
            }

            Vector_double MyStart = new DenseVector(new double[] { 100, 0.01, 1 });
            double[] xData = new double[] { 1, 2, 5, 6 };
            double[] yData = new double[] { 3, 6, 8, 10 };
            var result1 = Fit.Exponential(xData, yData);
            double goodnessOfFit1 =
                GoodnessOfFit.RSquared(xData.Select(x => result1.A * Math.Exp(result1.R * x)), yData);
            Console.WriteLine("goodnessOfFit1:" + goodnessOfFit1);


            var obj = ObjectiveFunction.NonlinearModel(MyModel, MyPrime, new DenseVector(xData),
                new DenseVector(yData));
            var solver = new LevenbergMarquardtMinimizer();
            var result2 = solver.FindMinimum(obj, MyStart);
            double goodnessOfFit2 =
                GoodnessOfFit.RSquared(
                    xData.Select(x =>
                        result2.MinimizingPoint[0] *
                        Math.Exp((result2.MinimizingPoint[1] * (x - result2.MinimizingPoint[2])))), yData);
            Console.WriteLine("goodnessOfFit2:" + goodnessOfFit2);
        }

        public static void NelderMeadSimplex_球状模型拟合()
        {
            double[] h = new[]
            {
                1.5,
                4.5,
                7.5,
                10.5,
                13.5,
                16.5,
                19.5,
                22.5,
                25.5,
                28.5,
                31.5
            };
            double[] gamma = new[]
            {
                0.037861122,
                0.08184661,
                0.115992736,
                0.162627551,
                0.179835028,
                0.19123492,
                0.198578655,
                0.203582236,
                0.206729351,
                0.208934695,
                0.207543387
            };

            //input:nugget sill range
            double func(Vector_double input)
            {
                double loss = 0.0;
                for (int i = 0; i < h.Length; i++)
                {
                    var nugget = input[0];
                    var sill = input[1];
                    var range = input[2];
                    var gamma1 = 0.0;
                    if (h[i] == 0)
                        gamma1 = nugget;
                    if (h[i] > 0 && h[i] <= range)
                        gamma1 = nugget + (sill - nugget) * (1.5 * (h[i] / range) - 0.5 * Math.Pow(h[i] / range, 3));
                    if (h[i] > range)
                        gamma1 = sill;
                    var distance_i = gamma1 - gamma[i];
                    loss += distance_i * distance_i;
                }

                return loss;
            }

            var obj = ObjectiveFunction.Value(func);
            var solver = new NelderMeadSimplex(convergenceTolerance: 0.000000001, maximumIterations: 1000);
            var initialGuess = new DenseVector(new double[] { gamma.Min(), gamma.Max(), h.Max() });

            var result = solver.FindMinimum(obj, initialGuess);
            Console.WriteLine("Value:\t" + result.FunctionInfoAtMinimum.Value);
            Console.WriteLine("Point:\t" + result.MinimizingPoint[0] + " , " + result.MinimizingPoint[1] + "," +
                              result.MinimizingPoint[2]);
            Console.WriteLine("Iterations:\t" + result.Iterations);
        }

        public static void NelderMeadSimplex_Test()
        {
            static double func(Vector_double input)
            {
                return Math.Pow(1 - input[0], 2) + 100 * Math.Pow((input[1] - input[0] * input[0]), 2);
            }

            var obj = ObjectiveFunction.Value(func);
            var solver = new NelderMeadSimplex(convergenceTolerance: 0.0000000001, maximumIterations: 1000);
            var initialGuess = new DenseVector(new double[] { 0.2, 1.2 });

            var result = solver.FindMinimum(obj, initialGuess);
            Console.WriteLine("Value:\t" + result.FunctionInfoAtMinimum.Value);
            Console.WriteLine("Point:\t" + result.MinimizingPoint[0] + " , " + result.MinimizingPoint[1]);
            Console.WriteLine("Iterations:\t" + result.Iterations);
        }

        public static void FFTShift_Test()
        {
            double[] values = new double[] { 1, 2, 3, 4, 5, 6, 7 };
            MyArrayHelper.print<double>(values);
            MyArrayHelper.print<double>(MyFFT.ifftshift(values));
            MyArrayHelper.print<double>(MyFFT.fftshift(values));
            MyArrayHelper.print<double>(MyFFT.ifftshift(MyFFT.fftshift(values)));

            values = new double[] { 1, 2, 3, 4, 5, 6, 7, 8 };
            MyArrayHelper.print<double>(values);
            MyArrayHelper.print<double>(MyFFT.ifftshift(values));
            MyArrayHelper.print<double>(MyFFT.fftshift(values));
            MyArrayHelper.print<double>(MyFFT.ifftshift(MyFFT.fftshift(values)));
        }

        public static void QuickChart_Test()
        {
            Form_QuickChart frm = new();
            frm.Show();
            for (int i = 1; i < 50000; i += 10)
            {
                double[] x = MyGenerator.range(0, i, 1).Select(a => (double)a).ToArray();
                double[] y = new double[x.Length];
                for (int j = 0; j < y.Length; j++)
                {
                    y[j] = Math.Sin(Math.PI / 180 * x[j]) + Math.Sin(Math.PI / 180 * x[j]);
                }

                frm.DrawLine(x, y, null, "", "", "");
            }
        }

        public static void FFT2_Test()
        {
            OpenFileDialog ofd = new();
            if (ofd.ShowDialog() != DialogResult.OK)
                return;
            Bitmap b = Image.FromFile(ofd.FileName) as Bitmap;
            b = ImageProcess.color_to_gray(b);
            Form_QuickChart.ImagePlot(b);

            Complex[,] array = new Complex[b.Width, b.Height];
            for (int j = 0; j < b.Height; j++)
            {
                for (int i = 0; i < b.Width; i++)
                {
                    array[i, j] = new Complex(b.GetPixel(i, j).R, 0);
                }
            }

            //变换
            array = MyFFT.fft2(array);
            Form_QuickChart.ArrayPlot2(MyFFT.abs(array));

            array = MyFFT.fftshift2(array);
            Form_QuickChart.ArrayPlot2(MyFFT.abs(array));

            //for (int idx_dim1 = 0; idx_dim1 < array.GetLength(0); idx_dim1++)
            //{
            //    for (int idx_dim2 = 0; idx_dim2 < array.GetLength(1); idx_dim2++)
            //    {
            //        if (idx_dim1 >= array.GetLength(0) / 2 - 25 &&
            //            idx_dim1 < array.GetLength(0) / 2 + 25 &&
            //            idx_dim2 >= array.GetLength(1) / 2 - 25 &&
            //            idx_dim2 < array.GetLength(1) / 2 + 25)
            //            //array[idx_dim1, idx_dim2] = 0;//低通滤波
            //            continue;
            //        else
            //            array[idx_dim1, idx_dim2] = 0;//高通滤波
            //    }
            //}
            //逆变换
            array = MyFFT.ifftshift2(array);
            Form_QuickChart.ArrayPlot2(MyFFT.abs(array));

            array = MyFFT.ifft2(array);
            Form_QuickChart.ArrayPlot2(MyFFT.abs(array));
        }

        public static void FFT_Test2()
        {
            Complex c = new(10, 10);
            Console.WriteLine(c.Magnitude);

            Form_QuickChart frm = new();
            frm.Show();
            double[] x = MyGenerator.linspace(0, 1, 1400);
            double[] x_tick = MyGenerator.range(0, 1400, 1).Select(a => (double)a).ToArray();
            double[] y = new double[x.Length];
            for (int j = 0; j < y.Length; j++)
            {
                y[j] = 7 * Math.Sin(2 * Math.PI * 200 * x[j])
                       + 5 * Math.Sin(2 * Math.PI * 400 * x[j]) + 3 * Math.Sin(2 * Math.PI * 600 * x[j]);
            }

            frm.DrawLine(x_tick, y, null);
            Complex[] input = new Complex[x.Length];
            for (int i = 0; i < input.Length; i++)
            {
                input[i] = new Complex((float)y[i], 0);
            }

            Fourier.Forward(input, FourierOptions.Matlab);
            double[] mod = new double[input.Length];
            double[] angle = new double[input.Length];
            for (int i = 0; i < input.Length; i++)
            {
                mod[i] = input[i].Magnitude / input.Length;
                angle[i] = input[i].Phase;
            }

            Form_QuickChart.LinePlot(x_tick, mod);
            Form_QuickChart.LinePlot(x_tick, angle);
        }

        public static void FFT_Test1()
        {
            Form_QuickChart frm = new();
            frm.Show();
            double[] x = MyGenerator.range(0, 1000, 1).Select(a => (double)a).ToArray();
            double[] y = new double[x.Length];
            for (int j = 0; j < y.Length; j++)
            {
                y[j] = Math.Sin(Math.PI / 180 * x[j]) + Math.Sin(2 * Math.PI / 180 * x[j]);
            }

            frm.DrawLine(x, y);
            Complex[] input = new Complex[x.Length];
            for (int i = 0; i < input.Length; i++)
            {
                input[i] = new Complex((float)y[i], 0);
            }

            input = MyFFT.fft(input);
            var fftshift = MyFFT.fftshift(input);
            double[] mod = new double[fftshift.Length];
            double[] angle = new double[fftshift.Length];
            for (int i = 0; i < fftshift.Length; i++)
            {
                mod[i] = fftshift[i].Magnitude / fftshift.Length;
                angle[i] = fftshift[i].Phase;
            }

            Form_QuickChart.LinePlot(x, mod);
            Form_QuickChart.LinePlot(x, angle);
            Fourier.Inverse(input, FourierOptions.Matlab);
        }

        public static void Quantile_Test()
        {
            List<double> values = new();
            Random rnd = new();
            Gaussian gau = new(0, 1);
            for (int i = 0; i < 10000; i++)
            {
                values.Add(gau.Sample());
            }

            Quantile q = Quantile.create(values);
            Form_QuickChart chart1 = new("分位数图");
            chart1.Show();
            chart1.DrawScatter(q.quantile_values, q.cumulative_probabilities, null, "quantile", "cpf", "分位数图");
        }

        public static void SimulationPath_Test()
        {
            GridStructure gs = GridStructure.create_win();
            SimulationPath path = SimulationPath.create(gs, 1, new MersenneTwister(111));
            for (int i = 10; i <= gs.N / 2; i++)
            {
                path.freeze(gs.get_spatial_index(i));
            }

            while (false == path.is_visit_over())
            {
                var si = path.visit_next();
                MyConsoleProgress.Print(path.progress, "");
            }
        }

        public static void MyConsoleProgress_Test()
        {
            int N = 1000;
            var mcp = new MyConsoleProgress();

            for (int i = 0; i < N; i++)
            {
                Thread.Sleep(100);
                mcp.PrintWithRemainTime(i, N, "");
            }
        }

        public static void HNSW_Test()
        {
            List<float[]> pnts = new();
            Random rnd = new();
            for (int n = 0; n < 20000; n++)
            {
                List<float> vector = new();
                for (int i = 0; i < 2000; i++)
                {
                    vector.Add(rnd.Next(1, 4));
                }

                pnts.Add(vector.ToArray());
            }

            SmallWorld<float[], float>.Parameters paras = new()
            {
                M = 10,
                LevelLambda = 1.0 / Math.Log(10),
                NeighbourHeuristic = NeighbourSelectionHeuristic.SelectSimple,
                ConstructionPruning = 50,
                ExpandBestSelection = false,
                KeepPrunedConnections = false,
                EnableDistanceCacheForConstruction = true,
                InitialDistanceCacheSize = 1048576,
                InitialItemsSize = 1024
            };
            SmallWorld<float[], float> graph = new(L2_distance, DefaultRandomGenerator.Instance, paras);
            graph.AddItems(pnts);

            Stopwatch sw = new();
            sw.Start();
            int flag = 0;
            var mcp = new MyConsoleProgress();
            Parallel.For(0, 200000, n =>
            {
                List<float> vector1 = new();
                for (int i = 0; i < 2000; i++)
                {
                    vector1.Add(rnd.Next(0, 4));
                }

                var founds = graph.KNNSearch(vector1.ToArray(), 20);
                float min = float.MaxValue;
                foreach (var pnt in pnts)
                {
                    float dist = L2_distance(pnt, vector1.ToArray());
                    if (dist < min)
                        min = dist;
                }

                //MyConsoleProgress.Print(flag++, 200000, "", $"{founds.Min(a => a.Distance)}");
                //MyConsoleProgress.Print(flag++, 200000, "", $"{min}");
                mcp.PrintWithRemainTime(flag++, 200000, "", $"{founds.Min(a => a.Distance)}  {min}");
                //Console.WriteLine(founds.Min(a => a.Distance));
            });
            sw.Stop();
            Console.WriteLine($@"{sw.ElapsedMilliseconds}ms");
        }

        /// <summary>
        /// L2距离
        /// </summary>
        /// <param name="u"></param>
        /// <param name="v"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        private static float L2_distance(float[] u, float[] v)
        {
            if (u.Length != v.Length)
            {
                throw new ArgumentException("Vectors have non-matching dimensions");
            }

            float dist = 0.0f;
            for (int i = 0; i < u.Length; i++)
            {
                dist += (u[i] - v[i]) * (u[i] - v[i]);
            }

            return (float)Math.Sqrt(dist);
        }

        public static void KDQuery_Test()
        {
            List<Vector3> pointCloud = new();
            var query = new KDQuery();
            Random rnd = new();
            for (int k = 0; k < 101; k += 2)
            {
                for (int j = 0; j < 101; j += 2)
                {
                    for (int i = 0; i < 101; i += 2)
                    {
                        pointCloud.Add(new Vector3(i, j, k));
                    }
                }
            }

            Console.WriteLine(pointCloud.Count.ToString());
            //for (int i = 0; i < pointCloud.Length; i++)
            //{
            //    pointCloud[i] = new Vector3(
            //        (float)rnd.Next(1, 101) + 20,
            //        (float)rnd.Next(1, 101) + 20,
            //        (float)rnd.Next(1, 101) + 20
            //    //1f
            //    );
            //}

            //for (int i = 0; i < pointCloud.Length; i++)
            //{
            //    for (int j = 0; j < i; j++)
            //    {
            //        pointCloud[i] += LorenzStep(pointCloud[i]) * 0.01f;
            //    }
            //}

            var tree = new KDTree(pointCloud.ToArray(), 32);

            Vector3 LorenzStep(Vector3 p)
            {
                float ρ = 28f;
                float σ = 10f;
                float β = 8 / 3f;

                return new Vector3(
                    σ * (p.y - p.x),
                    p.x * (ρ - p.z) - p.y,
                    p.x * p.y - β * p.z
                );
            }


            var resultDistance = new List<float>();
            var pnt = new Vector3(50, 122, 1);
            Console.WriteLine(pnt.ToString());
            Console.WriteLine();
            //query.ClosestPoint(tree, pnt, resultIndices, resultDistance);
            var N = 300000;
            int flag = 0;
            Stopwatch sw = new();
            sw.Start();
            for (int n = 0; n < N; n++)
            {
                flag++;
                var resultIndices = new List<int>();
                int x = rnd.Next(1, 100);
                int y = rnd.Next(1, 100);
                int z = rnd.Next(1, 30);
                query.Interval(tree, new Vector3(x - 11, y - 11, z - 2),
                    new Vector3(x + 11, y + 11, z + 2), resultIndices);
                MyConsoleProgress.Print(flag, N, "进度", resultIndices.Count.ToString());
                //Console.WriteLine(resultIndices.Count);
                //for (int i = 0; i < resultIndices.Count; i++)
                //{
                //    Console.WriteLine(pointCloud[resultIndices[i]].ToString() + "    " + Vector3.Distance(pnt, pointCloud[resultIndices[i]]));
                //}
            }

            sw.Stop();
            Console.WriteLine(sw.ElapsedMilliseconds);
        }

        public static void Mould_Test()
        {
            var g = Grid.create_from_gslibwin().grid;
            g.add_gridProperty("new");
            Mould m = Mould.create_by_ellipse(11, 11, 1);
            var mis = MouldInstance.create_from_gridProperty(m, g.first_gridProperty());
            return;
            Random rnd = new();
            int N = 100000;
            for (int i = 0; i < N; i++)
            {
                MyConsoleProgress.Print(i, N, "Mould_Test");
                short ix = (short)rnd.Next(1, 101);
                short iy = (short)rnd.Next(1, 101);
                //    short iz = (short)rnd.Next(1, 31);
                //    MouldInstance mi = MouldInstance.from_gridProperty(m, SpatialIndex.create(ix, iy, iz), g[0]);
                //    MouldInstance.to_gridProperty(m, SpatialIndex.create(ix, iy, iz), g[1], mi);
                MouldInstance mi = MouldInstance.create_from_gridProperty(m, SpatialIndex.create(ix, iy), g[0]);
                mi.paste_to_gridProperty(m, SpatialIndex.create((short)(ix - 20), (short)(iy - 20)), g[1]);
            }

            g.showGrid_win();
        }

        public static void OK_Test()
        {
            var (cd, _) = CData.read_from_gslib_win();
            GridStructure gs = GridStructure.create_win();
            Variogram vm = Variogram.create(VariogramType.Spherical, 0, 1, 40);
            Stopwatch sw = new();
            sw.Start();
            var rot_mat = new double[] { 0, 0, 0, 20, 20, 2 };
            OK.Run(gs, vm, cd, cd.property_names[0], 30, rot_mat, 5).result[1].show_win();
            sw.Stop();
            Console.WriteLine($@"Time:{sw.ElapsedMilliseconds}");
        }

        public static void Coord_Test()
        {
            GridStructure gs = GridStructure.create_win();
            Console.WriteLine(gs.to_string());
            for (int i = -100; i < 100; i++)
            {
                Coord c = Coord.create((float)(0.1 * i), 0);
                SpatialIndex si = gs.coord_to_spatial_index(c);
                if (si != null)
                {
                    Coord c2 = gs.spatial_index_to_coord(si);
                    Console.WriteLine($@"{c.ToString()}  	  {si.view_text()}   	  {c2.ToString()}");
                }
            }
        }

        public static void ExcelHelper_Test1()
        {
            var dt = ExcelHelper.excel_to_dataTable(FileDialogHelper.OpenExcel());
            DataTableHelper.show_win(dt);
        }

        public static void Matrix_Test1()
        {
            MyMatrix mat = MyMatrix.create(2, 2);
            mat[0, 0] = 1;
            mat[0, 1] = 2;
            mat[1, 0] = 1;
            mat[1, 1] = 3;
            Console.WriteLine(mat.view_text());
            MyVector v = MyVector.create(2);
            v[0] = 10;
            v[1] = 12;
            var v1 = MyMatrix.solve_mathnet(mat, v);
            Console.WriteLine(v1[0]);
            Console.WriteLine(v1[1]);
        }

        public static void DataMapper_Test()
        {
            DataMapper dm = new DataMapper();
            dm.Reset(1, 10, 1, 100);
            var v = dm.MapAToB(3);
            Console.WriteLine(v);
        }
    }
}