using System.Collections.Concurrent;
using System.Diagnostics;
using JAM8.Algorithms.Numerics;
using JAM8.Utilities;

namespace JAM8.Algorithms.Geometry
{
    /// <summary>
    /// 普通克里金
    /// </summary>
    public class OK
    {
        private OK() { }
        public GridStructure gs_re { get; internal set; }
        public Variogram vm { get; internal set; }
        public CData cd { get; internal set; }
        /// <summary>
        /// 条件数据的属性名称，例如"孔隙度"
        /// </summary>
        public string propertyName { get; internal set; }

        /// <summary>
        /// 创建OK对象
        /// </summary>
        /// <param name="vm"></param>
        /// <param name="cd"></param>
        /// <param name="gs"></param>
        /// <returns></returns>
        public static OK create(GridStructure gs_re, Variogram vm, CData cd, string propertyName)
        {
            OK ok = new()
            {
                gs_re = gs_re,
                vm = vm,
                cd = cd,
                propertyName = propertyName
            };
            return ok;
        }

        /// <summary>
        /// 主程序，返回克里金插值结果,包括cd_assign_to_grid、estimate和variance
        /// </summary>
        /// <param name="search_radius">数据点搜索半径</param>
        /// <param name="max_k_cdi">允许使用条件数据的最大数量，二维4~8之间 三维18~24之间</param>
        /// <returns>模型和估计方差</returns>
        public (Grid result, double elapsed_milliseconds) Run(int search_radius, double[] rot_mat, int max_k_cdi)
        {
            Stopwatch sw = new();
            sw.Start();

            Grid g = Grid.create(gs_re);//根据gridStructure创建新grid
            g.add_gridProperty("cd_assign_to_grid", cd.assign_to_grid(gs_re).grid_assigned[propertyName]);//cd赋值到Grid
            g.add_gridProperty("estimate", g["cd_assign_to_grid"].deep_clone());//估计结果
            g.add_gridProperty("variance");//估计方差

            RotMat rotMat = new(rot_mat[0], rot_mat[1], rot_mat[2], rot_mat[3], rot_mat[4], rot_mat[5]);

            #region 各向同性

            Dictionary<float, float> covariance_table = new();//两点协方差的查询表
            float range_power2 = vm.range * vm.range;
            for (int dz = 0; dz < g.gridStructure.nz; dz++)
            {
                for (int dy = 0; dy < g.gridStructure.ny; dy++)
                {
                    for (int dx = 0; dx < g.gridStructure.nx; dx++)
                    {
                        float anis_distance_power2 = AnisotropicDistance.calc_anis_distance_power2(rotMat, dx, dy, dz);
                        float cov = vm.calc_covariance((float)Math.Sqrt(anis_distance_power2));
                        covariance_table.TryAdd(anis_distance_power2, cov);
                    }
                }
            }
            #endregion

            bool is_parallel = true;

            if (is_parallel == false)
            {
                //var cd_finder4 = CDataNearestFinder_kdtree4.create(gs_re, cd);
                var cd_finder4 = CDataNearestFinder_kdtree4_anisotropy.create(gs_re, cd);

                #region 串行

                for (int n = 0; n < gs_re.N; n++)//计算工区网格的所有节点
                {
                    SpatialIndex si = gs_re.get_spatial_index(n);
                    if (g["estimate"].get_value(n) == null)//如果该点没有数据，则需要插值
                    {
                        var cd_founds4 = cd_finder4.find(si, max_k_cdi);
                        var cd_founds = cd_founds4;

                        if (cd_founds.Count == 0)
                            continue;
                        MyConsoleProgress.Print(n, gs_re.N, "OK", cd_founds.Count.ToString());

                        int k = cd_founds.Count;

                        MyMatrix matrixA = null;//协方差矩阵

                        #region 计算协方差矩阵

                        float[,] _协方差矩阵 = new float[k + 1, k + 1];
                        for (int j = 0; j < k + 1; j++)
                            for (int i = 0; i < k + 1; i++)
                                _协方差矩阵[i, j] = 1;
                        _协方差矩阵[k, k] = 0;

                        for (int j = 0; j < k; j++)
                            for (int i = 0; i < k; i++)
                            {
                                var anis_dist_power2 = AnisotropicDistance.calc_anis_distance_power2(rotMat, cd_founds[i].si, cd_founds[j].si);
                                _协方差矩阵[i, j] = covariance_table[anis_dist_power2];
                            }
                        matrixA = MyMatrix.create(_协方差矩阵);//初始化协方差矩阵（左1矩阵）

                        #endregion

                        MyVector vectorB = null;//协方差向量

                        #region 计算协方差向量

                        float[] _协方差向量 = new float[k + 1];

                        for (int i = 0; i < k; i++)
                        {
                            var anis_dist_power2 = AnisotropicDistance.calc_anis_distance_power2(rotMat, si, cd_founds[i].si);
                            _协方差向量[i] = covariance_table[anis_dist_power2];
                        }
                        _协方差向量[k] = 1;

                        //初始化协方差向量（右1向量）
                        vectorB = MyVector.create(_协方差向量);

                        #endregion

                        MyVector vectorX = MyMatrix.solve_accord(matrixA, vectorB);//求解权重向量
                        var weights = vectorX.buffer.Take(k).ToArray();//删除拉格朗日常数

                        #region 负数权重的校正——去掉负值

                        //float averageNegativeWeights = 0;
                        //float averageNegativeCorrelation = 0;
                        ////权重为负的点数量
                        //int m = 0;
                        //for (int i = 0; i < weights.Length; i++)
                        //{
                        //    if (weights[i] < 0)
                        //    {
                        //        m += 1;
                        //        averageNegativeWeights += weights[i];
                        //        averageNegativeCorrelation += vectorB[i];
                        //    }
                        //}
                        //averageNegativeWeights /= m;//计算均值
                        //averageNegativeCorrelation /= m;//计算均值

                        //for (int i = 0; i < weights.Length; i++)//更新权重值
                        //{
                        //    if (weights[i] < 0)
                        //        weights[i] = 0;
                        //    else
                        //    {
                        //        if (weights[i] < averageNegativeWeights && vectorB[i] < averageNegativeCorrelation)
                        //            weights[i] = 0;
                        //    }
                        //}

                        //for (int i = 0; i < weights.Length; i++)//对weights进行归一化
                        //    weights[i] /= weights.Sum();

                        #endregion

                        float estimate = 0;//计算待估值
                        for (int i = 0; i < k; i++)
                            estimate += cd_founds[i].cdi[propertyName].Value * weights[i];
                        g["estimate"].set_value(n, estimate);

                        float var = _协方差矩阵[0, 0] + vectorX[k];//计算估计方差
                        for (int i = 0; i < k; i++)
                            var -= vectorX[i] * vectorB[i];
                        g["variance"].set_value(n, var);
                    }
                    else
                    {
                        g["variance"].set_value(n, 0);
                    }
                }

                #endregion
            }
            else
            {
                //var cd_finder4 = CDataNearestFinder_kdtree4.create(gs_re, cd);
                var cd_finder4 = CDataNearestFinder_kdtree4_anisotropy.create(gs_re, cd);

                #region 并行

                ParallelOptions options = new();
                int n_processor = Environment.ProcessorCount;
                options.MaxDegreeOfParallelism = n_processor;//设置并发数
                ConcurrentBag<int> flag = [];//计数器
                Parallel.For(0, gs_re.N, options, n =>//计算工区网格的所有节点
                {
                    flag.Add(n);
                    MyConsoleProgress.Print(flag.Count, gs_re.N, "OK[并行]");

                    SpatialIndex si = gs_re.get_spatial_index(n);
                    if (g["estimate"].get_value(n) == null)//如果某个点没有数据，则需要插值
                    {
                        var cd_founds = cd_finder4.find(si, max_k_cdi);
                        if (cd_founds.Count == 0)
                            return;

                        int k = cd_founds.Count;

                        MyMatrix matrixA = null;//协方差矩阵

                        #region 计算协方差矩阵

                        float[,] _协方差矩阵 = new float[k + 1, k + 1];
                        for (int j = 0; j < k + 1; j++)
                            for (int i = 0; i < k + 1; i++)
                                _协方差矩阵[i, j] = 1;
                        _协方差矩阵[k, k] = 0;

                        for (int j = 0; j < k; j++)
                        {
                            for (int i = 0; i < k; i++)
                            {
                                #region 各向同性

                                //int dx = cd_founds[i].si.ix - cd_founds[j].si.ix;
                                //int dy = cd_founds[i].si.iy - cd_founds[j].si.iy;
                                //int dz = cd_founds[i].si.iz - cd_founds[j].si.iz;
                                //float h_power2 = dx * dx + dy * dy + dz * dz;
                                //_协方差矩阵[i, j] = covariance_table[h_power2];

                                #endregion

                                #region 各向异性

                                var anis_dist_power2 = AnisotropicDistance.calc_anis_distance_power2(rotMat, cd_founds[i].si, cd_founds[j].si);
                                _协方差矩阵[i, j] = covariance_table[anis_dist_power2];

                                #endregion
                            }
                        }
                        matrixA = MyMatrix.create(_协方差矩阵);//初始化协方差矩阵（左1矩阵）

                        #endregion

                        MyVector vectorB = null;//协方差向量

                        #region 计算协方差向量

                        float[] _协方差向量 = new float[k + 1];

                        for (int i = 0; i < k; i++)
                        {
                            #region 各向同性

                            //_协方差向量[i] = vm.calc_covariance(cd_founds[i].distance);

                            #endregion

                            #region 各向异性

                            var anis_dist_power2 = AnisotropicDistance.calc_anis_distance_power2(rotMat, si, cd_founds[i].si);
                            _协方差向量[i] = covariance_table[anis_dist_power2];

                            #endregion
                        }
                        _协方差向量[k] = 1;

                        //初始化协方差向量（右1向量）
                        vectorB = MyVector.create(_协方差向量);

                        #endregion

                        MyVector vectorX = MyMatrix.solve_mathnet(matrixA, vectorB);//求解权重向量
                        var weights = vectorX.buffer.Take(k).ToArray();//删除拉格朗日常数

                        #region 负数权重的校正——去掉负值

                        //float averageNegativeWeights = 0;
                        //float averageNegativeCorrelation = 0;
                        ////权重为负的点数量
                        //int m = 0;
                        //for (int i = 0; i < weights.Length; i++)
                        //{
                        //    if (weights[i] < 0)
                        //    {
                        //        m += 1;
                        //        averageNegativeWeights += weights[i];
                        //        averageNegativeCorrelation += vectorB[i];
                        //    }
                        //}
                        //averageNegativeWeights /= m;//计算均值
                        //averageNegativeCorrelation /= m;//计算均值

                        //for (int i = 0; i < weights.Length; i++)//更新权重值
                        //{
                        //    if (weights[i] < 0)
                        //        weights[i] = 0;
                        //    else
                        //    {
                        //        if (weights[i] < averageNegativeWeights && vectorB[i] < averageNegativeCorrelation)
                        //            weights[i] = 0;
                        //    }
                        //}

                        //for (int i = 0; i < weights.Length; i++)//对weights进行归一化
                        //    weights[i] /= weights.Sum();

                        #endregion

                        float estimate = 0;//计算待估值
                        for (int i = 0; i < k; i++)
                            estimate += cd_founds[i].cdi[propertyName].Value * weights[i];
                        g["estimate"].set_value(n, estimate);

                        float var = _协方差矩阵[0, 0] + vectorX[k];//计算估计方差
                        for (int i = 0; i < k; i++)
                            var -= vectorX[i] * vectorB[i];
                        g["variance"].set_value(n, var);
                    }
                    else
                    {
                        g["variance"].set_value(n, 0);
                    }
                });

                #endregion
            }

            sw.Stop();

            return (g, sw.ElapsedMilliseconds);
        }

