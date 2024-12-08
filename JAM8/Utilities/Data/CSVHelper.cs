using CsvHelper;
using System.Data;
using System.Globalization;

namespace JAM8.Utilities
{
    /// <summary>
    /// csv文件帮助类
    /// </summary>
    public class CSVHelper
    {
        /// <summary>
        /// 读取csv文件，并以二维数组形式输出
        /// </summary>
        /// <param name="file"></param>
        /// <returns>(headers,data)</returns>
        public static (string[] header, string[,] data) csv_to_array(string fileName)
        {
            using FileStream fs = new(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            using var reader = new StreamReader(fs);
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
            var records = csv.GetRecords<dynamic>().ToList();
            IDictionary<string, object> dict = records[0];
            int rows = records.Count;
            int cols = dict.Count;
            string[] header = new string[cols];
            string[,] data = new string[rows, cols];

            for (int i = 0; i < cols; i++)
            {
                header[i] = csv.HeaderRecord[i];
            }

            for (int row = 0; row < rows; row++)
            {
                dict = records[row];
                var list = dict.Values.ToList();
                for (int col = 0; col < cols; col++)
                {
                    data[row, col] = list[col].ToString();
                }
            }
            return (header, data);
        }

        /// <summary>
        /// csv格式转换为MyDataFrame对象
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static MyDataFrame csv_to_dataframe(string fileName)
        {
            using var reader = new StreamReader(fileName) ;
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture) ;
            using var dr = new CsvDataReader(csv);
            var dt = new DataTable();
            dt.Load(dr);
            MyDataFrame df = MyDataFrame.create_from_datatable(dt);
            return df;
        }

        /// <summary>
        /// csv格式转换为DataTable对象
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static DataTable csv_to_dataTable(string file)
        {
            using FileStream fs = new(file, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            using var reader = new StreamReader(fs);
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
            var records = csv.GetRecords<dynamic>().ToList();

            IDictionary<string, object> dict = records[0];
            int rows = records.Count;
            int cols = dict.Count;
            string[,] array = new string[rows, cols];
            DataTable dt = new();
            for (int i = 0; i < cols; i++)
            {
                dt.Columns.Add(csv.HeaderRecord[i]);
            }

            for (int row = 0; row < rows; row++)
            {
                var newRow = dt.NewRow();
                dict = records[row];
                var list = dict.Values.ToList();
                for (int col = 0; col < cols; col++)
                {
                    newRow[col] = list[col];
                }
                dt.Rows.Add(newRow);
            }
            return dt;
        }
    }
}
