﻿using JAM8.Algorithms.Images;
using JAM8.Algorithms.Numerics;

namespace JAM8.Algorithms.Geometry
{
    /// <summary>
    /// 网格属性
    /// </summary>
    public class GridProperty
    {
        private GridProperty() { }

        #region 属性

        /// <summary>
        /// 数据缓存
        /// </summary>
        public float?[] buffer { get; internal set; }
        /// <summary>
        /// 网格单元等于Null的数量
        /// </summary>
        public int N_Nulls { get; internal set; } = 0;
        /// <summary>
        /// 网格结构
        /// </summary>
        public GridStructure gridStructure { get; internal set; }

        public float? Min
        {
            get
            {
                return buffer.Min();
            }
        }

        public float? Max
        {
            get
            {
                return buffer.Max();
            }
        }

        public float? Average
        {
            get
            {
                return buffer.Average();
            }
        }

        /// <summary>
        /// 统计非Null数据，返回值(每个bin的所占比例,每个bin的位置)，默认bins总数等于20
        /// </summary>
        public (List<double>, List<double>) Histogram
        {
            get
            {
                // 获取自动生成的 ticks，用于确定 bin 的位置
                var ticks = Numerics.AutoTicks.GetTicks(Min.Value, Max.Value, 20);

                // 使用 List 初始化 positions 和 ratios，确保 positions 的大小和 ticks.TickCount 一致
                var positions = new List<double>(ticks.TickCount + 1);
                var ratios = new List<int>(ticks.TickCount + 1);

                // 初始化 positions 和 ratios
                for (int i = 0; i <= ticks.TickCount; i++)
                {
                    positions.Add(ticks.Lower + ticks.Step * i);  // 填充 bin 位置
                    ratios.Add(0);  // 初始化所有 bin 的频率为 0
                }

                // 统计非 null 数据的数量
                int N = buffer.Count(b => b != null);

                // 对 buffer 中的每个数据进行计算，确定其所在的 bin，并增加对应 bin 的计数
                foreach (var item in buffer.Where(b => b != null))
                {
                    // 计算数据应该落入哪个 bin，并更新对应 bin 的频率
                    int idx = (int)((item.Value - ticks.Lower) / ticks.Step + 0.5);  // 四舍五入到最近的整数
                    ratios[idx]++;
                }

                // 计算每个 bin 的比例
                var totalCount = (double)N;
                var frequencyRatios = ratios.Select(r => r / totalCount).ToList();  // 计算频率比例

                return (frequencyRatios, positions);
            }
        }

        /// <summary>
        /// 获取离散变量的值及其对应频率
        /// </summary>
        /// <param name="buffer">包含离散变量的集合</param>
        /// <param name="nullReserve">是否保留空值的统计</param>
        /// <returns>值和频率的列表</returns>
        public List<(float? value, float freq)> discrete_category_freq(bool nullReserve = true)
        {
            // 检查输入是否为空
            if (buffer == null || buffer.Length == 0)
                return [];

            // 按值分组统计频数，并将 null 转为 "null" 字符串作为键
            var frequencyDict = buffer
                .GroupBy(x => x?.ToString() ?? "null")
                .ToDictionary(
                    g => g.Key,
                    g => (float)g.Count()
                );

            // 如果不保留空值，移除 "null" 键
            if (!nullReserve)
                frequencyDict.Remove("null");

            // 计算总频数，用于归一化
            float total = frequencyDict.Values.Sum();

            // 构造结果，解析键为浮点数或 null，并计算归一化频率
            return frequencyDict
                .Select(kv => (
                    kv.Key == "null" ? (float?)null : float.Parse(kv.Key),
                    kv.Value / total
                ))
                .ToList();
        }


        /// <summary>
        /// 获取离散变量的值及其对应的频数
        /// </summary>
        /// <param name="null_reserve">是否保留空值的统计</param>
        /// <returns>值和频数的列表</returns>
        public List<(float? value, int count)> discrete_category_count(bool null_reserve = true)
        {
            // 检查 buffer 是否为空
            if (buffer == null || buffer.Length == 0)
                return [];

            // 按值统计频数，处理 null 值为 "null"
            var frequencyDict = buffer
                .GroupBy(x => x?.ToString() ?? "null")
                .ToDictionary(
                    g => g.Key,
                    g => g.Count()
                );

            // 如果不保留空值，移除 "null" 键
            if (!null_reserve)
                frequencyDict.Remove("null");

            // 构造结果列表，将键解析为 float? 类型
            return frequencyDict
                .Select(kv => (
                    kv.Key == "null" ? (float?)null : float.Parse(kv.Key),
                    kv.Value
                ))
                .ToList();
        }

