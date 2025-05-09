using System.Collections.Concurrent;
using System.Diagnostics;
using System.Text;
using JAM8.Utilities;
using LiteDB;

namespace JAM8.Algorithms.Geometry
{
    /// <summary>
    /// search tree
    /// 搜索树
    /// </summary>
    public class STree
    {
        private STree() { }

        /// <summary>
        /// root of search tree
        /// 搜索树的根
        /// </summary>
        private STreeNode root { get; set; }

        /// <summary>
        /// template
        /// 模板
        /// </summary>
        private Mould mould = null;

        /// <summary>
        /// training images
        /// 训练图像
        /// </summary>
        private GridProperty TI = null;

        /// <summary>
        /// Values of discrete variables
        /// 离散变量的取值
        /// </summary>
        public List<float?> categories { get; internal set; }

        /// <summary>
        /// Search for the number of layers of the tree. Since the depth of root is 1, 
        /// the depth and number of layers of the search tree should be equal to the number 
        /// of template neighbor nodes + 1.
        /// 搜索树的层数，因为root深度是1，因此搜索树深度层数应该等于模板邻居节点数+1
        /// </summary>
        public int tree_depth
        {
            get
            {
                return mould.neighbors_number + 1;
            }
        }

        /// <summary>
        /// Auxiliary “reverse” query structure 
        /// 辅助"逆向"查询结构
        /// </summary>
        public List<Dictionary<float?, STreeNode[]>> stree_nodes_with_levels = null;

        public MyDataFrame df = null;//Statistics on the number of access nodes and calculation time 统计访问节点数量和计算时间

        /// <summary>
        /// Create a search tree 创建搜索树
        /// </summary>
        /// <param name="mould"></param>
        /// <param name="TI">Training image variable values require discrete variable types. 训练图像变量值要求离散变量类型</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static STree create(Mould mould, GridProperty TI)
        {
            var distinct = TI.buffer.Distinct().ToList();
            distinct.Remove(null);
            if (distinct.Count > 10)
            {
                MessageBox.Show("变量值过多，请检查训练图像是否为连续变量");
                return null;
            }
            distinct.Sort();

            STree tree = new()
            {
                mould = mould,
                TI = TI,
                categories = distinct.Select(a => a).ToList(),
            };
            tree.init_tree();
            tree.init_reverse_query_structure();
            MyConsoleHelper.write_string_to_console("Total number of search tree nodes", tree.get_nodes_count().ToString());

            tree.df = MyDataFrame.create(["访问节点总数", "计算时间(毫秒)"]);

            return tree;
        }

