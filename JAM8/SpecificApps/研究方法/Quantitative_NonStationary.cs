﻿using System.Collections.Concurrent;
using System.Diagnostics;
using JAM8.Algorithms;
using JAM8.Algorithms.Forms;
using JAM8.Algorithms.Geometry;
using JAM8.Algorithms.MachineLearning;
using JAM8.Algorithms.Numerics;
using JAM8.Utilities;

namespace JAM8.SpecificApps.研究方法
{
    /// <summary>
    /// 定量评价非平稳性
    /// </summary>
    public class Quantitative_NonStationary
    {
        public static void run()
        {
            //step1_模式相似度();
            step1_实验变差函数();
            //直接计算方法2d();
        }

        #region 分步计算方法

        #region step1 模式相似度

        static void step1_模式相似度()
        {
            #region 加载TI

            var (grid, fileName) = Grid.create_from_gslibwin();
            GridStructure gs = grid.gridStructure;
            var ti = grid.select_gridProperty_win("选择TI").grid_property;

            #endregion

            #region 预先提取所有样式

            var m = Mould.create_by_ellipse(4, 4, 1);
            var pats = Patterns.create(m, ti);//预先提取所有样式

            #endregion

            #region 锚点距离模型

            var g_anchors = Grid.create(gs);
            int flag = 0;

            Console.WriteLine();
            for (int iy = 10; iy < gs.ny; iy += 24)//锚点(Current)间距
            {
                for (int ix = 10; ix < gs.nx; ix += 24)
                {
                    flag++;
                    var temp_x = MyGenerator.range(10, gs.nx, 24);
                    MyConsoleProgress.Print(flag, temp_x.Count * temp_x.Count, "");

                    g_anchors.add_gridProperty(SpatialIndex.create(ix, iy).view_text());

                    var anchor_arrayIndex = gs.get_arrayIndex(SpatialIndex.create(ix, iy));//锚点位置
                    var anchor_neighbors = find_neighbors(gs, pats, anchor_arrayIndex, 3, 3);
                    Parallel.ForEach(pats, item =>
                    {
                        var other_neighbors = find_neighbors(gs, pats, item.Value.core_arrayIndex, 3, 3);
                        g_anchors.last_gridProperty().set_value(item.Value.core_arrayIndex, calc_distance(anchor_neighbors, other_neighbors));
                    });

                    //归一化到0-1之间
                    DataMapper dm = new();
                    dm.Reset(g_anchors.last_gridProperty().Min.Value, g_anchors.last_gridProperty().Max.Value, 0, 1);
                    for (int n = 0; n < gs.N; n++)
                    {
                        var value = g_anchors.last_gridProperty().get_value(n);
                        if (value != null)
                        {
                            g_anchors.last_gridProperty().set_value(n, (float)dm.MapAToB(value.Value));
                        }
                    }
                }
            }
            g_anchors.showGrid_win();

            #endregion

            #region 计算平稳系数

            double measure = 0;

            List<double> distance_average = new();
            for (int n1 = 0; n1 < gs.N; n1++)
            {
                for (int n2 = 0; n2 < gs.N; n2++)
                {
                    var world_distance = SpatialIndex.calc_dist(gs.get_spatialIndex(n1), gs.get_spatialIndex(n2));
                    distance_average.Add(world_distance);
                }
            }
            double distance_average1 = distance_average.Average();

            //step2_平稳性度量(anchor_grid);
            foreach (var n1 in pats.Keys)
            {
                List<(double, double)> ordered = new();
                foreach (var n2 in pats.Keys)
                {
                    if (n1 == n2)
                        continue;

                    var distance = MyDistance.calc_manhattan(pats[n1].neighbor_values, pats[n2].neighbor_values);
                    var world_distance = SpatialIndex.calc_dist(gs.get_spatialIndex(n1), gs.get_spatialIndex(n2));
                    ordered.Add((distance, world_distance));
                }
                measure += ordered.OrderBy(a => a.Item1).Take(100).ToList().Average(a => a.Item2);
            }
            measure /= (pats.Count);
            Console.WriteLine(measure / distance_average1);

            #endregion
        }