        #endregion

        #region create

        /// <summary>
        /// 创建GridProperty
        /// </summary>
        /// <param name="gs"></param>
        /// <returns></returns>
        public static GridProperty create(GridStructure gs)
        {
            GridProperty gp = new()//开辟缓存空间
            {
                gridStructure = gs,
                buffer = new float?[gs.N],
                N_Nulls = gs.N
            };
            return gp;
        }

        /// <summary>
        /// 根据指定的多个条件判断网格节点值，返回新的修改后的 GridProperty。
        /// 条件按顺序执行，只有前一个条件满足，后续条件才会继续判断。
        /// 每个条件使用其自己的 NewValue 更新值，后续条件基于之前的修改值计算。
        /// </summary>
        /// <param name="gp">原始 GridProperty 对象</param>
        /// <param name="conditions">条件列表，每个条件包含比较值、目标值和比较类型</param>
        /// <returns>修改后的新的 GridProperty 对象</returns>
        public static GridProperty create(GridProperty gp, params (float? ComparedValue, float? NewValue, CompareType CompareType)[] conditions)
        {
            GridProperty clone = gp.deep_clone();

            for (int n = 0; n < clone.gridStructure.N; n++)
            {
                float? currentValue = clone.get_value(n);

                foreach (var (comparedValue, newValue, compareType) in conditions)
                {
                    // 判断当前条件是否满足
                    bool shouldReplace = compareType switch
                    {
                        CompareType.NoCompared => true,
                        CompareType.Equals => currentValue == comparedValue,
                        CompareType.NotEqual => currentValue != comparedValue,
                        CompareType.GreaterThan => currentValue > comparedValue,
                        CompareType.GreaterEqualsThan => currentValue >= comparedValue,
                        CompareType.LessThan => currentValue < comparedValue,
                        CompareType.LessEqualsThan => currentValue <= comparedValue,
                        _ => false
                    };

                    // 如果当前条件满足，更新值，并使用该条件的 NewValue
                    if (shouldReplace)
                    {
                        currentValue = newValue; // 更新当前值为当前条件的 NewValue
                        clone.set_value(n, currentValue); // 更新节点值
                    }
                }
            }

            return clone;
        }

        /// <summary>
        /// 根据指定的多个区间条件判断网格节点值，返回新的修改后的 GridProperty。
        /// 每个区间条件使用其自己的 NewValue 更新值，后续条件基于之前的修改值计算。
        /// </summary>
        /// <param name="gp">原始 GridProperty 对象</param>
        /// <param name="conditions">区间条件列表，每个条件包含区间的起始值、结束值和目标值</param>
        /// <returns>修改后的新的 GridProperty 对象</returns>
        public static GridProperty create(GridProperty gp, params (float? MinValue, float? MaxValue, float? NewValue)[] conditions)
        {
            GridProperty clone = gp.deep_clone();

            for (int n = 0; n < clone.gridStructure.N; n++)
            {
                float? currentValue = clone.get_value(n);

                foreach (var (minValue, maxValue, newValue) in conditions)
                {
                    // 判断当前值是否在区间内
                    if (currentValue >= minValue && currentValue <= maxValue)
                    {
                        clone.set_value(n, newValue); // 更新值
                        break; // 找到匹配的区间后，跳出当前条件判断
                    }
                }
            }

            return clone;
        }

        #endregion

        #region + - * /

