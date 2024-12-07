using static TorchSharp.torch;

namespace JAM7.DeepLearning
{
    internal static class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            //var g = Grid.create_from_gslibwin().grid;
            //for (int i = 1; i <= g.gridStructure.N; i++)
            //{
            //    if (g[0].get_value(i) != null)
            //    {
            //        g[1].set_value(i, g[0].get_value(i));
            //    }
            //}
            //g.showGrid_win();

            Console.WriteLine(cuda.is_cudnn_available());
            //空间插值1.run();
            //空间插值2.run();
            //空间插值3.run();

            //Console.WriteLine();

            NDdim_regression.train_and_predict();
            //NDimRegression.多维数据的拟合训练并保存();
            //NDimRegression.导入多维数据拟合模型并测试();
            ////LinearRegressionTest.LinearRegression();
            //Console.ReadLine();


            //var T = torch.rand(4, 4);
            //T.print();
            //Console.WriteLine(T[2, 2].item<float>());
            //Console.WriteLine();

            //torch.random.manual_seed(4711);
            //torch.rand(10).print();
            //torch.randn(10).print();
            //torch.randint(-100, 100, 20).print();
            //torch.randperm(25).print();

            //CIFAR10.Main(args);

        }
    }
}