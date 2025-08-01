#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;

namespace JAM8.Utilities
{
    /// <summary>
    /// 通用排序辅助类，支持对 Dictionary、KeyValuePair、元组、List、字符串等数据结构按值排序。
    /// 特性：
    /// - 支持 null 值安全排序（升序时 null 最小，降序时 null 最大）；
    /// - 支持传入自定义比较器；
    /// - 所有方法均返回排序后的新集合（不会修改原输入）。
    /// </summary>
    public static class MySortHelper
    {
        /// <summary>
        /// 构建一个支持 null 值的比较器（默认 null 最小）
        /// </summary>
        private static IComparer<T?> BuildNullSafeComparer<T>(IComparer<T>? baseComparer = null)
        {
            return Comparer<T?>.Create((a, b) =>
            {
                if (a is null && b is null) return 0;
                if (a is null) return -1;
                if (b is null) return 1;
                return (baseComparer ?? Comparer<T>.Default).Compare(a, b);
            });
        }

        /// <summary>
        /// 对 Dictionary<TKey, TValue?> 按值排序，返回一个新字典（顺序即为排序后顺序）。
        /// 默认 null 值最小，支持升序/降序选择。
        /// </summary>
        /// <typeparam name="TKey">字典键类型</typeparam>
        /// <typeparam name="TValue">值类型，可为 null</typeparam>
        /// <param name="dict">输入字典</param>
        /// <param name="descending">是否降序（默认 false，即升序）</param>
        /// <param name="comparer">自定义值比较器，可选</param>
        /// <returns>排序后的新字典</returns>
        /// <example>
        /// var dict = new Dictionary&lt;string, int?&gt; { ["a"] = 5, ["b"] = null, ["c"] = 3 };
        /// var sorted = MySortHelper.sort_dict_by_value(dict);
        /// // 结果：{ ["b"]=null, ["c"]=3, ["a"]=5 }
        /// </example>
        public static Dictionary<TKey, TValue?> sort_dict_by_value<TKey, TValue>(
            Dictionary<TKey, TValue?> dict,
            bool descending = false,
            IComparer<TValue?>? comparer = null)
        {
            comparer ??= BuildNullSafeComparer<TValue>();
            var kvList = dict.ToList();
            kvList.Sort((a, b) => descending ? -comparer.Compare(a.Value, b.Value) : comparer.Compare(a.Value, b.Value));
            return kvList.ToDictionary(p => p.Key, p => p.Value);
        }

        /// <summary>
        /// 对 KeyValuePair 列表按 Value 值排序。
        /// </summary>
        /// <typeparam name="TKey">键类型</typeparam>
        /// <typeparam name="TValue">值类型（可为 null）</typeparam>
        /// <param name="list">KVP 列表</param>
        /// <param name="descending">是否降序</param>
        /// <param name="comparer">可选值比较器</param>
        /// <returns>排序后的 KVP 列表</returns>
        /// <example>
        /// var list = new List&lt;KeyValuePair&lt;string, int?&gt;&gt;
        /// {
        ///     new("X", null),
        ///     new("Y", 2),
        ///     new("Z", 1)
        /// };
        /// var sorted = MySortHelper.sort_kvp_list_by_value(list); // X, Z, Y
        /// </example>
        public static List<KeyValuePair<TKey, TValue?>> sort_kvp_list_by_value<TKey, TValue>(
            List<KeyValuePair<TKey, TValue?>> list,
            bool descending = false,
            IComparer<TValue?>? comparer = null)
        {
            comparer ??= BuildNullSafeComparer<TValue>();
            var copy = new List<KeyValuePair<TKey, TValue?>>(list);
            copy.Sort((a, b) => descending ? -comparer.Compare(a.Value, b.Value) : comparer.Compare(a.Value, b.Value));
            return copy;
        }

        /// <summary>
        /// 对 List&lt;(TKey, TValue)&gt; 的元组列表，按 Item2 排序。
        /// </summary>
        /// <typeparam name="TKey">元组第一项类型</typeparam>
        /// <typeparam name="TValue">元组第二项类型（可为 null）</typeparam>
        /// <param name="list">元组列表</param>
        /// <param name="descending">是否降序</param>
        /// <param name="comparer">可选比较器</param>
        /// <returns>排序后的元组列表</returns>
        /// <example>
        /// var list = new List&lt;(string, double?)&gt; { ("a", 2.3), ("b", null), ("c", 1.1) };
        /// var sorted = MySortHelper.sort_tuple_list_by_item2(list); // b, c, a
        /// </example>
        public static List<(TKey, TValue?)> sort_tuple_list_by_item2<TKey, TValue>(
            List<(TKey, TValue?)> list,
            bool descending = false,
            IComparer<TValue?>? comparer = null)
        {
            comparer ??= BuildNullSafeComparer<TValue>();
            var copy = new List<(TKey, TValue?)>(list);
            copy.Sort((a, b) => descending ? -comparer.Compare(a.Item2, b.Item2) : comparer.Compare(a.Item2, b.Item2));
            return copy;
        }

        /// <summary>
        /// 对 List&lt;T&gt; 进行排序。支持 null 值，默认 null 最小。
        /// </summary>
        /// <typeparam name="T">元素类型</typeparam>
        /// <param name="list">输入列表（不修改原列表）</param>
        /// <param name="descending">是否降序</param>
        /// <param name="comparer">可选自定义比较器</param>
        /// <returns>排序后的新列表</returns>
        /// <example>
        /// var list = new List&lt;int?&gt; { 10, null, 3 };
        /// var sorted = MySortHelper.sort_list(list); // null, 3, 10
        /// </example>
        public static List<T?> sort_list<T>(
            List<T?> list,
            bool descending = false,
            IComparer<T?>? comparer = null)
        {
            comparer ??= BuildNullSafeComparer<T>();
            var copy = new List<T?>(list);
            copy.Sort((a, b) => descending ? -comparer.Compare(a, b) : comparer.Compare(a, b));
            return copy;
        }

        /// <summary>
        /// 对字符串集合排序，支持 null，默认按 Ordinal 字典序。
        /// </summary>
        /// <param name="strings">字符串序列</param>
        /// <param name="descending">是否降序</param>
        /// <param name="comparer">可选字符串比较器（默认使用 Ordinal）</param>
        /// <returns>排序后的字符串列表</returns>
        /// <example>
        /// var arr = new[] { "dog", "apple", null };
        /// var sorted = MySortHelper.sort_strings(arr); // null, apple, dog
        /// </example>
        public static List<string?> sort_strings(
            IEnumerable<string?> strings,
            bool descending = false,
            IComparer<string?>? comparer = null)
        {
            comparer ??= BuildNullSafeComparer<string>(StringComparer.Ordinal);
            var list = strings.ToList();
            list.Sort((a, b) => descending ? -comparer.Compare(a, b) : comparer.Compare(a, b));
            return list;
        }
    }
}
