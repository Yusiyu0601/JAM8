namespace JAM8.Utilities
{
    /// <summary>
    /// 序列生成器
    /// </summary>
    public class MyGenerator
    {
        /// <summary>
        /// 生成一个整数序列（左闭右开），例如 range(1, 5) → [1, 2, 3, 4]
        /// 用法与 Python 的 range 函数类似。
        /// </summary>
        /// <param name="start">起始值（包含）</param>
        /// <param name="stop">终止值（不包含）</param>
        /// <param name="step">步长，不能为 0。正数表示递增，负数表示递减。</param>
        /// <returns>
        /// 一个整数列表，包含从 <paramref name="start"/> 开始，按照 <paramref name="step"/> 递增或递减，
        /// 直到到达或超过（不包含）<paramref name="stop"/> 为止的所有整数。
        /// </returns>
        /// <exception cref="ArgumentException">当 <paramref name="step"/> 为零时抛出。</exception>
        /// <example>
        /// 示例：
        /// <code>
        /// var r1 = range(1, 5);        // [1, 2, 3, 4]
        /// var r2 = range(5, 1, -1);    // [5, 4, 3, 2]
        /// var r3 = range(0, 10, 3);    // [0, 3, 6, 9]
        /// </code>
        /// </example>
        public static List<int> range(int start, int stop, int step = 1)
        {
            if (step == 0)
                throw new ArgumentException("Step cannot be zero.");

            List<int> result = new();
            if (step > 0)
            {
                for (int i = start; i < stop; i += step)
                    result.Add(i);
            }
            else
            {
                for (int i = start; i > stop; i += step)
                    result.Add(i);
            }

            return result;
        }

        /// <summary>
        /// 生成从 <paramref name="start"/> 到 <paramref name="stop"/> 的等间距 double 数组，
        /// 类似 MATLAB 或 NumPy 的 linspace 函数。
        /// 起点总是包含，是否包含终点由 <paramref name="endpoint"/> 决定。
        /// 示例：
        /// linspace(0.0, 1.0, 5)        → [0.0, 0.25, 0.5, 0.75, 1.0];
        /// linspace(0.0, 1.0, 5, false) → [0.0, 0.2, 0.4, 0.6, 0.8]
        /// </summary>
        /// <param name="start">起始值（包含）</param>
        /// <param name="stop">终止值（是否包含由 endpoint 决定）</param>
        /// <param name="N">生成点数，必须 ≥ 2</param>
        /// <param name="endpoint">是否包含终点，默认 true</param>
        /// <returns>等间距 double 数组，长度为 N</returns>
        /// <exception cref="ArgumentException">当 N 小于 2 时抛出</exception>
        public static double[] linspace(double start, double stop, int N, bool endpoint = true)
        {
            if (N < 2)
                throw new ArgumentException("N must be at least 2.");

            double[] result = new double[N];

            double step = endpoint
                ? (stop - start) / (N - 1)
                : (stop - start) / N;

            for (int i = 0; i < N; i++)
            {
                result[i] = start + step * i;
            }

            if (endpoint)
            {
                result[N - 1] = stop; // 强制终点精确等于 stop，防止浮点误差
            }

            return result;
        }
    }
}