        /// <summary>
        /// Search tree initialization 搜索树初始化
        /// </summary>
        private void init_tree()
        {
            var patterns = Patterns.create(mould, TI);//提取TI所有pattern
            long id = 0;//节点id

            #region 创建根节点

            //初始化搜索树的根节点root
            root = new STreeNode
            {
                value = -99.99f, // root 节点值初始化为 -99.99
                id = id++,       // 自增 ID
                guid = "r",      // "r" 代表 root
                depth_in_tree = -1, // 根节点深度为 -1
                core_values_repl = categories.ToDictionary(
                    category => category,
                    category => patterns.Count(pattern => pattern.Value.core_value == category)
                ),
            };

            // 填充root的 children
            root.children = categories.ToDictionary(
                category => (float?)category,
                category => new STreeNode
                {
                    value = category,        // 子节点的值为当前分类
                    id = id++,               // 分配唯一 ID
                    guid = $"r{category}",   // GUID，例如 "r1"
                    depth_in_tree = 0,       // 子节点深度为 0
                    core_values_repl = [], // 空的 core_values_repl
                    children = [],  // 空的 children
                    father = root            // 父节点正确设置为 root
                }
            );

            #endregion

            #region 遍历所有pattern，创建树的分支

            //遍历所有pattern
            foreach (var (_, pattern) in patterns)
            {
                // 每个 pattern 的解析从 root的孩子节点（即第1个邻居节点）开始
                STreeNode temp_node = root.children[pattern[0]];

                // 遍历 neighbors 中的所有邻居节点，由近及远分析
                for (int neighbor_idx = 0; neighbor_idx < pattern.mould.neighbors_number; neighbor_idx++)
                {
                    //判断temp_node的重复数是否包含分类变量值
                    if (temp_node.core_values_repl.ContainsKey(pattern.core_value))
                        temp_node.core_values_repl[pattern.core_value] += 1;//存在则加1
                    else
                        temp_node.core_values_repl.Add(pattern.core_value, 1);//不存在则新建，并赋值为1

                    //如果当前节点没有孩子节点，跳出循环
                    if (neighbor_idx + 1 == pattern.mould.neighbors_number)
                        break;
                    //获取孩子节点（后续邻居节点）的值
                    var child_value = pattern[neighbor_idx + 1];
                    //如果 temp_node 的 children 中不存在以 child_value 为键的节点，则新建一个孩子节点
                    if (!temp_node.children.ContainsKey(child_value))
                    {
                        temp_node.children[child_value] = new STreeNode
                        {
                            value = child_value,                          // 设置新节点的值
                            id = id++,                                    // 分配一个唯一的自增 ID
                            guid = $"{temp_node.guid}{child_value}",      // GUID 继承父节点的 GUID，并附加子节点的值
                            core_values_repl = [],                        // 初始化 core_values_repl 为一个空字典
                            depth_in_tree = temp_node.depth_in_tree + 1,  // 子节点的深度比父节点深一层
                            father = temp_node,                           // 设置父节点为当前的 temp_node
                            children = []                                 // 初始化子节点的 children 为一个空字典
                        };
                    }
                    temp_node = temp_node.children[child_value];//用孩子节点更新temp node，进入下一层
                }
            }

            #endregion
        }

        private void init_reverse_query_structure()
        {
            stree_nodes_with_levels = [];

            var level_root = new Dictionary<float?, STreeNode[]>()
            {
                { -1, new STreeNode[] { root } }
            };
            stree_nodes_with_levels.Add(level_root);//Add the first level (i.e., root). 添加第1层(即root)

            List<STreeNode> temp_nodes = [root];//Temporary node, initialized as root. 临时节点，初始化为root
            for (int level = 1; level < tree_depth; level++)//Add level i. 添加第i层
            {
                MyConsoleProgress.Print(level, tree_depth, "Construct a reverse query structure");

                List<STreeNode> nodes_level = [];
                foreach (var node in temp_nodes)//Query all nodes on the ith layer. 查询第i层所有nodes
                    nodes_level.AddRange(node.children.Values);
                temp_nodes = nodes_level;//Update temp_nodes. 更新temp_nodes

                Dictionary<float?, STreeNode[]> nodes_level_category = [];
                //Classify nodes of different categories in this layer. 对该层中不同category的node进行分类
                foreach (var category in categories)
                    nodes_level_category.Add(category, [.. nodes_level.FindAll(a => a.value == category)]);

                stree_nodes_with_levels.Add(nodes_level_category);
            }
        }

        /// <summary>
        /// Count the total number of nodes in the search tree. 统计搜索树节点总数
        /// </summary>
        /// <returns></returns>
        private int get_nodes_count()
        {
            int nodes_count = 1;//初始化为1
            List<STreeNode> nodes_temp = [root];//Temporary node collection 临时节点集合
            for (int level = 0; level <= tree_depth; level++)//添加第i层
            {
                List<STreeNode> nodes_level = [];
                foreach (var node in nodes_temp)//查询第i层所有nodes
                    nodes_level.AddRange(node.children.Values);
                nodes_temp = nodes_level;//更新
                nodes_count += nodes_level.Count;
            }
            return nodes_count;
        }

