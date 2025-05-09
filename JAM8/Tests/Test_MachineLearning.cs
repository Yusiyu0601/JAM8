using JAM8.Algorithms.Geometry;
using System.Diagnostics;
using JAM8.Utilities;
using JAM8.Algorithms.MachineLearning;
using JAM8.Algorithms.Numerics;

namespace JAM8.Tests
{
    public class Test_MachineLearning
    {
        public static void SVM_test()
        {
            MyDataFrame df = MyDataFrame.read_from_excel();
            df.show_win();
            var model = Algorithms.MachineLearning.MySVM.train(df, df.series_names.Take(df.N_Series - 1).ToArray(),
                  df.series_names[df.series_names.Length - 1]);
            var predict = Algorithms.MachineLearning.MySVM.predict(df, df.series_names.Take(df.N_Series - 1).ToArray(), model);
            predict.show_win();
        }

        public static void LSH_test()
        {
            GridStructure gs = GridStructure.create_win();
            GridProperty gp = GridProperty.create(gs);
            gp.set_values_gaussian(0, 1, new Random(1));
            CData cd = CData.create_from_gridProperty(gp, null, false);
            List<MyVector> my_vectors = new();
            for (int n = 0; n < gs.N; n++)
            {
                SpatialIndex si = gs.get_spatialIndex(n);
                my_vectors.Add(MyVector.create(new float[] { si.ix, si.iy, si.iz }));
            }
            PStableLSH lsh = new(3, 3, 0.1f, 0.05f, 1);
            lsh.MapVectorToHashTable(my_vectors);
            Stopwatch sw = new();
            sw.Start();
            Console.WriteLine(@"start");
            Random rnd = new();
            for (int i = 0; i < 10000000; i++)
            {
                SpatialIndex si = SpatialIndex.create(rnd.Next(0, gs.nx), rnd.Next(0, gs.ny), rnd.Next(0, gs.nz));
                var (a, b) = lsh.Search(MyVector.create(new float[] { si.ix, si.iy, si.iz }));
                MyConsoleProgress.Print(i, 10000000, a.Count.ToString());
            }
            Console.WriteLine(@"end");
            sw.Stop();
            Console.WriteLine(sw.ElapsedMilliseconds);
        }
    }
}
