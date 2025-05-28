namespace JAM8.Algorithms.Geometry
{
    /// <summary>
    /// 搜索树节点类
    /// </summary>
    public class STreeNode
    {
        //节点在树的深度
        public int depth_in_tree { get; internal set; }

        //id,唯一标识码
        public int id { get; internal set; }

        //guid
        public string guid { get; internal set; }

        public List<int> guid_array { get; internal set; }

        //节点值
        public int value { get; internal set; }

        //节点的双亲节点
        public STreeNode father { get; internal set; }

        //节点的孩子节点
        public Dictionary<int, STreeNode> children { get; internal set; }

        /// <summary>
        /// 条件点约束下的取值与重复数
        /// </summary>
        public Dictionary<int, int> core_values_repl { get; internal set; }
    }
}