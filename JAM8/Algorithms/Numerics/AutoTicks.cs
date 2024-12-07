using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JAM8.Algorithms.Numerics
{
    /// <summary>
    /// 名称:TicksProperty 刻度属性
    /// 作者:喻思羽
    /// 时间:2016-4-4
    /// </summary>
    public class TicksProperty
    {
        /// <summary>
        /// 下界
        /// </summary>
        public double Lower { get; set; }
        /// <summary>
        /// 上界
        /// </summary>
        public double Upper { get; set; }
        /// <summary>
        /// 刻度间距
        /// </summary>
        public double Step { get; set; }
        /// <summary>
        /// 刻度数量
        /// </summary>
        public int TickCount { get; set; }
    }

    /// <summary>
    /// 名称:AutoTicks 自动计算坐标轴刻度
    /// 作者:喻思羽
    /// 时间:2016-4-4
    /// </summary>
    public class AutoTicks
    {
        /// <summary>
        /// 自动计算刻度方法1
        /// 参考文献《VB环境中平面曲线的绘制与坐标轴刻度的标注》
        /// 
        /// 修改bug
        /// 之前的程序无法计算刻度值间距小于1的情况，新添加了一个判断，
        /// 当Math.Log10(differX)小于0,则表示间距小于1,此时把VMin和VMax都进行了
        /// Math.Pow(10, Math.Ceiling(-Math.Log10(differX)))倍数的放大，
        /// 直到可以计算刻度值为止，计算出刻度值后，再进行相同倍数的缩小计算。
        /// 2014.7.24 喻思羽
        /// </summary>
        /// <param name="MinValue">最小值</param>
        /// <param name="MaxValue">最大值</param>
        /// <returns></returns>
        public static TicksProperty GetTicks(double MinValue, double MaxValue)
        {
            TicksProperty TicksProperty = new();

            double Step = 0.0; //坐标轴的单位长度
            double Lower = 0.0;//坐标轴的最小刻度
            double Upper = 0.0;//坐标轴的最大刻度
            int K = 0;//坐标刻度数量
            double VMin = MinValue;//数据的最小值
            double VMax = MaxValue;//数据的最大值

            //是否进行修正
            bool flag = false;

            //原始极差
            double differX = VMax - VMin;
            //修正后的极差
            double differX_new = differX;
            //进行修正
            if (Math.Log10(differX) < 0)
            {
                VMin = VMin * Math.Pow(10, Math.Ceiling(-Math.Log10(differX)));
                VMax = VMax * Math.Pow(10, Math.Ceiling(-Math.Log10(differX)));
                differX_new = VMax - VMin;
                flag = true;
            }

            int b = (int)(Math.Log(differX_new) / Math.Log(10));
            double a = differX_new / (Math.Pow(10, b));

            if (a >= 1.0 && a < 2.0)
            {
                Step = 0.2 * Math.Pow(10, b);
                Lower = Math.Floor(VMin / Step) * Step;
                Upper = Math.Ceiling(VMax / Step) * Step;
                K = (int)((Upper - Lower) / Step);
            }
            else if (a >= 2.0 && a < 5.0)
            {
                Step = 0.5 * Math.Pow(10, b);
                Lower = Math.Floor(VMin / Step) * Step;
                Upper = Math.Ceiling(VMax / Step) * Step;
                K = (int)((Upper - Lower) / Step);
            }
            else if (a >= 5.0 && a < 10.0)
            {
                Step = 1 * Math.Pow(10, b);
                Lower = Math.Floor(VMin / Step) * Step;
                Upper = Math.Ceiling(VMax / Step) * Step;
                K = (int)((Upper - Lower) / Step);
            }

            //对修正后计算的刻度进行逆向计算，回归到原始的刻度范围
            if (flag)
            {
                Step = Step / Math.Pow(10, Math.Ceiling(-Math.Log10(differX)));
                Lower = Lower / Math.Pow(10, Math.Ceiling(-Math.Log10(differX)));
                Upper = Upper / Math.Pow(10, Math.Ceiling(-Math.Log10(differX)));
            }

            TicksProperty.TickCount = K;
            TicksProperty.Lower = Lower;
            TicksProperty.Upper = Upper;
            TicksProperty.Step = Step;

            return TicksProperty;
        }

        /// <summary>
        /// 自动计算刻度方法2
        /// 参考文献《坐标值刻度的规范化标定处理》
        /// </summary>
        /// <param name="MinValue">最小值</param>
        /// <param name="MaxValue">最大值</param>
        /// <param name="TickCount">刻度数量</param>
        /// <returns></returns>
        public static TicksProperty GetTicks(double MinValue, double MaxValue, int TickCount)
        {
            TicksProperty TicksProperty = new();

            //double tmpMax, tmpMin;
            double CorStep, tmpStep;
            int tmpNumber;
            double temp;
            int extraNumber;
            double CorMax = MaxValue;
            double CorMin = MinValue;
            int CorNumber = TickCount;

            if (CorMax <= CorMin) return null;

            //计算原始步长
            CorStep = (CorMax - CorMin) / CorNumber;
            //计算步长的数量级
            int a = (int)Math.Floor(Math.Log10(CorStep));
            if (Math.Pow(10, a) == CorStep)
            {
                temp = Math.Pow(10, a);
            }
            else
            {
                temp = Math.Pow(10, a + 1);
            }

            //将步长修正到(0,1)之间
            tmpStep = CorStep / temp;
            //选取规范步长
            if (tmpStep >= 0 && tmpStep <= 0.1)
            {
                tmpStep = 0.1;
            }
            if (tmpStep >= 0.100001 && tmpStep <= 0.2)
            {
                tmpStep = 0.2;
            }
            if (tmpStep >= 0.200001 && tmpStep <= 0.25)
            {
                tmpStep = 0.25;
            }
            if (tmpStep >= 0.25001 && tmpStep <= 0.5)
            {
                tmpStep = 0.5;
            }
            if (tmpStep >= 0.50001)
            {
                tmpStep = 1.0;
            }
            //规范步长按数量级还原
            tmpStep *= temp;
            //修正起点值
            if ((int)Math.Floor(CorMin / tmpStep) != (CorMin / tmpStep))
            {
                CorMin = (int)Math.Floor(CorMin / tmpStep) * tmpStep;
            }
            //修正终点值
            if ((int)Math.Floor(CorMax / tmpStep) != (CorMax / tmpStep))
            {
                CorMax = (int)Math.Floor(CorMax / tmpStep + 1) * tmpStep;
            }
            //看最后修正是否必要，包括：刻度值、起点值、终点值
            tmpNumber = (int)Math.Floor((CorMax - CorMin) / tmpStep);
            if (tmpNumber < CorNumber)
            {
                extraNumber = CorNumber - tmpNumber;
                tmpNumber = CorNumber;
                if (extraNumber % 2 == 0)
                {
                    CorMax += tmpStep * (int)Math.Floor(extraNumber / 2.0);
                }
                else
                {
                    CorMax += tmpStep * (int)Math.Floor(extraNumber / 2.0 + 1);
                }
                CorMin -= tmpStep * (int)Math.Floor(extraNumber / 2.0);
            }
            CorNumber = tmpNumber;

            TicksProperty.TickCount = CorNumber;
            TicksProperty.Lower = CorMin;
            TicksProperty.Upper = CorMax;
            TicksProperty.Step = tmpStep;
            return TicksProperty;
        }
    }
}
