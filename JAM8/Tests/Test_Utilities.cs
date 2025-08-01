using JAM8.Algorithms.Numerics;
using JAM8.Utilities;

namespace JAM8.Tests
{
    public class Test_Utilities
    {
        public static void Test_MyDataFrame()
        {
            var _50_realiztions_纹理特征 = new Dictionary<string, MyDataFrame>();
            var file_paths = DirHelper.GetFileNames();
            foreach (var file_path in file_paths)
            {
                var df = MyDataFrame.read_from_csv(file_path);
                //df = df.get_series_subset(new List<string>() { "\"Entropy\"" });
                for (int iRecord = 0; iRecord < df.N_Record; iRecord++)
                {
                    string value = df[iRecord, 0].ToString().Trim('\"');
                    df[iRecord, 0] = value;
                }

                string name = FileHelper.GetFileName(file_path, false);
                name = name.Split(" ")[0];
                _50_realiztions_纹理特征.Add(name, df);
            }

            //50个TI的纹理特征
            var df_TIs = MyDataFrame.read_from_csv();
            //df_TIs = df_TIs.get_series_subset(new List<string>() { "Entropy", "Label" });
            df_TIs.show_win();

            //var df_result = MyDataFrame.create_from_dataframe(df_TIs, new string[] { "hsim" });
            var df_result = df_TIs.deep_clone();
            df_result.add_series("hsim");
            for (int record_idx = 0; record_idx < df_TIs.N_Record; record_idx++)
            {
                string name = df_TIs[record_idx, "Label"].ToString();
                List<double> vector_ti = new();
                for (int series_idx = 0; series_idx <= 12; series_idx++)
                {
                    double value_ti = double.Parse(df_TIs[record_idx, series_idx].ToString());
                    if (series_idx == 2)
                        vector_ti.Add(value_ti);
                }

                var realiztion_纹理特征 = _50_realiztions_纹理特征[name];
                List<double> differences = new();
                for (int iRecord2 = 0; iRecord2 < realiztion_纹理特征.N_Record; iRecord2++)
                {
                    List<double> vector_re = new();
                    for (int series_idx = 0; series_idx <= 12; series_idx++)
                    {
                        double value_re = double.Parse(realiztion_纹理特征[record_idx, series_idx].ToString().Trim('\"'));
                        if (series_idx == 2)
                            vector_re.Add(value_re);
                    }

                    var hsim = MyDistance.calc_hsim(vector_ti.ToArray(), vector_re.ToArray());

                    differences.Add(hsim);
                }

                df_result[record_idx, "hsim"] = differences.Average();
            }

            df_result.show_win();
        }


        public static void Linespace()
        {
            var idx = MyGenerator.linspace(1, 100, 100);
        }

        private class ABC
        {
            public string a { get; set; }
            public string b { get; set; }
            public string c { get; set; }
        }

        public static void EntityHelper_Test()
        {
            OpenFileDialog ofd = new();
            ofd.ShowDialog();
            var dt = ExcelHelper.excel_to_dataTable(ofd.FileName);
            MyDataFrame df = MyDataFrame.create_from_datatable(dt);
            df.print();
            List<ABC> abc = EntityHelper.dataTable_to_entities<ABC>(dt);
            Console.WriteLine(abc.Count);
            foreach (var item in abc)
            {
                Console.WriteLine($@"{item.a}, {item.b}, {item.c}");
            }
        }

        public static void Excel_Test()
        {
            var dt = ExcelHelper.excel_to_dataTable(FileDialogHelper.OpenExcel());
            DataTableHelper.show_win(dt);
            ExcelHelper.dataTable_to_excel(FileDialogHelper.SaveExcel(), dt);
            return;
            //OpenFileDialog ofd = new();
            //ofd.ShowDialog();
            //var dt = ExcelHelper.xls_to_dataTable(ofd.FileName);
            //var ds = ExcelHelper.xls_to_dataSet(ofd.FileName);
            //var df = MyDataFrame.create_from_dataTable(dt);
            //df.show_console();
            //SaveFileDialog sfd = new();
            //sfd.ShowDialog();
            //ExcelHelper.dataTable_to_xls(sfd.FileName, dt);
        }

        public static void GenericCopier_Test()
        {
            List<List<int>> a = new()
            {
                new() { 1 }
            };
            var c = new List<List<int>>();
            c.AddRange(a);
            List<List<int>> d = a.Select(i => i).ToList();
            a[0].Add(333);
        }

        public static void MyDataFrame_Test()
        {
            //string[] series_names = new string[] { "AAAA", "BB", "AAA", "AAAA", "BB", "AAAA" };
            //var df = MyDataFrame.create(series_names);
            //df.show_win();

            MyDataFrame df = MyDataFrame.read_from_excel();
            var j_array = df.get_series_subset(df.series_names.Take(1).ToArray()).convert_to_double_jagged_array();
            var array = df.convert_to_float_2dArray();
        }
    }
}