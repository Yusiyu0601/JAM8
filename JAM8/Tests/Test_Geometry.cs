using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using JAM8.Algorithms;
using JAM8.Algorithms.Geometry;
using JAM8.Algorithms.Numerics;
using JAM8.Utilities;
using ScottPlot.Drawing.Colormaps;

namespace JAM8.Tests
{
    public class Test_Geometry
    {
        public static void Test_SimulationPath()
        {
            GridStructure gs = GridStructure.create_win();
            for (int m = 1; m <= 3; m++)
            {
                var path = SimulationPath.create(gs, m, new Random());
                var g = Grid.create(gs, "测试");
                g.add_gridProperty("属性");

                while (path.is_visit_over() == false)
                {
                    var si = path.visit_next();
                    g.first_gridProperty().set_value(si, (float?)path.progress);
                }
                g.showGrid_win(g.grid_name);
            }

        }

        public static void Test_GridCatalog()
        {
            string path = "";
            var option = MyConsoleHelper.read_int_from_console("选择", "0:打开已有文件;1:创建新文件");
            GridCatalog gc = null;
            if (option == 0)
            {
                OpenFileDialog ofd = new()
                {
                    Filter = "excel文件|*.xlsx"
                };
                if (ofd.ShowDialog() != DialogResult.OK)
                    return;
                path = ofd.FileName;
                gc = GridCatalog.open(path);
            }
            else
            {
                SaveFileDialog sfd = new()
                {
                    Filter = "excel文件|*.xlsx"
                };
                if (sfd.ShowDialog() != DialogResult.OK)
                    return;
                path = sfd.FileName;
                gc = GridCatalog.create(path);
            }

            while (true)
            {
                var (g, file_name) = Grid.create_from_gslibwin();
                if (g == null)
                    break;
                gc.add_item(g, file_name, -99);
            }
            var catalog = gc.get_items();
            foreach (var item in catalog)
            {
                gc.read_grid(item.grid_name).showGrid_win();
            }
        }

        public static void Test_CoarsenedCData()
        {
            CData cd = CData.read_from_gslibwin().cdata;
            GridStructure gs = GridStructure.create_win();
            var (ccd, N_out_of_range) = CoarsenedCData.create(gs, cd);
            MyConsoleHelper.write_string_to_console("N_out_of_range", N_out_of_range.ToString());
            var g = ccd.to_grid();
            g.showGrid_win();
            var ccd1 = CoarsenedCData.create(ccd.to_grid());
        }

        //测试用表存储所有点对的各向异性距离
        public static void AnisotropicDistance3d_Test2()
        {
            Dictionary<string, float> dict = new();
            RotMat rm = new(0, 0, 0, 1, 1, 0.2);

            Grid g = Grid.create(GridStructure.create_win());
            g.add_gridProperty("circle");
            SpatialIndex core = SpatialIndex.create(g.gridStructure.nx / 2, g.gridStructure.ny / 2, g.gridStructure.nz / 2);
            for (int n = 0; n < g.gridStructure.N; n++)
            {
                SpatialIndex si = g.gridStructure.get_spatial_index(n);
                var dsi = SpatialIndex.create(si.ix - core.ix, si.iy - core.iy, si.iz - core.iz);
                var anis_dist = AnisotropicDistance.calc_anis_distance_power2(rm, dsi);
                dict.Add(dsi.view_text(), anis_dist);
            }
            MessageBox.Show($"{dict.Count}");
        }

        public static void AnisotropicDistance3d_Test()
        {
            RotMat rm = new(0, 0, 0, 1, 1, 0.2);

            Coord c = Coord.create(1, 1, 1);
            c.ToString();
            var dist = Coord.get_distance_to_origin(c);
            Console.WriteLine(dist);
            var anis_dist = AnisotropicDistance.calc_anis_dist(rm, c);
            Console.WriteLine(anis_dist);

            Grid g = Grid.create(GridStructure.create_win());
            g.add_gridProperty("circle");
            for (int n = 0; n < g.gridStructure.N; n++)
            {
                SpatialIndex si = g.gridStructure.get_spatial_index(n);
                c = Coord.create(si.ix, si.iy, si.iz);
                anis_dist = AnisotropicDistance.calc_anis_dist(rm, Coord.create(50, 50, 50), c);
                if (anis_dist < 50)
                    g.last_gridProperty().set_value(n, 1);
                else
                    g.last_gridProperty().set_value(n, 2);
            }
            g.showGrid_win();

        }

