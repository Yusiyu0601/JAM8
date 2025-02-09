using System.Text;
using Accord.Math;
using JAM8.Algorithms.Numerics;
using JAM8.Utilities;

namespace JAM8.Algorithms.Geometry
{
    /// <summary>
    /// 模板实例类,是使用模板扫描网格属性获取的对象
    /// </summary>
    public class MouldInstance
    {
        /// <summary>
        /// 记录使用的模板
        /// </summary>
        public Mould mould { get; internal set; }

        /// <summary>
        /// 从grid property中提取core位置mould instance之后,记录core的值
        /// </summary>
        public float? core_value { get; internal set; }

        /// <summary>
        /// 从grid property中提取core位置mould instance之后，采用array index记录core提取位置
        /// </summary>
        public int core_arrayIndex { get; internal set; } = -1;

        /// <summary>
        /// 邻居节点的数据缓存区
        /// </summary>
        public List<float?> neighbor_values { get; internal set; }

        /// <summary>
        /// 邻居中null节点在(mould)spiral_mapper的索引
        /// </summary>
        public List<int> neighbor_nulls_ids { get; internal set; }

        /// <summary>
        /// 邻居中not null节点在(mould)spiral_mapper的索引
        /// </summary>
        public List<int> neighbor_not_nulls_ids { get; internal set; }

        /// <summary>
        /// 根据neighbor_idx获取值
        /// </summary>
        /// <param name="neighbor_idx"></param>
        /// <returns></returns>
        public float? this[int neighbor_idx]
        {
            get
            {
                return neighbor_values[neighbor_idx];
            }
            set
            {
                neighbor_values[neighbor_idx] = value;
            }
        }

        /// <summary>
        /// 私有构造函数
        /// </summary>
        private MouldInstance() { }

        /// <summary>
        /// 打印
        /// </summary>
        /// <returns></returns>
        public string view_text()
        {
            StringBuilder str_builder = new();
            string str_core = core_value == null ? "[N] " : $"[{core_value}] ";
            str_builder.Append(str_core);
            // 处理前 n-1 项
            for (int i = 0; i < mould.neighbors_number - 1; i++)
            {
                str_builder.Append(neighbor_values[i] == null ? "N" : neighbor_values[i]);
                str_builder.Append('_'); // 添加分隔符
            }
            // 处理最后一项，不添加分隔符
            str_builder.Append(neighbor_values[mould.neighbors_number - 1] == null ? "N" : neighbor_values[mould.neighbors_number - 1]);

            return str_builder.ToString();
        }

        public override string ToString()
        {
            return core_arrayIndex.ToString();
        }

        /// <summary>
        /// 更新模板实例的数据，具体操作是基于mould从grid_property中提取core位置的mould_instance
        /// 作用是避免重复创建新对象的低效率
        /// </summary>
        /// <param name="core"></param>
        /// <param name="gp_source"></param>
        public void update_from_gridProperty(SpatialIndex core, GridProperty gp_source)
        {
            neighbor_values.Clear();//清空数据，保留容量
            neighbor_nulls_ids.Clear();//清空数据
            neighbor_not_nulls_ids.Clear();//清空数据

            //记录中心点的arrayindex
            core_arrayIndex = gp_source.gridStructure.get_arrayIndex(core);
            //中心点单独提取
            core_value = gp_source.get_value(core);

            for (int n = 0; n < mould.neighbors_number; n++)
            {
                // 获取邻居索引
                SpatialIndex neighbor = core.offset(mould.neighbor_spiral_mapper[n].spatial_index);
                var value = gp_source.get_value(neighbor);// 获取对应的值
                neighbor_values.Add(value);
                if (value == null)//判断该节点的数据是否为null
                    neighbor_nulls_ids.Add(n);
                else
                    neighbor_not_nulls_ids.Add(n);
            }
        }

        /// <summary>
        /// 更新模板实例的数据，具体操作是通过传递新数据覆盖原始数据，超过模板范围的数组元素被丢弃
        /// </summary>
        /// <param name="mould"></param>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public void update(float? core_value, int core_arrayIndex, float?[] neighbor_values)
        {
            this.neighbor_values.Clear();//清空数据，保留容量
            this.neighbor_nulls_ids.Clear();//清空数据
            this.neighbor_not_nulls_ids.Clear();//清空数据

            this.core_arrayIndex = core_arrayIndex;//更新core_arrayIndex
            this.core_value = core_value;//更新core_value

            for (int n = 0; n < mould.neighbors_number; n++)
            {
                this.neighbor_values.Add(neighbor_values[n]);
                //判断该节点的数据是否为null
                if (neighbor_values[n] == null)
                    this.neighbor_nulls_ids.Add(n);
                else
                    this.neighbor_not_nulls_ids.Add(n);
            }
        }

        /// <summary>
        /// 基于Mould创建一个空的mouldInstance
        /// </summary>
        /// <param name="mould"></param>
        /// <param name="core"></param>
        /// <param name="gp_source"></param>
        /// <param name="gp_restricedRegion"></param>
        /// <returns></returns>
        public static MouldInstance create(Mould mould)
        {
            MouldInstance instance = new()
            {
                mould = mould,
                neighbor_values = new(mould.neighbors_number),
                neighbor_nulls_ids = new(mould.neighbors_number),
                neighbor_not_nulls_ids = new(mould.neighbors_number),
            };
            return instance;
        }

