using System.Diagnostics;
using JAM8.Algorithms.Geometry;
using JAM8.Utilities;
using Octree;
using Shouldly;

namespace JAM8.Work.模块测试
{

    internal class test_in_work
    {
        public static void Run()
        {
            var menu = new EasyConsole.Menu()

                .Add("退出", CommonFunctions.Cancel)
                .Add("循环", 循环测试)
                .Add("八叉树调用测试", 八叉树调用测试)
                .Add("并行卷积效率测试", 并行卷积效率测试)
                .Add("非并行卷积效率测试", 非并行卷积效率测试)
                .Add("频繁创建对象效率测试", 频繁创建对象效率测试)
                .Add("数组的乱序排列", 数组的乱序排列)
                .Add("生成高斯分布随机数", 生成高斯分布随机数)
                .Add("生成高斯分布随机数时间", 生成高斯分布随机数时间)
                .Add("创建多属性的网格", 创建多属性的网格)
                ;

            menu.Display();
        }

        private static void 循环测试()
        {
            for (int i = 0; i < 10000; i++)
            {
                Console.WriteLine($@"{i}");
            }
        }

        private static void 创建多属性的网格()
        {
            Grid g = Grid.create(GridStructure.create_win(), "grid");
            g.add_gridProperty("a");
            g.add_gridProperty("b");
            for (int i = 0; i < g.gridStructure.N; i++)
            {
                g[0].set_value(i, i);
                g[1].set_value(i, i + 1);
            }
            g.showGrid_win();
        }

        private static void 生成高斯分布随机数时间()
        {
            Stopwatch sw = new();
            sw.Start();
            int N = 100_000_000;
            List<int> l = new();
            Random rnd = new();
            for (int i = 0; i < N; i++)
            {
                l.Add(rnd.Next());
            }
            sw.Stop();
            Console.WriteLine(sw.ElapsedMilliseconds);
        }

        private static void 生成高斯分布随机数()
        {
            uint seed = 123456;
            MersenneTwister mt = new(seed);

            // 生成 10000 个符合高斯分布的随机数
            const int N = 10000;
            double mean = 0.0;
            double stddev = 1.0;

            // 创建直方图桶（区间）
            int binCount = 20;
            int[] histogram = new int[binCount];
            double min = -5.0, max = 5.0;
            double binWidth = (max - min) / binCount;

            // 生成随机数并填充直方图
            for (int i = 0; i < N; ++i)
            {
                var (gaussian1, gaussian2) = mt.NextGaussian(mean, stddev);
                int bin = (int)((gaussian1 - min) / binWidth);
                if (bin >= 0 && bin < binCount)
                {
                    histogram[bin]++;
                }
            }

            // 输出直方图
            Console.WriteLine(@"Histogram of 10000 Gaussian random numbers:");
            for (int i = 0; i < binCount; ++i)
            {
                Console.WriteLine($@"Bin {i - binCount / 2,2}: {histogram[i]}");
            }

            Console.WriteLine();
        }

        private static void 数组的乱序排列()
        {
            uint seed = 123456; // 设定相同的种子
            var mt = new MersenneTwister(seed);
            int[] data = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

            // 使用梅森旋转乱序
            Randomize(data, mt);

            Console.WriteLine(@"Shuffled data:");
            foreach (var item in data)
            {
                Console.Write(item + " ");
            }
        }
        // 基于梅森旋转乱序数据
        static void Randomize(int[] data, MersenneTwister mt)
        {
            for (int i = data.Length - 1; i > 0; i--)
            {
                int j = (int)(mt.Next() % (uint)(i + 1));
                int temp = data[i];
                data[i] = data[j];
                data[j] = temp;
            }
        }

        const int iterations = 10000000; // 全局迭代次数
        const int arraySize = 30;        // 全局数组大小
        class MyClass
        {
            public List<int> Values { get; private set; }

            public MyClass(int size)
            {
                Values = new List<int>(size);
            }

            public void Update(int iteration)
            {
                Values.Clear(); // 清空现有数据
                for (int j = 0; j < arraySize; j++)
                {
                    Values.Add(j + iteration); // 更新数组内容
                }
            }
        }
        class ObjectPool
        {
            private readonly Stack<MyClass> pool = new Stack<MyClass>();
            private readonly int size;

            public ObjectPool(int objectSize)
            {
                size = objectSize;
            }

            public MyClass Acquire()
            {
                if (pool.Count > 0)
                {
                    return pool.Pop();
                }
                else
                {
                    return new MyClass(size);
                }
            }

