using JAM8.Utilities;
using MathNet.Numerics.LinearAlgebra.Single;

namespace JAM8.Algorithms.Numerics
{
    /// <summary>
    /// 名称:Matrix矩阵类
    /// 作者:喻思羽
    /// 时间:2015
    /// </summary>
    public class MyMatrix
    {
        private MyMatrix() { }

        /// <summary>
        /// 缓冲区
        /// </summary>
        public float[,] buffer { get; internal set; }

        /// <summary>
        /// 转换为双精度类型
        /// </summary>
        /// <returns></returns>
        public double[,] convert_to_doubleType()
        {
            double[,] buffer = new double[N_Rows, N_Cols];
            for (int i_col = 0; i_col < N_Cols; i_col++)
            {
                for (int i_row = 0; i_row < N_Rows; i_row++)
                {
                    buffer[i_row, i_col] = this.buffer[i_row, i_col];
                }
            }
            return buffer;
        }

        /// <summary>
        /// 行数
        /// </summary>
        public int N_Rows
        {
            get
            {
                return buffer.GetLength(0);
            }
        }

        /// <summary>
        /// 列数
        /// </summary>
        public int N_Cols
        {
            get
            {
                return buffer.GetLength(1);
            }
        }

        /// <summary>
        /// 打印
        /// </summary>
        /// <returns></returns>
        public string view_text()
        {
            return $"MyMatrix [{N_Cols}_{N_Cols}]";
        }

        /// <summary>
        /// 索引器
        /// </summary>
        /// <param name="iRow"></param>
        /// <param name="iCol"></param>
        /// <returns></returns>
        public float this[int iRow, int iCol]
        {
            get { return buffer[iRow, iCol]; }
            set { buffer[iRow, iCol] = value; }
        }

        #region 创建MyMatrix

        /// <summary>
        /// 创建MyMatrix
        /// </summary>
        /// <param name="N_Rows"></param>
        /// <param name="N_Cols"></param>
        /// <returns></returns>
        public static MyMatrix create(int N_Rows, int N_Cols)
        {
            MyMatrix mat = new()
            {
                buffer = new float[N_Rows, N_Cols]
            };
            return mat;
        }

        /// <summary>
        /// 创建MyMatrix
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        public static MyMatrix create(float[,] array)
        {
            MyMatrix mat = new()
            {
                buffer = array
            };
            return mat;
        }

        /// <summary>
        /// 从MathNet的DenseMatrix转换为MyMatrix
        /// </summary>
        /// <param name="mathnet_matrix"></param>
        /// <returns></returns>
        public static MyMatrix create(DenseMatrix mathnet_matrix)
        {
            return create(mathnet_matrix.ToArray());
        }

        #endregion

        /// <summary>
        /// 转换为MathNet的DenseMatrix
        /// </summary>
        /// <returns></returns>
        public static DenseMatrix to_mathnet(MyMatrix matrix)
        {
            return DenseMatrix.OfArray(matrix.buffer);
        }

        #region 线性方程组求解

        /// <summary>
        /// 解线性方程组
        /// Solve linear equations
        /// ------------- demo ------------
        /// Solve next system of linear equations (Ax=b):
        /// 5*x + 2*y - 4*z = -7
        /// 3*x - 7*y + 6*z = 38
        /// 4*x + 1*y + 5*z = 43
        /// Matrix matrixA = new Matrix(new[,] { { 5.00, 2.00, -4.00 }, { 3.00, -7.00, 6.00 }, { 4.00, 1.00, 5.00 } })
        /// Vector vectorB = new Vector(new[] { -7.0, 38.0, 43.0 })
        /// ------------- demo ------------
        /// </summary>
        /// <param name="matrixA"></param>
        /// <param name="vectorB"></param>
        /// <returns></returns>
        public static MyVector solve_mathnet(MyMatrix matrix, MyVector vector)
        {
            DenseMatrix mathnet_matrix = to_mathnet(matrix);
            DenseVector mathnet_vector = MyVector.to_MathNet(vector);
            DenseVector result = (DenseVector)mathnet_matrix.LU().Solve(mathnet_vector);
            return MyVector.create(result);
        }

