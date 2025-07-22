using System.Diagnostics;
using EasyConsole;
using JAM8.Algorithms;
using JAM8.Algorithms.Forms;
using JAM8.Algorithms.Geometry;
using JAM8.Algorithms.Numerics;
using JAM8.Utilities;

namespace JAM8.Console.Pages
{
    internal class Test_Algs : Page
    {
        public Test_Algs(EasyConsole.Program program) : base("Test_Algs", program)
        {
        }

        public override void Display()
        {
            base.Display();

            Output.WriteLine(ConsoleColor.Green, "Test_Algs 功能：");

            Perform();

            System.Console.WriteLine(@"按任意键返回");
            System.Console.ReadKey();

            Program.NavigateBack();
        }

        private void Perform()
        {
            var menu = new EasyConsole.Menu()
                    .Add("退出", CommonFunctions.Cancel)
                    .Add("Snesim_Test1(指定网格级别)", Snesim_Test1)
                    .Add("Snesim_Test2(多重网格)", Snesim_Test2)
                    .Add("统计", 统计)
                    .Add("从很大数组里等间距取值（例如等份100份）", 从很大数组里等间距取值)
                    .Add("GridProperty_replace_with_threshold", GridProperty_replace_with_threshold)
                    .Add("ENESIM测试", ENESIM测试)
                    .Add("CData2测试", CData2测试)
                    .Add("Patterns测试", Patterns测试)
                    .Add("TEST_DistanceTransform", TEST_DistanceTransform)
                    .Add("Test_GridProperty_connected_components_labeling",
                        Test_GridProperty_connected_components_labeling)
                    .Add("Test_GridProperty_connectivity_function",
                        Test_GridProperty_connectivity_function)
                    .Add("GridProperty_去噪点", GridProperty_去噪点)
                    .Add("Variogram_calc_experiment_variogram_grid", Variogram_calc_experiment_variogram_grid)
                    .Add("Form_VariogramFit4PointSet", Form_VariogramFit4PointSet)
                    .Add("test_CDataNearestFinder_kdtree", test_CDataNearestFinder_kdtree)
                ;

            menu.Display();
        }

        private void test_CDataNearestFinder_kdtree()
        {
            CData cdata2 = CData.read_from_gslib_win().cdata;
            var finder = CDataNearestFinder_kdtree.create(cdata2);
            var results = finder.find(Coord.create(50, 30, 10), 5);
            foreach (var (idx, coord, attrs, dist) in results)
            {
                System.Console.WriteLine($"Index: {idx}, Coord: {coord}, Distance: {dist:F2}");
                foreach (var kv in attrs)
                {
                    System.Console.WriteLine($"  {kv.Key} = {kv.Value}");
                }
            }
        }

        private void Form_VariogramFit4PointSet()
        {
            Form_VariogramFit4PointSet form = new();
            form.ShowDialog();
        }

        private void Variogram_calc_experiment_variogram_grid()
        {
            GridProperty gp = Grid.create_from_gslibwin().grid.select_gridProperty_win("选择GridProperty").grid_property;

            gp.show_win();

            // var (a, b, c) = Variogram.calc_variogram_from_grid_2d(gp, 0, 30, 1, 3);
        }

        private void GridProperty_去噪点()
        {
            GridStructure gs = GridStructure.create_win();
            var g = Grid.create(gs);
            OpenFileDialog ofd = new();
            if (ofd.ShowDialog() != DialogResult.OK)
                return;

            g.read_from_gslib(ofd.FileName, 0, -99);
            var gp = g.first_gridProperty();

            gp.show_win("原始");

            var result = gp.deep_clone();
            Mould mould = Mould.create_by_ellipse(3, 3, 3, 1);
            for (int n = 0; n < gp.grid_structure.N; n++)
            {
                MyConsoleProgress.Print(n, gp.grid_structure.N, "去噪点");
                var pattern = MouldInstance.create_from_gridProperty(mould, gp.grid_structure.get_spatial_index(n), gp);
                var values = pattern.neighbor_values;
                var (a, b) = MyArrayHelper.FindMode(values, false);
                result.set_value(n, a);
            }

            result.show_win("去噪点");
        }