        /// <summary>
        /// 提取GridProperty里所有loc位置的MouldInstance
        /// </summary>
        /// <param name="mould"></param>
        /// <param name="locs"></param>
        /// <param name="gp_source"></param>
        /// <returns></returns>
        public static MouldInstance create_from_gridProperty(Mould mould, SpatialIndex loc, GridProperty gp_source)
        {
            var mould_instance = create(mould);  // 创建新的MouldInstance
            mould_instance.update_from_gridProperty(loc, gp_source);  // 更新MouldInstance

            return mould_instance;
        }

        /// <summary>
        /// 提取GridProperty里所有loc位置的MouldInstance
        /// </summary>
        /// <param name="mould"></param>
        /// <param name="locs"></param>
        /// <param name="gp_source"></param>
        /// <returns></returns>
        public static List<MouldInstance> create_from_gridProperty(Mould mould, List<SpatialIndex> locs, GridProperty gp_source)
        {
            // 预分配实例列表的容量，避免多次扩容
            List<MouldInstance> instances = new(locs.Count);

            // 遍历locs列表并创建对应的MouldInstance
            foreach (var loc in locs)
            {
                var mould_instance = create(mould);  // 创建新的MouldInstance
                mould_instance.update_from_gridProperty(loc, gp_source);  // 更新MouldInstance
                instances.Add(mould_instance);  // 将创建的实例添加到结果列表
            }

            return instances;
        }

        /// <summary>
        /// 提取GridProperty里所有loc位置的MouldInstance
        /// </summary>
        /// <param name="mould"></param>
        /// <param name="gp_source"></param>
        /// <returns></returns>
        public static List<MouldInstance> create_from_gridProperty(Mould mould, GridProperty gp_source)
        {
            // 预分配实例列表的容量，避免多次扩容
            List<MouldInstance> instances = new(gp_source.gridStructure.N);

            for (int n = 0; n < gp_source.gridStructure.N; n++)
            {
                MyConsoleProgress.Print(n, gp_source.gridStructure.N, "提取模式");
                var loc = gp_source.gridStructure.get_spatialIndex(n);
                var mould_instance = create(mould);  // 创建新的MouldInstance
                mould_instance.update_from_gridProperty(loc, gp_source);  // 更新MouldInstance
                instances.Add(mould_instance);  // 将创建的实例添加到结果列表
            }
            return instances;
        }

        /// <summary>
        /// 将mouldInstance粘贴到GridProperty里面的Core位置
        /// </summary>
        /// <param name="CoreLoc"></param>
        /// <param name="Grid"></param>
        /// <param name="instance"></param>
        /// <returns></returns>
        public List<SpatialIndex> paste_to_gridProperty(Mould mould, SpatialIndex core_in_gp_target, GridProperty gp_target)
        {
            //此处注意：gp_target的起始点x和y都是1
            List<SpatialIndex> spatialIndexes = [];
            for (int n = 0; n < mould.neighbors_number; n++)
            {
                SpatialIndex neighbor = core_in_gp_target.offset(mould.neighbor_spiral_mapper[n].spatial_index);
                gp_target.set_value(neighbor, neighbor_values[n]);
                spatialIndexes.Add(neighbor);
            }
            return spatialIndexes;
        }

        /// <summary>
        /// 评估模板实例每个位置的值是否符合判断条件，只有满足条件的位置会被保留。最终组成一个新的MouldInstance。
        /// </summary>
        /// <param name="compare_type"></param>
        /// <param name="compare_value"></param>
        /// <returns></returns>
        public MouldInstance trim(CompareType compare_type, float? compare_value)
        {
            Dictionary<string, float?> neighbors_values_dict = [];//<spatialIndex_str,value>，辅助找到对应的neighbor_value
            SpatialIndex core = mould.dim == Dimension.D2 ? SpatialIndex.create(0, 0) : SpatialIndex.create(0, 0, 0);
            List<SpatialIndex> neighbors = [];

            //判断模板实例每个位置的值是否符合条件
            for (int i = 0; i < mould.neighbors_number; i++)
            {
                float? currentValue = neighbor_values[i];
                neighbors_values_dict.Add(mould.neighbor_spiral_mapper[i].spatial_index.view_text(), currentValue);

                // 判断当前条件是否满足
                bool satisfy = compare_type switch
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

                // 如果当前条件满足
                if (satisfy)
                {
                    neighbors.Add(mould.neighbor_spiral_mapper[i].spatial_index);
                }
            }
            //生成修剪后的模板
            Mould trim_mould = Mould.create_by_location(core, neighbors);

            //使用精简模板，生成精简模板实例
            MouldInstance trim_mouldInstance = MouldInstance.create(trim_mould);

            //查找对应位置的值
            trim_mouldInstance.core_value = this.core_value;
            trim_mouldInstance.core_arrayIndex = this.core_arrayIndex;
            for (int i = 0; i < trim_mould.neighbors_number; i++)
            {
                var si_str = trim_mould.neighbor_spiral_mapper[i].spatial_index.view_text();
                var value = neighbors_values_dict[si_str];
                trim_mouldInstance.neighbor_values.Add(value);
                //判断该节点的数据是否为null
                if (value == null)
                    trim_mouldInstance.neighbor_nulls_ids.Add(i);
                else
                    trim_mouldInstance.neighbor_not_nulls_ids.Add(i);
            }

            return trim_mouldInstance;
        }
    }
}
