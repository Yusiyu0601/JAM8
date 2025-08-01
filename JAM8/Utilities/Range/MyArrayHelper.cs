#nullable enable

using Easy.Common.Extensions;

namespace JAM8.Utilities
{
    /// <summary>
    /// 数组(列表)助手
    /// </summary>
    public class MyArrayHelper
    {
        /// <summary>
        /// 通用数组打印函数，支持一维、二维、三维数组输出到控制台。
        /// 支持浮点格式控制。
        /// 格式字符串含义
        /// "G"	    常规格式（默认格式，保留必要位数）
        /// "F2"	固定小数点格式，保留 2 位小数
        /// "F3"	保留 3 位小数
        /// "E"	    科学计数法（如 1.23E+003）
        /// "N0"	带千分位的整数（如 1,234）
        /// "P1"	百分比格式（如 12.3%）
        /// </summary>
        /// <typeparam name="T">数组元素类型</typeparam>
        /// <param name="array">支持 1D, 2D, 3D 数组</param>
        /// <param name="label">可选标签名称</param>
        /// <param name="floatFormat">浮点输出格式（例如 "F3" 保留三位小数）</param>
        /// <param name="printTips">是否输出提示信息</param>
        public static void print<T>(Array array, string label = "", string floatFormat = "G", bool printTips = true)
        {
            if (array == null)
            {
                Console.WriteLine("数组为空！");
                return;
            }

            int rank = array.Rank;

            if (printTips)
            {
                Console.WriteLine();
                Console.WriteLine($"--- 打印{rank}维数组{(string.IsNullOrEmpty(label) ? "" : $"：{label}")} ---");
            }

            if (rank == 1)
            {
                int length = array.GetLength(0);
                for (int i = 0; i < length; i++)
                {
                    print(array.GetValue(i), floatFormat);
                    Console.Write("\t");
                }

                Console.WriteLine();
            }
            else if (rank == 2)
            {
                int rows = array.GetLength(0);
                int cols = array.GetLength(1);
                for (int i = 0; i < rows; i++)
                {
                    for (int j = 0; j < cols; j++)
                    {
                        print(array.GetValue(i, j), floatFormat);
                        Console.Write("\t");
                    }

                    Console.WriteLine();
                }
            }
            else if (rank == 3)
            {
                int dim0 = array.GetLength(0);
                int dim1 = array.GetLength(1);
                int dim2 = array.GetLength(2);

                for (int i = 0; i < dim0; i++)
                {
                    Console.WriteLine($"\n>>> 第 {i} 层 (slice in dim0 = {i})");
                    for (int j = 0; j < dim1; j++)
                    {
                        for (int k = 0; k < dim2; k++)
                        {
                            print(array.GetValue(i, j, k), floatFormat);
                            Console.Write("\t");
                        }

                        Console.WriteLine();
                    }
                }
            }
            else
            {
                Console.WriteLine("暂不支持4维或以上数组的打印。");
            }
        }

        /// <summary>
        /// 按指定格式打印值（主要用于浮点数格式化）
        /// </summary>
        static void print(object? value, string floatFormat)
        {
            if (value == null)
            {
                Console.Write("null");
                return;
            }

            if (value is double d)
                Console.Write(d.ToString(floatFormat));
            else if (value is float f)
                Console.Write(f.ToString(floatFormat));
            else
                Console.Write(value);
        }

        /// <summary>
        /// 统计任意维度数组中每个值的出现次数，支持值类型、引用类型和 null 值处理。
        /// 返回一个包含 (值, 频数) 的列表，值可为 null。
        /// </summary>
        /// <typeparam name="T">数组元素类型（可为值类型、引用类型、Nullable 类型）</typeparam>
        /// <param name="array">输入数组（支持任意维度，如 1D、2D、3D 等）</param>
        /// <param name="keep_null">是否将 null 元素计入频数统计</param>
        /// <returns>列表，每项为 (值, 频数)，值可能为 null</returns>
        public static List<(T? value, int count)> count_frequency<T>(Array array, bool keep_null = true)
        {
            // 使用字符串作为字典键，避免直接使用 T?（可能是 null）作为键带来的问题
            // Dictionary<T?, int> 在值类型场景下不能接收 null，且 T 的哈希逻辑不可控
            var dict = new Dictionary<string, (T? value, int count)>();

            // 遍历数组中所有元素（支持任意维度数组，因为 Array 实现了 IEnumerable）
            foreach (var obj in array)
            {
                // === 情况一：元素为 null ===
                if (obj == null)
                {
                    // 如果不保留 null，则跳过此元素
                    if (!keep_null) continue;

                    // 使用固定字符串 "<null>" 作为 null 的唯一标识键
                    const string nullKey = "<null>";

                    // 如果字典中还没有这个键，初始化为 (null, 0)
                    if (!dict.ContainsKey(nullKey))
                        dict[nullKey] = (default, 0); // default 表示 null

                    // 将对应频数加一
                    dict[nullKey] = (default, dict[nullKey].count + 1);
                }
                else
                {
                    // === 情况二：元素不为 null ===

                    // 强制将 object 转为目标类型 T
                    T item = (T)obj;

                    // 将 item 转为字符串作为键（避免直接用 item 作为键）
                    // ToString() 可能返回 null，因此用 ?? 替代为 "<unknown>"
                    string key = item?.ToString() ?? "<unknown>";

                    // 初始化该 key 对应的频数
                    if (!dict.ContainsKey(key))
                        dict[key] = (item, 0);

                    // 累加频数
                    dict[key] = (item, dict[key].count + 1);
                }
            }

            // 返回字典中所有的值（即 (value, count) 列表）
            return dict.Values.ToList();
        }

        /// <summary>
        /// 查找数组中的所有并列众数（即出现次数最多的值，可有多个）。
        /// 支持任意维度数组（如 1D、2D、3D），值类型、引用类型、Nullable 和 null 值。
        /// </summary>
        /// <typeparam name="T">数组元素类型（支持任意类型）</typeparam>
        /// <param name="array">输入数组（System.Array，可为任意维度）</param>
        /// <param name="keep_null">是否将 null 值参与众数统计</param>
        /// <returns>
        /// 一个元组：
        /// - modes：所有频数最大的值列表（可能含多个并列众数）
        /// - count：众数的频数（即最大出现次数）
        /// 若数组为空或所有元素被跳过，返回空列表和 0。
        /// </returns>
        public static (List<T?> modes, int count) find_all_modes<T>(Array array, bool keep_null = true)
        {
            // 调用频数统计函数，获取所有元素及其对应频数
            List<(T? value, int count)> freqList = count_frequency<T>(array, keep_null);

            // 如果数组为空或所有元素都被跳过（例如都是 null 且 keepNull 为 false），直接返回空结果
            if (freqList.Count == 0)
                return (new List<T?>(), 0);

            // 找出最大频数，即众数的频数
            int max = freqList.Max(x => x.count);

            // 筛选出所有频数等于最大值的元素，即为并列众数
            List<T?> modes = freqList
                .Where(x => x.count == max)
                .Select(x => x.value)
                .ToList();

            return (modes, max);
        }
    }
}