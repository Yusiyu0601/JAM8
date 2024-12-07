using JAM8.Utilities;

namespace JAM8.Algorithms.Geometry
{
    /// <summary>
    /// GSLIB文件读写
    /// </summary>
    public class GSLIB
    {
        /// <summary>
        /// 从gslib文件里读取数据为mydf
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        public static MyDataFrame gslib_to_df(string file_name)
        {
            using StreamReader sr = new(file_name);
            string Title = sr.ReadLine();//GSLIB数据的标题
            int ColumnCount = Convert.ToInt32(sr.ReadLine());//GSLIB数据的属性总数
            List<string> Colomns = new(ColumnCount);//读取GSLIB数据的属性
            for (int i = 0; i < ColumnCount; i++)
                Colomns.Add(sr.ReadLine());

            MyDataFrame df = MyDataFrame.create(Colomns.ToArray());

            while (sr.Peek() != -1)//读取数据，以row的形式添加到数据表中
            {
                string str = sr.ReadLine();//读取一行
                string[] str_record = str.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                df.add_record(str_record.ToArray());
            }
            return df;
        }

        /// <summary>
        /// 将mydf输出到gslib文件
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="null_value"></param>
        /// <param name="df"></param>
        public static void df_to_gslib(string file_name, float null_value, MyDataFrame df)
        {
            if (df == null) return;
            using StreamWriter sw = new(file_name);
            //输出GSLIB数据的标题
            sw.WriteLine("gslib");
            //输出变量数目
            sw.WriteLine(df.N_Series);
            //输出属性名称
            foreach (var item in df.series_names)
                sw.WriteLine(item);

            //输出数据
            for (int iRecord = 0; iRecord < df.N_Record; iRecord++)
            {
                string data = string.Empty;
                for (int iSeries = 0; iSeries < df.N_Series; iSeries++)
                {
                    string text_cell = df[iRecord, iSeries] == null ?
                        null_value.ToString() :
                        df[iRecord, iSeries].ToString();

                    data += text_cell.PadLeft(20) + "          ";
                }
                sw.WriteLine(data);
            }

        }
    }
}