        public static GridProperty operator +(GridProperty left, GridProperty right)
        {
            if (!left.gridStructure.Equals(right.gridStructure))
                throw new Exception("gridStructure不一致");
            GridProperty result = create(left.gridStructure);
            for (int n = 0; n < left.gridStructure.N; n++)
            {
                float? value = left.get_value(n) + right.get_value(n);
                result.set_value(n, value);
            }
            return result;
        }
        public static GridProperty operator -(GridProperty left, GridProperty right)
        {
            if (!left.gridStructure.Equals(right.gridStructure))
                throw new Exception("gridStructure不一致");
            GridProperty result = create(left.gridStructure);
            for (int n = 0; n < left.gridStructure.N; n++)
            {
                float? value = left.get_value(n) - right.get_value(n);
                result.set_value(n, value);
            }
            return result;
        }
        public static GridProperty operator *(GridProperty left, GridProperty right)
        {
            if (!left.gridStructure.Equals(right.gridStructure))
                throw new Exception("gridStructure不一致");
            GridProperty result = create(left.gridStructure);
            for (int n = 0; n < left.gridStructure.N; n++)
            {
                float? value = left.get_value(n) * right.get_value(n);
                result.set_value(n, value);
            }
            return result;
        }
        public static GridProperty operator /(GridProperty left, GridProperty right)
        {
            if (!left.gridStructure.Equals(right.gridStructure))
                throw new Exception("gridStructure不一致");
            GridProperty result = create(left.gridStructure);
            for (int n = 0; n < left.gridStructure.N; n++)
            {
                float? value = left.get_value(n) / right.get_value(n);
                result.set_value(n, value);
            }
            return result;
        }

        #endregion

        #region 取值与赋值

        #region get_value

        /// <summary>
        /// 根据array索引获取value
        /// </summary>
        /// <param name="arrayIndex">arrayIndex范围从0到gs.N-1</param>
        /// <returns></returns>
        public float? get_value(int arrayIndex)
        {
            // 检查 arrayIndex 是否在合法范围内
            if (arrayIndex < 0 || arrayIndex >= gridStructure.N)
            {
                return null; // 如果超出范围，直接返回 null
            }

            // 根据索引返回对应的 Buffer 值
            return buffer[arrayIndex];
        }

        /// <summary>
        /// 根据spatial索引获取value，spatial index的ix、iy、iz从0开始，到N-1结束
        /// </summary>
        /// <param name="si"></param>
        /// <returns></returns>
        public float? get_value(SpatialIndex si)
        {
            return get_value(gridStructure.get_arrayIndex(si));
        }

        /// <summary>
        /// 根据二维spatial索引的ix、iy获取value，ix、iy从0开始，到N-1结束
        /// </summary>
        /// <param name="ix"></param>
        /// <param name="iy"></param>
        /// <returns></returns>
        public float? get_value(int ix, int iy)
        {
            return get_value(gridStructure.get_arrayIndex(ix, iy, 0));
        }

        /// <summary>
        /// 根据三维spatial索引的ix、iy、iz获取value，ix、iy、iz从0开始，到N-1结束
        /// </summary>
        /// <param name="ix"></param>
        /// <param name="iy"></param>
        /// <param name="iz"></param>
        /// <returns></returns>
        public float? get_value(int ix, int iy, int iz)
        {
            return get_value(gridStructure.get_arrayIndex(ix, iy, iz));
        }

        /// <summary>
        /// 获取满足判断条件的values
        /// </summary>
        /// <param name="compare_type">比较类型(等于、不等于、大于、大于等于、小于、小于等于)</param>
        /// <param name="compare_value">参与比较的数值</param>
        /// <returns></returns>
        public (List<int>, List<float?>) get_values_by_condition(float? compare_value, CompareType compare_type)
        {
            List<int> idx = [];
            List<float?> values = [];

            // 遍历网格节点并进行条件判断
            for (int n = 0; n < gridStructure.N; n++)
            {
                float? currentValue = get_value(n);

                // 使用通用方法来进行比较
                bool shouldAdd = compare_type switch
                {
                    CompareType.NoCompared => true,
                    CompareType.Equals => currentValue == compare_value,
                    CompareType.NotEqual => currentValue != compare_value,
                    CompareType.GreaterThan => currentValue > compare_value,
                    CompareType.GreaterEqualsThan => currentValue >= compare_value,
                    CompareType.LessThan => currentValue < compare_value,
                    CompareType.LessEqualsThan => currentValue <= compare_value,
                    _ => false
                };

                // 如果满足条件，添加索引和值
                if (shouldAdd)
                {
                    idx.Add(n);
                    values.Add(currentValue);
                }
            }

            return (idx, values);
        }

