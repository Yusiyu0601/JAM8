using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JAM8.Utilities;

namespace JAM8.SpecificApps.常用工具
{
    public class Tool_WellLog
    {
        public static void 提取测井曲线部分列()
        {
            Form_提取测井列数据 frm = new();
            frm.ShowDialog();
        }

        public static void 展示测井曲线()
        {
            Form_展示测井曲线 frm = new();
            frm.Show();
        }

        public static void 计算层位内的测井属性均值()
        {
            Form_层位测井均值 frm = new();
            frm.ShowDialog();

            Console.WriteLine();
            Console.WriteLine("本方法实现:根据文件A的某行井名查找相应测井文件B，根据文件A的该行顶深、低深从文件B中提取指定深度段数据，" +
                "计算属性平均值，然后追加到文件A的该行尾部。");
            Console.WriteLine();
            Console.WriteLine(@"提示:输入xlsx文件A格式为 <首行标题> [井名] [层位] [顶深] [低深]");

            DataTable dt = ExcelHelper.excel_to_dataTable(FileDialogHelper.OpenExcel());
            MyDataFrame df_测井解释 = MyDataFrame.create_from_dataTable(dt);
            df_测井解释.show_win("测井解释", true);

            Console.WriteLine();
            Console.Write("\t设置 井名的列序 <从1开始> =");
            int idx_井名 = int.Parse(Console.ReadLine()) - 1;
            Console.Write("\t设置 顶深的列序 <从1开始> =");
            int iDepth_top = int.Parse(Console.ReadLine()) - 1;
            Console.Write("\t设置 底深的列序 <从1开始> =");
            int iDepth_bottom = int.Parse(Console.ReadLine()) - 1;

            Console.WriteLine();
            Console.WriteLine("提示:输入xlsx测井曲线文件B，<首行标题> [1-深度] [2-测井属性1] [3-测井属性2] ...，要求目录下所有文件标题相同、顺序相同");
            Dictionary<string, MyDataFrame> dfs_测井曲线 = new();
            var fileNames = DirHelper.GetFileNames();
            List<string> curve_names = null;//曲线名称
            foreach (var fileName in fileNames)
            {
                if (!fileName.Contains("xlsx"))//仅处理xlsx文件
                    continue;
                Console.WriteLine(fileName);
                MyDataFrame df_测井曲线 = MyDataFrame.read_from_excel(fileName);

                if (df_测井曲线 != null)
                {
                    if (curve_names == null)
                    {
                        int length = df_测井曲线.series_names.Length;
                        curve_names = df_测井曲线.series_names[1..length].ToList();
                    }
                    string 井名 = FileHelper.GetFileName(fileName, false);
                    dfs_测井曲线.Add(井名, df_测井曲线);
                    Console.WriteLine(df_测井曲线.view_text(5));
                }
            }

            List<string> series_names = new(df_测井解释.series_names);//添加AC、RT和DEN等3列测井曲线
            series_names.AddRange(curve_names);
            MyDataFrame df_result = MyDataFrame.create(series_names.ToArray());//创建一个新DataFrame，比原始解释表相比新增了3列测井曲线列

            for (int i = 0; i < df_测井解释.N_Record; i++)//对解释数据逐行计算
            {
                double depth_top = double.Parse(df_测井解释[i, iDepth_top].ToString());//获取顶深
                double depth_bottom = double.Parse(df_测井解释[i, iDepth_bottom].ToString());//获取底深
                string 井名 = df_测井解释[i, idx_井名].ToString();//获取井名

                var record = df_result.new_record();//结果表创建新记录

                //将原始解释数据复制到新表记录中
                foreach (var series_name in df_测井解释.series_names)
                    record[series_name] = df_测井解释[i, series_name];

                if (dfs_测井曲线.ContainsKey(井名))
                {
                    var df_测井曲线 = dfs_测井曲线[井名];//根据井名筛选测井曲线表

                    Dictionary<string, double> sum_curve_深度段内 = new();
                    foreach (var curve_name in curve_names)
                        sum_curve_深度段内.Add(curve_name, 0);
                    int count_samples = 0;//深度段内的样本数量
                    for (int j = 0; j < df_测井曲线.N_Record; j++)
                    {
                        double depth = double.Parse(df_测井曲线[j, 0].ToString());
                        double tolerance_thickness = 0.125;//厚度容差
                        //从测井曲线表里提取指定深度段的测井数据，并求取平均值
                        if (depth > depth_top - tolerance_thickness && depth < depth_bottom + tolerance_thickness)
                        {
                            count_samples += 1;//样本数累加
                            foreach (var curve_name in curve_names)
                                sum_curve_深度段内[curve_name] += double.Parse(df_测井曲线[j, curve_name].ToString());//属性值累加
                        }
                    }

                    if (count_samples > 0)//计算平均值作为该段曲线的数值
                    {
                        foreach (var curve_name in curve_names)
                        {
                            double mean = sum_curve_深度段内[curve_name] / count_samples;
                            record[curve_name] = Math.Round(mean, 2);
                        }
                    }
                    else//实在查找不到，就用-9999
                    {
                        foreach (var curve_name in curve_names)
                            record[curve_name] = -9999;
                    }
                }
                else//找不到该井名的测井数据，也用-9999
                {
                    foreach (var curve_name in curve_names)
                        record[curve_name] = -9999;
                }

                df_result.add_record(record);
            }

            df_result.show_win();

            string root_path = Directory.GetDirectoryRoot(Directory.GetCurrentDirectory());
            string save_path = root_path += "MyDataBox\\tmp files\\测井曲线深度提取_result.xlsx";
            MyDataFrame.write_to_excel(df_result, save_path);
        }
    }
}