        static List<MouldInstance> find_neighbors(GridStructure gs, Patterns pats, int core_arrayIndex, int rx, int ry)
        {
            List<MouldInstance> neighbors = new();
            SpatialIndex core_si = gs.get_spatialIndex(core_arrayIndex);
            for (int dx = -rx; dx <= rx; dx++)
            {
                for (int dy = -ry; dy <= ry; dy++)
                {
                    var neighbor_arrayIndex = gs.get_arrayIndex(core_si.offset(dx, dy));
                    if (pats.ContainsKey(neighbor_arrayIndex))
                        neighbors.Add(pats[neighbor_arrayIndex]);
                }
            }
            return neighbors;
        }

        static float calc_distance(List<MouldInstance> pats1, List<MouldInstance> pats2)
        {
            List<float> distances = new();

            foreach (var pat1 in pats1)
            {
                List<float> d = new();
                foreach (var pat2 in pats2)
                {
                    if (pat1 != null && pat2 != null)
                        d.Add(MyDistance.calc_hsim(pat1.neighbor_values, pat2.neighbor_values));
                }
                if (d.Count != 0)
                    distances.Add(d.Max());
            }

            return distances.Average();
        }

        #endregion

        #region step1 实验变差函数

        static void step1_实验变差函数()
        {
            step1_实验变差函数2d();
        }

        public static void step1_实验变差函数2d()
        {
            #region 加载TI

            var (grid, fileName) = Grid.create_from_gslibwin();
            GridStructure gs = grid.gridStructure;
            var ti = grid.select_gridProperty_win("选择TI").grid_property;

            #endregion

            #region 设置参数

            int radius = 0;//Region的半径

            Console.WriteLine("设置Region的尺寸");
            Console.Write("\tradius = ");
            radius = int.Parse(Console.ReadLine());

            Grid anchor_grid = Grid.create(gs);
            anchor_grid.add_gridProperty("TI", ti);
            Console.Write("设置锚点在模型中的间隔(根据模型尺寸设置，如果模型的网格维度为100，可以设置为15或者20)\n\t输入 = ");
            int step = int.Parse(Console.ReadLine());

            #endregion

            #region 计算所有位置的实验变差函数

            //进度、运算时间
            Dictionary<int, List<double>> lags_different_locs = new();
            var locs_random = MyGenerator.range(1, gs.N, 1, true);
            locs_random = SortHelper.RandomSelect(locs_random, 500, new Random());
            int flag = 0;
            Dictionary<int, Bitmap> images = new();
            foreach (var n in locs_random)
            {
                flag++;
                MyConsoleProgress.Print(flag, locs_random.Count, "计算所有位置的实验变差函数");

                var (region, index_out_of_bounds) = ti.get_region_by_center(gs.get_spatialIndex(n), radius, radius);
                int N_lag = radius;
                if (index_out_of_bounds)//丢弃不完整的region
                    continue;

                List<double> lags_loc = new();
                var ev1_Anchor = Variogram.calc_experiment_variogram(region, 0, N_lag, 1).gamma;
                var ev2_Anchor = Variogram.calc_experiment_variogram(region, 45, N_lag, 1).gamma;
                var ev3_Anchor = Variogram.calc_experiment_variogram(region, 90, N_lag, 1).gamma;
                var ev4_Anchor = Variogram.calc_experiment_variogram(region, 135, N_lag, 1).gamma;
                lags_loc.AddRange(ev1_Anchor);
                lags_loc.AddRange(ev2_Anchor);
                lags_loc.AddRange(ev3_Anchor);
                lags_loc.AddRange(ev4_Anchor);

                lags_different_locs.Add(n, lags_loc);

                var image = region.draw_image_2d(Color.Gray, Algorithms.Images.ColorMapEnum.Jet);
                images.Add(n, image);
            }

            #endregion

            #region 计算锚点位置

            //锚点(Current)间距
            //for (int j = 10; j < gs.ny; j += step)
            //{
            //    for (int i = 10; i < gs.nx; i += step)
            //    {
            //        var anchor_loc = gs.get_arrayIndex(SpatialIndex.create(i, j));
            //        var anchor_lags = lags_different_locs[anchor_loc];
            //        anchor_grid.add_gridProperty($"anchor_loc[{i}-{j}]");//新建Property

            //        foreach (var (idx, lags) in lags_different_locs)
            //        {
            //            var neighbor_lags = lags_different_locs[idx];
            //            var hsim = MyDistance.calc_hsim(anchor_lags.ToArray(), neighbor_lags.ToArray());
            //            anchor_grid.last_gridProperty().set_value(idx, (float?)hsim);
            //        }
            //        MyConsoleProgress.Print(1, anchor_grid.propertyNames.Last());
            //    }
            //}
            //anchor_grid.showGrid_win();

            #endregion

            #region MDS降维

            var idx_labels = lags_different_locs.Keys.ToArray();
            var labels = idx_labels.Select(a => gs.get_spatialIndex(a).view_text());
            var data = lags_different_locs.Values.ToArray();
            var dismat = MyMatrix.create_dismat(data, MyDistanceType.manhattan);
            var locs = CMDSCALE.CMDSCALE_MathNet(dismat, 2);
            string[] series_names = new string[] { "dim1", "dim2" };
            var result = MyDataFrame.create_from_array(series_names, locs.buffer);
            result.add_series("ID");
            result.move_series("ID", 0);
            result.show_win("降维后");

            Form_QuickChart.ScatterPlot(
                result.get_series<double>("dim1"),
                result.get_series<double>("dim2"),
                labels);

            var images1 = images.Values.ToArray();
            Form_QuickChart.ImagePlot(images1, result.get_series<float>("dim1"),
                result.get_series<float>("dim2"));

            #endregion
        }