        /// <summary>
        /// 解线性方程组
        /// Solve linear equations
        /// ------------- demo ------------
        /// Solve next system of linear equations (Ax=b):
        /// 5*x + 2*y - 4*z = -7
        /// 3*x - 7*y + 6*z = 38
        /// 4*x + 1*y + 5*z = 43
        /// Matrix matrixA = new Matrix(new[,] { { 5.00, 2.00, -4.00 }, { 3.00, -7.00, 6.00 }, { 4.00, 1.00, 5.00 } })
        /// Vector vectorB = new Vector(new[] { -7.0, 38.0, 43.0 })
        /// ------------- demo ------------
        /// </summary>
        /// <param name="matrixA"></param>
        /// <param name="vectorB"></param>
        /// <returns></returns>
        public static MyVector solve_accord(MyMatrix matrix, MyVector vector)
        {
            float[,] b = new float[vector.N, 1];
            for (int i = 0; i < vector.N; i++)
            {
                b[i, 0] = vector[i];
            }
            DenseVector mathnet_vector = MyVector.to_MathNet(vector);
            //float[,] result = Accord.Math.Matrix.Solve(matrix.buffer, b);
            float[] v = new float[vector.N];
            //for (int i = 0; i < vector.N; i++)
            //{
            //    v[i] = result[i, 0];
            //}
            return MyVector.create(v);
        }

        #endregion

        #region 生成距离矩阵

