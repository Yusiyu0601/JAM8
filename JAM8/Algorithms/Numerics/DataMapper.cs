namespace JAM8.Algorithms.Numerics
{
    /// <summary>
    /// 名称：数据映射器
    /// 作用：把数据从一个区间映射到另外一个区间
    /// 作者：喻思羽
    /// 编写时间：2015-10-15
    /// </summary>
    public class DataMapper
    {
        private double m_MinA, m_MaxA, m_MinB, m_MaxB;

        public DataMapper()
        {

        }

        /// <summary>
        /// 重新设置数据映射的区间
        /// </summary>
        /// <param name="MinA">变量A的最小值</param>
        /// <param name="MaxA">变量A的最大值</param>
        /// <param name="MinB">变量B的最小值</param>
        /// <param name="MaxB">变量B的最大值</param>
        public void Reset(double MinA, double MaxA, double MinB, double MaxB)
        {
            m_MinA = MinA;
            m_MaxA = MaxA;
            m_MinB = MinB;
            m_MaxB = MaxB;
        }

        /// <summary>
        /// A是输入值
        /// B是输出值
        /// </summary>
        /// <param name="A"></param>
        /// <returns></returns>
        public double MapAToB(double A)
        {
            return (A - m_MinA) / (m_MaxA - m_MinA) * (m_MaxB - m_MinB) + m_MinB;
        }
        /// <summary>
        /// B是输入值
        /// A是输出值
        /// </summary>
        /// <param name="B"></param>
        /// <returns></returns>
        public double MapBToA(double B)
        {
            return (B - m_MinB) / (m_MaxB - m_MinB) * (m_MaxA - m_MinA) + m_MinA;
        }

        /// <summary>
        /// A是输入值,B是输出值
        /// </summary>
        /// <param name="A"></param>
        /// <returns></returns>
        public double[] MapAToB(IEnumerable<double> A)
        {
            double[] A_temp = A.ToArray();
            double[] B = new double[A_temp.Length];
            for (int i = 0; i < A_temp.Length; i++)
            {
                B[i] = MapAToB(A_temp[i]);
            }
            return B;
        }
        /// <summary>
        /// B是输入值,A是输出值
        /// </summary>
        /// <param name="B"></param>
        /// <returns></returns>
        public double[] MapBToA(IEnumerable<double> B)
        {
            double[] B_temp = B.ToArray();
            double[] A = new double[B_temp.Length];
            for (int i = 0; i < B_temp.Length; i++)
            {
                A[i] = MapBToA(B_temp[i]);
            }
            return A;
        }

        /// <summary>
        /// 数据映射的静态方法
        /// </summary>
        /// <param name="MinA"></param>
        /// <param name="MaxA"></param>
        /// <param name="MinB"></param>
        /// <param name="MaxB"></param>
        /// <param name="A"></param>
        /// <returns></returns>
        public static double MapAToB(double MinA, double MaxA, double MinB, double MaxB, double A)
        {
            return (A - MinA) / (MaxA - MinA) * (MaxB - MinB) + MinB;
        }
        /// <summary>
        /// 数据映射的静态方法
        /// </summary>
        /// <param name="MinA"></param>
        /// <param name="MaxA"></param>
        /// <param name="MinB"></param>
        /// <param name="MaxB"></param>
        /// <param name="A"></param>
        /// <returns></returns>
        public static double[] MapAToB(double MinA, double MaxA, double MinB, double MaxB, double[] A)
        {
            double[] B = new double[A.Length];
            for (int i = 0; i < A.Length; i++)
            {
                B[i] = MapAToB(MinA, MaxA, MinB, MaxB, A[i]);
            }
            return B;
        }

        /// <summary>
        /// 数据归一化方法
        /// </summary>
        /// <param name="MinInput">The minimum input.</param>
        /// <param name="MaxInput">The maximum input.</param>
        /// <param name="MinOutput">The minimum output.</param>
        /// <param name="MaxOutput">The maximum output.</param>
        /// <param name="Input">The input.</param>
        /// <returns>System.Double[].</returns>
        public static double[] Normalize(out double MinInput, out double MaxInput, double MinOutput, double MaxOutput, double[] Input)
        {
            /*  归一化到任意区间
             * 
             * 方法引自 http://blog.sina.com.cn/s/blog_46d834600100sul8.html
             * （原MATLAB方法）调用方法：
               %normalize test
               load A;
               %记住A的最大值和最小值，以便反归一化时用
               minA=min(min(A));maxA=max(max(B));
               %归一化A到B
               B=normalize(A,0,255);
               %反归一化B到A
               inverse_B=normalize(B,minA,maxA);
               %此时inverse_B应等于A
             */
            int Count = Input.Length;
            double[] Output = new double[Count];

            MinInput = Input.Min();
            MaxInput = Input.Max();

            for (int i = 0; i < Count; i++)
            {
                Output[i] = MapAToB(MinInput, MaxInput, MinOutput, MaxOutput, Input[i]);
            }
            return Output;
        }
        /// <summary>
        /// 数据归一化方法
        /// </summary>
        /// <param name="MinOutput"></param>
        /// <param name="MaxOutput"></param>
        /// <param name="Input"></param>
        /// <returns></returns>
        public static (double[], double, double) Normalize(double MinOutput, double MaxOutput, double[] Input)
        {
            double MinInput = double.MinValue;
            double MaxInput = double.MinValue;
            var output = Normalize(out MinInput, out MaxInput, MinOutput, MaxOutput, Input);
            return (output, MinInput, MaxInput);
        }
    }
}