        public static void AnisotropicDistance_Test()
        {
            RotMat rm = new(90, 0, 0, 1, 0.5, 1);

            //计算互换两个点的各向异性距离
            SpatialIndex si1 = SpatialIndex.create(10, 10);
            SpatialIndex si2 = SpatialIndex.create(12, 30);
            Console.WriteLine($@"{si1}与{si2}的各向同性距离为{SpatialIndex.calc_dist(si1, si2)}");
            Console.WriteLine($@"{si2}与{si1}的各向同性距离为{SpatialIndex.calc_dist(si2, si2)}");
            Console.WriteLine($@"{si1}与{si2}的各向异性距离为{AnisotropicDistance.calc_anis_distance_power2(rm, si1, si2)}");
            Console.WriteLine($@"{si2}与{si1}的各向异性距离为{AnisotropicDistance.calc_anis_distance_power2(rm, si2, si1)}");

            Coord c = Coord.create(1, 1);
            c.ToString();
            var dist = Coord.get_distance_to_origin(c);
            Console.WriteLine(dist);
            var anis_dist = AnisotropicDistance.calc_anis_dist(rm, c);
            Console.WriteLine(anis_dist);

            Grid g = Grid.create(GridStructure.create_win());
            g.add_gridProperty("circle");
            for (int n = 0; n < g.gridStructure.N; n++)
            {
                SpatialIndex si = g.gridStructure.get_spatial_index(n);
                c = Coord.create(si.ix, si.iy);
                anis_dist = AnisotropicDistance.calc_anis_dist(rm, Coord.create(50, 50), c);
                if (anis_dist < 25)
                    g.last_gridProperty().set_value(n, 1);
                else
                    g.last_gridProperty().set_value(n, 2);
            }
            g.showGrid_win();

        }

        public static void Scottplot4Grid_Test()
        {
            var (g, file_name) = Grid.create_from_gslibwin();
            Form_Scottplot4Grid frm = new(g, FileHelper.GetFileName(file_name));
            frm.Show();
        }

        public static void 超大Grid读写()
        {
            GridStructure gs = GridStructure.create_win();
            Grid g = Grid.create(gs);
            g.add_gridProperty("gp1");
            g[0].set_values_gaussian(0, 1, new Random());
            g.add_gridProperty("gp2");
            g[1].set_values_gaussian(0, 1, new Random());
            g.add_gridProperty("gp3");
            g[2].set_values_gaussian(0, 1, new Random());
            g.add_gridProperty("gp4");
            g[3].set_values_gaussian(0, 1, new Random());
            g.add_gridProperty("gp5");
            g[4].set_values_gaussian(0, 1, new Random());
            g.add_gridProperty("gp6");
            g[5].set_values_gaussian(0, 1, new Random());
            g.add_gridProperty("gp7");
            g[6].set_values_gaussian(0, 1, new Random());
            g.add_gridProperty("gp8");
            g[7].set_values_gaussian(0, 1, new Random());
            g.add_gridProperty("gp9");
            g[8].set_values_gaussian(0, 1, new Random());
            g.add_gridProperty("gp10");
            g[9].set_values_gaussian(0, 1, new Random());
            g.add_gridProperty("gp11");
            g[10].set_values_gaussian(0, 1, new Random());
            Grid.save_to_gslibwin(g);
        }

        public static void GridStructure设置()
        {
            GridStructure gs = GridStructure.create_win();
            Form_GridStructure frm = new(gs);
            if (frm.ShowDialog() != DialogResult.OK)
                return;

        }

        public static void kdtree测试3()
        {
            GridStructure gs = GridStructure.create_win();
            GridProperty gp = GridProperty.create(gs);
            gp.set_values_gaussian(0, 1, new Random(1));
            gp.show_win();
            CData cd = CData.create_from_gridProperty(gp, null, false);

            Stopwatch sw = new();
            sw.Start();
            Console.WriteLine(@"start");
            //CDataNearestFinder_kdtree4 tree = CDataNearestFinder_kdtree4.create(gs, cd);
            Console.WriteLine(@"end");
            sw.Stop();
            Console.WriteLine(sw.ElapsedMilliseconds);


            sw.Restart();
            Console.WriteLine(@"start");
            Random rnd = new();
            for (int i = 0; i < 10000000; i++)
            {
                SpatialIndex si = SpatialIndex.create(rnd.Next(0, gs.nx), rnd.Next(0, gs.ny), rnd.Next(0, gs.nz));
                //tree.find(si, 50);
                MyConsoleProgress.Print(i, 10000000, "");
            }
            Console.WriteLine(@"end");
            sw.Stop();
            Console.WriteLine(sw.ElapsedMilliseconds);
        }