        /// <summary>
        /// 计算普通kriging的权重
        /// </summary>
        /// <param name="predict_location"></param>
        /// <param name="cd_locations"></param>
        /// <param name="vm"></param>
        /// <returns></returns>
        public static float[] calc_weights_ok(SpatialIndex predict_location, SpatialIndex[] cd_locations, Variogram vm)
        {
            int n = cd_locations.Length;

            MyMatrix matrixA = null;//协方差矩阵

            #region 计算协方差矩阵

            float[,] _协方差矩阵 = new float[n + 1, n + 1];

            for (int j = 0; j < n + 1; j++)
                for (int i = 0; i < n + 1; i++)
                    _协方差矩阵[i, j] = 1;

            _协方差矩阵[n, n] = 0;

            for (int j = 0; j < n; j++)
                for (int i = 0; i < n; i++)
                    _协方差矩阵[i, j] = vm.calc_covariance(SpatialIndex.calc_dist(cd_locations[i], cd_locations[j]));

            matrixA = MyMatrix.create(_协方差矩阵);//初始化协方差矩阵（左1矩阵）

            #endregion

            MyVector vectorB = null;//协方差向量

            #region 计算协方差向量

            float[] _协方差向量 = new float[n + 1];

            for (int i = 0; i < n; i++)
                _协方差向量[i] = vm.calc_covariance(SpatialIndex.calc_dist(cd_locations[i], predict_location));

            _协方差向量[n] = 1;

            vectorB = MyVector.create(_协方差向量);//初始化协方差向量（右1向量）

            #endregion

            MyVector vectorX = MyMatrix.solve_mathnet(matrixA, vectorB);//求解权重向量
            var weights = vectorX.buffer.Take(n).ToArray();//删除拉格朗日常数
            return weights;
        }