            public void Release(MyClass obj)
            {
                obj.Values.Clear(); // 清空数据以重用
                pool.Push(obj);
            }
        }
        static void FrequentObjectCreation()
        {
            var stopwatch = Stopwatch.StartNew();

            for (int i = 0; i < iterations; i++)
            {
                var obj = new MyClass(arraySize); // 每次都创建新对象
                for (int j = 0; j < arraySize; j++)
                {
                    obj.Values.Add(j + i); // 更新数组内容
                }
            }

            stopwatch.Stop();
            Console.WriteLine($@"Frequent Object Creation Time: {stopwatch.ElapsedMilliseconds} ms");
        }

        static void ObjectUpdate()
        {
            var obj = new MyClass(arraySize); // 只创建一个对象

            var stopwatch = Stopwatch.StartNew();

            for (int i = 0; i < iterations; i++)
            {
                obj.Update(i); // 更新对象的数组内容
            }

            stopwatch.Stop();
            Console.WriteLine($@"Object Update Time: {stopwatch.ElapsedMilliseconds} ms");
        }
        static void ObjectPoolUsage()
        {
            var pool = new ObjectPool(arraySize);

            var stopwatch = Stopwatch.StartNew();

            for (int i = 0; i < iterations; i++)
            {
                var obj = pool.Acquire();
                obj.Update(i);
                pool.Release(obj);
            }

            stopwatch.Stop();
            Console.WriteLine($@"Object Pool Usage Time: {stopwatch.ElapsedMilliseconds} ms");
        }
        private static void 频繁创建对象效率测试()
        {
            FrequentObjectCreation();
            ObjectUpdate();
            ObjectPoolUsage();
        }

        private static void 非并行卷积效率测试()
        {
            static float[,] Convolution2D(float[,] image, float[,] kernel)
            {
                int imageHeight = image.GetLength(0);
                int imageWidth = image.GetLength(1);
                int kernelHeight = kernel.GetLength(0);
                int kernelWidth = kernel.GetLength(1);

                // 计算输出图像的大小
                int outputHeight = imageHeight - kernelHeight + 1;
                int outputWidth = imageWidth - kernelWidth + 1;

                // 初始化输出图像
                float[,] output = new float[outputHeight, outputWidth];

                // 逐像素计算卷积
                for (int i = 0; i < outputHeight; i++)
                {
                    for (int j = 0; j < outputWidth; j++)
                    {
                        float sum = 0.0f;
                        for (int m = 0; m < kernelHeight; m++)
                        {
                            for (int n = 0; n < kernelWidth; n++)
                            {
                                sum += image[i + m, j + n] * kernel[m, n];
                            }
                        }
                        output[i, j] = sum;
                    }
                }

                return output;
            }

            static float[,] GenerateRandomImage(int height, int width)
            {
                Random rand = new Random();
                float[,] image = new float[height, width];
                for (int i = 0; i < height; i++)
                {
                    for (int j = 0; j < width; j++)
                    {
                        image[i, j] = (float)(rand.Next(256)); // 随机生成0-255的像素值
                    }
                }
                return image;
            }

            static float[,] CreateKernel(int size)
            {
                float[,] kernel = new float[size, size];
                float value = 1.0f / (size * size); // 均值滤波器
                for (int i = 0; i < size; i++)
                {
                    for (int j = 0; j < size; j++)
                    {
                        kernel[i, j] = value;
                    }
                }
                return kernel;
            }

            int imageSize = 5000;
            int kernelSize = 30;

            // 生成随机输入图像
            float[,] image = GenerateRandomImage(imageSize, imageSize);
            // 创建卷积核
            float[,] kernel = CreateKernel(kernelSize);

            // 记录开始时间
            Stopwatch stopwatch = Stopwatch.StartNew();

            // 进行卷积
            float[,] result = Convolution2D(image, kernel);

            // 记录结束时间
            stopwatch.Stop();

            // 打印部分结果
            Console.WriteLine(@"Convolved Image (Partial):");
            for (int i = 0; i < 10; ++i) // 仅打印前10行
            {
                for (int j = 0; j < 10; ++j) // 仅打印前10列
                {
                    Console.Write(result[i, j] + " ");
                }
                Console.WriteLine();
            }

            // 输出卷积所需时间
            Console.WriteLine($@"Time taken for convolution: {stopwatch.ElapsedMilliseconds} milliseconds.");


        }



