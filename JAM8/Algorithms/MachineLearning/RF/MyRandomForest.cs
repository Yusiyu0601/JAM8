using JAM8.Utilities;

namespace JAM8.Algorithms.MachineLearning
{
    public class MyRandomForest
    {
        //RandomForest rf_model = null;
        private MyDataFrame df_training = null;
        private Dictionary<string, int> category_map_to_code = null;
        private Dictionary<int, string> code_map_to_category = null;

        private MyRandomForest()
        {
            category_map_to_code = new();
            code_map_to_category = new();
        }

        public static MyRandomForest create(MyDataFrame df_training, string[] input_series_names, string output_series_name)
        {
            MyRandomForest my_rf = new();

            //从dt提取input列的数据（多输入）
            double[][] input = df_training.get_series_subset(input_series_names).convert_to_double_jagged_array();
            //从dt提取output列的数据(单输出)
            string[] output_category = df_training.get_series<string>(output_series_name);
            //去重
            string[] output_category_distinct = MyDistinct.distinct_by_reference<string>(output_category).values;
            //编码为整数
            for (int i = 0; i < output_category_distinct.Length; i++)
            {
                my_rf.category_map_to_code.Add(output_category_distinct[i], i);
                my_rf.code_map_to_category.Add(i, output_category_distinct[i]);
            }
            int[] output_coding = new int[output_category.Length];
            for (int i = 0; i < output_category.Length; i++)
            {
                string category = output_category[i];
                output_coding[i] = my_rf.category_map_to_code[category];
            }
            //RandomForestLearning rfl = new();


            //my_rf.rf_model = rfl.Learn(input, output_coding);
            my_rf.df_training = df_training;

            return my_rf;
        }

        public MyDataFrame predict(MyDataFrame df_predict, string[] input_series_names)
        {
            //MyDataFrame df_result = MyDataFrame.create_from_dataframe(df_predict, new string[] { "predict_value" });
            MyDataFrame df_result = df_predict.deep_clone();
            df_result.add_series("predict_value");
            //从dt提取input列的数据（多输入）
            double[][] input = df_result.get_series_subset(input_series_names).convert_to_double_jagged_array();
            //int[] ouput = rf_model.Decide(input);
            //for (int iRecord = 0; iRecord < df_result.N_Record; iRecord++)
            //{
            //    int code_predict = ouput[iRecord];
            //    df_result[iRecord, df_result.N_Series - 1] = code_map_to_category[code_predict];
            //}
            return df_result;
        }
    }
}
