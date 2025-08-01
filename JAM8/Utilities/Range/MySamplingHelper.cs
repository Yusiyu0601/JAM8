using System;
using System.Collections.Generic;

namespace JAM8.Utilities
{
    /// <summary>
    /// 抽样工具类，提供从列表或字典中进行随机抽取的功能。
    /// <para>作者：喻思羽 & ChatGPT</para>
    /// <para>创建时间：2025-08-01</para>
    /// </summary>
    public static class MySamplingHelper
    {
        /// <summary>
        /// 从列表中随机抽取 N 个元素（不重复，等概率）。
        /// </summary>
        /// <typeparam name="T">列表元素的类型</typeparam>
        /// <param name="list">待抽样的输入列表</param>
        /// <param name="N">抽样数量</param>
        /// <param name="mt">梅森旋转随机数生成器</param>
        /// <returns>包含 N 个随机元素的新列表</returns>
        /// <remarks>若 N 超过原始列表大小，则返回完整打乱后的列表。</remarks>
        public static List<T> sample_from_list<T>(IList<T> list, int N, MersenneTwister mt)
        {
            if (list.Count < N) N = list.Count;

            var (shuffled, _) = MyShuffleHelper.fisher_yates_shuffle(list, mt);
            return shuffled.GetRange(0, N);
        }

        /// <summary>
        /// 从字典中随机抽取 N 个键值对（不重复，等概率）。
        /// </summary>
        /// <typeparam name="T">字典键类型</typeparam>
        /// <typeparam name="V">字典值类型</typeparam>
        /// <param name="dict">待抽样的字典</param>
        /// <param name="N">抽样数量</param>
        /// <param name="mt">梅森旋转随机数生成器</param>
        /// <returns>一个包含 N 个随机键值对的新字典</returns>
        /// <remarks>
        /// 字典的遍历顺序将被打乱，仅适用于不关心插入顺序的场景。
        /// 若 N 超过字典大小，则返回完整打乱的字典。
        /// </remarks>
        public static Dictionary<T, V> sample_from_dict<T, V>(Dictionary<T, V> dict, int N, MersenneTwister mt)
        {
            if (dict.Count < N) N = dict.Count;

            var shuffled = MyShuffleHelper.fisher_yates_shuffle(dict, mt);
            Dictionary<T, V> result = new();

            int counter = 0;
            foreach (var kv in shuffled)
            {
                result.Add(kv.Key, kv.Value);
                counter++;
                if (counter >= N) break;
            }

            return result;
        }
    }
}