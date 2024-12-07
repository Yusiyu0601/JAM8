using System.Diagnostics;

namespace JAM8.Utilities
{
    /// <summary>
    /// 文本文件帮助类
    /// </summary>
    public class TxtHelper
    {
        /// <summary>
        /// 读取文本文件，并将所有行转换为字符串数组
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string[] txt_to_string_array(string file_name)
        {
            List<string> str_list = new();
            using FileStream fs = File.Open(file_name, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            using BufferedStream bs = new(fs);
            using StreamReader sr = new(bs);
            string s;
            long N = get_row_count(file_name);
            long flag = 0;

            Stopwatch sw = new();
            sw.Start();
            while ((s = sr.ReadLine()) != null)
            {
                MyConsoleProgress.Print(flag++, N, "加载text文件");
                str_list.Add(s);
            }
            sw.Stop();
            MyConsoleHelper.write_string_to_console($"{sw.ElapsedMilliseconds}毫秒");

            return str_list.ToArray();
        }

        /// <summary>
        /// 获取总行数
        /// </summary>
        /// <param name="file_name"></param>
        /// <returns></returns>
        public static long get_row_count(string file_name)
        {
            FileInfo file = new(file_name);
            long CountLines = 0;
            using (var stream = file.OpenRead())//读取txt文件的行数
            {
                CountLines = Easy.Common.Extensions.StreamExtensions.CountLines(stream);
            }
            return CountLines;
        }

        /// <summary>
        /// 读取txt的前N行数据
        /// </summary>
        /// <returns></returns>
        public static string[] first_N_rows(string file_name, int N)
        {
            int rows = 0;
            List<string> firstThreeRows = new();
            using (StreamReader sr = new(file_name))//读取所有行数据
            {
                while (sr.Peek() > -1)
                {
                    string data = sr.ReadLine();//读取一行数据
                    firstThreeRows.Add(data);
                    rows++;
                    if (rows == N)
                        break;
                }
            }
            return firstThreeRows.ToArray();
        }
    }
}
