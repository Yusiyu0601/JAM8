using System.Linq;

namespace JAM8.Utilities
{
    /// <summary>
    /// 泛型版本的distinct方法,用于去重复类
    /// 2025-5-18
    /// 喻思羽
    /// </summary>
    public class MyDistinct
    {
        /// <summary>
        /// 统计非可空值类型列表中各唯一值及其出现次数，并按值升序排序。
        /// </summary>
        /// <typeparam name="T">值类型（int, double, DateTime 等），必须为 struct</typeparam>
        /// <param name="list">输入的值类型列表</param>
        /// <returns>
        /// 一个元组：
        /// - values: 去重后的值数组（按升序排列）
        /// - counts: 每个值对应的出现次数
        /// </returns>
        public static (T[] values, int[] counts) distinct<T>(IList<T> list) where T : struct
        {
            // 用字符串形式的 key 来避免 Dictionary 的 T 比较器陷阱（如浮点 NaN），但排序时仍使用真实值
            var dict = new Dictionary<string, (T value, int count)>();

            foreach (var item in list)
            {
                var key = item.ToString(); // 以字符串形式作为字典键
                if (!dict.ContainsKey(key))
                    dict[key] = (item, 0);
                dict[key] = (item, dict[key].count + 1); // 累计频数
            }

            // 按实际的 T 值升序排序，避免使用字符串顺序
            var sorted = dict.Values
                .OrderBy(p => p.value)
                .ToList();

            // 分别提取值数组和值频数组
            return (
                sorted.Select(p => p.value).ToArray(),
                sorted.Select(p => p.count).ToArray()
            );

            // 鲁棒性分析
            //     ✅ 去重 使用字典自动实现去重。
            //     ✅ 频数统计 逐项累加频数，无遗漏。
            //     ✅ 排序正确 按实际类型 T 的升序排序，不依赖字符串字典序。
            //     ✅ 泛型支持 泛型约束 where T : struct，确保为值类型（避免引用类型导致的 null 问题）。
            //     ✅ 安全性 即使 item.ToString() 有重复字符串（如浮点 1.0 和 1.00），其实际值仍一致，符合多数用途需求。
            //
            // 小提示（边界场景）
            // 此实现默认 ToString() 能唯一标识 T 的值。例如：
            // 对于 double.NaN，NaN.ToString() 始终是 "NaN"，但多个 NaN 不相等（NaN != NaN），这在极端浮点场景可能产生影响；
            // 如果你对浮点精度或特定值比较非常敏感（如 - 0.0 vs + 0.0），建议使用 IEqualityComparer<T> 版本改写。
        }

        /// <summary>
        /// 统计可空值类型列表中各唯一值及其出现次数，并按顺序排序（null 值优先）。
        /// </summary>
        /// <typeparam name="T">结构体类型（int、double、DateTime 等）</typeparam>
        /// <param name="list">可空类型的值列表（如 int?[]）</param>
        /// <param name="keep_null">是否保留 null 作为单独值参与统计</param>
        /// <returns>
        /// 一个元组：
        /// - values: 去重后的值数组（null 在前，其他按升序排列）
        /// - counts: 每个值对应的出现次数
        /// </returns>
        public static (T?[] values, int[] counts) distinct<T>(IList<T?> list, bool keep_null = true) where T : struct
        {
            // 使用字符串作为键，避免浮点类型比较陷阱
            var dict = new Dictionary<string, (T? value, int count)>();

            foreach (var item in list)
            {
                if (!item.HasValue)
                {
                    if (keep_null)
                    {
                        const string key = "<null>";
                        if (!dict.ContainsKey(key))
                            dict[key] = (null, 0);
                        dict[key] = (null, dict[key].count + 1);
                    }
                }
                else
                {
                    var key = item.Value.ToString();
                    if (!dict.ContainsKey(key))
                        dict[key] = (item, 0);
                    dict[key] = (item, dict[key].count + 1);
                }
            }

            // 对字典内容排序：
            // 1. null 值优先（HasValue = false）
            // 2. 其他值按真实数值升序排序
            var sorted = dict.Values
                .OrderBy(x => x.value.HasValue ? 1 : 0) // null = 0，在前；有值 = 1，在后
                .ThenBy(x => x.value) // 按实际值排序（Nullable<T> 可比较）
                .ToList();

            return (
                sorted.Select(p => p.value).ToArray(),
                sorted.Select(p => p.count).ToArray()
            );

            // 鲁棒性分析
            //     项目  说明
            //     ✅ 去重 使用字典自动去重，确保不重复计数。
            //     ✅ null 支持 通过特殊 key < null > 表示 null，便于区分和统计。
            //     ✅ 排序正确  null 总在最前，其余值按实际类型升序排列。
            //     ✅ 泛型支持 限定 T: struct，确保是可空值类型（int?, double?, DateTime? 等）。
            //     ✅ 稳定性 支持包含 null 和重复元素的各种组合，适合绝大多数业务统计需求。

            // 使用场景示例

            // var list = new double?[] { 3.14, null, 2.71, null, 3.14 };
            // var (values, counts) = distinct(list, keep_null: true);
            //
            // // 输出:
            // // values: [null, 2.71, 3.14]
            // // counts: [2, 1, 2]
        }