        /// <summary>
        /// 获取满足区间范围条件的values
        /// </summary>
        /// <param name="minValue">区间下限（包含）</param>
        /// <param name="maxValue">区间上限（包含）</param>
        /// <returns>符合条件的索引和对应值</returns>
        public (List<int>, List<float?>) get_values_by_range(float? minValue, float? maxValue)
        {
            List<int> idx = [];
            List<float?> values = [];

            // 遍历网格节点并进行区间判断
            for (int n = 0; n < gridStructure.N; n++)
            {
                float? currentValue = get_value(n);

                // 判断值是否在区间范围内
                if (currentValue >= minValue && currentValue <= maxValue)
                {
                    idx.Add(n);
                    values.Add(currentValue);
                }
            }

            return (idx, values);
        }


        #endregion

        #region set_value

        /// <summary>
        /// 根据array索引对网格单元赋值value
        /// </summary>
        /// <param name="arrayIndex">array index从0开始，到gs.N-1结束</param>
        /// <param name="value"></param>
        public void set_value(int arrayIndex, float? value)
        {
            if (arrayIndex >= 0 && arrayIndex < gridStructure.N)
            {
                float? old = get_value(arrayIndex);
                if (old == null && value != null)
                    N_Nulls -= 1;
                if (old != null && value == null)
                    N_Nulls += 1;
                buffer[arrayIndex] = value;
            }
        }

        /// <summary>
        /// 根据spatial索引对网格单元赋值value，spatial_index的ix、iy、iz从1开始，到gs.N结束
        /// </summary>
        /// <param name="si"></param>
        /// <param name="value"></param>
        public void set_value(SpatialIndex si, float? value)
        {
            set_value(gridStructure.get_arrayIndex(si), value);
        }

        /// <summary>
        /// 根据ix、iy对网格单元赋值value，ix、iy从0开始，到N-1结束
        /// </summary>
        /// <param name="ix"></param>
        /// <param name="iy"></param>
        /// <param name="value"></param>
        public void set_value(int ix, int iy, float? value)
        {
            set_value(gridStructure.get_arrayIndex(ix, iy, 0), value);
        }

        /// <summary>
        /// 根据ix、iy、iz对网格单元赋值value，ix、iy、iz从0开始，到N-1结束
        /// </summary>
        /// <param name="ix"></param>
        /// <param name="iy"></param>
        /// <param name="iz"></param>
        /// <param name="value"></param>
        public void set_value(int ix, int iy, int iz, float? value)
        {
            set_value(gridStructure.get_arrayIndex(ix, iy, iz), value);
        }

        /// <summary>
        /// 对所有网格单元均赋值为value
        /// </summary>
        /// <param name="value"></param>
        public void set_value(float? value)
        {
            for (int n = 0; n < gridStructure.N; n++)
            {
                set_value(n, value);
            }
        }

        /// <summary>
        /// 所有节点赋值，每个节点的值位于Min与Max之间的均匀随机数
        /// </summary>
        /// <param name="Min"></param>
        /// <param name="Max"></param>
        /// <param name="rnd"></param>
        public void set_values_uniform(float min, float max, Random rnd)
        {
            DataMapper mapper = new();
            mapper.Reset(0, 1, min, max);
            for (int i = 0; i < gridStructure.N; i++)
            {
                float value = (float)rnd.NextDouble();
                value = (float)mapper.MapAToB(value);
                set_value(i, value);
            }

        }

        /// <summary>
        /// 给网格赋值，服从高斯分布
        /// </summary>
        /// <param name="rnd"></param>
        /// <param name="mean">均值</param>
        /// <param name="dev">方差</param>
        public void set_values_gaussian(double mean, double dev, Random rnd)
        {
            Gaussian norm = new(mean, dev);
            for (int i = 0; i < gridStructure.N; i++)
            {
                set_value(i, (float)norm.Sample(rnd));
            }
        }