        /// <summary>
        /// Retrieve the duplicate count of data events from the search tree. 从搜索树里取回数据事件的重复数
        /// </summary>
        /// <param name="data_event"></param>
        /// <param name="cd_min"></param>
        /// <returns></returns>
        public Dictionary<float?, int> retrieve(MouldInstance data_event, int cd_min)
        {
            //Record the count of duplicate core_value entries at each depth level in the tree,
            //followed by statistical comparison with cd_min
            //记录树中各深度的core_value重复数，后续还要统计与cd_min比较
            List<Dictionary<float?, int>> core_values_all_levels = [];

            int node_max = 0; // Maximum number of valid nodes 有效节点的最大数量
            List<STreeNode> temp_level_nodes = [root];//Temp node collection (initialized to root) 临时节点集合，初始化为root

            //总数与当前层
            int Sum = 0, Level = 0;
            Stopwatch sw = new();
            sw.Start();
            //For the neighbors of the data event, search sequentially from nearest to farthest
            //(until the last non-null node position) for nodes that meet the condition.
            //针对数据事件的邻居，由近及远（直到最后一个非空节点的位置）逐个查找满足条件的节点
            for (int neighbor_idx = 0; neighbor_idx <= data_event.neighbor_not_nulls_ids.Last(); neighbor_idx++)
            {
                List<STreeNode> nodes_temp = [];
                float? neighbor_value = data_event[neighbor_idx];//邻居节点取值

                //At this time, if the current neighbor node set is empty, then record all the child nodes of the nodes
                //(the previous neighbor nodes) in the temporary node set.
                //此时当前邻居节点为空，则记录临时节点集里节点（前一个邻居节点）的所有孩子节点
                if (neighbor_value == null)
                {
                    foreach (var node in temp_level_nodes)
                    {
                        nodes_temp.AddRange(node.children.Values);
                    }
                }
                //If the neighbor node set is not empty, then make a judgment on `nodes_temp` based on its value
                //and filter out the unmatched child nodes.
                //如果邻居节点不为空，则根据其值对nodes_temp进行判断，过滤掉不匹配的孩子节点
                else
                {
                    foreach (var node in temp_level_nodes)
                    {
                        if (node.children.TryGetValue(neighbor_value, out STreeNode value))
                            nodes_temp.Add(value);
                    }
                    node_max++;

                    // Initialize the core_values dictionary with keys as categorical variables (categories) and
                    // values as the sum of the repetitions of the corresponding categorical variable in all nodes
                    // at this layer of the search tree.
                    // 初始化 core_values 字典，键为分类变量（categories），值为搜索树该层所有节点中对应分类变量的重复数之和
                    var core_values = categories.ToDictionary(
                        category => category,
                        // Sum the current categorical variable values in core_values_repl for each node. If it
                        // does not exist, it is defaulted to 0.
                        // 对每个节点的 core_values_repl 中当前分类变量值求和，不存在则默认为 0
                        category => nodes_temp.Sum(node => node.core_values_repl.GetValueOrDefault(category, 0))
                    );

                    core_values_all_levels.Add(core_values);//Record the repetition number of this layer.记录该层的重复数
                }

                temp_level_nodes = nodes_temp;//update 更新

                Sum += nodes_temp.Count;
                Level = nodes_temp.Count;
            }
            sw.Stop();

            // 将 tick 转换为毫秒
            double elapsedMicroseconds = (sw.ElapsedTicks / (double)Stopwatch.Frequency) * 1_000;
            df.add_record([Sum, elapsedMicroseconds]);

            // 输出结果
            return core_values_all_levels
                .AsEnumerable()
                .Reverse()// Reverse the order of the set. 反转集合的顺序
                .FirstOrDefault(core_values
                => core_values.Sum(a => a.Value) > cd_min);// Find the first element that meets the condition,
                                                           // which is that the sum of all 'Value' in 'core_values' is greater
                                                           // than 'cd_min'. If there is no element that meets the condition,
                                                           // return 'null'.
                                                           // 查找第一个符合条件的元素,条件是 core_values 中所有 Value 的总和大于
                                                           // cd_min。如果没有符合条件的元素，返回 null。
        }

