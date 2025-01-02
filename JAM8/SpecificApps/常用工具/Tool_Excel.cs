using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using JAM8.Utilities;

namespace JAM8.SpecificApps.常用工具
{
    public class Tool_Excel
    {
        public static void N个excel文件按行合并()
        {
            Dictionary<string, MyDataFrame> dfs = new();
            OpenFileDialog ofd = new()
            {
                Multiselect = true,
                Filter = "Excel(*.xlsx;*.xls)|*.xlsx;*.xls"
            };
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                var file_paths = ofd.FileNames;
                List<string> series_names = new() { "表源(source)" };//序列名称
                foreach (var file_path in file_paths)
                {
                    string file_name = FileHelper.GetFileName(file_path, false);
                    dfs.Add(file_name, MyDataFrame.read_from_excel(file_path));
                    for (int i = 0; i < dfs[file_name].series_names.Length; i++)
                        series_names.Add($"{dfs[file_name].series_names[i]}");
                }
                var series_names_distinct = series_names.Distinct().ToList();
                bool is_same = true;
                Console.WriteLine();
                Console.WriteLine("各个excel表结构信息为:");
                foreach (var (file_name, df) in dfs)
                {
                    Console.Write($"{file_name}[{df.N_Series}列;{df.N_Record}行]\n\t");
                    foreach (var item in df.series_names)
                    {
                        Console.Write(item + "\t");
                    }
                    Console.WriteLine();
                    if (df.N_Series + 1 != series_names_distinct.Count)//加1，因为增加了一列:数据来源
                        is_same = false;
                }
                Console.WriteLine();
                Console.Write($"所有excel合并后，有[{series_names_distinct.Count - 1}列]，列名为:\n\t");
                for (int i = 1; i < series_names_distinct.Count; i++)
                {
                    Console.Write(series_names_distinct[i] + "\t");
                }
                Console.WriteLine();
                if (is_same == false)
                {
                    Console.WriteLine("所有excel表的格式存在不一致");
                    return;
                }
                else
                    Console.WriteLine("\n所有excel表的格式一致，可以采用行形式合并");

                MyDataFrame df_combine = MyDataFrame.create(series_names_distinct.ToArray());//创建新dataframe

                foreach (var (file_name, df) in dfs)
                {
                    for (int i = 0; i < df.N_Record; i++)
                    {
                        var record = df_combine.new_record();
                        foreach (var series_name in df.series_names)
                        {
                            record[series_name] = df[i, series_name];
                        }
                        record["表源(source)"] = file_name;
                        df_combine.add_record(record);
                    }
                }
                df_combine.show_win();
            }

        }

        //合并选择Excel文件
        //示例：
        //Excel-A
        //0 a
        //1 b
        //2 c
        //3 d
        //Excel-B
        //10
        //12
        //13
        //14
        //Excel-Combine
        //0 a 10
        //1 b 12
        //2 c 13
        //3 d 14
        public static void N个excel文件按列合并()
        {
            Dictionary<string, MyDataFrame> dfs = new();
            OpenFileDialog ofd = new()
            {
                Multiselect = true,
                Filter = "Excel(*.xlsx;*.xls)|*.xlsx;*.xls"
            };
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                var file_paths = ofd.FileNames;
                List<string> series_names = new();//序列名称=excel文件名称+excel文件列名
                int count_max = 1;//最大行数
                foreach (var file_path in file_paths)
                {
                    string file_name = FileHelper.GetFileName(file_path, false);
                    dfs.Add(file_name, MyDataFrame.read_from_excel(file_path));
                    for (int i = 0; i < dfs[file_name].series_names.Length; i++)
                        series_names.Add($"{file_name}_|_{dfs[file_name].series_names[i]}");
                    count_max = Math.Max(count_max, dfs[file_name].N_Record);
                }
                MyDataFrame df_combine = MyDataFrame.create(series_names.ToArray());//创建新dataframe

                for (int i = 0; i < count_max; i++)
                {
                    var record = df_combine.new_record();
                    foreach (var series_name in df_combine.series_names)
                    {
                        string[] array_split = series_name.Split("_|_");
                        string file_name = array_split[0];
                        string series_name_in_single_file = array_split[1];
                        MyDataFrame df = dfs[file_name];
                        object value = null;
                        if (i < df.N_Record)
                            value = df[i, series_name_in_single_file];
                        record[series_name] = value;
                    }
                    df_combine.add_record(record);
                }
                df_combine.show_win();
                MyDataFrame.write_to_excel(df_combine);
            }
        }

        public static void xls与xlsx格式互相转换()
        {
            var operate_type = MyConsoleHelper.read_string_from_console("\n输入1:xls => xlsx \n输入2:xlsx => xls");
            OpenFileDialog ofd = new();
            if (operate_type == "1")
            {
                ofd.Filter = "(*.xls)|*.xls";
            }
            else if (operate_type == "2")
                ofd.Filter = "(*.xlsx)|*.xlsx";
            ofd.Multiselect = true;
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                var fileNames = ofd.FileNames;
                foreach (var fileName in fileNames)
                {
                    var dt = ExcelHelper.excel_to_dataTable(fileName);
                    if (operate_type == "1")
                    {
                        var fileName_xlsx = Path.ChangeExtension(fileName, "xlsx");
                        ExcelHelper.dataTable_to_excel(fileName_xlsx, dt);
                    }
                    else if (operate_type == "2")
                    {
                        var fileName_xls = Path.ChangeExtension(fileName, "xls");
                        ExcelHelper.dataTable_to_excel(fileName_xls, dt);
                    }
                }
                MessageBox.Show("任务完成!");
            }

        }

        //暂时只这对第1列
        public static void excel单元格向下充填()
        {
            Console.Write("\n(输入1:直接使用 输入2:示例数据) 输入值= ");
            string input = Console.ReadLine();
            if (input == "1")
            {
                var df = MyDataFrame.read_from_excel();
                df.show_win();
                object valid = null;
                for (int i = 0; i < df.N_Record; i++)
                {
                    if (df[i, 0].ToString() != string.Empty)
                    {
                        valid = df[i, 0];
                    }
                    else
                    {
                        df[i, 0] = valid;
                    }
                }
                df.show_win();
                MyDataFrame.write_to_excel(df);
            }
            if (input == "2")
            {
                string embeddedFilePath = "JAM8.资源文件.[示例数据]excel单元格向下充填.xlsx";
                string ext = Path.GetExtension(embeddedFilePath);
                using Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(embeddedFilePath);
                // 把 Stream 转换成 byte[]   
                byte[] bytes = new byte[stream.Length];
                stream.Read(bytes, 0, bytes.Length);
                // 设置当前流的位置为流的开始   
                stream.Seek(0, SeekOrigin.Begin);
                ExcelHelper.ExcelStreamType excel_stream_type = ExcelHelper.ExcelStreamType.xls;
                if (ext == ".xlsx")
                    excel_stream_type = ExcelHelper.ExcelStreamType.xlsx;
                var df = MyDataFrame.read_from_excel(stream, excel_stream_type);
                df.show_win("实例数据", true);
            }
        }

        public static void 去除excel指定列的单元格字符串中所有空格符()
        {
            var df = MyDataFrame.read_from_excel();
            df.show_win();
            for (int i = 0; i < df.N_Record; i++)
            {
                df[i, 0] = df[i, 0].ToString().Replace(" ", "");
            }
            df.show_win();
            MyDataFrame.write_to_excel(df);
        }
    }
}