        /// <summary>
        /// 根据指定条件在原先网格属性上修改网格节点值
        /// </summary>
        /// <param name="compared_value">用于比较的值</param>
        /// <param name="new_value">需要设置的新值</param>
        /// <param name="compare_type">比较条件</param>
        /// <returns>发生修改的网格节点的array_index集合</returns>
        public List<int> set_values_by_condition(float? compared_value, float? new_value, CompareType compare_type)
        {
            List<int> idx = [];
            for (int n = 0; n < gridStructure.N; n++)
            {
                float? currentValue = get_value(n);

                // 在循环中直接实现比较逻辑
                bool shouldReplace = compare_type switch
                {
                    CompareType.NoCompared => true,
                    CompareType.Equals => currentValue == compared_value,
                    CompareType.NotEqual => currentValue != compared_value,
                    CompareType.GreaterThan => currentValue > compared_value,
                    CompareType.GreaterEqualsThan => currentValue >= compared_value,
                    CompareType.LessThan => currentValue < compared_value,
                    CompareType.LessEqualsThan => currentValue <= compared_value,
                    _ => false
                };

                if (shouldReplace)
                {
                    idx.Add(n);
                    set_value(n, new_value);
                }
            }
            return idx;
        }

        /// <summary>
        /// 根据指定区间范围修改网格节点值
        /// </summary>
        /// <param name="minValue">区间下限（包含）</param>
        /// <param name="maxValue">区间上限（包含）</param>
        /// <param name="newValue">需要设置的新值</param>
        /// <returns>发生修改的网格节点的 array_index 集合</returns>
        public List<int> set_values_by_range(float? minValue, float? maxValue, float? newValue)
        {
            List<int> idx = [];

            // 遍历所有网格节点并判断是否在区间范围内
            for (int n = 0; n < gridStructure.N; n++)
            {
                float? currentValue = get_value(n);

                // 判断值是否在区间范围内
                if (currentValue >= minValue && currentValue <= maxValue)
                {
                    idx.Add(n);
                    set_value(n, newValue); // 更新值
                }
            }

            return idx;
        }

        #endregion

        #endregion

        #region 局部提取

        /// <summary>
        /// 根据索引的界限提取区域部分网格，[ix_min,ix_max]和[iy_min,iy_max]是闭区间
        /// </summary>
        /// <param name="ix_min">从1开始</param>
        /// <param name="ix_max">gs.nx结束</param>
        /// <param name="iy_min">从1开始</param>
        /// <param name="iy_max">gs.ny结束</param>
        /// <returns></returns>
        public (GridProperty region, bool index_out_of_bounds) get_region_by_range(int ix_min, int ix_max, int iy_min, int iy_max)
        {
            bool index_out_of_bounds = false;//是否越界，默认是假
            if (Dimension.D3 == gridStructure.dim)
                throw new Exception(MyExceptions.Geometry_DimensionException);

            int extent_x = ix_max - ix_min;
            int extent_y = iy_max - iy_min;

            ix_min = Math.Max(ix_min, 0);
            ix_max = Math.Min(ix_max, gridStructure.nx - 1);
            iy_min = Math.Max(iy_min, 0);
            iy_max = Math.Min(iy_max, gridStructure.ny - 1);

            if (extent_x != ix_max - ix_min || extent_y != iy_max - iy_min)
                index_out_of_bounds = true;

            //创建一个新网格对象
            var gs = GridStructure.create_simple(ix_max - ix_min + 1, iy_max - iy_min + 1, 1);
            var region = create(gs);

            for (int iy = iy_min; iy <= iy_max; iy++)
                for (int ix = ix_min; ix <= ix_max; ix++)
                    region.set_value(ix - ix_min, iy - iy_min, get_value(ix, iy));

            return (region, index_out_of_bounds);
        }

