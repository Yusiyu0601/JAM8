namespace JAM8.Algorithms.Numerics
{
    /// <summary>
    /// 名称：数学类Math的简化类
    /// 作用：C#版本的Math类的补充功能类
    /// 作者：喻思羽
    /// 时间：2016-1-17
    /// </summary>
    public class EasyMath
    {
        /// <summary>
        /// 精度（0.0000001 或者 1E-7）
        /// </summary>
        public static readonly double ESPILON = 1E-7;

        /// <summary>
        /// 判断双精度值是否相等（满足误差范围内可认为是相等）
        /// </summary>
        /// <param name="d1">值1</param>
        /// <param name="d2">值2</param>
        /// <returns></returns>
        public static bool Equals(double d1, double d2)
        {
            return Math.Abs(d1 - d2) < ESPILON;
        }

        /// <summary>
        /// 圆周率
        /// </summary>
        public static double PI { get { return Math.PI; } }

        /// <summary>
        /// 弧度 变换为 角度
        /// </summary>
        /// <param name="Radius"></param>
        /// <returns></returns>
        public static double RadiusToAngle(double Radius)
        {
            return Radius * 180.0 / PI;
        }

        /// <summary>
        /// 角度 转换为 弧度
        /// </summary>
        /// <param name="Angle"></param>
        /// <returns></returns>
        public static double AngleToRadius(double Angle)
        {
            return Angle * PI / 180.0;
        }

        /// <summary>
        /// 计算反正弦值，返回值是角度
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public static double ASin_4Angle(double d)
        {
            return RadiusToAngle(Math.Asin(d));
        }

        /// <summary>
        /// 计算正弦值，输入值是角度
        /// </summary>
        /// <param name="Angle"></param>
        /// <returns></returns>
        public static double Sin_4Angle(double Angle)
        {
            return Math.Sin(AngleToRadius(Angle));
        }

        /// <summary>
        /// 计算余弦值，输入值是角度
        /// </summary>
        /// <param name="Angle"></param>
        /// <returns></returns>
        public static double Cos_4Angle(double Angle)
        {
            return Math.Cos(AngleToRadius(Angle));
        }

        /// <summary>
        /// 计算value值的平方
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static double Pow2(double value)
        {
            return Math.Pow(value, 2);
        }

        /// <summary>
        /// 计算value值的三次方
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static double Pow3(double value)
        {
            return Math.Pow(value, 3);
        }

        /// <summary>
        /// 开平方
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static double Sqrt(double value)
        {
            return Math.Sqrt(value);
        }

        /// <summary>
        /// 判断一个整数是否奇数
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsOddNumber(int value)
        {
            return Convert.ToBoolean(value & 1);//汇编方式计算(效率最高)
        }

        /// <summary>
        /// 是否是偶数
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsEvenNumber(int value)
        {
            if ((value & 1) == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 计算标准差
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public static double StdDev(IEnumerable<double> values)
        {
            double ret = 0;
            if (values.Count() > 0)
            {
                //计算平均数
                double avg = values.Average();
                //计算各个数值与平均数的差值平方，然后求和
                double sum = values.Sum(d => Math.Pow(d - avg, 2));
                //除以数量，然后开方
                ret = Math.Sqrt(sum / values.Count());
            }
            return ret;
        }
    }
}