        /// <summary>
        /// 根据数据集生成距离矩阵
        /// </summary>
        /// <param name="Data"></param>
        /// <param name="DistanceType"></param>
        /// <returns></returns>
        public static MyMatrix create_dismat(MyDataFrame df, MyDistanceType dist_type, float null_value = -99.99f)
        {
            var array_df = df.convert_to_float_2dArray(null_value);
            int N = array_df.GetUpperBound(0) + 1;//N是数据项条目的总数（第1个维度）
            int M = array_df.GetUpperBound(1) + 1;//M是数据项条目变量数量（第2个维度）
            MyMatrix dismat = MyMatrix.create(N, N);
            List<float?[]> vectors = new();//所有项
            for (int n = 0; n < N; n++)//分解输入数据
            {
                float?[] vector = new float?[M];
                for (int m = 0; m < M; m++)
                    vector[m] = array_df[n, m];

                vectors.Add(vector);
            }
            for (int n1 = 0; n1 < N; n1++)//点对点计算所有数据项的距离测度，建立距离矩阵
                for (int n2 = 0; n2 < N; n2++)
                    dismat[n2, n1] = MyDistance.calc_distance(vectors[n1], vectors[n2], dist_type);

            return dismat;
        }
        /// <summary>
        /// 根据数据集生成距离矩阵
        /// </summary>
        /// <param name="data"></param>
        /// <param name="dist_type"></param>
        /// <param name="null_value"></param>
        /// <returns></returns>
        public static MyMatrix create_dismat(float?[][] data, MyDistanceType dist_type)
        {
            int N = data.Length;
            MyMatrix dismat = MyMatrix.create(N, N);
            for (int n1 = 0; n1 < N; n1++)//点对点计算所有数据项的距离测度，建立距离矩阵
                for (int n2 = 0; n2 < N; n2++)
                    dismat[n2, n1] = MyDistance.calc_distance(data[n1], data[n2], dist_type);

            return dismat;
        }
        /// <summary>
        /// 根据数据集生成距离矩阵
        /// </summary>
        /// <param name="data"></param>
        /// <param name="dist_type"></param>
        /// <param name="null_value"></param>
        /// <returns></returns>
        public static MyMatrix create_dismat(List<float?>[] data, MyDistanceType dist_type)
        {
            float?[][] data1 = new float?[data.Length][];
            for (int i = 0; i < data.Length; i++)
            {
                data1[i] = data[i].ToArray();
            }
            return create_dismat(data1, dist_type);
        }
        /// <summary>
        /// 根据数据集生成距离矩阵
        /// </summary>
        /// <param name="data"></param>
        /// <param name="dist_type"></param>
        /// <param name="null_value"></param>
        /// <returns></returns>
        public static MyMatrix create_dismat(float[][] data, MyDistanceType dist_type)
        {
            int N = data.Length;
            MyMatrix dismat = MyMatrix.create(N, N);
            for (int n1 = 0; n1 < N; n1++)//点对点计算所有数据项的距离测度，建立距离矩阵
                for (int n2 = 0; n2 < N; n2++)
                    dismat[n2, n1] = MyDistance.calc_distance(data[n1], data[n2], dist_type);

            return dismat;
        }
        /// <summary>
        /// 根据数据集生成距离矩阵
        /// </summary>
        /// <param name="data"></param>
        /// <param name="dist_type"></param>
        /// <param name="null_value"></param>
        /// <returns></returns>
        public static MyMatrix create_dismat(List<float>[] data, MyDistanceType dist_type)
        {
            float[][] data1 = new float[data.Length][];
            for (int i = 0; i < data.Length; i++)
            {
                data1[i] = data[i].ToArray();
            }
            return create_dismat(data1, dist_type);
        }
        /// <summary>
        /// 根据数据集生成距离矩阵
        /// </summary>
        /// <param name="data"></param>
        /// <param name="dist_type"></param>
        /// <param name="null_value"></param>
        /// <returns></returns>
        public static MyMatrix create_dismat(double?[][] data, MyDistanceType dist_type)
        {
            int N = data.Length;
            MyMatrix dismat = MyMatrix.create(N, N);
            for (int n1 = 0; n1 < N; n1++)//点对点计算所有数据项的距离测度，建立距离矩阵
                for (int n2 = 0; n2 < N; n2++)
                    dismat[n2, n1] = (float)MyDistance.calc_distance(data[n1], data[n2], dist_type);

            return dismat;
        }
        /// <summary>
        /// 根据数据集生成距离矩阵
        /// </summary>
        /// <param name="data"></param>
        /// <param name="dist_type"></param>
        /// <param name="null_value"></param>
        /// <returns></returns>
        public static MyMatrix create_dismat(List<double?>[] data, MyDistanceType dist_type)
        {
            double?[][] data1 = new double?[data.Length][];
            for (int i = 0; i < data.Length; i++)
            {
                data1[i] = data[i].ToArray();
            }
            return create_dismat(data1, dist_type);
        }
        /// <summary>
        /// 根据数据集生成距离矩阵
        /// </summary>
        /// <param name="data"></param>
        /// <param name="dist_type"></param>
        /// <param name="null_value"></param>
        /// <returns></returns>
        public static MyMatrix create_dismat(double[][] data, MyDistanceType dist_type)
        {
            int N = data.Length;
            MyMatrix dismat = MyMatrix.create(N, N);
            for (int n1 = 0; n1 < N; n1++)//点对点计算所有数据项的距离测度，建立距离矩阵
                for (int n2 = 0; n2 < N; n2++)
                    dismat[n2, n1] = (float)MyDistance.calc_distance(data[n1], data[n2], dist_type);

            return dismat;
        }
        /// <summary>
        /// 根据数据集生成距离矩阵
        /// </summary>
        /// <param name="data"></param>
        /// <param name="dist_type"></param>
        /// <param name="null_value"></param>
        /// <returns></returns>
        public static MyMatrix create_dismat(List<double>[] data, MyDistanceType dist_type)
        {
            double[][] data1 = new double[data.Length][];
            for (int i = 0; i < data.Length; i++)
            {
                data1[i] = data[i].ToArray();
            }
            return create_dismat(data1, dist_type);
        }

        #endregion
    }
}
