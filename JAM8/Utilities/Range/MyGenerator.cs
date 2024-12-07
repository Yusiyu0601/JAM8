namespace JAM8.Utilities
{
    /// <summary>
    /// 序列生成器
    /// </summary>
    public class MyGenerator
    {
        /// <summary>
        /// 可以生成指定步长的整数递增（递减）序列
        /// </summary>
        /// <param name="start">序列起点(闭区间)</param>
        /// <param name="stop">序列终点(闭区间 or 开区间,由EndClosed控制)</param>
        /// <param name="step">步长</param>
        /// <param name="end_closed">是否是闭区间，默认为否</param>
        /// <returns></returns>
        public static List<int> range(int start, int stop, int step, bool end_closed = false)
        {
            List<int> result = new();
            for (int i = start; i <= stop; i += step)
            {
                if (i != stop)//如果i没有达到End时，都满足条件的都添加
                    result.Add(i);
                else//否则
                {
                    if (end_closed)//如果是闭区间，添加End
                        result.Add(i);
                    else//否则不添加
                        continue;
                }
            }
            return result;
        }

        /// <summary>
        /// 生成一组从start到stop等距的N个数据，等效于MATLAB linspace。
        /// </summary>
        /// <param name="start"></param>
        /// <param name="stop"></param>
        /// <param name="N"></param>
        /// <returns></returns>
        public static double[] linespace(double start, double stop, int N)
        {
            return MathNet.Numerics.Generate.LinearSpaced(N, start, stop);
        }
    }
}