        /// <summary>
        /// 根据索引的界限提取区域部分网格，[ix_min,ix_max]和[iy_min,iy_max]是闭区间
        /// </summary>
        /// <param name="ix_min">从1开始</param>
        /// <param name="ix_max">gs.nx结束</param>
        /// <param name="iy_min">从1开始</param>
        /// <param name="iy_max">gs.ny结束</param>
        /// <param name="iz_min">从1开始</param>
        /// <param name="iz_max">gs.ny结束</param>
        /// <returns></returns>
        public (GridProperty region, bool index_out_of_bounds) get_region_by_range(int ix_min, int ix_max, int iy_min, int iy_max, int iz_min, int iz_max)
        {
            bool index_out_of_bounds = false;//是否越界，默认是假
            if (Dimension.D2 == gridStructure.dim)
                throw new Exception(MyExceptions.Geometry_DimensionException);

            int extent_x = ix_max - ix_min;
            int extent_y = iy_max - iy_min;
            int extent_z = iz_max - iz_min;

            ix_min = Math.Max(ix_min, 1);
            ix_max = Math.Min(ix_max, gridStructure.nx);
            iy_min = Math.Max(iy_min, 1);
            iy_max = Math.Min(iy_max, gridStructure.ny);
            iz_min = Math.Max(iz_min, 1);
            iz_max = Math.Min(iz_max, gridStructure.nz);

            if (extent_x != ix_max - ix_min || extent_y != iy_max - iy_min || extent_z != iz_max - iz_min)
                index_out_of_bounds = true;

            //创建一个新网格对象
            var gs = GridStructure.create_simple(ix_max - ix_min + 1, iy_max - iy_min + 1, iz_max - iz_min + 1);

            var region = create(gs);

            for (int iz = iz_min; iz <= iz_max; iz++)
                for (int iy = iy_min; iy <= iy_max; iy++)
                    for (int ix = ix_min; ix <= ix_max; ix++)
                        region.set_value(ix - ix_min + 1, iy - iy_min + 1, iz - iz_min + 1, get_value(ix, iy, iz));

            return (region, index_out_of_bounds);
        }

        /// <summary>
        /// 根据中心的位置索引和半径提取区域
        /// </summary>
        /// <param name="center"></param>
        /// <param name="radius_x"></param>
        /// <param name="radius_y"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public (GridProperty region, bool index_out_of_bounds) get_region_by_center(SpatialIndex center, int radius_x, int radius_y)
        {
            if (Dimension.D3 == gridStructure.dim)
                throw new Exception(MyExceptions.Geometry_DimensionException);

            return get_region_by_range(
                center.ix - radius_x,
                center.ix + radius_x,
                center.iy - radius_y,
                center.iy + radius_y);
        }

        /// <summary>
        /// 根据中心的位置索引和半径提取区域
        /// </summary>
        /// <param name="center"></param>
        /// <param name="radius_x"></param>
        /// <param name="radius_y"></param>
        /// <param name="radius_z"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public (GridProperty region, bool index_out_of_bounds) get_region_by_center(SpatialIndex center, int radius_x, int radius_y, int radius_z)
        {
            if (Dimension.D3 == gridStructure.dim)
                throw new Exception(MyExceptions.Geometry_DimensionException);

            return get_region_by_range(
                center.ix - radius_x,
                center.ix + radius_x,
                center.iy - radius_y,
                center.iy + radius_y,
                center.iz - radius_z,
                center.iz + radius_z);
        }

        #endregion

        #region 缩放计算

        /// <summary>
        /// 缩放计算
        /// </summary>
        /// <param name="gp"></param>
        /// <param name="gs_resize"></param>
        /// <returns></returns>
        public GridProperty resize(GridStructure resized_gs)
        {
            /// 名称：近邻取样插值方法(重采样) 2D维度
            /// 作用：最简单的插值算法，输出像素等于距离它映射的位置最近的输入像素的值
            /// 作者：喻思羽
            /// 编写时间：2015-10-9
            /// 参考资料：http://blog.chinaunix.net/uid-7525568-id-3452691.html
            /// 将Grid从nx,ny缩小到nx1,ny1，本质是缩略图思想
            /// 
            if (gridStructure.dim != resized_gs.dim)
            {
                Console.WriteLine(MyExceptions.Geometry_DimensionException);
                return null;
            }
            if (resized_gs == this.gridStructure)
                return this.deep_clone();

            GridStructure old_gs = gridStructure;//原始GridStructure
            GridProperty resized_gp = create(resized_gs);//创建缩放后的gp

            DataMapper mapper_x = new();
            mapper_x.Reset(0, gridStructure.nx - 1, 0, resized_gs.nx - 1);
            DataMapper mapper_y = new();
            mapper_y.Reset(0, gridStructure.ny - 1, 0, resized_gs.ny - 1);
            DataMapper mapper_z = new();
            mapper_z.Reset(0, gridStructure.nz - 1, 0, resized_gs.nz - 1);

            for (int n = 0; n < resized_gs.N; n++)
            {
                SpatialIndex si_resized = resized_gs.get_spatialIndex(n);
                var ix = mapper_x.MapBToA(si_resized.ix);
                var iy = mapper_y.MapBToA(si_resized.iy);
                var iz = mapper_z.MapBToA(si_resized.iz);

                SpatialIndex si = gridStructure.dim == Dimension.D2 ?
                    SpatialIndex.create((int)ix, (int)iy)
                    : SpatialIndex.create((int)ix, (int)iy, (int)iz);

                resized_gp.set_value(si_resized, get_value(si));//取样赋值
            }
            return resized_gp;
        }

