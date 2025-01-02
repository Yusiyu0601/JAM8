using Easy.Common.Extensions;
using JAM8.Algorithms.Numerics;
using JAM8.Utilities;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;

namespace JAM8.Algorithms.Geometry
{
    /// <summary>
    /// 样式库，字典类<arrayIndex,mouldInstance>
    /// </summary>
    public class Patterns : Dictionary<int, MouldInstance>
    {
        private Patterns() { }

        List<int> arrayIndexes = null;//样式在训练图像中的arrayIndex

        /// <summary>
        /// 通过样式在样式库中的索引（非gridstruture的arrayIndex）获取样式，
        /// 虽然样式在字典中，但是仍然有顺序。
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public MouldInstance get_by_index(int index)
        {
            return this[arrayIndexes[index]];
        }

        /// <summary>
        /// 从样式库中随机抽样，返回样式的arrayIndex
        /// </summary>
        /// <param name="seed"></param>
        /// <returns></returns>
        public int random_select(Random rnd)
        {
            return arrayIndexes[rnd.Next(0, arrayIndexes.Count)];
        }

        public override string ToString()
        {
            return $"N:{Count}";
        }

        /// <summary>
        /// 创建样式库
        /// 支持并行处理和去重功能
        /// </summary>
        /// <param name="mould">给定样板</param>
        /// <param name="gp_source">网格属性</param>
        /// <param name="parallel">是否使用并行处理</param>
        /// <param name="distinct">是否去重</param>
        /// <returns>生成的 Patterns</returns>
        public static Patterns create(Mould mould, GridProperty gp_source, bool parallel = true, bool distinct = true)
        {
            GridStructure gs = gp_source.gridStructure;
            ConcurrentBag<MouldInstance> patterns_list = [];

            if (parallel)
            {
                // 使用并行提取 Patterns
                ConcurrentBag<int> flag = [];//计数器
                // 使用 Parallel.For 提取 patterns
                Parallel.For(0, gp_source.gridStructure.N, n =>
                {
                    var pattern = MouldInstance.create_from_gridProperty(mould, gs.get_spatialIndex(n), gp_source);
                    if (pattern.neighbor_not_nulls_ids.Count == pattern.mould.neighbors_number)
                        patterns_list.Add(pattern);

                    flag.Add(1);// 更新进度，仅记录索引
                    MyConsoleProgress.Print(flag.Count, gs.N, "提取patterns");
                });
            }
            else
            {
                // 使用串行提取 Patterns
                int progress = 0; // 进度计数器
                for (int n = 0; n < gs.N; n++)
                {
                    // 创建模式
                    var pattern = MouldInstance.create_from_gridProperty(mould, gs.get_spatialIndex(n), gp_source);

                    // 仅记录所有邻居非空的模式
                    if (pattern.neighbor_not_nulls_ids.Count == pattern.mould.neighbors_number)
                        patterns_list.Add(pattern);

                    // 更新进度
                    progress++;
                    MyConsoleProgress.Print(progress, gs.N, "提取Patterns（去重）");
                }
            }

            // 输出结果
            Patterns patterns = new()
            {
                arrayIndexes = []
            };

            
            if (!distinct)
            {
                //不去重，直接返回结果
                foreach (var item in patterns_list)
                {
                    patterns.arrayIndexes.Add(item.core_arrayIndex);
                    patterns.Add(item.core_arrayIndex, item);
                }
                return patterns;
            }
            else
            {
                //去重复
                Dictionary<string, int> unicodes = [];
                foreach (var item in patterns_list)
                {
                    string unicode = item.view_text();
                    if (!unicodes.TryGetValue(unicode, out int value))
                    {
                        unicodes.Add(unicode, 1);
                        patterns.arrayIndexes.Add(item.core_arrayIndex);
                        patterns.Add(item.core_arrayIndex, item);
                    }
                    else
                        unicodes[unicode] = value + 1;
                }
                return patterns;
            }
        }
    }
}
