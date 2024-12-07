using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using JAM8.Algorithms.Geometry;
using JAM8.Utilities;

namespace JAM8.SpecificApps.常用工具
{
    public class Tool_Variogram
    {
        public static void 根据实验变差函数自动拟合_多组计算()
        {
            Form_MultiVariograms frm = new();
            frm.ShowDialog();
        }

        public static void 计算GridProperty的实验变差函数()
        {
            Variogram.variogramFit4Grid_win();
        }

        public static void 根据实验变差函数自动拟合()
        {
            Variogram.variogramFit_win();
        }

        public static void 根据理论变差函数模型计算实验变差函数()
        {
            MyConsoleHelper.write_string_to_console("导入文件格式说明", "1.excel文件；\n2.每行是一个理论模型[range nugget sill type]");
            MyDataFrame df = MyDataFrame.read_from_excel();
            df.show_win();

            double max = MyConsoleHelper.read_double_from_console("设置滞后距最大值");
            int N = 100;
            double step = max / 100;

            List<string> series_names = new();
            series_names.Add("id");
            for (int i = 0; i < 100; i++)
            {
                series_names.Add($"lag{i + 1}");
            }
            MyDataFrame df_结果 = MyDataFrame.create(series_names);


            for (int record_idx = 0; record_idx < df.N_Record; record_idx++)
            {
                var range = float.Parse(df[record_idx, "range"].ToString());
                var nugget = float.Parse(df[record_idx, "nugget"].ToString());
                var sill = float.Parse(df[record_idx, "sill"].ToString());
                var type = df[record_idx, "type"].ToString();

                Variogram variogram = Variogram.create(VariogramType.Spherical, nugget, sill, range);

                var record = df_结果.new_record();
                record["id"] = record_idx + 1;
                for (int i = 0; i < N; i += 1)
                {
                    record[$"lag{i + 1}"] = variogram.calc_variogram((float)(i * step));
                }
                df_结果.add_record(record);
            }
            df_结果.show_win();
        }
    }
}
