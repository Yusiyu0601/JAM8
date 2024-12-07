using Easy.Common.Extensions;
using JAM8.Algorithms.Numerics;

namespace JAM8.Utilities
{
    /// <summary>
    /// 排序与抽样辅助类
    /// </summary>
    public class SortHelper
    {
        /// <summary>
        /// 生成一个不重复的整数随机序列，结果包含min和max
        /// </summary>
        /// <param name="min">闭区间</param>
        /// <param name="max">闭区间</param>
        /// <returns></returns>
        public static int[] Create_RandomNumbers_NotRepeat(int min, int max, Random rnd)
        {
            int count = max - min + 1;
            int[] array = new int[count];

            for (int i = 0; i < count; i++)
            {
                array[i] = min;
                min++;
            }

            //洗牌算法
            for (int i = 0; i < count; i++)
            {
                int temp = rnd.Next(count);
                (array[i], array[temp]) = (array[temp], array[i]);
            }

            return array;
        }

        /// <summary>
        /// 随机排序，返回 ( 排序后的列表 , 原始元素在新列表中的索引 )
        /// </summary>
        /// <param name="oldList"></param>
        /// <returns></returns>
        public static (List<T> sorted, int[] index_mapper) RandomSort<T>(IList<T> oldList, Random rnd)
        {
            //排序后的对象
            List<T> newList = new();

            int count = oldList.Count;
            int max = oldList.Count - 1, min = 0;
            //生成sortArray的随机索引
            int[] randomIndex = Create_RandomNumbers_NotRepeat(min, max, rnd);
            Dictionary<int, int> temp = new();
            for (int i = 0; i < count; i++)
            {
                int newIndex = randomIndex[i];
                T newItem = oldList[newIndex];
                newList.Add(newItem);
                temp.Add(newIndex, i);
            }
            //原始元素在新列表中的索引
            List<int> index_mapper = new();
            for (int i = 0; i < count; i++)
                index_mapper.Add(temp[i]);

            return (newList, index_mapper.ToArray());
        }

        /// <summary>
        /// Fisher-Yates 洗牌算法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <param name="mt"></param>
        /// <returns></returns>
        public static List<T> FisherYatesShuffle<T>(List<T> data, MersenneTwister mt)
        {
            // 复制原始数组，确保原始数组不被修改
            List<T> shuffled = new(data);

            // 从数组的最后一个元素开始
            for (int i = shuffled.Count - 1; i > 0; i--)
            {
                // 生成一个范围为 [0, i] 的随机索引
                int j = mt.Next(0, i + 1);  // 生成一个 [0, i] 范围内的随机数
                T temp = shuffled[i];
                shuffled[i] = shuffled[j];
                shuffled[j] = temp;  // 交换元素
            }

            // 返回打乱顺序的新数组
            return shuffled;
        }

        /// <summary>
        /// 随机排序
        /// </summary>
        /// <param name="oldList"></param>
        /// <returns></returns>
        public static Dictionary<T, V> RandomSort<T, V>(Dictionary<T, V> old_list, Random rnd)
        {
            Dictionary<T, V> result = new();
            List<int> index = new();//随机序号
            for (int i = 0; i < old_list.Count; i++)
                index.Add(i);
            index = RandomSort(index, rnd).sorted;//随机排序
            List<T> keys = new();
            List<V> values = new();
            foreach (var item in old_list)
            {
                keys.Add(item.Key);
                values.Add(item.Value);
            }
            for (int i = 0; i < index.Count; i++)
            {
                int rnd_index = index[i];//随机序号
                result.Add(keys[rnd_index], values[rnd_index]);
            }
            return result;
        }

        /// <summary>
        /// 随机抽样
        /// </summary>
        public static List<T> RandomSelect<T>(IList<T> list, int N, Random rnd)
        {
            if (list.Count < N)
                N = list.Count;
            List<T> randoms = RandomSort(list, rnd).sorted;
            List<T> result = new();
            for (int i = 0; i < N; i++)
            {
                result.Add(randoms[i]);
            }
            return result;
        }

        /// <summary>
        /// 随机抽样
        /// </summary>
        public static Dictionary<T, V> RandomSelect<T, V>(Dictionary<T, V> dict, int N, Random rnd)
        {
            if (dict.Count < N)
                N = dict.Count;
            Dictionary<T, V> result = new();
            Dictionary<T, V> randoms = RandomSort(dict, rnd);

            List<int> index = new();
            for (int i = 0; i < dict.Count; i++)
                index.Add(i);
            index = RandomSelect(index, N, rnd);

            int counter = 1;
            foreach (var item in randoms)
            {
                result.Add(item.Key, item.Value);

                if (counter == N)
                    break;
                counter++;
            }

            return result;
        }

        /// <summary>
        /// 根据value对字典排序
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dict"></param>
        /// <returns></returns>
        public static Dictionary<T, double> sorted_by_value<T>(Dictionary<T, double> dict)
        {
            Dictionary<T, double> dict_SortedByValue = dict.
                OrderBy(p => p.Value).
                ToDictionary(p => p.Key, o => o.Value);
            return dict_SortedByValue;
        }

        /// <summary>
        /// 根据value对字典排序
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dict"></param>
        /// <returns></returns>
        public static Dictionary<T, int> sorted_by_value<T>(Dictionary<T, int> dict)
        {
            Dictionary<T, int> dict_SortedByValue = dict.
                OrderBy(p => p.Value).
                ToDictionary(p => p.Key, o => o.Value);
            return dict_SortedByValue;
        }
    }
}
