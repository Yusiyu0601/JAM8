using JAM8.Algorithms.Numerics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JAM8.Algorithms.Images
{
    /// <summary>
    /// 名称：ColorMapEnum 颜色映射类型枚举
    /// 作用：颜色映射类型
    /// 作者：喻思羽
    /// 编写时间：2015-10-15
    /// </summary>
    public enum ColorMapEnum
    {
        Spring,
        Summer,
        Autumn,
        Winter,
        Gray,
        GrayR,//灰度翻转
        Jet,
        Hot,
        Cool
    }

    /// <summary>
    /// 名称：ColorMap 颜色映射表
    /// 作用：把数值与颜色进行映射
    /// 作者：Jack J. H. Xu
    /// 网址：http://www.codeproject.com/Articles/18150/Create-Custom-Color-Maps-in-C
    /// 
    /// 修改1：喻思羽
    /// 修改内容：
    /// 1.删除了两个构造函数，修改了构造函数（添加了参数，具体参考代码）
    /// 2.添加了MapValueToColor方法，方便数值向颜色转换
    /// 编写时间：2015-10-15
    /// </summary>
    public class ColorMap
    {
        #region 通用操作接口

        private bool m_IsContinue = true;

        public Color MapValueToColor(double Value)
        {
            if (m_IsContinue)//连续变量
            {
                return MapContinueValueToColor(Value);
            }
            else//离散变量
            {
                return MapDiscreteValueToColor(Value);
            }
            throw new Exception("暂时不知道会出现什么异常");
        }

        #endregion

        #region continue类型变量

        private int colormapLength = 64;//颜色映射表的颜色总数量
        private int alphaValue = 255;//透明度
        private double m_Min, m_Max;//数据的最小值和最大值
        private int[,] ColorMapTable = null;//颜色映射表

        /// <summary>
        /// ColorMap 构造函数（continue类型变量）
        /// </summary>
        /// <param name="Min">最小值</param>
        /// <param name="Max">最大值</param>
        /// <param name="colorLength">颜色范围大小，默认64</param>
        /// <param name="alpha">透明度,默认255</param>
        /// <param name="ColorMapEnum">颜色映射类型</param>
        public ColorMap(double Min, double Max, int colorLength = 64, int alpha = 255, ColorMapEnum ColorMapEnum = ColorMapEnum.Jet)
        {
            m_IsContinue = true;

            m_Min = Min;
            m_Max = Max;
            colormapLength = colorLength;
            alphaValue = alpha;
            switch (ColorMapEnum)
            {
                case ColorMapEnum.Spring:
                    ColorMapTable = Spring();
                    break;
                case ColorMapEnum.Summer:
                    ColorMapTable = Summer();
                    break;
                case ColorMapEnum.Autumn:
                    ColorMapTable = Autumn();
                    break;
                case ColorMapEnum.Winter:
                    ColorMapTable = Winter();
                    break;
                case ColorMapEnum.Gray:
                    ColorMapTable = Gray();
                    break;
                case ColorMapEnum.GrayR:
                    ColorMapTable = GrayR();
                    break;
                case ColorMapEnum.Jet:
                    ColorMapTable = Jet();
                    break;
                case ColorMapEnum.Hot:
                    ColorMapTable = Hot();
                    break;
                case ColorMapEnum.Cool:
                    ColorMapTable = Cool();
                    break;
            }
        }

        #region 不同颜色搭配

        private int[,] Spring()
        {
            int[,] cmap = new int[colormapLength, 4];
            float[] spring = new float[colormapLength];
            for (int i = 0; i < colormapLength; i++)
            {
                spring[i] = 1.0f * i / (colormapLength - 1);
                cmap[i, 0] = alphaValue;
                cmap[i, 1] = 255;
                cmap[i, 2] = (int)(255 * spring[i]);
                cmap[i, 3] = 255 - cmap[i, 1];
            }
            return cmap;
        }

        private int[,] Summer()
        {
            int[,] cmap = new int[colormapLength, 4];
            float[] summer = new float[colormapLength];
            for (int i = 0; i < colormapLength; i++)
            {
                summer[i] = 1.0f * i / (colormapLength - 1);
                cmap[i, 0] = alphaValue;
                cmap[i, 1] = (int)(255 * summer[i]);
                cmap[i, 2] = (int)(255 * 0.5f * (1 + summer[i]));
                cmap[i, 3] = (int)(255 * 0.4f);
            }
            return cmap;
        }

        private int[,] Autumn()
        {
            int[,] cmap = new int[colormapLength, 4];
            float[] autumn = new float[colormapLength];
            for (int i = 0; i < colormapLength; i++)
            {
                autumn[i] = 1.0f * i / (colormapLength - 1);
                cmap[i, 0] = alphaValue;
                cmap[i, 1] = 255;
                cmap[i, 2] = (int)(255 * autumn[i]);
                cmap[i, 3] = 0;
            }
            return cmap;
        }

        private int[,] Winter()
        {
            int[,] cmap = new int[colormapLength, 4];
            float[] winter = new float[colormapLength];
            for (int i = 0; i < colormapLength; i++)
            {
                winter[i] = 1.0f * i / (colormapLength - 1);
                cmap[i, 0] = alphaValue;
                cmap[i, 1] = 0;
                cmap[i, 2] = (int)(255 * winter[i]);
                cmap[i, 3] = (int)(255 * (1.0f - 0.5f * winter[i]));
            }
            return cmap;
        }

        private int[,] Gray()
        {
            int[,] cmap = new int[colormapLength, 4];
            float[] gray = new float[colormapLength];
            for (int i = 0; i < colormapLength; i++)
            {
                gray[i] = 1.0f * i / (colormapLength - 1);
                cmap[i, 0] = alphaValue;
                cmap[i, 1] = (int)(255 * gray[i]);
                cmap[i, 2] = (int)(255 * gray[i]);
                cmap[i, 3] = (int)(255 * gray[i]);
            }
            return cmap;
        }

        private int[,] GrayR()
        {
            int[,] cmap = new int[colormapLength, 4];
            float[] gray = new float[colormapLength];
            for (int i = 0; i < colormapLength; i++)
            {
                gray[i] = 1 - 1.0f * i / (colormapLength - 1);
                cmap[i, 0] = alphaValue;
                cmap[i, 1] = (int)(255 * gray[i]);
                cmap[i, 2] = (int)(255 * gray[i]);
                cmap[i, 3] = (int)(255 * gray[i]);
            }
            return cmap;
        }

        private int[,] Jet()
        {
            int[,] cmap = new int[colormapLength, 4];
            float[,] cMatrix = new float[colormapLength, 3];
            int n = (int)Math.Ceiling(colormapLength / 4.0f);
            int nMod = 0;
            float[] fArray = new float[3 * n - 1];
            int[] red = new int[fArray.Length];
            int[] green = new int[fArray.Length];
            int[] blue = new int[fArray.Length];

            if (colormapLength % 4 == 1)
            {
                nMod = 1;
            }

            for (int i = 0; i < fArray.Length; i++)
            {
                if (i < n)
                    fArray[i] = (float)(i + 1) / n;
                else if (i >= n && i < 2 * n - 1)
                    fArray[i] = 1.0f;
                else if (i >= 2 * n - 1)
                    fArray[i] = (float)(3 * n - 1 - i) / n;
                green[i] = (int)Math.Ceiling(n / 2.0f) - nMod + i;
                red[i] = green[i] + n;
                blue[i] = green[i] - n;
            }

            int nb = 0;
            for (int i = 0; i < blue.Length; i++)
            {
                if (blue[i] > 0)
                    nb++;
            }

            for (int i = 0; i < colormapLength; i++)
            {
                for (int j = 0; j < red.Length; j++)
                {
                    if (i == red[j] && red[j] < colormapLength)
                    {
                        cMatrix[i, 0] = fArray[i - red[0]];
                    }
                }
                for (int j = 0; j < green.Length; j++)
                {
                    if (i == green[j] && green[j] < colormapLength)
                        cMatrix[i, 1] = fArray[i - (int)green[0]];
                }
                for (int j = 0; j < blue.Length; j++)
                {
                    if (i == blue[j] && blue[j] >= 0)
                        cMatrix[i, 2] = fArray[fArray.Length - 1 - nb + i];
                }
            }

            for (int i = 0; i < colormapLength; i++)
            {
                cmap[i, 0] = alphaValue;
                for (int j = 0; j < 3; j++)
                {
                    cmap[i, j + 1] = (int)(cMatrix[i, j] * 255);
                }
            }
            return cmap;
        }

        private int[,] Hot()
        {
            int[,] cmap = new int[colormapLength, 4];
            int n = 3 * colormapLength / 8;
            float[] red = new float[colormapLength];
            float[] green = new float[colormapLength];
            float[] blue = new float[colormapLength];
            for (int i = 0; i < colormapLength; i++)
            {
                if (i < n)
                    red[i] = 1.0f * (i + 1) / n;
                else
                    red[i] = 1.0f;
                if (i < n)
                    green[i] = 0f;
                else if (i >= n && i < 2 * n)
                    green[i] = 1.0f * (i + 1 - n) / n;
                else
                    green[i] = 1f;
                if (i < 2 * n)
                    blue[i] = 0f;
                else
                    blue[i] = 1.0f * (i + 1 - 2 * n) / (colormapLength - 2 * n);
                cmap[i, 0] = alphaValue;
                cmap[i, 1] = (int)(255 * red[i]);
                cmap[i, 2] = (int)(255 * green[i]);
                cmap[i, 3] = (int)(255 * blue[i]);
            }
            return cmap;
        }

        private int[,] Cool()
        {
            int[,] cmap = new int[colormapLength, 4];
            float[] cool = new float[colormapLength];
            for (int i = 0; i < colormapLength; i++)
            {
                cool[i] = 1.0f * i / (colormapLength - 1);
                cmap[i, 0] = alphaValue;
                cmap[i, 1] = (int)(255 * cool[i]);
                cmap[i, 2] = (int)(255 * (1 - cool[i]));
                cmap[i, 3] = 255;
            }
            return cmap;
        }

        #endregion

        /// <summary>
        /// 数值映射到颜色表
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        private Color MapContinueValueToColor(double Value)
        {
            if (double.IsNaN(Value) || double.IsInfinity(Value))
                throw new Exception("数值异常，即数值映射到颜色表");
            //如果无法映射，则返回护眼色
            if (m_Min == m_Max) return Color.FromArgb(255, 207, 232, 207);

            int colorIndex = (int)((Value - m_Min) / (m_Max - m_Min) * (colormapLength - 1));
            return Color.FromArgb(ColorMapTable[colorIndex, 0], ColorMapTable[colorIndex, 1], ColorMapTable[colorIndex, 2], ColorMapTable[colorIndex, 3]);
        }

        #endregion

        #region discrete类型变量

        private Dictionary<double, Color> m_DiscreteColorDictionary = null;//离散变量=>颜色映射表
        private List<double> m_DiscreteList = null;//离散变量表
        private ColorMapEnum m_DiscreteColorMapEnum = ColorMapEnum.Summer;

        /// <summary>
        /// ColorMap 构造函数（discrete类型变量）
        /// </summary>
        /// <param name="DiscreteList"></param>
        /// <param name="ColorMapEnum"></param>
        public ColorMap(List<double> DiscreteList, ColorMapEnum ColorMapEnum = ColorMapEnum.Jet)
        {
            m_IsContinue = false;

            if (DiscreteList == null) throw new Exception("离散数据不能为空");
            if (DiscreteList.Count == 0) throw new Exception("离散数据不能为空");
            m_DiscreteList = DiscreteList;
            m_DiscreteColorMapEnum = ColorMapEnum;
            InitializeDiscreteColorDictionary();
        }

        //初始化离散颜色映射字典
        private void InitializeDiscreteColorDictionary()
        {
            int Number = m_DiscreteList.Count;
            m_DiscreteColorDictionary = new Dictionary<double, Color>();

            float min, max;
            if (Number == 1)//如果离散变量只有一个值，给最大值增加1
            {
                min = (float)m_DiscreteList.Min();
                max = min + 1;
            }
            else
            {
                min = (float)m_DiscreteList.Min();
                max = (float)m_DiscreteList.Max();
            }


            DataMapper mapper = new();//原始数据映射到[0,100]
            mapper.Reset(min, max, 0, 100);

            ColorMap ColorMap = new ColorMap(0, 100, 64, 255, m_DiscreteColorMapEnum);//根据[0,100]计算颜色映射表

            for (int i = 0; i < Number; i++)
            {
                double value = mapper.MapAToB((float)m_DiscreteList[i]); //原始数据映射到[0, 100]
                Color color = ColorMap.MapValueToColor(value);
                m_DiscreteColorDictionary.Add(m_DiscreteList[i], color);
            }


        }

        /// <summary>
        /// 数值映射到颜色表
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        private Color MapDiscreteValueToColor(double Value)
        {
            if (!m_DiscreteColorDictionary.ContainsKey(Value)) throw new Exception("离散数据表没有输入value");
            return m_DiscreteColorDictionary[Value];
        }

        #endregion
    }
}