        public static void spatialIndex_to_coord()
        {
            GridStructure gs = GridStructure.create_win();
            CData cd = CData.read_from_gslibwin().cdata;
            var si = gs.get_spatial_index(20);
            Coord c = gs.spatial_index_to_coord(si);
            Console.WriteLine(c.ToString());
            SpatialIndex si1 = gs.coord_to_spatial_index(c);
            Console.WriteLine(si1.view_text());
        }

        public static void calc_weights_ok_条带效应校正()
        {
            Variogram vm = Variogram.create(VariogramType.Spherical, 0.0f, 1, 20);
            Coord pred_loc = Coord.create(10, 22);
            Coord[] cd_locs = new[]
            {
                Coord.create(5, 10),
                Coord.create(6, 10),
                Coord.create(7, 10),
                Coord.create(8, 10),
                Coord.create(9, 10),
                Coord.create(10, 10),
                Coord.create(11, 10),
                Coord.create(12, 10),
                Coord.create(13, 10),
                Coord.create(14, 10),
                Coord.create(15, 10)
            };

            float[] weights = new float[cd_locs.Length];//由近及远的权重
            var ordered_locs = pred_loc.order_by_distance(cd_locs);//根据与pre_loc的距离由近及远排序
            for (int i = 1; i <= ordered_locs.Count; i++)//由近及远计算权重
            {
                var cd_locs_tmp = ordered_locs.Take(new Range(0, i)).Select(a => a.coord).ToArray();
                var weights_temp = OK.calc_weights_ok(pred_loc, cd_locs_tmp, vm);
                for (int j = 0; j < weights_temp.Length; j++)
                {
                    weights[j] += weights_temp[j];
                }
            }

            var xs = ordered_locs.Select(a => a.coord.x).ToArray();
            var tmp = weights.Zip(xs);
            tmp = tmp.OrderBy(a => a.Second).ToList();
            xs = tmp.Select(a => a.Second).ToArray();
            weights = tmp.Select(a => a.First).ToArray();
            Form_QuickChart.ScatterPlot(xs, weights, null, "位置", "权重");
            Form_QuickChart.BarPlot(weights, xs, "位置", "权重", "有限域克里金权重计算");
        }

        public static void GridStructure_相等运算符重载测试()
        {
            GridStructure gs_left = GridStructure.create_simple(100, 100, 1);
            GridStructure gs_right = GridStructure.create_simple(100, 100, 1);
            Console.WriteLine($@"{gs_left.to_string()}  {gs_right.to_string()}");
            Console.WriteLine(gs_left == gs_right);


            gs_left = GridStructure.create_simple(100, 200, 1);
            gs_right = GridStructure.create_simple(100, 100, 1);
            Console.WriteLine($@"{gs_left.to_string()}  {gs_right.to_string()}");
            Console.WriteLine(gs_left == gs_right);


            gs_left = GridStructure.create_simple(100, 200, 1);
            gs_right = gs_left;
            Console.WriteLine($@"{gs_left.to_string()}  {gs_right.to_string()}");
            Console.WriteLine(gs_left == gs_right);
        }

        public static void Mould_test()
        {
            var mould = Mould.create_by_ellipse(10, 10, 1);
            GridProperty gp = Grid.create_from_gslibwin().grid.first_gridProperty();
            var mould_instance = MouldInstance.create_from_gridProperty(mould, SpatialIndex.create(50, 50), gp);

        }

        public static void calc_weights_ok()
        {
            Variogram vm = Variogram.create(VariogramType.Spherical, 0.0f, 1, 10);
            Coord pred_loc = Coord.create(10, 22);
            Coord[] cd_locs = new Coord[]
            {
                Coord.create(5, 10),
                Coord.create(6, 10),
                Coord.create(7, 10),
                Coord.create(8, 10),
                Coord.create(9, 10),
                Coord.create(10, 10),
                Coord.create(11, 10),
                Coord.create(12, 10),
                Coord.create(13, 10),
                Coord.create(14, 10),
                Coord.create(15, 10)
            };
            var weights = OK.calc_weights_ok(pred_loc, cd_locs, vm);
            var xs = cd_locs.Select(a => a.x).ToArray();
            var ys = cd_locs.Select(a => a.y).ToList();
            Form_QuickChart.ScatterPlot(xs, ys, null, "x", "y", "数据位置");
            Form_QuickChart.BarPlot(weights, xs, "位置", "权重", "普通克里金权重计算");

            var xs1 = cd_locs.Select(a => a.x).ToList();
            xs1.Add(pred_loc.x);
            var ys1 = cd_locs.Select(a => a.y).ToList();
            ys1.Add(pred_loc.y);
            Form_QuickChart.ScatterPlot(xs1, ys1, null, "x", "y", "数据位置");
        }

