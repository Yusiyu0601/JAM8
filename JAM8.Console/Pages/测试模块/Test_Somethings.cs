using EasyConsole;

namespace JAM8.Console.Pages
{
    class Test_Somethings : Page
    {
        public Test_Somethings(EasyConsole.Program program) : base("Test_Somethings", program) { }

        public override void Display()
        {
            base.Display();

            Output.WriteLine(ConsoleColor.Green, "Test_Somethings 功能：");

            Perform();

            System.Console.WriteLine("按任意键返回");
            System.Console.ReadKey();

            Program.NavigateBack();
        }

        private void Perform()
        {
            var menu = new EasyConsole.Menu()

           .Add("退出", CommonFunctions.Cancel)
           .Add("基于排序位置的加权方法", 基于排序位置的加权方法)
           .Add("线性衰减权重计算", 线性衰减权重计算)
           .Add("对数衰减权重计算", 对数衰减权重计算)
           .Add("高斯函数衰减权重计算", 高斯函数衰减权重计算)
           ;

            menu.Display();
        }

        private void 高斯函数衰减权重计算()
        {
            // 高斯函数衰减权重计算
            static double[] CalculateGaussianWeights(int N, double sigma)
            {
                double[] weights = new double[N];
                double sumWeights = 0;

                // 计算每个点的半高斯权重
                for (int i = 0; i < N; i++)
                {
                    // 计算半个高斯分布的权重
                    weights[i] = Math.Exp(-Math.Pow(i, 2) / (2 * Math.Pow(sigma, 2)));
                    sumWeights += weights[i];
                }

                // 归一化权重，使得所有权重之和为1
                for (int i = 0; i < N; i++)
                {
                    weights[i] /= sumWeights;
                }

                return weights;
            }

            // 打印前10个权重
            static void PrintTopWeights(double[] weights, int topN)
            {
                System.Console.WriteLine($"前{topN}个点的权重：");
                for (int i = 0; i < topN && i < weights.Length; i++)
                {
                    System.Console.WriteLine($"点{i + 1}: {weights[i]:F4}");
                }
            }

            int N = 1000; // 这里可以改成100或10000，表示点的数量
            double sigma = 200; // 控制高斯函数衰减速度的标准差

            // 计算高斯衰减权重
            double[] gaussianWeights = CalculateGaussianWeights(N, sigma);

            // 打印前10个高斯衰减权重
            System.Console.WriteLine("高斯衰减权重：");
            PrintTopWeights(gaussianWeights, 1000);
        }

        private void 对数衰减权重计算()
        {
            // 对数衰减权重计算
            static double[] CalculateLogWeights(int N)
            {
                double[] weights = new double[N];
                double sumWeights = 0;

                // 计算每个点的对数衰减权重
                for (int i = 0; i < N; i++)
                {
                    weights[i] = Math.Log(i + 1);  // i + 1 因为排序是从1开始
                    sumWeights += weights[i];
                }

                // 归一化权重，使得所有权重之和为1
                for (int i = 0; i < N; i++)
                {
                    weights[i] /= sumWeights;
                }

                return weights;
            }
            // 打印前10个权重
            static void PrintTopWeights(double[] weights, int topN)
            {
                System.Console.WriteLine($"前{topN}个点的权重：");
                for (int i = 0; i < topN && i < weights.Length; i++)
                {
                    System.Console.WriteLine($"点{i + 1}: {weights[i]:F4}");
                }
            }

            int N = 100; // 这里可以改成100或10000，表示点的数量

            // 计算对数衰减权重
            double[] logWeights = CalculateLogWeights(N);

            // 打印前10个对数衰减权重
            System.Console.WriteLine("\n对数衰减权重：");
            PrintTopWeights(logWeights, 10);
        }

        private void 线性衰减权重计算()
        {
            // 线性衰减权重计算
            static double[] CalculateLinearWeights(int N)
            {
                double[] weights = new double[N];
                double sumWeights = 0;

                // 计算每个点的平方根衰减权重
                for (int i = 0; i < N; i++)
                {
                    weights[i] = 1.0 / Math.Sqrt(i + 1);  // 使用平方根衰减
                    sumWeights += weights[i];
                }

                // 归一化权重，使得所有权重之和为1
                for (int i = 0; i < N; i++)
                {
                    weights[i] /= sumWeights;
                }

                return weights;
            }
            // 打印前10个权重
            static void PrintTopWeights(double[] weights, int topN)
            {
                System.Console.WriteLine($"前{topN}个点的权重：");
                for (int i = 0; i < topN && i < weights.Length; i++)
                {
                    System.Console.WriteLine($"点{i + 1}: {weights[i]:F4}");
                }
            }
            int N = 1000; // 这里可以改成100或10000，表示点的数量

            // 计算线性衰减权重
            double[] linearWeights = CalculateLinearWeights(N);

            // 打印前10个线性衰减权重
            System.Console.WriteLine("线性衰减权重：");
            PrintTopWeights(linearWeights, 100);
        }

        private void 基于排序位置的加权方法()
        {
            static double[] CalculateWeights(int N, double p)
            {
                // 计算每个点的权重
                double[] weights = new double[N];
                double sumWeights = 0;

                // 计算每个点的权重
                for (int i = 0; i < N; i++)
                {
                    weights[i] = 1.0 / Math.Pow(i + 1, p); // i+1 因为排序是从1开始
                    sumWeights += weights[i];
                }

                // 归一化权重，使得所有权重之和为1
                for (int i = 0; i < N; i++)
                {
                    weights[i] /= sumWeights;
                }

                return weights;
            }

            // 打印前10个权重
            static void PrintTopWeights(double[] weights, int topN)
            {
                System.Console.WriteLine($"前{topN}个点的权重：");
                for (int i = 0; i < topN && i < weights.Length; i++)
                {
                    System.Console.WriteLine($"点{i + 1}: {weights[i]:F4}");
                }
            }

            int N = 10000; // 这里可以改成100或10000，表示点的数量
            double p = 1; // 幂次，可以根据需要调整

            // 计算权重
            double[] weights = CalculateWeights(N, p);

            // 打印前10个权重
            PrintTopWeights(weights, 1000);
        }
    }
}