        static void step1_实验变差函数3d()
        {
            #region 加载TI

            var (grid, fileName) = Grid.create_from_gslibwin();
            GridStructure gs = grid.gridStructure;
            var ti = grid.select_gridProperty_win("选择TI").grid_property;

            #endregion

            #region 设置参数

            List<string> paramters = new();
            int radius = 0, vradius = 0;//Region的半径
            if (gs.dim == Dimension.D2)
            {
                Console.WriteLine("设置Region的尺寸");
                Console.Write("\tradius = ");
                radius = int.Parse(Console.ReadLine());
                paramters.Add(radius.ToString());
            }
            if (gs.dim == Dimension.D3)
            {
                Console.WriteLine("设置Region的尺寸");
                Console.Write("\t水平的radius\n\t输入 = ");
                radius = int.Parse(Console.ReadLine());
                paramters.Add(radius.ToString());
                Console.Write("\t垂向的radius\n\t输入 = ");
                vradius = int.Parse(Console.ReadLine());
                paramters.Add(vradius.ToString());
            }

            Grid Anchor_Grid = Grid.create(gs);
            Anchor_Grid.add_gridProperty("TI", ti);
            Console.Write("设置锚点在模型中的间隔(根据模型尺寸设置，如果模型的网格维度为100，可以设置为15或者20)\n\t输入 = ");
            int step = int.Parse(Console.ReadLine());
            paramters.Add(step.ToString());

            Console.Write("设置抽样比例(0~1的小数)\n\t输入 = ");
            double Ratio = double.Parse(Console.ReadLine());//计算比例
            paramters.Add(Ratio.ToString());

            #endregion

            #region 保存路径设置

            string paramters_info = string.Empty;
            foreach (var item in paramters)
            {
                paramters_info += item;
                paramters_info += " ";
            }
            paramters_info = paramters_info.Trim();

            string path = "";
            if (Directory.Exists(@"D:\"))
                path = $"D:\\temp_output\\NewMethods_非平稳分区\\";
            else
                path = $"C:\\temp_output\\NewMethods_非平稳分区\\";
            path += $"Test_变差函数_平稳性度量_第1步V1\\{FileHelper.GetFileName(fileName, false)}\\{paramters_info}";
            if (!DirHelper.IsExistDirectory(path))
                DirHelper.CreateDir(path);
            Console.WriteLine($"结果保存文件目录 : {path}");
            bool b1 = true;//是否覆盖之前的结果文件
            if (b1)
                DirHelper.ClearDirectory(path);

            #endregion

            #region 计算锚点位置

            //锚点(锁住该点位置，计算其他位置与该点的区块相似度)
            List<SpatialIndex> Anchors = new();
            if (gs.dim == Dimension.D2)
            {
                //锚点(Current)间距
                for (int j = 10; j < gs.ny; j += step)
                {
                    for (int i = 10; i < gs.nx; i += step)
                    {
                        var (Region_Anchor, _) = ti.get_region_by_center(SpatialIndex.create(i, j), radius, radius);
                        if (Region_Anchor.N_Nulls > 0)//不考虑以锚点为中心的范围出界的情况
                            continue;

                        string AnchorName = $"AnchorPoint[{i}-{j}]";
                        string fileName_Anchor = $"{path}\\{AnchorName}.out";
                        if (FileHelper.IsExistFile(fileName_Anchor))
                        {
                            Console.WriteLine($"{AnchorName}.out文件已存在");
                            continue;
                        }
                        //添加锚点
                        Anchors.Add(SpatialIndex.create(i, j));
                        //新建Property
                        Anchor_Grid.add_gridProperty(AnchorName);
                    }
                }
            }
            if (gs.dim == Dimension.D3)
            {
                //锚点(Current)间距
                for (int k = 2; k < gs.nz; k += 3)
                {
                    for (int j = 10; j < gs.ny; j += step)
                    {
                        for (int i = 10; i < gs.nx; i += step)
                        {
                            var (Region_Anchor, _) = ti.get_region_by_center(SpatialIndex.create(i, j, k), radius, radius, vradius);
                            if (Region_Anchor.N_Nulls > 0)//不考虑以锚点为中心的范围出界的情况
                                continue;

                            string AnchorName = $"AnchorPoint[{i}-{j}-{k}]";
                            string fileName_Anchor = $"{path}\\{AnchorName}.out";
                            if (FileHelper.IsExistFile(fileName_Anchor))
                            {
                                Console.WriteLine($"{AnchorName}.out文件已存在");
                                continue;
                            }
                            Anchors.Add(SpatialIndex.create(i, j, k));
                            //新建Property
                            Anchor_Grid.add_gridProperty(AnchorName);
                        }
                    }
                }
            }

            #endregion

            #region 进度、运算时间

            int counter = 0;//进度计数
            Stopwatch sw = new();
            sw.Start();

            #endregion

            #region 计算所有位置的实验变差函数



            #endregion

            //遍历所有锚点
            foreach (var Anchor in Anchors) //遍历所有锚点
            {
                #region 设置计算（抽稀）比例

                List<int> OtherArrayIndexs = new();
                for (int i = 0; i < gs.N; i++)
                    OtherArrayIndexs.Add(i);
                int N = (int)(gs.N * Ratio);//计算量
                var rnd = new Random(1);
                OtherArrayIndexs = SortHelper.RandomSelect(OtherArrayIndexs, N, rnd);

                #endregion

                #region 构造AnchorName

                string AnchorName = string.Empty;
                if (gs.dim == Dimension.D2)
                    AnchorName = $"AnchorPoint[{Anchor.ix}-{Anchor.iy}]";//新建Property
                if (gs.dim == Dimension.D3)
                    AnchorName = $"AnchorPoint[{Anchor.ix}-{Anchor.iy}-{Anchor.iz}]";//新建Property

                #endregion

                #region 计算Anchor的实验变差函数

                List<double[]> lags_Anchor = new();
                if (gs.dim == Dimension.D2)
                {
                    var (Region_Anchor, _) = ti.get_region_by_center(SpatialIndex.create(Anchor.ix, Anchor.iy), radius, radius);
                    int lagCount = radius;
                    if (Region_Anchor.N_Nulls > 0)
                    {
                        #region 更新运算进度

                        counter += N;

                        #endregion

                        continue;
                    }
                    var ev1_Anchor = Variogram.calc_experiment_variogram(Region_Anchor, lagCount, 1, 0).gamma;
                    var ev2_Anchor = Variogram.calc_experiment_variogram(Region_Anchor, lagCount, 1, 45).gamma;
                    var ev3_Anchor = Variogram.calc_experiment_variogram(Region_Anchor, lagCount, 1, 90).gamma;
                    var ev4_Anchor = Variogram.calc_experiment_variogram(Region_Anchor, lagCount, 1, 135).gamma;
                    lags_Anchor.Add(ev1_Anchor);
                    lags_Anchor.Add(ev2_Anchor);
                    lags_Anchor.Add(ev3_Anchor);
                    lags_Anchor.Add(ev4_Anchor);
                }
                if (gs.dim == Dimension.D3)
                {
                    var (Region_Anchor, _) = ti.get_region_by_center(SpatialIndex.create(Anchor.ix, Anchor.iy, Anchor.iz), radius, radius, vradius);
                    int lagCount = radius;
                    if (Region_Anchor.N_Nulls > 0)
                    {
                        #region 更新运算进度

                        counter += N;

                        #endregion

                        continue;
                    }
                    var ev1_Anchor = Variogram.calc_3d_horizontal_experiment_variogram(Region_Anchor, lagCount, 1, 0).gamma;
                    var ev2_Anchor = Variogram.calc_3d_horizontal_experiment_variogram(Region_Anchor, lagCount, 1, 45).gamma;
                    var ev3_Anchor = Variogram.calc_3d_horizontal_experiment_variogram(Region_Anchor, lagCount, 1, 90).gamma;
                    var ev4_Anchor = Variogram.calc_3d_horizontal_experiment_variogram(Region_Anchor, lagCount, 1, 135).gamma;
                    var ev5_Anchor = Variogram.calc_3d_vertical_experiment_variogram(Region_Anchor, vradius, 1).gamma;

                    lags_Anchor.Add(ev1_Anchor);
                    lags_Anchor.Add(ev2_Anchor);
                    lags_Anchor.Add(ev3_Anchor);
                    lags_Anchor.Add(ev4_Anchor);
                    lags_Anchor.Add(ev5_Anchor);
                }

                #endregion


                ParallelOptions parallelOptions = new()
                {
                    MaxDegreeOfParallelism = Environment.ProcessorCount - 1//设置并发任务的最大数目
                };
                Parallel.ForEach(OtherArrayIndexs, parallelOptions, item =>
                {
                    #region 更新运算进度

                    counter += 1;//显示进度
                    MyConsoleProgress.Print(counter, N * Anchors.Count, "锚点与其他位置的相似度计算");

                    #endregion

                    #region 计算Other的实验变差函数

                    var Other_SpatialIndex = gs.index_mapper[item];//index_mapper修改过（请注意）
                    if (gs.dim == Dimension.D2)
                    {
                        var (Region_Other, _) = ti.get_region_by_center(Other_SpatialIndex, radius, radius);
                        int lagCount = radius;
                        var ev1_Other = Variogram.calc_experiment_variogram(Region_Other, lagCount, 1, 0).gamma;
                        var ev2_Other = Variogram.calc_experiment_variogram(Region_Other, lagCount, 1, 45).gamma;
                        var ev3_Other = Variogram.calc_experiment_variogram(Region_Other, lagCount, 1, 90).gamma;
                        var ev4_Other = Variogram.calc_experiment_variogram(Region_Other, lagCount, 1, 135).gamma;

                        double hsim1 = MyDistance.calc_hsim(ev1_Other, lags_Anchor[0]);
                        double hsim2 = MyDistance.calc_hsim(ev2_Other, lags_Anchor[1]);
                        double hsim3 = MyDistance.calc_hsim(ev3_Other, lags_Anchor[2]);
                        double hsim4 = MyDistance.calc_hsim(ev4_Other, lags_Anchor[3]);
                        double hsim = (hsim1 + hsim2 + hsim3 + hsim4) / 4;

                        Anchor_Grid[AnchorName].set_value(Other_SpatialIndex, (float?)hsim);
                    }
                    if (gs.dim == Dimension.D3)
                    {
                        var (Region_Other, _) = ti.get_region_by_center(Other_SpatialIndex, radius, radius, vradius);
                        int lagCount = radius;

                        var ev1_Other = Variogram.calc_3d_horizontal_experiment_variogram(Region_Other, lagCount, 1, 0).gamma;
                        var ev2_Other = Variogram.calc_3d_horizontal_experiment_variogram(Region_Other, lagCount, 1, 45).gamma;
                        var ev3_Other = Variogram.calc_3d_horizontal_experiment_variogram(Region_Other, lagCount, 1, 90).gamma;
                        var ev4_Other = Variogram.calc_3d_horizontal_experiment_variogram(Region_Other, lagCount, 1, 135).gamma;
                        var ev5_Other = Variogram.calc_3d_vertical_experiment_variogram(Region_Other, vradius, 1).gamma;

                        double hsim1 = MyDistance.calc_hsim(ev1_Other, lags_Anchor[0]);
                        double hsim2 = MyDistance.calc_hsim(ev2_Other, lags_Anchor[1]);
                        double hsim3 = MyDistance.calc_hsim(ev3_Other, lags_Anchor[2]);
                        double hsim4 = MyDistance.calc_hsim(ev4_Other, lags_Anchor[3]);
                        double hsim5 = MyDistance.calc_hsim(ev5_Other, lags_Anchor[4]);
                        double hsim = (hsim1 + hsim2 + hsim3 + hsim4 + hsim5) / 5;

                        Anchor_Grid[AnchorName].set_value(Other_SpatialIndex, (float?)hsim);
                    }

                    #endregion
                });
            }
            Anchor_Grid.showGrid_win();
        }

        #endregion

        #region step2 平稳性度量

        static void step2_平稳性度量(Grid g)
        {
            try
            {
                var gs = g.gridStructure;
                int N = 6;//分段数量
                Console.WriteLine();
                Console.Write("选择从第几个模型开始计算[从0开始数]\n\t输入 = ");
                int start = int.Parse(Console.ReadLine());
                ConcurrentBag<double> temp = new();

                int counter = 0;//进度计数
                Parallel.For(start, g.Count, k =>
                {
                    var Property = g[k];
                    Grid fragments = Grid.create(gs);

                    string 分段策略 = "分段策略1";

                    #region 分段策略1:等比例分成N段

                    if (分段策略 == "分段策略1")
                    {
                        List<double> data = new();
                        for (int i = 0; i < gs.N; i++)
                        {
                            if (Property.get_value(i) != null)
                                data.Add(Property.get_value(i).Value);
                        }
                        //对数据进行排序
                        data = data.OrderBy(a => a).ToList();
                        //将数据从小到大，均匀地划分为N等份
                        for (int i = 0; i < N; i++)
                        {
                            int index_left = i * (data.Count / N);
                            int index_right = data.Count / N + index_left - 1;
                            float left = (float)data[index_left];
                            float right = (float)data[index_right];

                            var fragment = Property.greater_than(right, null).less_than(left, null);
                            if (fragment.N_Nulls < fragment.gridStructure.N * 0.99)
                                fragments.Add($"[{i}] : {left}-{right}", fragment);
                        }

                        #region 绘制图

                        Grid temp_fragments = Grid.create(gs);
                        for (int i = 0; i < N; i++)
                        {
                            var temp_property = fragments[i].not_equal_than(null, 1);
                            temp_property = temp_property.equal_than(null, 0);
                            temp_fragments.Add($"{i}", temp_property);
                        }
                        //temp_fragments.Show();

                        #endregion

                    }

                    #endregion

                    #region 分段策略2:数值等间距分成N段

                    if (分段策略 == "分段策略2")
                    {
                        decimal range = (decimal)Property.Max.Value - (decimal)Property.Min.Value;//计算取值范围
                        decimal interval = range / N;//等分间隔

                        //将数据从小到大，均匀地划分为N等份
                        for (int i = 0; i < N; i++)
                        {
                            decimal left = (decimal)Property.Min.Value + interval * i;
                            decimal right = left + interval;

                            var fragment = Property.
                                    greater_than((float?)right, null).
                                    less_than((float?)left, null);
                            if (fragment.N_Nulls < fragment.gridStructure.N * 0.99)
                                fragments.Add($"[{i}] : {left}-{right}", fragment);
                        }
                        fragments.showGrid_win();
                    }

                    #endregion


                    string MethodType = "算法2";

                    #region 算法1:计算统计所有非NUll值的点落在坐标系刻度里的数量

                    if (MethodType == "算法1")
                    {
                        double v = 0;
                        for (int m = 0; m < fragments.Count; m++)
                        {
                            var fragment = fragments[m];

                            double[] N_X = new double[gs.nx];
                            double[] N_Y = new double[gs.nx];
                            //计算统计所有非NUll值的点落在坐标系刻度里的数量
                            for (int n = 0; n < fragment.gridStructure.N; n++)
                            {
                                if (fragment.get_value(n) == null)
                                    continue;
                                var p = gs.index_mapper[n];//index_mapper修改过（请注意）
                                int x = p.ix;
                                int y = p.iy;
                                N_X[x] += 1;
                                N_Y[y] += 1;
                            }
                            var std_x = EasyMath.StdDev(N_X);
                            var std_y = EasyMath.StdDev(N_Y);
                            MyArrayHelper.Print(N_X);
                            MyArrayHelper.Print(N_Y);
                            var std = std_x + std_y;
                            v += std;
                        }
                        Console.WriteLine(v / fragments.Count);
                    }

                    #endregion

                    #region 算法2:基于点与其他点的距离评价(经过测试，效果较好)

                    if (MethodType == "算法2")
                    {
                        fragments.showGrid_win();
                        double v = 0;
                        for (int m = 0; m < fragments.Count; m++)
                        {
                            var fragment = fragments[m];
                            double dists = 0;
                            long dist_counter = 0;
                            //计算统计所有非空值的点之间距离
                            for (int i = 0; i < fragment.gridStructure.N; i++)
                            {
                                if (fragment.get_value(i) == null)
                                    continue;
                                var IJK1 = gs.index_mapper[i];//index_mapper修改过（请注意）
                                for (int j = 0; j < fragment.gridStructure.N; j++)
                                {
                                    if (i == j)
                                        continue;
                                    if (fragment.get_value(j) == null)
                                        continue;

                                    var IJK2 = gs.index_mapper[j];//index_mapper修改过（请注意）
                                    var distance = SpatialIndex.calc_dist(IJK1, IJK2);
                                    dists += distance;
                                    dist_counter++;
                                }
                            }
                            v += dists / dist_counter;
                        }
                        double 对角线 = gs.diagonal_distance();
                        //v /= Math.Sqrt(gs.Count);//开平方是否合理？在长条形工区中，可能会严重放大结果
                        v /= 对角线;//需要测试一下，两种不同方案的差别
                        temp.Add(v / fragments.Count);
                    }

                    #endregion

                });
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        #endregion

        #endregion

        #region 直接计算方法

        public static void 直接计算方法2d()
        {
            //设置参数
            Console.WriteLine("设置Region的尺寸");
            Console.Write("\tradius = ");
            int radius = int.Parse(Console.ReadLine());

            MyDataFrame df = MyDataFrame.create(["name", "value"]);

            var choice = MyConsoleHelper.read_int_from_console("选择TI集文件类型", "0-litedb;1-gslib(TI尺寸要求相同)");

            GridStructure gs = GridStructure.create_simple(100, 100, 1);
            Grid g = Grid.create(gs);
            if (choice == 0)
            {
                //加载TI
                Form_GridCatalog frm = new();
                if (frm.ShowDialog() != DialogResult.OK)
                    return;
                foreach (var selected_grid in frm.selected_grids)
                {
                    GridProperty ti = (GridProperty)selected_grid.first_gridProperty();
                    ti = ti.resize(gs);
                    g.add_gridProperty(selected_grid.grid_name, ti);
                }

            }
            if (choice == 1)
            {
                var temp_grid = Grid.create_from_gslibwin("打开训练图像").grid;
                foreach (var (name, gp) in temp_grid)
                {
                    g.add_gridProperty(name, gp.resize(gs));
                }
            }

            Console.WriteLine("*************分界线*************");

            foreach (var (name, ti) in g)
            {
                #region 计算所有位置的实验变差函数

                Dictionary<int, List<double>> lags_different_locs = [];
                var locs_random = MyGenerator.range(0, gs.N, 1);
                int flag = 0;
                Dictionary<int, Bitmap> images = [];
                foreach (var n in locs_random)
                {
                    flag++;
                    MyConsoleProgress.Print(flag, locs_random.Count, "计算所有位置的实验变差函数");

                    var (region, index_out_of_bounds) = ti.get_region_by_center(gs.get_spatialIndex(n), radius, radius);
                    int N_lag = radius;
                    if (index_out_of_bounds)//丢弃不完整的region
                        continue;

                    List<double> lags_loc =
                    [
                        .. Variogram.calc_experiment_variogram(region, 0, N_lag, 1).gamma,
                        .. Variogram.calc_experiment_variogram(region, 45, N_lag, 1).gamma,
                        .. Variogram.calc_experiment_variogram(region, 90, N_lag, 1).gamma,
                        .. Variogram.calc_experiment_variogram(region, 135, N_lag, 1).gamma,
                    ];
                    lags_different_locs.Add(n, lags_loc);
                }

                #endregion

                #region 计算平稳系数

                #region 并行计算

                ConcurrentBag<double> measures = [];
                Parallel.ForEach(lags_different_locs.Keys, n1 =>
                {
                    List<(double, double)> ordered = [];
                    foreach (var n2 in lags_different_locs.Keys)
                    {
                        if (n1 != n2)
                        {
                            var distance = MyDistance.calc_hsim(lags_different_locs[n1], lags_different_locs[n2]);
                            var world_distance = SpatialIndex.calc_dist(gs.get_spatialIndex(n1), gs.get_spatialIndex(n2));
                            ordered.Add((distance, world_distance));
                        }
                    }

                    //方案2
                    var tmp = ordered.OrderByDescending(a => a.Item1).ToList();
                    for (int i = 0; i < tmp.Count; i++)
                    {
                        measures.Add(tmp[i].Item2 * Math.Pow(1 - (float)i / tmp.Count, 2.0));
                    }
                });
                double measure = measures.Average();


                #endregion

                #region 串行计算

                //double measure = 0;
                //foreach (var n1 in lags_different_locs.Keys)
                //{
                //    List<(double, double)> ordered = new();
                //    foreach (var n2 in lags_different_locs.Keys)
                //    {
                //        if (n1 != n2)
                //        {
                //            var distance = MyDistance.calc_hsim(lags_different_locs[n1], lags_different_locs[n2]);
                //            var world_distance = SpatialIndex.calc_dist(gs.get_spatialIndex(n1), gs.get_spatialIndex(n2));
                //            ordered.Add((distance, world_distance));
                //        }
                //    }

                //    //方案1
                //    //int n = (int)(lags_different_locs.Count * 0.1);
                //    //measure += ordered.OrderByDescending(a => a.Item1).Take(n).Average(a => a.Item2);
                //    //方案2
                //    var tmp = ordered.OrderByDescending(a => a.Item1).ToList();
                //    for (int i = 0; i < tmp.Count; i++)
                //    {
                //        measure += tmp[i].Item2 * Math.Pow(1 - (float)i / tmp.Count, 2.0);
                //    }

                //}
                //measure /= (lags_different_locs.Count);

                #endregion

                Console.WriteLine($"{name}={measure}");
                var record = df.new_record();
                record["name"] = name;
                record["value"] = measure;
                df.add_record(record);

                #endregion
            }
            df.show_win();
        }

        #endregion

    }


}