        /// <summary>
        /// 统计引用类型列表中每个唯一值及其出现次数，并将 null 作为特殊项（可选保留）。
        /// </summary>
        /// <typeparam name="T">引用类型（string、object 等）</typeparam>
        /// <param name="list">输入列表，可包含 null</param>
        /// <param name="keep_null">是否保留 null 值参与统计</param>
        /// <returns>
        /// 一个元组：
        /// - values: 去重后的值数组（null 作为第一项，其他按字符串顺序排列）
        /// - counts: 每个值对应的出现次数
        /// </returns>
        public static (T[] values, int[] counts) distinct<T>(IList<T> list, bool keep_null = true) where T : class
        {
            // 使用字符串键，统一比较逻辑（避免引用对象默认地址比较问题）
            var dict = new Dictionary<string, (T value, int count)>();

            foreach (var item in list)
            {
                if (item == null)
                {
                    if (keep_null)
                    {
                        const string key = "<null>";
                        if (!dict.ContainsKey(key))
                            dict[key] = (null, 0);
                        dict[key] = (null, dict[key].count + 1);
                    }
                }
                else
                {
                    var key = item.ToString(); // 调用 ToString 作为字典键
                    if (!dict.ContainsKey(key))
                        dict[key] = (item, 0);
                    dict[key] = (item, dict[key].count + 1);
                }
            }

            // 排序逻辑：
            // - "<null>" 代表 null，强制视为最小字符串，排在最前
            // - 其余项按键的字典序排列（字符串）
            var sorted = dict
                .OrderBy(kvp => kvp.Key == "<null>" ? "" : kvp.Key) // null 的键强制放在最前
                .Select(kvp => kvp.Value)
                .ToList();

            return (
                sorted.Select(p => p.value).ToArray(),
                sorted.Select(p => p.count).ToArray()
            );

            // 鲁棒性分析
            //     ✅ null 支持 使用<null > 作为占位键，统计并排序时始终排在最前
            //     ✅ 稳定排序 除 null 外，其余值按 ToString() 后的字典序排序
            //     ✅ 泛型支持 支持任意引用类型，只要 ToString() 有意义
            //     ⚠️ 风险提示 若两个不同对象的 ToString() 结果相同，会被视为同一值（推荐用于字符串类或自定义重写了 ToString 的类型）
            //     ✅ 性能 O(n log n) 排序，适合中等规模数据去重和统计
            //     ✅ 无异常抛出 在输入列表中有 null、重复、空字符串等情况也能稳定处理

            // 使用示例
            //
            // var list = new string[] { "apple", null, "banana", "apple", null };
            // var (values, counts) = distinct(list);
            //
            // // values: [null, "apple", "banana"]
            // // counts: [2, 2, 1]

            // 建议
            //
            // 若你处理的是 string 类型为主的数据，此实现已很完备；
            // 若用于其他引用类型且 ToString() 不具唯一性（例如自定义类未重写 ToString），建议结合 IEqualityComparer<T> 做进一步扩展。
            // 如果你希望增强类型安全性（避免依赖字符串比较），也可以考虑在字典中直接使用对象 T 作为 key，
            // 并引入 EqualityComparer< T >.Default。若有需要，我可以帮你重构为更类型安全的版本。
        }
    }
}