        private void Test_GridProperty_connectivity_function()
        {
            int distanceSteps = 50;
            double maxDistance = 100;

            GridProperty gp = Grid.create_from_gslibwin().grid.select_gridProperty_win("选择GridProperty").grid_property;
            gp.set_values_by_condition(1, null, CompareType.NotEqual);
            gp.show_win();

            List<double> connectivityValues = new List<double>(); // 用于存储每个滞后距离的连通性值
            List<double> distances = new List<double>(); // 用于存储每个滞后距离

            // 计算从最小距离到最大距离的连通性函数
            for (int i = 1; i <= distanceSteps; i++)
            {
                double distance = (maxDistance / distanceSteps) * i; // 当前滞后距离
                double connectivity = gp.ConnectivityFunction(distance, Math.PI / 2); // 角度设置为45度
                distances.Add(distance);
                connectivityValues.Add(connectivity);
            }

            // 输出每个距离对应的连通性值
            for (int i = 0; i < distances.Count; i++)
            {
                System.Console.Out.WriteLine($"Distance: {distances[i]}, Connectivity: {connectivityValues[i]}");
            }

            Form_QuickChart.ScatterPlot(distances, connectivityValues, null,
                "x", "y", "title");
        }

        private void Test_GridProperty_connected_components_labeling()
        {
            GridProperty gp = Grid.create_from_gslibwin().grid.select_gridProperty_win("选择GridProperty").grid_property;
            gp.set_values_by_condition(1, null, CompareType.NotEqual);
            gp.show_win();

            gp.connected_components_labeling_3d().show_win();
        }

        private void TEST_DistanceTransform()
        {
            GridProperty gp = Grid.create_from_gslibwin().grid.select_gridProperty_win("选择GridProperty").grid_property;
            gp.set_values_by_condition(2, 0, CompareType.Equals);
            gp.show_win("原始");

            var dist_grid = DistanceTransform.EuclideanDistanceTransform(gp, 1);
            dist_grid.show_win("距离变换");
        }

        private void Patterns测试()
        {
            var ti = Grid.create_from_gslibwin().grid.select_gridProperty_win().grid_property;
            var mould = Mould.create_by_ellipse(8, 8, 1);
            var patterns = Patterns.create(mould, ti, false);
            System.Console.WriteLine(patterns.Count);
        }