        private static void 并行卷积效率测试()
        {
            static float[,] Convolution2D(float[,] image, float[,] kernel)
            {
                int imageHeight = image.GetLength(0);
                int imageWidth = image.GetLength(1);
                int kernelHeight = kernel.GetLength(0);
                int kernelWidth = kernel.GetLength(1);

                // 计算输出图像的大小
                int outputHeight = imageHeight - kernelHeight + 1;
                int outputWidth = imageWidth - kernelWidth + 1;

                // 初始化输出图像
                float[,] output = new float[outputHeight, outputWidth];

                // 并行处理输出图像的每个像素
                Parallel.For(0, outputHeight, i =>
                {
                    for (int j = 0; j < outputWidth; j++)
                    {
                        float sum = 0.0f;
                        for (int m = 0; m < kernelHeight; m++)
                        {
                            for (int n = 0; n < kernelWidth; n++)
                            {
                                sum += image[i + m, j + n] * kernel[m, n];
                            }
                        }
                        output[i, j] = sum;
                    }
                });

                return output;
            }

            static float[,] GenerateRandomImage(int height, int width)
            {
                Random rand = new Random();
                float[,] image = new float[height, width];
                for (int i = 0; i < height; i++)
                {
                    for (int j = 0; j < width; j++)
                    {
                        image[i, j] = (float)(rand.Next(256)); // 随机生成0-255的像素值
                    }
                }
                return image;
            }

            static float[,] CreateKernel(int size)
            {
                float[,] kernel = new float[size, size];
                float value = 1.0f / (size * size); // 均值滤波器
                for (int i = 0; i < size; i++)
                {
                    for (int j = 0; j < size; j++)
                    {
                        kernel[i, j] = value;
                    }
                }
                return kernel;
            }

            int imageSize = 5000;
            int kernelSize = 30;

            // 生成随机输入图像
            float[,] image = GenerateRandomImage(imageSize, imageSize);
            // 创建卷积核
            float[,] kernel = CreateKernel(kernelSize);

            // 记录开始时间
            Stopwatch stopwatch = Stopwatch.StartNew();

            // 进行卷积
            float[,] result = Convolution2D(image, kernel);

            // 记录结束时间
            stopwatch.Stop();

            // 打印部分结果
            Console.WriteLine(@"Convolved Image (Partial):");
            for (int i = 0; i < 10; ++i) // 仅打印前10行
            {
                for (int j = 0; j < 10; ++j) // 仅打印前10列
                {
                    Console.Write(result[i, j] + " ");
                }
                Console.WriteLine();
            }

            // 输出卷积所需时间
            Console.WriteLine($@"Time taken for convolution: {stopwatch.ElapsedMilliseconds} milliseconds.");
        }



        private static void 八叉树调用测试()
        {
            PointOctree<int> _octree;
            _octree = new PointOctree<int>(50, System.Numerics.Vector3.Zero, 1);

            // Add points
            for (int i = 1; i < 100; ++i)
                for (int j = 0; j < 100; j++)
                    for (int k = 0; k < 100; k++)
                        _octree.Add(i, new System.Numerics.Vector3(i, j, k));

            var nn = _octree.GetNearby(new System.Numerics.Vector3(10, 10, 10), 10);


            // Get single point
            for (int i = 1; i < 100; ++i)
                _octree.GetNearby(new System.Numerics.Vector3(i), 0).Length.ShouldBe(1);
            _octree.GetNearby(new System.Numerics.Vector3(100), 0).Length.ShouldBe(0);

            // Should be empty for bounding boxes that do not contain any of the geometries
            _octree.GetNearby(new System.Numerics.Vector3(0.5f), 0.2f).Length.ShouldBe(0);
            _octree.GetNearby(new System.Numerics.Vector3(100), 1).Length.ShouldBe(0);
            _octree.GetNearby(new System.Numerics.Vector3(200), 20).Length.ShouldBe(0);

            // Should find all geometries
            _octree.GetNearby(new System.Numerics.Vector3(50), 100).Length.ShouldBe(_octree.Count);

            // Should find some geometries
            _octree.GetNearby(new System.Numerics.Vector3(50), 10).Length.ShouldBe(11);

            // Non-alloc test
            List<int> result = new List<int>(new[] { 999 });
            _octree.GetNearbyNonAlloc(new System.Numerics.Vector3(50), 10, result).ShouldBeTrue();
            result.Count.ShouldBe(11);
        }
    }
}
