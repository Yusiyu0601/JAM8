using System.Collections.Generic;
using EasyConsole;
using JAM8.Algorithms.Forms;
using JAM8.Algorithms.Geometry;
using JAM8.Algorithms.Numerics;
using JAM8.SpecificApps.常用工具;
using JAM8.Utilities;

namespace JAM8.Console.Pages
{
    internal class Test_Algs : Page
    {
        public Test_Algs(EasyConsole.Program program) : base("Test_Algs", program) { }

        public override void Display()
        {
            base.Display();

            Output.WriteLine(ConsoleColor.Green, "Test_Algs 功能：");

            Perform();

            System.Console.WriteLine("按任意键返回");
            System.Console.ReadKey();

            Program.NavigateBack();
        }

        private void Perform()
        {
            var menu = new EasyConsole.Menu()

           .Add("退出", CommonFunctions.Cancel)
           .Add("Snesim测试（正逆查询树）", Snesim测试)
           .Add("统计", 统计)
           .Add("从很大数组里等间距取值（例如等份100份）", 从很大数组里等间距取值)
           ;

            menu.Display();
        }

        private void 从很大数组里等间距取值()
        {
            MyDataFrame df = MyDataFrame.read_from_excel();
            df.show_win("", true);
            var start = 0;
            var end = df.N_Record - 1;
            var sample_indexes = MyGenerator.linespace(start, end, 101);

            MyDataFrame df_sample = MyDataFrame.create(df.series_names);
            var record = df_sample.new_record();
            foreach (var series_name in df_sample.series_names)
            {
                record[series_name] = 0;
            }
            df_sample.add_record(record);

            for (int i = 1; i < sample_indexes.Length; i++)
            {
                var idx = (int)sample_indexes[i];
                df_sample.add_record(df.get_record(idx));
            }

            df_sample.show_win();
        }

        private void 统计()
        {
            MyDataFrame df = MyDataFrame.read_from_excel();
            df.show_win("", true);

            MyDataFrame new_df = MyDataFrame.create(df.series_names);

            Dictionary<string, double[]> temp = [];

            for (int i = 0; i < df.N_Series; i++)
            {
                var series_buffer = df.get_series<double>(i);

                // 分成100等份
                int partitionSize = series_buffer.Length / 100; // 每一份的大小

                var list2 = series_buffer
                    .Select((value, index) => new { value, index }) // 把值和索引配对
                    .GroupBy(x => x.index / partitionSize)         // 按索引分组
                    .Select(group => group.Average(x => x.value))  // 计算每组的均值
                    .ToArray();

                temp.Add(df.series_names[i], list2);
            }

            for (int i = 0; i < temp.FirstOrDefault().Value.Length; i++)
            {
                List<object> objs = [];
                foreach (var (key, value) in temp)
                {
                    objs.Add(temp[key][i]);
                }

                new_df.add_record(objs.ToArray());
            }

            new_df.show_win("NodeCountAverage", true);
        }

        private void Snesim测试()
        {
            Output.WriteLine(ConsoleColor.Yellow, "加载训练图像");
            Grid g = null;
            Form_GridCatalog frm = new();
            if (frm.ShowDialog() != DialogResult.OK)
            {
                g = Grid.create_from_gslibwin().grid;
            }
            else
            {
                g = frm.selected_grids.FirstOrDefault();
            }

            if (g == null)
                return;

            var ti = g.first_gridProperty();
            Mould mould = ti.gridStructure.dim == Dimension.D2 ? Mould.create_by_ellipse(10, 10, 1) :
                Mould.create_by_ellipse(7, 7, 2, 1);
            mould = Mould.create_by_mould(mould, 60);

            //手动测试
            int progress_for_inverse_retrieve = EasyConsole.Input.ReadInt("逆向查询占比", 0, 100);
            Snesim snesim = Snesim.create();
            GridStructure re_gs = ti.gridStructure;
            //re_gs = GridStructure.create_simple(100, 100, 1);
            var (re, time) = snesim.run(ti, null, re_gs, 1001, mould, 1, progress_for_inverse_retrieve);
            //re?.showGrid_win();
            Output.WriteLine(ConsoleColor.Red, $"{time}毫秒");

            //批量测试
            //MyDataFrame df = MyDataFrame.create(["占比", "时间"]);
            //for (int i = 0; i <= 100; i += 5)
            //{
            //    int progress_for_inverse_retrieve = i;

            //    Snesim snesim = Snesim.create();
            //    var (re, time) = snesim.run(ti, null, ti.gridStructure, 1001, mould, 1, progress_for_inverse_retrieve);
            //    //re?.showGrid_win();
            //    Output.WriteLine(ConsoleColor.Red, $"{time}毫秒");

            //    df.add_record([i, time]);
            //}
            //df.show_win("逆向查询占比与时间", true);
        }
    }
}
