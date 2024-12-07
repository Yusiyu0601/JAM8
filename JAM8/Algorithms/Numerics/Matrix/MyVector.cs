using MathNet.Numerics.LinearAlgebra.Single;

namespace JAM8.Algorithms.Numerics
{
    /// <summary>
    /// 名称：Vector向量类
    /// 作者：喻思羽
    /// 时间：2015
    /// </summary>
    public class MyVector
    {
        private MyVector() { }

        /// <summary>
        /// 缓冲区
        /// </summary>
        public float[] buffer { get; internal set; }

        /// <summary>
        /// 矢量的长度
        /// </summary>
        public int N
        {
            get
            {
                return buffer.Length;
            }
        }

        /// <summary>
        /// 创建MyVector
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static MyVector create(int length)
        {
            MyVector v = new()
            {
                buffer = new float[length]
            };
            return v;
        }

        /// <summary>
        /// 创建MyVector
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        public static MyVector create(float[] array)
        {
            MyVector v = new()
            {
                buffer = array
            };
            return v;
        }

        /// <summary>
        /// 创建MyVector，从double转换为float
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        public static MyVector create(double[] array)
        {
            float[] temp = new float[array.Length];
            for (int i = 0; i < array.Length; i++)
            {
                temp[i] = (float)array[i];
            }
            MyVector v = new()
            {
                buffer = temp
            };
            return v;
        }

        /// <summary>
        /// 创建MyVector
        /// </summary>
        /// <returns></returns>
        public static MyVector create(DenseVector mathnet_vector)
        {
            return create(mathnet_vector.ToArray());
        }

        /// <summary>
        /// 矢量元素索引器
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public float this[int index]
        {
            get { return buffer[index]; }
            set { buffer[index] = value; }
        }

        /// <summary>
        /// 转换为MathNet的DenseVector
        /// </summary>
        /// <returns></returns>
        public static DenseVector to_MathNet(MyVector vector)
        {
            return vector.buffer;
        }
    }
}
