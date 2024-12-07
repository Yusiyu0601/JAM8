namespace JAM8.Algorithms.Geometry
{
    /// <summary>
    /// 条件数据项，(属性名称=>属性值)
    /// </summary>
    public class CDataItem : Dictionary<string, float?>
    {
        private CDataItem() { }

        /// <summary>
        /// 维度
        /// </summary>
        public Dimension dim { get { return coord.dim; } }

        /// <summary>
        /// 坐标
        /// </summary>
        public Coord coord { get; internal set; }

        /// <summary>
        /// 创建新cd_item
        /// </summary>
        /// <param name="c"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public static CDataItem create(Coord coord, Dictionary<string, float?> values)
        {
            CDataItem cdi = new()
            {
                coord = coord
            };
            foreach (var (key, value) in values)
                cdi.Add(key, value);

            return cdi;
        }

        public override string ToString()
        {
            return $"{coord.view_text()} {this.Count}";
        }

        /// <summary>
        /// 深度复制
        /// </summary>
        /// <returns></returns>
        public CDataItem deep_clone()
        {
            CDataItem cdi = new()
            {
                coord = coord.deep_clone()
            };
            foreach (var (key, value) in this)
                cdi.Add(key, value);

            return cdi;
        }
    }
}
