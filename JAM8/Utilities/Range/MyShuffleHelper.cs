using Easy.Common.Extensions;

namespace JAM8.Utilities
{
    /// <summary>
    /// 排序与抽样辅助类
    /// </summary>
    public class MyShuffleHelper
    {
        /// <summary>
        /// 对输入列表进行 Fisher-Yates 洗牌，打乱元素顺序。
        /// 同时返回原始元素在打乱后列表中的新位置索引映射。
        /// </summary>
        /// <typeparam name="T">列表中元素的类型</typeparam>
        /// <param name="data">待打乱的原始列表（不会被修改）</param>
        /// <param name="mt">梅森旋转随机数生成器</param>
        /// <returns>
        /// 元组：
        /// - <c>shuffled</c>: 打乱后的列表（顺序已随机）
        /// - <c>index_mapper</c>: 原始索引 i 在打乱后列表中对应的新位置 j，满足 shuffled[j] == data[i]
        /// </returns>
        /// <example>
        /// 示例：
        /// <code>
        /// var list = new List&lt;string&gt; { "A", "B", "C" };
        /// var (shuffled, mapper) = fisher_yates_shuffle(list, mt);
        /// // shuffled: 比如 [ "C", "A", "B" ]
        /// // mapper:   [1, 2, 0] 表示原始 list[0]="A" -> shuffled[1], list[1]="B" -> shuffled[2], list[2]="C" -> shuffled[0]
        /// </code>
        /// </example>
        public static (List<T> shuffled, int[] index_mapper) fisher_yates_shuffle<T>(IList<T> data, MersenneTwister mt)
        {
            List<T> shuffled = new(data);
            int n = data.Count;
            int[] index_mapper = Enumerable.Range(0, n).ToArray(); // 初始索引映射

            for (int i = n - 1; i > 0; i--)
            {
                int j = mt.Next(0, i + 1);
                // 交换数据
                (shuffled[i], shuffled[j]) = (shuffled[j], shuffled[i]);
                // 同步交换索引映射
                (index_mapper[i], index_mapper[j]) = (index_mapper[j], index_mapper[i]);
            }

            return (shuffled, index_mapper);
        }

        /// <summary>
        /// 对字典中的键值对顺序进行 Fisher-Yates 随机打乱，生成新的 Dictionary。
        /// 保证打乱后键值一一对应但顺序被随机重排（仅限遍历顺序）。
        /// </summary>
        /// <typeparam name="T">键的类型</typeparam>
        /// <typeparam name="V">值的类型</typeparam>
        /// <param name="data">原始字典</param>
        /// <param name="mt">梅森旋转随机数生成器</param>
        /// <returns>
        /// 一个新的 <see cref="Dictionary{T,V}"/>，包含原始键值对，但遍历顺序被打乱。
        /// 注意：Dictionary 本身无序，打乱的顺序仅体现在遍历时（如 foreach）。
        /// </returns>
        /// <remarks>
        /// ⚠️ 注意：若在.NET 5+/Core 环境下使用，Dictionary 插入顺序将影响遍历顺序；在旧版 .NET Framework 中则无法保证顺序。
        /// 若对打乱后的顺序有严格需求，建议使用 List&lt;(T key, V value)&gt; 替代返回结果。
        /// </remarks>
        /// <example>
        /// 示例：
        /// <code>
        /// var dict = new Dictionary&lt;string, int&gt; { ["A"] = 1, ["B"] = 2, ["C"] = 3 };
        /// var shuffled_dict = fisher_yates_shuffle(dict, mt);
        /// // shuffled_dict 顺序可能变为：{ ["B"]=2, ["C"]=3, ["A"]=1 }
        /// </code>
        /// </example>
        public static Dictionary<T, V> fisher_yates_shuffle<T, V>(Dictionary<T, V> data, MersenneTwister mt)
        {
            Dictionary<T, V> result = new();
            List<int> index = new(); //随机序号
            for (int i = 0; i < data.Count; i++)
                index.Add(i);
            index = fisher_yates_shuffle(index, mt).shuffled; //随机排序
            List<T> keys = new();
            List<V> values = new();
            foreach (var item in data)
            {
                keys.Add(item.Key);
                values.Add(item.Value);
            }

            for (int i = 0; i < index.Count; i++)
            {
                int rnd_index = index[i]; //随机序号
                result.Add(keys[rnd_index], values[rnd_index]);
            }

            return result;
        }
    }
}