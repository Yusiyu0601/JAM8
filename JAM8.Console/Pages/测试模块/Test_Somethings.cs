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

            System.Console.WriteLine(@"按任意键返回");
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
           .Add("莫兰指数", 莫兰指数)
           .Add("分形维数", 分形维数)
           .Add("SpatialEntropy", SpatialEntropy)
           ;

            menu.Display();
        }

        private void SpatialEntropy()
        {
            static double CalculateEntropy(double[,] grid)
            {
                // 将网格数据离散化为若干区间
                int bins = 10; // 分成10个区间
                double min = grid.Cast<double>().Min();
                double max = grid.Cast<double>().Max();
                double binSize = (max - min) / bins;

                // 统计每个区间的频率
                var frequencies = new int[bins];
                foreach (var value in grid)
                {
                    int binIndex = Math.Min((int)((value - min) / binSize), bins - 1);
                    frequencies[binIndex]++;
                }

                // 转换为概率分布
                int totalCount = grid.Length;
                var probabilities = frequencies.Select(f => (double)f / totalCount).ToArray();

                // 计算熵值
                double entropy = 0;
                foreach (var p in probabilities)
                {
                    if (p > 0)
                        entropy -= p * Math.Log(p);
                }

                return entropy;
            }
            // 生成随机分布网格
            static double[,] GenerateRandomGrid(int rows, int cols, double variation)
            {
                var rand = new Random();
                double[,] grid = new double[rows, cols];
                for (int i = 0; i < rows; i++)
                    for (int j = 0; j < cols; j++)
                        grid[i, j] = rand.NextDouble() * variation;
                return grid;
            }

            // 生成均匀分布网格
            static double[,] GenerateUniformGrid(int rows, int cols)
            {
                double[,] grid = new double[rows, cols];
                for (int i = 0; i < rows; i++)
                    for (int j = 0; j < cols; j++)
                        grid[i, j] = 0.5; // 恒定值
                return grid;
            }

            // 生成三个不同平稳性的网格
            double[,] lowStabilityGrid = GenerateRandomGrid(100, 100, 0.8);
            double[,] mediumStabilityGrid = GenerateRandomGrid(100, 100, 0.4);
            double[,] highStabilityGrid = GenerateUniformGrid(100, 100);

            // 计算熵值
            System.Console.WriteLine($@"低平稳性网格的熵值: {CalculateEntropy(lowStabilityGrid):F4}");
            System.Console.WriteLine($@"中平稳性网格的熵值: {CalculateEntropy(mediumStabilityGrid):F4}");
            System.Console.WriteLine($@"高平稳性网格的熵值: {CalculateEntropy(highStabilityGrid):F4}");
        }

        class FractalDimensionCalculator
        {
            // 差分盒子计数法计算分形维数
            public static double CalculateFractalDimension(double[,] data, int minBoxSize = 5, int maxBoxSize = -1, int numBoxes = 10)
            {
                int rows = data.GetLength(0);
                int cols = data.GetLength(1);

                // 设置最大盒子大小
                if (maxBoxSize == -1 || maxBoxSize > Math.Min(rows, cols))
                    maxBoxSize = Math.Min(rows, cols);

                // 生成盒子大小的列表（对数均匀分布）
                List<int> boxSizes = GenerateLogSpace(minBoxSize, maxBoxSize, numBoxes);

                List<double> sizesLog = new List<double>();
                List<double> countsLog = new List<double>();

                foreach (int boxSize in boxSizes)
                {
                    int boxCount = MeasureBoxCount(data, boxSize);
                    if (boxCount > 0)
                    {
                        sizesLog.Add(Math.Log(boxSize));
                        countsLog.Add(Math.Log(boxCount));
                    }
                }

                // 使用线性拟合计算斜率，斜率的绝对值即分形维数
                double slope = LinearFit(sizesLog.ToArray(), countsLog.ToArray());
                return -slope; // 返回分形维数
            }

            // 测量盒子的数量
            private static int MeasureBoxCount(double[,] data, int boxSize)
            {
                int rows = data.GetLength(0);
                int cols = data.GetLength(1);

                int blockCount = 0;

                for (int i = 0; i < rows; i += boxSize)
                {
                    for (int j = 0; j < cols; j += boxSize)
                    {
                        double min = double.MaxValue;
                        double max = double.MinValue;

                        for (int x = i; x < Math.Min(i + boxSize, rows); x++)
                        {
                            for (int y = j; y < Math.Min(j + boxSize, cols); y++)
                            {
                                double value = data[x, y];
                                if (value > max) max = value;
                                if (value < min) min = value;
                            }
                        }

                        // 计算盒子的层数
                        if (max != min)
                        {
                            blockCount++;
                        }
                    }
                }

                return blockCount;
            }

            // 生成对数均匀分布的盒子大小列表
            private static List<int> GenerateLogSpace(int minBoxSize, int maxBoxSize, int numBoxes)
            {
                List<int> sizes = new List<int>();
                double logMin = Math.Log(minBoxSize);
                double logMax = Math.Log(maxBoxSize);

                for (int i = 0; i < numBoxes; i++)
                {
                    double logSize = logMin + i * (logMax - logMin) / (numBoxes - 1);
                    int size = (int)Math.Round(Math.Exp(logSize));
                    if (!sizes.Contains(size))
                        sizes.Add(size);
                }

                return sizes;
            }

            // 线性拟合，返回斜率
            private static double LinearFit(double[] x, double[] y)
            {
                int n = x.Length;

                double sumX = 0, sumY = 0, sumXY = 0, sumX2 = 0;

                for (int i = 0; i < n; i++)
                {
                    sumX += x[i];
                    sumY += y[i];
                    sumXY += x[i] * y[i];
                    sumX2 += x[i] * x[i];
                }

                double slope = (n * sumXY - sumX * sumY) / (n * sumX2 - sumX * sumX);
                return slope;
            }

        }
        private void 分形维数()
        {
            // 示例二维数组1（棋盘格模式）
            double[,] data1 = {
            { 1, 0, 1, 0 },
            { 0, 1, 1, 1 },
            { 1, 1, 1, 0 },
            { 0, 0, 1, 1 }
        };

            // 示例二维数组2（条带模式）
            double[,] data2 = {
            { 1, 1, 0, 0 },
            { 1, 0, 1, 0 },
            { 1, 0, 1, 0 },
            { 1, 1, 0, 0 }
        };

            double fractalDimension1 = FractalDimensionCalculator.CalculateFractalDimension(data1, 3);
            double fractalDimension2 = FractalDimensionCalculator.CalculateFractalDimension(data2, 3);

            System.Console.WriteLine($@"数据1的分形维数：{Math.Abs(fractalDimension1)}");
            System.Console.WriteLine($@"数据2的分形维数：{Math.Abs(fractalDimension2)}");
        }

        public class Moran
        {
            // Fields
            private double[,] _image;
            private int _rowNum;
            private int _columnNum;
            private double _nodataValue;
            private int[,] _matrixIndex;
            private double[] _valueIndex;
            private int[,] _weightMatrix;
            private int _pixelCount;
            private double _imageMean;
            private int _weightMatrixSum;

            // Constructor
            public Moran(double[,] image, int rowNum, int columnNum, double nodataValue)
            {
                _image = image;
                _rowNum = rowNum;
                _columnNum = columnNum;
                _nodataValue = nodataValue;
                CalculateIndex();
                CalculateWeightMatrix();
            }

            // Calculate the raster index mapping matrix
            private void CalculateIndex()
            {
                _pixelCount = 0;
                _matrixIndex = new int[_rowNum, _columnNum];
                _valueIndex = new double[_rowNum * _columnNum];

                for (int row = 0; row < _rowNum; row++)
                {
                    for (int col = 0; col < _columnNum; col++)
                    {
                        if (_image[row, col] == _nodataValue)
                        {
                            _matrixIndex[row, col] = -1; // Mark as invalid
                        }
                        else
                        {
                            _matrixIndex[row, col] = _pixelCount;
                            _valueIndex[_pixelCount] = _image[row, col];
                            _pixelCount++;
                        }
                    }
                }
            }

            // Calculate the spatial weight matrix
            private void CalculateWeightMatrix()
            {
                _weightMatrix = new int[_pixelCount, _pixelCount];
                _weightMatrixSum = 0;
                _imageMean = 0;

                for (int row = 0; row < _rowNum; row++)
                {
                    for (int col = 0; col < _columnNum; col++)
                    {
                        if (_image[row, col] == _nodataValue)
                            continue;

                        _imageMean += _image[row, col];

                        for (int i = -1; i <= 1; i++)
                        {
                            for (int j = -1; j <= 1; j++)
                            {
                                if (i == 0 && j == 0)
                                    continue;

                                int neighborRow = row + i;
                                int neighborCol = col + j;

                                if (neighborRow >= 0 && neighborRow < _rowNum && neighborCol >= 0 && neighborCol < _columnNum)
                                {
                                    if (_image[neighborRow, neighborCol] != _nodataValue)
                                    {
                                        _weightMatrixSum++;
                                        int indexI = _matrixIndex[row, col];
                                        int indexJ = _matrixIndex[neighborRow, neighborCol];
                                        _weightMatrix[indexI, indexJ] = 1;
                                    }
                                }
                            }
                        }
                    }
                }

                _imageMean /= _pixelCount;
            }

            // Calculate Moran's I
            public double CalculateMoransI()
            {
                double numerator = 0;
                for (int i = 0; i < _pixelCount; i++)
                {
                    for (int j = 0; j < _pixelCount; j++)
                    {
                        numerator += _weightMatrix[i, j] * (_valueIndex[i] - _imageMean) * (_valueIndex[j] - _imageMean);
                    }
                }

                double denominator = 0;
                for (int i = 0; i < _pixelCount; i++)
                {
                    denominator += (_valueIndex[i] - _imageMean) * (_valueIndex[i] - _imageMean);
                }

                return _pixelCount * numerator / (_weightMatrixSum * denominator);
            }
        }
        private void 莫兰指数()
        {
            int numRows = 5;
            int numCols = 5;
            double[,] testArray = new double[numRows, numCols];
            Random random = new();

            for (int i = 0; i < numRows; i++)
            {
                for (int j = 0; j < numCols; j++)
                {
                    testArray[i, j] = random.NextDouble();
                }
            }

            // Print the generated array
            System.Console.WriteLine(@"Generated Array:");
            for (int i = 0; i < numRows; i++)
            {
                for (int j = 0; j < numCols; j++)
                {
                    System.Console.Write($"{testArray[i, j]:0.00} ");
                }
                System.Console.WriteLine();
            }

            Moran moran = new(testArray, numRows, numCols, -9999);
            double moransI = moran.CalculateMoransI();
            System.Console.WriteLine($@"Moran's I: {moransI}");
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
                System.Console.WriteLine($@"前{topN}个点的权重：");
                for (int i = 0; i < topN && i < weights.Length; i++)
                {
                    System.Console.WriteLine($@"点{i + 1}: {weights[i]:F4}");
                }
            }

            int N = 1000; // 这里可以改成100或10000，表示点的数量
            double sigma = 200; // 控制高斯函数衰减速度的标准差

            // 计算高斯衰减权重
            double[] gaussianWeights = CalculateGaussianWeights(N, sigma);

            // 打印前10个高斯衰减权重
            System.Console.WriteLine(@"高斯衰减权重：");
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
                System.Console.WriteLine($@"前{topN}个点的权重：");
                for (int i = 0; i < topN && i < weights.Length; i++)
                {
                    System.Console.WriteLine($@"点{i + 1}: {weights[i]:F4}");
                }
            }

            int N = 100; // 这里可以改成100或10000，表示点的数量

            // 计算对数衰减权重
            double[] logWeights = CalculateLogWeights(N);

            // 打印前10个对数衰减权重
            System.Console.WriteLine(@"
对数衰减权重：");
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
                System.Console.WriteLine($@"前{topN}个点的权重：");
                for (int i = 0; i < topN && i < weights.Length; i++)
                {
                    System.Console.WriteLine($@"点{i + 1}: {weights[i]:F4}");
                }
            }
            int N = 1000; // 这里可以改成100或10000，表示点的数量

            // 计算线性衰减权重
            double[] linearWeights = CalculateLinearWeights(N);

            // 打印前10个线性衰减权重
            System.Console.WriteLine(@"线性衰减权重：");
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
                System.Console.WriteLine($@"前{topN}个点的权重：");
                for (int i = 0; i < topN && i < weights.Length; i++)
                {
                    System.Console.WriteLine($@"点{i + 1}: {weights[i]:F4}");
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
