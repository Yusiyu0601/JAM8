using JAM8.Algorithms.Numerics;
using MathNet.Numerics.LinearAlgebra.Double;

namespace JAM8.Algorithms.MachineLearning
{
    /// <summary>
    /// 名称：EigenObject
    /// 作用：保存 矩阵进行特征分解得到的特征值、特征向量
    ///       EigenValue是特征值
    ///       EigenVector是特征向量，以一维数组的方式存放特征向量值
    /// 作者：喻思羽
    /// 编写时间：2014-7-16
    /// </summary>
    public class EigenObject
    {
        /// <summary>
        /// 特征值
        /// </summary>
        public double EigenValue;

        /// <summary>
        /// 特征向量
        /// </summary>
        public double[] EigenVector;
    }

    /// <summary>
    /// CMDSCALE 静态类
    /// 功能：经典多维尺度分析类(多元统计分析领域)
    /// 说明：CMDSCALE进行矩阵的特征分解采用了MathNet开源数学类库
    /// 作者：喻思羽
    /// 时间：2014-7-16
    /// 
    /// 静态方法 EigenObjects_MathNet
    /// 功能：对距离矩阵进行特征分解
    /// 输入参数：研究对象的距离矩阵
    /// 输出参数：距离矩阵的特征值&特征向量
    /// 
    /// 静态方法 CMDSCALE_MathNet
    /// 功能：计算研究对象在低维的空间构图
    /// 输入参数：研究对象的距离矩阵、指定降低的维度
    /// 输出参数：研究对象在给定维度上的空间构图坐标
    /// </summary>
    public class CMDSCALE
    {
        #region 基于MathNet的方法

        public static List<EigenObject> EigenObjects_MathNet(double[,] distance_matrix)
        {
            List<EigenObject> EigenObjects = new();

            //矩阵的第一个维度
            int Dim1 = distance_matrix.GetUpperBound(0) + 1;
            //矩阵的第二个维度
            int Dim2 = distance_matrix.GetUpperBound(1) + 1;
            //如果输入矩阵不是方阵，则返回null
            if (Dim1 != Dim2) return null;
            //方阵维度
            int N = Dim1;

            DenseMatrix I = DenseMatrix.CreateIdentity(N);
            DenseMatrix ones = DenseMatrix.Create(N, N, 1.0);
            DenseMatrix H = I - 1.0 / N * ones;
            DenseMatrix dismat = DenseMatrix.OfArray(distance_matrix);
            DenseMatrix A = (-0.5) * (DenseMatrix)dismat.PointwiseMultiply(dismat);
            DenseMatrix B = (H * A) * H;
            //矩阵特征值分解
            var Evd = B.Evd();
            //计算特征值
            var EigenValues = Evd.EigenValues;
            //计算特征向量
            var EigenVectors = Evd.EigenVectors;
            for (int i = 0; i < N; i++)
            {
                //由于mathnet的特征值是复数，取实部作为特征值
                double EigenValue = EigenValues[i].Real;
                //获取特征值对应的特征向量
                var EigenVector = EigenVectors.Column(i);
                EigenObject EigenObject = new()
                {
                    EigenValue = EigenValue,
                    EigenVector = EigenVector.ToArray()
                };
                EigenObjects.Add(EigenObject);
            }
            //根据特征值对距离矩阵的特征值和特征向量进行降序排序
            EigenObjects = (from item in EigenObjects
                            orderby item.EigenValue descending
                            select item).ToList();

            return EigenObjects;
        }
        /// <summary>
        /// 多维尺度分析，基于数值计算库MathNet
        /// </summary>
        /// <param name="DistanceMatrix">距离矩阵，必须是对称的矩阵</param>
        /// <returns></returns>
        public static double[,] CMDSCALE_MathNet(double[,] distance_matrix, int Dimension)
        {
            //矩阵的第一个维度
            int Dim1 = distance_matrix.GetUpperBound(0) + 1;
            //矩阵的第二个维度
            int Dim2 = distance_matrix.GetUpperBound(1) + 1;
            //方阵维度
            int N = Dim1;
            //根据计算的特征值和特征向量计算降低维度后的数据列表
            double[,] Result = new double[N, Dimension];
            //计算距离矩阵的特征值和特征向量
            List<EigenObject> EigenObjects = EigenObjects_MathNet(distance_matrix);
            //维度索引
            for (int dim = 0; dim < Dimension; dim++)
            {
                //数据项索引
                for (int n = 0; n < N; n++)
                {
                    EigenObject EigenObject = EigenObjects[dim];
                    double EigenValue = EigenObject.EigenValue;
                    double[] EigenVector = EigenObject.EigenVector;
                    double value = Math.Sqrt(EigenValue) * EigenVector[n];
                    Result[n, dim] = value;
                }
            }

            return Result;
        }