        public static void get_region_by_range()
        {
            Grid g = Grid.create_from_gslibwin().grid;
            g.showGrid_win();
            //g.first_gridProperty().get_region_by_range(1, 10, 1, 10, 1, 5).show_win();
            g.first_gridProperty().get_region_by_center(SpatialIndex.create(10, 10), 10, 10).region.show_win();
        }

        public static void CData_Test()
        {
            var (cd, _) = CData.read_from_gslibwin();
            //cd.save_to_gslib("D:\\cd.dat");
            //cd.save_to_gslibwin();
            cd.to_dataFrame().show_console();
        }

        public static void STree_Test()
        {
            Grid g = Grid.create_from_gslibwin().grid;
            Mould mould = Mould.create_by_ellipse(10, 10, 1);
            mould = Mould.create_by_mould(mould, 25);
            STree st = STree.create(mould, g.first_gridProperty());
        }

        public static void calc_dataEvent_hsim()
        {
            Grid g = Grid.create_from_gslibwin().grid;
            g.showGrid_win();

            var ix = MyConsoleHelper.read_int_from_console("ix=");
            var iy = MyConsoleHelper.read_int_from_console("iy=");
            var core_loc = SpatialIndex.create(ix, iy);
            int N = 15;
            List<SpatialIndex> points = new();
            Random rnd = new();
            int counter = 0;
            while (true)
            {
                var point = SpatialIndex.create(rnd.Next(ix - 10, ix + 10), rnd.Next(iy - 10, iy + 10));
                if (point.ix >= 1 && point.ix <= g.gridStructure.nx
                    && point.iy >= 1 && point.iy <= g.gridStructure.ny)
                {
                    points.Add(point);
                    counter++;
                }
                if (counter == N)
                    break;
            }
            Mould mould = Mould.create_by_location(core_loc, points);
            //mould = Mould.create_by_ellipse(15, 15, 1);

            MouldInstance mi = MouldInstance.create_from_gridProperty(mould, core_loc, g[0]);

            g.add_gridProperty("dataEvent", g[0].deep_clone());
            foreach (var (_, si) in mould.neighbor_spiral_mapper)
            {
                g["dataEvent"].set_value(si.offset(core_loc), null);
            }

            g.showGrid_win();

            g.add_gridProperty("dist");
            for (int n = 0; n < g.gridStructure.N; n++)
            {
                var core = g.gridStructure.get_spatial_index(n);
                var data_event = MouldInstance.create_from_gridProperty(mould, core, g[0]);
                if (data_event.neighbor_nulls_ids.Count == 0)
                {
                    var dist = MyDistance.calc_hsim(data_event.neighbor_values, mi.neighbor_values);
                    g[2].set_value(n, dist);
                }
            }
            g.showGrid_win();
        }

        public static void calc_time_dataEvent_ti()
        {
            GridStructure gs = GridStructure.create_simple(1000, 1000, 1);
            Grid g = Grid.create(gs);
            g.add_gridProperty("gp1");
            for (int n = 0; n < gs.N; n++)
            {
                g.first_gridProperty().set_value(n, 1);
            }


            Mould mould = Mould.create_by_rectangle(7, 7, 1);
            var data_event = MouldInstance.create_from_gridProperty(mould, SpatialIndex.create(30, 30), g.first_gridProperty());

            List<MouldInstance> patterns_cb = new();
            for (int n = 0; n < gs.N; n++)
            {
                MyConsoleProgress.Print(n, gs.N, "提取样式");
                var pattern = MouldInstance.create_from_gridProperty(mould, gs.get_spatial_index(n), g.first_gridProperty());
                if (pattern.neighbor_nulls_ids.Count == 0)
                    patterns_cb.Add(pattern);
            }

            Stopwatch sw = new();
            sw.Start();
            for (int i = 0; i < patterns_cb.Count; i++)
            {
                var distance = Mould.get_distance(data_event, patterns_cb[i]);
            }
            sw.Stop();
            MyConsoleHelper.write_string_to_console("计算时间", sw.ElapsedMilliseconds.ToString());

        }

        public static void create_mould_by_location()
        {
            SpatialIndex si = SpatialIndex.create(50, 50);

            List<SpatialIndex> locations = new()
            {
                SpatialIndex.create(19, 100),
                SpatialIndex.create(51, 170),
                SpatialIndex.create(15, 104)
            };

            Mould mould = Mould.create_by_location(si, locations);


        }
    }
}