        private void CData2测试()
        {
            CData cd2 = CData.read_from_gslib_win().cdata;
            var (coarsend, g) = cd2.coarsened(GridStructure.create_simple(300, 300, 1));
            g.showGrid_win();

            var value = cd2.get_value(10, 1);

            var cdi = cd2.get_cdata_item(19);

            var cd2_clone = cd2.deep_clone();

            var boundary = cd2_clone.get_boundary();

            cd2_clone.print();

            System.Console.WriteLine(cd2_clone);
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

            GridStructure re_gs = ti.grid_structure;
            Grid sim_grid = Grid.create(re_gs); //包含概率体数据、硬数据（赋值在模拟网格中）

            Output.WriteLine(ConsoleColor.Yellow, "打开cd");
            CData cd = CData.read_from_gslib_win("打开cd").cdata;
            if (cd != null)
            {
                var gp_cd = cd.coarsened(re_gs).coarsened_grid.select_gridProperty_win().grid_property;
                sim_grid.add_gridProperty("re", gp_cd); //将cd赋值给grid
            }
            else
            {
                sim_grid.add_gridProperty("re"); //将cd赋值给grid
            }

            // Output.WriteLine(ConsoleColor.Yellow, "加载软数据（概率体模型）");
            // var soft_cd = Grid.create_from_gslibwin().grid.select_gridProperty_win("选择软条件数据").grid_property;
            // sim_grid.add_gridProperty("soft_cd", soft_cd); //将cd赋值给grid

            // Mould mould = ti.grid_structure.dim == Dimension.D2
            //     ? Mould.create_by_ellipse(10, 10, 1)
            //     : Mould.create_by_ellipse(7, 7, 3, 1);
            // mould = Mould.create_by_mould(mould, 4);


            sim_grid.showGrid_win();

            ENESIM enesim = ENESIM.create(sim_grid, ti);

            //统计计算时间
            Stopwatch sw = new();
            sw.Start();

            var re = enesim.run(20, 15);

            sw.Stop();
            Output.WriteLine(ConsoleColor.Red, $"ENESIM运行时间: {sw.ElapsedMilliseconds}毫秒");

            re.showGrid_win("ENESIM结果");
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
                    .GroupBy(x => x.index / partitionSize) // 按索引分组
                    .Select(group => group.Average(x => x.value)) // 计算每组的均值
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

        //指定网格级别
        private void Snesim_Test1()
        {
            //read training image from GSLIB file
            Output.WriteLine(ConsoleColor.Yellow, "read training image from GSLIB file");
            Form_GridCatalog frm = new();
            var g_TI = (frm.ShowDialog() != DialogResult.OK
                ? Grid.create_from_gslibwin().grid
                : frm.selected_grids.FirstOrDefault());
            if (g_TI == null)
                return;


            //create mould
            Mould mould = g_TI.gridStructure.dim == Dimension.D2
                ? Mould.create_by_ellipse(10, 10, 1)
                : Mould.create_by_ellipse(10, 10, 10, 1);
            //set max number of nodes in mould for snesim
            int max_number = Input.ReadInt("max number of nodes in mould for snesim: ", 10, 100);
            mould = Mould.create_by_mould(mould, max_number);


            //create grid structure for simulation
            GridStructure re_gs = GridStructure.create_win(null, "create grid structure for simulation");

            //set random seed
            int random_seed = Input.ReadInt("set random seed: ", 0, 1000000);

            //set multigrid level
            int multigrid_level = Input.ReadInt("set multigrid level: ", 1, 5);

            Snesim snesim = Snesim.create();
            CData cd = CData.read_from_gslib_win().cdata;

            while (true)
            {
                //set retrieve inverse proportion
                int progress_for_retrieve_inverse = Input.ReadInt("set retrieve inverse proportion (0-100): ", -1, 100);

                if (progress_for_retrieve_inverse < 0)
                    break;

                Stopwatch sw = new();
                sw.Start();
                for (int j = 0; j < 10; j++)
                {
                    var (result, time) = snesim.run(g_TI.first_gridProperty(), cd, re_gs, random_seed, mould,
                        multigrid_level,
                        progress_for_retrieve_inverse);
                    Output.WriteLine(ConsoleColor.Red, $"时间:{time}");
                }

                sw.Stop();
                Output.WriteLine(ConsoleColor.Red, $"总时间:{sw.ElapsedMilliseconds}毫秒");
            }
        }


        private void Snesim_Test2()
        {
            Output.WriteLine(ConsoleColor.Yellow, "load training image");
            Grid g = null;
            g = Grid.create_from_gslibwin().grid;
            // Form_GridCatalog frm = new();
            // if (frm.ShowDialog() != DialogResult.OK)
            // {
            //     g = Grid.create_from_gslibwin().grid;
            // }
            // else
            // {
            //     g = frm.selected_grids.FirstOrDefault();
            // }

            if (g == null)
                return;

            var ti = g.first_gridProperty();

            //手动测试
            int progress_for_inverse_retrieve = EasyConsole.Input.ReadInt("逆向查询占比", 0, 100);

            Snesim snesim = Snesim.create();

            GridStructure re_gs = ti.grid_structure;
            re_gs = GridStructure.create_simple(101, 101, 1);
            CData cd = CData.read_from_gslib_win().cdata;

            // var corsened_cd = cd.coarsened(re_gs).coarsened_cd;

            var (re, time) = snesim.run(1001, 3, 25, (15, 15, 1),
                ti, cd, re_gs, progress_for_inverse_retrieve);

            // re.showGrid_win();
            //
            // var (not_match_number, not_match_array_index) =
            //     corsened_cd.check_match(re.first_gridProperty(), corsened_cd.property_names[0]);

            //re?.showGrid_win();
            Output.WriteLine(ConsoleColor.Red, $"{time}毫秒");
        }
    }
}