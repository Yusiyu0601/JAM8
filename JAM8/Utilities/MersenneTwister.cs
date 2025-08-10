using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JAM8.Utilities
{
    /// <summary>
    /// 
    /// </summary>
    public class MersenneTwister
    {
        private const int N = 624; // 状态向量大小
        private const int M = 397; // 魔术常数
        private uint[] mt = new uint[N]; // 梅森旋转算法的状态向量，大小为 N
        private int index; // 当前状态向量的索引
        private const uint MATRIX_A = 0x9908B0DF; // 用于梅森旋转的矩阵常数
        private const uint UPPER_MASK = 0x80000000; // 用于状态的掩码
        private const uint LOWER_MASK = 0x7FFFFFFF; // 用于状态的掩码
        private static readonly double Pi = Math.PI;

        // 构造函数
        public MersenneTwister(uint seed = 111)
        {
            mt[0] = seed; // 将种子赋值给第一个元素
            for (int i = 1; i < N; i++)
            {
                mt[i] = 0x6C078965 * (mt[i - 1] ^ mt[i - 1] >> 30) + (uint)i; // 初始化状态向量
            }

            index = 0; // 初始时索引为0
        }

        // 生成一个非负随机整数
        public uint Next()
        {
            if (index == 0)
            {
                Twist(); // 如果 index 为 0，则调用 Twist() 生成新的状态
            }

            uint y = mt[index++]; // 获取当前状态值，并递增索引
            y ^= y >> 11; // 梅森旋转算法中的移位与异或操作
            y ^= y << 7 & 0x9D2C5680;
            y ^= y << 15 & 0xEFC60000;
            y ^= y >> 18;

            index = index % N; // 保证 index 在 0 到 N-1 之间
            return y; // 返回计算后的随机数
        }

        /// <summary>
        /// 生成一个限定范围的随机数（左闭右开）
        /// </summary>
        /// <param name="minValue"></param>
        /// <param name="maxValue"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>        
        public int Next(int minValue, int maxValue)
        {
            if (maxValue <= minValue)
            {
                throw new ArgumentException("maxValue must be greater than minValue.");
            }

            uint randomValue = Next(); // 获取随机数
            uint range = (uint)(maxValue - minValue);

            return minValue + (int)(randomValue % range); // 生成 [minValue, maxValue) 的随机数
        }

        /// <summary>
        /// 将生成的32位整数映射到 [0, 1) 之间的浮点数值
        /// </summary>
        /// <returns></returns>
        public double NextDouble()
        {
            uint rand32 = Next(); // 获取 32 位无符号整数随机数
            return (double)rand32 / uint.MaxValue; // 转换为 [0, 1) 范围内的浮动数
        }

        // 生成符合高斯分布（正态分布）的随机数
        public (double, double) NextGaussian(double mean = 0.0, double stddev = 1.0)
        {
            double u1 = NextDouble(); // 生成一个 [0, 1) 范围内的随机浮点数
            double u2 = NextDouble(); // 生成另一个 [0, 1) 范围内的随机浮点数
            double z0 = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Cos(2.0 * Pi * u2); // Box-Muller 变换生成标准正态分布随机数
            double z1 = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Sin(2.0 * Pi * u2); // Box-Muller 变换生成第二个标准正态分布随机数

            return (mean + z0 * stddev, mean + z1 * stddev); // 通过调整均值和标准差生成高斯分布的随机数
        }

        // 批量生成 N 个随机数
        public List<uint> NextArray(int n)
        {
            List<uint> result = new List<uint>(n); // 创建一个指定大小的列表

            for (int i = 0; i < n; i++)
            {
                result.Add(Next()); // 批量生成随机数
            }

            return result;
        }

        // 批量生成 N 个随机数（左闭右开）
        public List<int> NextArray(int n, int minValue, int maxValue)
        {
            if (maxValue <= minValue)
            {
                throw new ArgumentException("maxValue must be greater than minValue.");
            }

            List<int> result = new List<int>(n); // 创建一个指定大小的列表
            uint range = (uint)(maxValue - minValue);

            for (int i = 0; i < n; i++)
            {
                uint randValue = Next(); // 获取一个随机数
                int mappedValue = minValue + (int)(randValue % range);
                result.Add(mappedValue); // 将映射后的值添加到结果数组中
            }

            return result;
        }

        // 批量生成 N 个浮点数，范围为 [0, 1) 之间
        public List<double> NextDoubleArray(int n)
        {
            List<double> result = new List<double>(n); // 创建一个指定大小的列表

            for (int i = 0; i < n; i++)
            {
                uint randomValue = Next(); // 获取一个随机的32位整数
                result.Add((double)randomValue / uint.MaxValue); // 映射到 [0, 1)
            }

            return result;
        }

        // 批量生成 N 个随机数，符合高斯分布（正态分布）的随机数
        public List<double> NextGaussianArray(int n, double mean = 0.0, double stddev = 1.0)
        {
            List<double> result = new List<double>(n); // 创建一个指定大小的列表

            for (int i = 0; i < n / 2; i++)
            {
                double u1 = NextDouble(); // [0, 1)
                double u2 = NextDouble(); // [0, 1)
                double z0 = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Cos(2.0 * Pi * u2); // 生成一个标准正态分布的随机数
                double z1 = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Sin(2.0 * Pi * u2); // 生成第二个标准正态分布的随机数

                result.Add(mean + z0 * stddev); // 第一个高斯分布随机数
                if (result.Count < n)
                {
                    result.Add(mean + z1 * stddev); // 第二个高斯分布随机数
                }
            }

            return result;
        }

        // 更新状态向量
        private void Twist()
        {
            for (int i = 0; i < N; i++)
            {
                uint x = (mt[i] & UPPER_MASK) + (mt[(i + 1) % N] & LOWER_MASK); // 生成新的状态
                uint xA = x >> 1; // 对 x 右移一位
                if ((x & 1) != 0) // 如果 x 的最低位为 1
                {
                    xA ^= MATRIX_A; // 使用矩阵常数修改 xA
                }

                mt[i] = mt[(i + M) % N] ^ xA; // 更新状态向量
            }
        }
    }
}