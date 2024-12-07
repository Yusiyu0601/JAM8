namespace JAM8.Algorithms.MachineLearning
{
    using JAM8.Utilities;
    using SVM;

    /// <summary>
    /// 基于SVM的支持向量机辅助类
    /// SVM网址：http://www.matthewajohnson.org/software/svm.html
    /// 喻思羽 2015.5
    /// </summary>
    public class MySVM
    {
        public class SVMModel
        {
            internal Model Model { get; set; }
            internal RangeTransform Range { get; set; }
        }

        /// <summary>
        /// 训练支持向量回归机SVR的Model
        /// </summary>
        /// <param name="df"></param>
        /// <param name="input_series_names"></param>
        /// <param name="output_series_name"></param>
        /// <param name="C"></param>
        /// <param name="Gamma"></param>
        /// <param name="type">0：回归 1：分类</param>
        /// <returns></returns>
        public static SVMModel train(MyDataFrame df, string[] input_series_names, string output_series_name,
            double C = 2, double Gamma = 0.5, int type = 0)
        {
            SVMModel SVMModel = new();
            //设置训练参数
            Parameter param = new()
            {
                C = C,
                Gamma = Gamma,
                EPS = 0.5,
                KernelType = KernelType.RBF,
                SvmType = type == 0 ? SvmType.EPSILON_SVR : SvmType.C_SVC
            };

            //从dt提取input列的数据（多输入）
            double[][] input = df.get_series_subset(input_series_names).convert_to_double_jagged_array();
            Node[][] _X = ConvertArrayToNodeList(input);
            //从dt提取output列的数据(单输出)
            double[] output = df.get_series<double>(output_series_name);
            Problem problem = new(df.N_Record, output, _X, input_series_names.Length);
            SVMModel.Range = RangeTransform.Compute(problem);
            problem = SVMModel.Range.Scale(problem);
            SVMModel.Model = Training.Train(problem, param);
            return SVMModel;
        }

        /// <summary>
        /// 根据SVM训练后的模型，计算预测值
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="InputColumnIndex"></param>
        /// <param name="SVMModel"></param>
        /// <returns></returns>
        public static MyDataFrame predict(MyDataFrame df, string[] input_series_names, SVMModel SVMModel)
        {
            MyDataFrame predict = df.deep_clone();

            //从dt提取input列的数据（多输入）
            double[][] input = df.get_series_subset(input_series_names).convert_to_double_jagged_array();
            Node[][] _X = ConvertArrayToNodeList(input);
            for (int iRecord = 0; iRecord < df.N_Record; iRecord++)
            {
                Node[] node = _X[iRecord];
                node = SVMModel.Range.Transform(node);
                double predictValue = Prediction.Predict(SVMModel.Model, node);
                predict[iRecord, df.N_Series - 1] = predictValue;
            }
            return predict;
        }

        static Node[][] ConvertArrayToNodeList(double[][] doubleArray)
        {
            int rowCount = doubleArray.Length;
            int colCount = doubleArray[0].Length;

            Node[][] nodeArray = new Node[rowCount][];

            for (int row = 0; row < rowCount; row++)
            {
                Node[] node = new Node[colCount];
                for (int col = 0; col < colCount; col++)
                {
                    Node n = new(col + 1, doubleArray[row][col]);
                    node[col] = n;
                }
                nodeArray[row] = node;
            }

            return nodeArray;
        }
    }
}
