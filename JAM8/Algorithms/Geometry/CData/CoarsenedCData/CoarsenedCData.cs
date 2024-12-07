using Easy.Common.Extensions;

namespace JAM8.Algorithms.Geometry
{
    /// <summary>
    /// 粗化后条件数据点的属性数据
    /// key是属性的名称
    /// value是属性的数值
    /// </summary>
    public class CoarsenedCDataItem : Dictionary<string, float?>
    {

    }

    /// <summary>
    /// 粗化到GridStructure后的条件数据
    /// key是条件数据点粗化落在grid中的array_index
    /// value是条件数据的属性值
    /// </summary>
    public class CoarsenedCData : Dictionary<int, CoarsenedCDataItem>
    {
        /// <summary>
        /// 粗化目标网格结构
        /// </summary>
        public GridStructure gridStructure { get; internal set; }

        private CoarsenedCData()
        {

        }

        /// <summary>
        /// 属性名称集合
        /// </summary>
        public List<string> PropertyNames
        {
            get; internal set;
        }

        public float? this[int array_index, string property_name]
        {
            get
            {
                return this[array_index, property_name];
            }
        }
        public float? this[int array_index, int property_index]
        {
            get
            {
                return this[array_index, PropertyNames[property_index]];
            }
        }

        /// <summary>
        /// 创建CoarsenedCData对象
        /// 超出范围的点将被删除
        /// </summary>
        /// <param name="gs">网格结构</param>
        /// <param name="cd">CData对象</param>
        /// <returns></returns>
        public static (CoarsenedCData ccd, int N_out_of_range) create(GridStructure gs, CData cd)
        {
            CoarsenedCData ccd = new()
            {
                gridStructure = gs,
                PropertyNames = new List<string>(cd.propertyNames)
            };

            int N_out_of_range = 0;
            foreach (var cdi in cd)
            {
                int array_index = gs.coord_to_arrayIndex(cdi.coord);
                CoarsenedCDataItem ccdi = new();
                foreach (var item in cdi)
                {
                    ccdi.Add(item.Key, item.Value);
                }
                if (array_index != -1)//判断是否超出范围
                    ccd.Add(array_index, ccdi);
                else//记录超出范围的点
                    N_out_of_range += 1;
            }
            return (ccd, N_out_of_range);
        }

        /// <summary>
        /// 创建CoarsenedCData对象
        /// </summary>
        /// <param name="g"></param>
        /// <param name="exclude_null"></param>
        /// <returns></returns>
        public static CoarsenedCData create(Grid g, bool exclude_null = true)
        {
            var gs = g.gridStructure;
            CoarsenedCData ccd = new()
            {
                gridStructure = gs,
                PropertyNames = new List<string>(g.propertyNames)
            };

            for (int array_index = 0; array_index < gs.N; array_index++)
            {
                CoarsenedCDataItem ccdi = [];
                foreach (var propertyName in g.propertyNames)
                {
                    ccdi.Add(propertyName, g[propertyName].get_value(array_index));
                }

                ccd.Add(array_index, ccdi);
            }
            return ccd;
        }

        /// <summary>
        /// 转换为grid对象
        /// </summary>
        /// <returns></returns>
        public Grid to_grid()
        {
            Grid g = Grid.create(gridStructure);
            foreach (var property_name in PropertyNames)
            {
                g.add_gridProperty(property_name);
            }

            foreach (var (array_index, ccdi) in this)
            {
                foreach (var (property_name, value) in ccdi)
                {
                    g[property_name].set_value(array_index, value);
                }
            }
            return g;
        }
    }
}
