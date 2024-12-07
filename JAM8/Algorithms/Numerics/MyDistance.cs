using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JAM8.Algorithms.Numerics
{
    public enum MyDistanceType
    {
        hsim,
        manhattan,
        euclidean
    }
    /// <summary>
    /// 距离量化类
    /// </summary>
    public class MyDistance
    {
        #region hsim

        /// <summary>
        /// 计算hsim相似度
        /// </summary>
        /// <param name="array1"></param>
        /// <param name="array2"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static float calc_hsim(IList<float?> array1, IList<float?> array2)
        {
            //如果Vector1和Vector2的元素数量不同，提示异常
            if (array1.Count != array2.Count)
                throw new Exception("两个矢量长度不同!");
            //测度值
            float dist = 0;

            //点对点计算两个矢量之间的测度
            for (int i = 0; i < array1.Count; i++)
            {
                if (array1[i] != null && array2[i] != null)
                {
                    dist += 1.0f / (1.0f + Math.Abs(array1[i].Value - array2[i].Value));
                }
            }
            //转换称为相似度
            float hsim = dist / array1.Count;
            return hsim;
        }
        /// <summary>
        /// 计算hsim相似度
        /// </summary>
        /// <param name="array1"></param>
        /// <param name="array2"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static double calc_hsim(IList<double?> array1, IList<double?> array2)
        {
            //如果Vector1和Vector2的元素数量不同，提示异常
            if (array1.Count != array2.Count)
                throw new Exception("两个矢量长度不同!");
            //测度值
            double dist = 0;

            //点对点计算两个矢量之间的测度
            for (int i = 0; i < array1.Count; i++)
            {
                if (array1[i] != null && array2[i] != null)
                {
                    dist += 1.0 / (1.0 + Math.Abs(array1[i].Value - array2[i].Value));
                }
            }
            //转换称为相似度
            double hsim = dist / array1.Count;
            return hsim;
        }
        /// <summary>
        /// 计算hsim相似度
        /// </summary>
        /// <param name="array1"></param>
        /// <param name="array2"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static float calc_hsim(IList<float> array1, IList<float> array2)
        {
            //如果Vector1和Vector2的元素数量不同，提示异常
            if (array1.Count != array2.Count)
                throw new Exception("两个矢量长度不同!");
            //测度值
            float dist = 0;

            //点对点计算两个矢量之间的测度
            for (int i = 0; i < array1.Count; i++)
            {
                dist += 1.0f / (1.0f + Math.Abs(array1[i] - array2[i]));
            }
            //转换称为相似度
            float hsim = dist / array1.Count;
            return hsim;
        }
        /// <summary>
        /// 计算hsim相似度
        /// </summary>
        /// <param name="array1"></param>
        /// <param name="array2"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static double calc_hsim(IList<double> array1, IList<double> array2)
        {
            //如果Vector1和Vector2的元素数量不同，提示异常
            if (array1.Count != array2.Count)
                throw new Exception("两个矢量长度不同!");
            //测度值
            double dist = 0;

            //点对点计算两个矢量之间的测度
            for (int i = 0; i < array1.Count; i++)
            {
                dist += 1.0 / (1.0 + Math.Abs(array1[i] - array2[i]));
            }
            //转换称为相似度
            double hsim = dist / array1.Count;
            return hsim;
        }

        #endregion

        #region manhattan

        /// <summary>
        /// 计算曼哈顿距离
        /// </summary>
        /// <param name="vector1"></param>
        /// <param name="vector2"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static float calc_manhattan(IList<float?> array1, IList<float?> array2)
        {
            //如果Vector1和Vector2的元素数量不同，提示异常
            if (array1.Count != array2.Count)
                throw new Exception("两个矢量长度不同!");
            //测度值
            float dist = 0;

            //点对点计算两个矢量之间的测度
            for (int i = 0; i < array1.Count; i++)
            {
                if (array1[i] != null && array2[i] != null)
                {
                    dist += Math.Abs(array1[i].Value - array2[i].Value);
                }
            }
            return dist;
        }
        /// <summary>
        /// 计算曼哈顿距离
        /// </summary>
        /// <param name="vector1"></param>
        /// <param name="vector2"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static double calc_manhattan(IList<double?> array1, IList<double?> array2)
        {
            //如果Vector1和Vector2的元素数量不同，提示异常
            if (array1.Count != array2.Count)
                throw new Exception("两个矢量长度不同!");
            //测度值
            double dist = 0;

            //点对点计算两个矢量之间的测度
            for (int i = 0; i < array1.Count; i++)
            {
                if (array1[i] != null && array2[i] != null)
                {
                    dist += Math.Abs(array1[i].Value - array2[i].Value);
                }
            }
            return dist;
        }
        /// <summary>
        /// 计算曼哈顿距离
        /// </summary>
        /// <param name="vector1"></param>
        /// <param name="vector2"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static float calc_manhattan(IList<float> array1, IList<float> array2)
        {
            //如果Vector1和Vector2的元素数量不同，提示异常
            if (array1.Count != array2.Count)
                throw new Exception("两个矢量长度不同!");
            //测度值
            float dist = 0;

            //点对点计算两个矢量之间的测度
            for (int i = 0; i < array1.Count; i++)
            {
                dist += Math.Abs(array1[i] - array2[i]);
            }
            return dist;
        }
        /// <summary>
        /// 计算曼哈顿距离
        /// </summary>
        /// <param name="vector1"></param>
        /// <param name="vector2"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static double calc_manhattan(IList<double> array1, IList<double> array2)
        {
            //如果Vector1和Vector2的元素数量不同，提示异常
            if (array1.Count != array2.Count)
                throw new Exception("两个矢量长度不同!");
            //测度值
            double dist = 0;

            //点对点计算两个矢量之间的测度
            for (int i = 0; i < array1.Count; i++)
            {
                dist += Math.Abs(array1[i] - array2[i]);
            }
            return dist;
        }

        #endregion

        #region euclidean

        /// <summary>
        /// 计算欧式距离L2
        /// </summary>
        /// <param name="vector1"></param>
        /// <param name="vector2"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static float calc_euclidean(IList<float?> array1, IList<float?> array2)
        {
            //如果Vector1和Vector2的元素数量不同，提示异常
            if (array1.Count != array2.Count)
                throw new Exception("两个矢量长度不同!");
            //测度值
            double dist = 0;

            //点对点计算两个矢量之间的测度
            for (int i = 0; i < array1.Count; i++)
            {
                if (array1[i] != null && array2[i] != null)
                {
                    dist += Math.Pow(array1[i].Value - array2[i].Value, 2);
                }
            }
            return (float)dist;
        }
        /// <summary>
        /// 计算欧式距离L2
        /// </summary>
        /// <param name="vector1"></param>
        /// <param name="vector2"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static double calc_euclidean(IList<double?> array1, IList<double?> array2)
        {
            //如果Vector1和Vector2的元素数量不同，提示异常
            if (array1.Count != array2.Count)
                throw new Exception("两个矢量长度不同!");
            //测度值
            double dist = 0;

            //点对点计算两个矢量之间的测度
            for (int i = 0; i < array1.Count; i++)
            {
                if (array1[i] != null && array2[i] != null)
                {
                    dist += Math.Pow(array1[i].Value - array2[i].Value, 2);
                }
            }
            return dist;
        }
        /// <summary>
        /// 计算欧式距离L2
        /// </summary>
        /// <param name="vector1"></param>
        /// <param name="vector2"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static float calc_euclidean(IList<float> array1, IList<float> array2)
        {
            //如果Vector1和Vector2的元素数量不同，提示异常
            if (array1.Count != array2.Count)
                throw new Exception("两个矢量长度不同!");
            //测度值
            double dist = 0;

            //点对点计算两个矢量之间的测度
            for (int i = 0; i < array1.Count; i++)
            {
                dist += Math.Pow(array1[i] - array2[i], 2);
            }
            return (float)dist;
        }
        /// <summary>
        /// 计算欧式距离L2
        /// </summary>
        /// <param name="vector1"></param>
        /// <param name="vector2"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static double calc_euclidean(IList<double> array1, IList<double> array2)
        {
            //如果Vector1和Vector2的元素数量不同，提示异常
            if (array1.Count != array2.Count)
                throw new Exception("两个矢量长度不同!");
            //测度值
            double dist = 0;

            //点对点计算两个矢量之间的测度
            for (int i = 0; i < array1.Count; i++)
            {
                dist += Math.Pow(array1[i] - array2[i], 2);
            }
            return dist;
        }

        #endregion

        #region 根据给定类型计算距离

        public static float calc_distance(IList<float?> array1, IList<float?> array2, MyDistanceType dist_type)
        {
            if (dist_type == MyDistanceType.hsim)
                return calc_hsim(array1, array2);
            if (dist_type == MyDistanceType.manhattan)
                return calc_manhattan(array1, array2);
            if (dist_type == MyDistanceType.euclidean)
                return calc_euclidean(array1, array2);
            return -9999.9999f;
        }
        public static double calc_distance(IList<double?> array1, IList<double?> array2, MyDistanceType dist_type)
        {
            if (dist_type == MyDistanceType.hsim)
                return calc_hsim(array1, array2);
            if (dist_type == MyDistanceType.manhattan)
                return calc_manhattan(array1, array2);
            if (dist_type == MyDistanceType.euclidean)
                return calc_euclidean(array1, array2);
            return -9999.9999f;
        }
        public static float calc_distance(IList<float> array1, IList<float> array2, MyDistanceType dist_type)
        {
            if (dist_type == MyDistanceType.hsim)
                return calc_hsim(array1, array2);
            if (dist_type == MyDistanceType.manhattan)
                return calc_manhattan(array1, array2);
            if (dist_type == MyDistanceType.euclidean)
                return calc_euclidean(array1, array2);
            return -9999.9999f;
        }
        public static double calc_distance(IList<double> array1, IList<double> array2, MyDistanceType dist_type)
        {
            if (dist_type == MyDistanceType.hsim)
                return calc_hsim(array1, array2);
            if (dist_type == MyDistanceType.manhattan)
                return calc_manhattan(array1, array2);
            if (dist_type == MyDistanceType.euclidean)
                return calc_euclidean(array1, array2);
            return -9999.9999f;
        }

        #endregion
    }
}