        #endregion

        #region 三维的切片操作

        /// <summary>
        /// 从三维里获取切片
        /// </summary>
        /// <param name="slice_pos"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public GridProperty get_slice(int slice_pos, GridSliceType type)
        {
            return type switch
            {
                GridSliceType.xy_slice => get_xy_slice(slice_pos),
                GridSliceType.yz_slice => get_yz_slice(slice_pos),
                GridSliceType.xz_slice => get_xz_slice(slice_pos),
                _ => null,
            };
        }
        /// <summary>
        /// 垂直于z方向的xy切片
        /// </summary>
        GridProperty get_xy_slice(int iz_pos)
        {
            GridStructure gs = GridStructure.create(gridStructure.nx, gridStructure.ny, 1,
                gridStructure.xsiz, gridStructure.ysiz, gridStructure.zsiz,
                gridStructure.xmn, gridStructure.ymn, gridStructure.zmn);
            GridProperty slice = create(gs);
            for (int iy = 0; iy < gridStructure.ny; iy++)
                for (int ix = 0; ix < gridStructure.nx; ix++)
                    slice.set_value(ix, iy, get_value(ix, iy, iz_pos));
            return slice;
        }
        /// <summary>
        /// 垂直于y方向的xz切片
        /// </summary>
        GridProperty get_xz_slice(int iy_pos)
        {
            GridStructure gs = GridStructure.create(gridStructure.nx, gridStructure.nz, 1,
                gridStructure.xsiz, gridStructure.zsiz, gridStructure.ysiz,
                gridStructure.xmn, gridStructure.zmn, gridStructure.ymn);
            GridProperty slice = create(gs);
            for (int iz = 0; iz < gridStructure.nz; iz++)
                for (int ix = 0; ix < gridStructure.nx; ix++)
                    slice.set_value(ix, iz, get_value(ix, iy_pos, iz));
            return slice;
        }
        /// <summary>
        /// 垂直于x方向的yz切片
        /// </summary>
        GridProperty get_yz_slice(int ix_pos)
        {
            GridStructure gs = GridStructure.create(gridStructure.ny, gridStructure.nz, 1,
                 gridStructure.ysiz, gridStructure.zsiz, gridStructure.xsiz,
                 gridStructure.ymn, gridStructure.zmn, gridStructure.xmn);
            GridProperty slice = create(gs);
            for (int iz = 0; iz < gridStructure.nz; iz++)
                for (int iy = 0; iy < gridStructure.ny; iy++)
                    slice.set_value(iy, iz, get_value(ix_pos, iy, iz));
            return slice;
        }

        /// <summary>
        /// 用切片对三维模型进行更新
        /// </summary>
        /// <param name="slice_pos"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public void set_slice(int slice_pos, GridSliceType type, GridProperty slice)
        {
            switch (type)
            {
                case GridSliceType.none:
                    break;
                case GridSliceType.xy_slice:
                    set_xy_slice(slice_pos, slice);
                    break;
                case GridSliceType.yz_slice:
                    set_yz_slice(slice_pos, slice);
                    break;
                case GridSliceType.xz_slice:
                    set_xz_slice(slice_pos, slice);
                    break;
                default:
                    break;
            }
        }
        /// <summary>
        /// 垂直于z方向的xy切片
        /// </summary>
        void set_xy_slice(int iz_pos, GridProperty xy_slice)
        {
            for (int iy = 0; iy < gridStructure.ny; iy++)
                for (int ix = 0; ix < gridStructure.nx; ix++)
                    set_value(ix, iy, iz_pos, xy_slice.get_value(ix, iy));
        }
        /// <summary>
        /// 垂直于y方向的xz切片
        /// </summary>
        void set_xz_slice(int iy_pos, GridProperty xz_slice)
        {
            for (int iz = 0; iz < gridStructure.nz; iz++)
                for (int ix = 0; ix < gridStructure.nx; ix++)
                    set_value(ix, iy_pos, iz, xz_slice.get_value(ix, iz));
        }
        /// <summary>
        /// 垂直于x方向的yz切片
        /// </summary>
        void set_yz_slice(int ix_pos, GridProperty yz_slice)
        {
            for (int iz = 0; iz < gridStructure.nz; iz++)
                for (int iy = 0; iy < gridStructure.ny; iy++)
                    set_value(ix_pos, iy, iz, yz_slice.get_value(iy, iz));
        }