        /// <summary>
        /// Retrieve the count of duplicate events from the search tree in reverse. 
        /// 反向从搜索树里取回数据事件的重复数
        /// </summary>
        /// <param name="data_event"></param>
        /// <param name="cd_min"></param>
        /// <returns></returns>
        public Dictionary<float?, int> retrieve_inverse(MouldInstance data_event, int cd_min)
        {
            var sb = new StringBuilder();
            //Add the core value in. 将core值加进来
            sb.Append(data_event.core_value == null ? "n" : data_event.core_value.ToString());
            for (int i = 0; i < data_event.mould.neighbors_number; i++)
            {
                sb.Append(data_event[i] == null ? "n" : data_event[i].ToString());
            }
            var Guid = sb.ToString();

            List<int> cd_indexes = [.. data_event.neighbor_not_nulls_ids];//深度复制
            cd_indexes.Reverse();//Reverse, from large to small. 反转，由大到小

            //The duplicate number result obtained by final statistics 最终统计得到的重复数结果
            Dictionary<float?, int> result = null;

            int flag = 0;
            //Query layer by layer from far to near. 由远到近的逐层查询
            foreach (var cd_level in cd_indexes)
            {
                var cd_value = data_event[cd_level];
                //Calculate the actual level 计算实际层次
                var cd_level_real = cd_level + 1;
                STreeNode[] nodes_matched = this.stree_nodes_with_levels[cd_level_real][cd_value];

                //Check whether the nodes of the cd that are loyal to the current level also match the previous
                //few cds (if any).
                //检查忠于当前level的cd的nodes，其前几个(如果存在)cd是否也匹配
                if (cd_indexes.Count > 1)
                {
                    ConcurrentBag<STreeNode> temp_nodes = [];
                    flag++;
                    var levels_above = cd_indexes.GetRange(flag, cd_indexes.Count - flag);//above是针对当前level的前几个cd

                    #region 并行计算

                    int NumberOfThreads = -1;

                    //If the number of threads is equal to the default -1, it means using all threads. 
                    ///如果线程数等于默认的-1,则表示使用所有线程数
                    if (NumberOfThreads == -1)
                        NumberOfThreads = Environment.ProcessorCount;
                    Parallel.ForEach(nodes_matched,
                        new ParallelOptions() { MaxDegreeOfParallelism = NumberOfThreads },
                        node =>
                        {
                            bool matched = true;
                            //Check whether all nodes at levels 1 to level - 1 are all matched.
                            //检查1 ~ level-1层的节点是否全部matched
                            foreach (var idx in levels_above)
                            {
                                //计算实际索引
                                int idx_real = idx + 1;
                                //As long as there is one difference, it won't work.
                                //只要有一个不同，就不符合条件
                                if (node.guid[idx_real] != Guid[idx_real])
                                {
                                    matched = false;
                                    break;//If one doesn't work, just give up. 有一个不行，就放弃吧
                                }
                            }
                            if (matched)
                                temp_nodes.Add(node);
                        });

                    #endregion

                    #region 串行计算

                    //foreach (var node in nodes_matched)//
                    //{
                    //    bool matched = true;
                    //    //检查1 ~ level-1层的节点是否全部matched
                    //    foreach (var idx in levels_above)
                    //    {
                    //        //计算实际索引
                    //        int idx_real = idx + 1;
                    //        if (node.guid[idx_real] != Guid[idx_real])//只要有一个不同，就不行
                    //        {
                    //            matched = false;
                    //            break;//有一个不行，就放弃吧
                    //        }
                    //    }
                    //    if (matched)
                    //        temp_nodes.Add(node);
                    //}

                    #endregion

                    nodes_matched = [.. temp_nodes];
                }

                if (nodes_matched.Length > 0)
                {
                    var coreValues_matched = categories.ToDictionary(category => category, _ => 0);
                    //Accumulate the coreValue of all nodes that match 'cd'.
                    //累积所有匹配cd的nodes的coreValue
                    foreach (var node in nodes_matched)
                    {
                        foreach (var KeyValuePair in node.core_values_repl)
                        {
                            coreValues_matched[KeyValuePair.Key] += KeyValuePair.Value;
                        }
                    }

                    //The coreValues of the current level meet CMin and jump out.
                    //当前level的coreValues满足CMin，跳出
                    if (coreValues_matched.Sum(a => a.Value) > cd_min)
                    {
                        result = coreValues_matched;
                        break;
                    }
                }
            }

            return result;
        }

    }
}