        /// <summary>
        /// 计算普通kriging的权重
        /// </summary>
        /// <param name="predict_location"></param>
        /// <param name="cd_locations"></param>
        /// <param name="vm"></param>
        /// <returns></returns>
        public static float[] calc_weights_ok(Coord predict_location, Coord[] cd_locations, Variogram vm)
        {
            int n = cd_locations.Length;

            MyMatrix matrixA = null;//协方差矩阵

            #region 计算协方差矩阵

            float[,] _协方差矩阵 = new float[n + 1, n + 1];

            for (int j = 0; j < n + 1; j++)
                for (int i = 0; i < n + 1; i++)
                    _协方差矩阵[i, j] = 1;

            _协方差矩阵[n, n] = 0;

            for (int j = 0; j < n; j++)
                for (int i = 0; i < n; i++)
                    _协方差矩阵[i, j] = vm.calc_variogram((float)Coord.get_distance(cd_locations[i], cd_locations[j]));

            matrixA = MyMatrix.create(_协方差矩阵);//初始化协方差矩阵（左1矩阵）

            #endregion

            MyVector vectorB = null;//协方差向量

            #region 计算协方差向量

            float[] _协方差向量 = new float[n + 1];

            for (int i = 0; i < n; i++)
                _协方差向量[i] = vm.calc_variogram((float)Coord.get_distance(cd_locations[i], predict_location));

            _协方差向量[n] = 1;

            vectorB = MyVector.create(_协方差向量);//初始化协方差向量（右1向量）

            #endregion

            MyVector vectorX = MyMatrix.solve_mathnet(matrixA, vectorB);//求解权重向量
            var weights = vectorX.buffer.Take(n).ToArray();//删除拉格朗日常数
            return weights;
        }
    }
}