        #endregion

        #region 二维操作

        /// <summary>
        /// 绘制二维模型
        /// </summary>
        /// <param name="color_null"></param>
        /// <returns></returns>
        public Bitmap draw_image_2d(Color color_of_null, ColorMapEnum color_map_enum)
        {
            Images.ColorMap colorMap = new(Min.Value, Max.Value, 64, 255, color_map_enum);//根据Grid网格值的范围计算颜色表
            Bitmap b = new(gridStructure.nx, gridStructure.ny);

            var reverse = reverse_updown_2d();

            for (int j = 0; j < b.Height; j++)//转换为图像
            {
                for (int i = 0; i < b.Width; i++)
                {
                    var cell = reverse.get_value(i + 1, j + 1);
                    if (cell == null)
                    {
                        cell = reverse.get_value(i + 1, j + 1);
                        b.SetPixel(i, j, color_of_null);
                    }
                    else
                    {
                        Color color = colorMap.MapValueToColor(cell.Value);//颜色映射表获取对应值的颜色
                        b.SetPixel(i, j, color);
                    }
                }
            }

            return b;
        }

        /// <summary>
        /// 二维垂向翻转
        /// </summary>
        /// <returns></returns>
        public GridProperty reverse_updown_2d()
        {
            GridProperty reverse = create(gridStructure);//创建一个新的网格体
                                                         //笛卡尔坐标系to屏幕坐标系（坐标转换）
            for (int iy = 0; iy < gridStructure.ny; iy++)
            {
                for (int ix = 0; ix < gridStructure.nx; ix++)
                {
                    reverse.set_value(ix, gridStructure.ny - iy + 1, get_value(ix, iy));
                }
            }

            return reverse;
        }

        #endregion

        /// <summary>
        /// 返回等于null的网格单元spatialIndex
        /// </summary>
        /// <returns></returns>
        public List<SpatialIndex> get_spatialIndex_eq_null()
        {
            List<SpatialIndex> result = [];
            for (int n = 0; n < gridStructure.N; n++)
            {
                if (get_value(n) == null)
                    result.Add(gridStructure.get_spatialIndex(n));
            }
            return result;
        }

        /// <summary>
        /// 返回不等于null的网格单元spatialIndex
        /// </summary>
        /// <returns></returns>
        public List<SpatialIndex> get_spatialIndex_ne_null()
        {
            List<SpatialIndex> result = [];
            for (int n = 0; n < gridStructure.N; n++)
            {
                if (get_value(n) != null)
                    result.Add(gridStructure.get_spatialIndex(n));
            }
            return result;
        }

        /// <summary>
        /// 显示
        /// </summary>
        public void show_win(string title = null)
        {
            Grid g = Grid.create(gridStructure);
            g.add_gridProperty("GridProperty", this);
            g.showGrid_win(title);
        }

        /// <summary>
        /// 深度复制
        /// </summary>
        /// <returns></returns>
        public GridProperty deep_clone()
        {
            GridProperty gp = create(gridStructure);
            for (int n = 0; n < gridStructure.N; n++)
                gp.set_value(n, get_value(n));
            return gp;
        }

        /// <summary>
        /// gp转换为grid
        /// </summary>
        /// <returns></returns>
        public Grid convert_to_grid()
        {
            Grid g = Grid.create(gridStructure);
            g.add_gridProperty("gp_name", deep_clone());
            return g;
        }

        public double[,] convert_to_array()
        {
            double[,] array = new double[gridStructure.nx, gridStructure.ny];
            for (int iy = 0; iy < gridStructure.ny; iy++)
            {
                for (int ix = 0; ix < gridStructure.nx; ix++)
                {
                    array[ix, iy] = get_value(ix, iy).Value;
                }
            }
            return array;
        }
    }
}