        public static List<EigenObject> EigenObjects_MathNet(MyMatrix distance_matrix)
        {
            List<EigenObject> EigenObjects = new();

            //矩阵的第一个维度
            int Dim1 = distance_matrix.N_Rows;
            //矩阵的第二个维度
            int Dim2 = distance_matrix.N_Cols;
            //如果输入矩阵不是方阵，则返回null
            if (Dim1 != Dim2) return null;
            //方阵维度
            int N = Dim1;

            DenseMatrix I = DenseMatrix.CreateIdentity(N);
            DenseMatrix ones = DenseMatrix.Create(N, N, 1.0);
            DenseMatrix H = I - 1.0 / N * ones;
            DenseMatrix dismat = DenseMatrix.OfArray(distance_matrix.convert_to_doubleType());
            DenseMatrix A = (-0.5) * (DenseMatrix)dismat.PointwiseMultiply(dismat);
            DenseMatrix B = (H * A) * H;
            //矩阵特征值分解
            var Evd = B.Evd();
            //计算特征值
            var EigenValues = Evd.EigenValues;
            //计算特征向量
            var EigenVectors = Evd.EigenVectors;
            for (int i = 0; i < N; i++)
            {
                //由于mathnet的特征值是复数，取实部作为特征值
                double EigenValue = EigenValues[i].Real;
                //获取特征值对应的特征向量
                var EigenVector = EigenVectors.Column(i);
                EigenObject EigenObject = new()
                {
                    EigenValue = EigenValue,
                    EigenVector = EigenVector.ToArray()
                };
                EigenObjects.Add(EigenObject);
            }
            //根据特征值对距离矩阵的特征值和特征向量进行降序排序
            EigenObjects = (from item in EigenObjects
                            orderby item.EigenValue descending
                            select item).ToList();

            return EigenObjects;
        }
        /// <summary>
        /// 多维尺度分析，基于数值计算库MathNet
        /// </summary>
        /// <param name="DistanceMatrix">距离矩阵，必须是对称的矩阵</param>
        /// <param name="Dimension">结果维度</param>
        /// <returns></returns>
        public static MyMatrix CMDSCALE_MathNet(MyMatrix distance_matrix, int Dimension)
        {
            //矩阵的第一个维度
            int Dim1 = distance_matrix.N_Rows;
            //矩阵的第二个维度
            int Dim2 = distance_matrix.N_Cols;
            //方阵维度
            int N = Dim1;
            //根据计算的特征值和特征向量计算降低维度后的数据列表
            var Result = MyMatrix.create(N, Dimension);
            //计算距离矩阵的特征值和特征向量
            List<EigenObject> EigenObjects = EigenObjects_MathNet(distance_matrix);
            //维度索引
            for (int dim = 0; dim < Dimension; dim++)
            {
                //数据项索引
                for (int n = 0; n < N; n++)
                {
                    EigenObject EigenObject = EigenObjects[dim];
                    double EigenValue = EigenObject.EigenValue;
                    double[] EigenVector = EigenObject.EigenVector;
                    double value = Math.Sqrt(EigenValue) * EigenVector[n];
                    Result[n, dim] = (float)value;
                }
            }

            return Result;
        }
        /// <summary>
        /// 多维尺度分析，基于数值计算库MathNet
        /// </summary>
        /// <param name="DistanceMatrix">距离矩阵（1.对称矩阵；2.上三角矩阵）</param>
        /// <param name="IsSymmetry">矩阵是否对称</param>
        /// <param name="Dimension">结果维度</param>
        /// <returns></returns>
        public static MyMatrix CMDSCALE_MathNet(MyMatrix distance_matrix, bool IsSymmetry, int Dimension)
        {
            if (IsSymmetry)
            {
                return CMDSCALE_MathNet(distance_matrix, Dimension);
            }
            else//先把上三角矩阵转换成为对称矩阵
            {
                int N = distance_matrix.N_Rows;
                MyMatrix mat = MyMatrix.create(N, N);

                for (int j = 0; j < N; j++)
                {
                    for (int i = 0; i < N; i++)
                    {
                        if (i > j)
                            mat[i, j] = distance_matrix[j, i];
                        else
                            mat[i, j] = distance_matrix[i, j];
                    }
                }
                return CMDSCALE_MathNet(mat, Dimension);
            }
        }

        #endregion
    }
}
