using EasyConsole;
using JAM8.Algorithms.Forms;
using JAM8.Algorithms.Geometry;
using JAM8.Algorithms.Numerics;
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
           .Add("Snesim_Test1", Snesim_Test1)
           .Add("Snesim_Test2", Snesim_Test2)
           .Add("统计", 统计)
           .Add("从很大数组里等间距取值（例如等份100份）", 从很大数组里等间距取值)
           .Add("GridProperty_replace_with_threshold", GridProperty_replace_with_threshold)
           .Add("ENESIM测试", ENESIM测试)
           .Add("CData2测试", CData2测试)
           ;

            menu.Display();
        }


        private void CData2测试()
        {
            CData2 cd2 = CData2.read_from_gslib_win().cdata;
            var (coarsend, g) = cd2.coarsened(GridStructure.create_simple(100, 100, 1));
            g.showGrid_win();

            var value = cd2[10, 1];

            var cd2_clone = cd2.deep_clone();
        }

        private void ENESIM测试()
        {
            Output.WriteLine(ConsoleColor.Yellow, "加载训练图像");

            GridProperty ti = null;
            Form_GridCatalog frm = new();
            if (frm.ShowDialog() != DialogResult.OK)
            {
                ti = Grid.create_from_gslibwin().grid.first_gridProperty();
            }
            else
            {
                ti = frm.selected_grids.FirstOrDefault().first_gridProperty();
            }

            if (ti == null)
                return;

            GridStructure re_gs = ti.gridStructure;
            Grid sim_grid = Grid.create(re_gs);//包含概率体数据、硬数据（赋值在模拟网格中）

            Output.WriteLine(ConsoleColor.Yellow, "打开cd");
            CData cd = CData.read_from_gslibwin("打开cd").cdata;
            if (cd != null)
            {
                var gp_cd = cd.assign_to_grid(re_gs).grid_assigned.select_gridProperty_win().grid_property;
                sim_grid.add_gridProperty("re", gp_cd);//将cd赋值给grid
            }
            else
            {
                sim_grid.add_gridProperty("re");//将cd赋值给grid
            }

            Output.WriteLine(ConsoleColor.Yellow, "加载软数据（概率体模型）");
            var soft_cd = Grid.create_from_gslibwin().grid.select_gridProperty_win("选择软条件数据").grid_property;
            sim_grid.add_gridProperty("soft_cd", soft_cd);//将cd赋值给grid

            Mould mould = ti.gridStructure.dim == Dimension.D2 ? Mould.create_by_ellipse(10, 10, 1) :
                Mould.create_by_ellipse(7, 7, 3, 1);
            mould = Mould.create_by_mould(mould, 4);


            sim_grid.showGrid_win();

            ENESIM enesim = ENESIM.create(sim_grid, ti);

            var re = enesim.run(15, 10);
        }

        private void GridProperty_replace_with_threshold()
        {
            Form_GridCatalog frm = new();
            if (frm.ShowDialog() != DialogResult.OK)
                return;
            var g = frm.selected_gridProperty;
            g.show_win("原始");

            var g1 = GridProperty.create(g,
                (g.Min, 0.2f, null),
                (0.5f, g.Max, null)
                );
            g1.show_win("测试1");

            var (a, b) = g.get_values_by_condition(0.2f, CompareType.GreaterThan);
            g.set_values_by_range(0.2f, 0.5f, null);
            g.show_win("测试2");

            //g.set_values_by_condition((float?)0.5, null, CompareType.GreaterThan);
            //g.set_values_by_condition((float?)0.2, null, CompareType.LessThan);
            //g.show_win("测试4");


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

        private void Snesim_Test1()
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
                Mould.create_by_ellipse(7, 7, 3, 1);
            mould = Mould.create_by_mould(mould, 40);

            //手动测试
            //int progress_for_inverse_retrieve = EasyConsole.Input.ReadInt("逆向查询占比", 0, 100);
            //Snesim snesim = Snesim.create();
            //GridStructure re_gs = ti.gridStructure;
            ////re_gs = GridStructure.create_simple(100, 100, 1);
            //var (re, time) = snesim.run(ti, null, re_gs, 1001, mould, 1, progress_for_inverse_retrieve);
            ////re?.showGrid_win();
            //Output.WriteLine(ConsoleColor.Red, $"{time}毫秒");

            //批量测试 反向查询占比 vs 加速比
            //MyDataFrame df = MyDataFrame.create(["占比", "时间(秒)", "加速比"]);
            //double first = 0;
            //for (int i = 0; i <= 100; i += 5)
            //{
            //    if (i != 35 && i != 0)
            //        continue;
            //    int progress_for_inverse_retrieve = i;

            //    Snesim snesim = Snesim.create();
            //    GridStructure re_gs = ti.gridStructure;
            //    GridStructure re_gs_2d = GridStructure.create_simple(300, 300, 1);
            //    GridStructure re_gs_3d = GridStructure.create_simple(80, 80, 30);
            //    re_gs = ti.gridStructure.dim == Dimension.D2 ? re_gs_2d : re_gs_3d;
            //    var (re, time) = snesim.run(ti, null, re_gs, 1001, mould, 1, progress_for_inverse_retrieve);
            //    if (i == 0)
            //        first = time;
            //    //re?.showGrid_win();
            //    //加速比
            //    double 加速比 = first / time;
            //    Output.WriteLine(ConsoleColor.Red, $"[{i}] {time / 1000.0}秒  加速比:{加速比}");

            //    df.add_record([i, time / 1000.0, 加速比]);
            //}
            //df.show_win("逆向查询占比与时间", true);

            //批量测试cpu vs 加速比
            double first = 0;
            double sum_35percent = 0;

            Snesim snesim = Snesim.create();
            GridStructure re_gs = ti.gridStructure;
            GridStructure re_gs_2d = GridStructure.create_simple(300, 300, 1);
            GridStructure re_gs_3d = GridStructure.create_simple(80, 80, 30);
            re_gs = ti.gridStructure.dim == Dimension.D2 ? re_gs_2d : re_gs_3d;

            //(var _, first) = snesim.run(ti, null, re_gs, 1001, mould, 1, 0);
            Output.WriteLine(ConsoleColor.Red, $"时间:{first}");

            CData2 cd = CData2.read_from_gslib_win().cdata;

            for (int j = 0; j < 10; j++)
            {
                var (result, time) = snesim.run(ti, cd, re_gs, 1001, mould, 1, 35);
                sum_35percent += time;
                Output.WriteLine(ConsoleColor.Red, $"时间:{time}");
                result.showGrid_win();
            }
            //加速比
            double 加速比 = first / (sum_35percent / 10.0);
            Output.WriteLine(ConsoleColor.Red, $"使用时间:{(sum_35percent / 10.0)}");
            Output.WriteLine(ConsoleColor.Red, $"加速比:{加速比}");

        }


        private void Snesim_Test2()
        {
            Output.WriteLine(ConsoleColor.Yellow, "load training image");
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
            Mould mould = ti.gridStructure.dim == Dimension.D2 ?
                Mould.create_by_ellipse(10, 10, 1) :
                Mould.create_by_ellipse(7, 7, 3, 1);

            mould = Mould.create_by_mould(mould, 40);

            //手动测试
            int progress_for_inverse_retrieve = EasyConsole.Input.ReadInt("逆向查询占比", 0, 100);

            Snesim snesim = Snesim.create();

            GridStructure re_gs = ti.gridStructure;

            CData2 cd = CData2.read_from_gslib_win().cdata;

            var corsened_cd = cd.coarsened(re_gs).coarsened_cd;

            var (re, time) = snesim.run(1001, 3, 40, (10, 10, 1), ti, cd, re_gs, 0);

            re.showGrid_win();

            var (not_match_number, not_match_array_index) = 
                corsened_cd.check_match(re.first_gridProperty(), corsened_cd.property_names[0]);

            //re?.showGrid_win();
            Output.WriteLine(ConsoleColor.Red, $"{time}毫秒");
        }
    }
}
