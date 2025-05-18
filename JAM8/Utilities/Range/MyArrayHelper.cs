using Easy.Common.Extensions;

namespace JAM8.Utilities
{
    /// <summary>
    /// 数组(列表)助手
    /// </summary>
    public class MyArrayHelper
    {
        #region 1d Array functions

        #region 合并只要有交集的所有集合

        /// <summary>
        /// 合并只要有交集的所有集合
        /// </summary>
        /// <param name="lists"></param>
        /// <returns></returns>
        public static List<List<double>> UnionArraysHaveIntersection(List<List<double>> lists)
        {
            //参考方法: https://www.cnblogs.com/Iconnector/p/10320579.html

            List<double> all = new(); //所有的集合数据					
            HashSet<double> repeated = new(); //得到没有重复的hashset				
            foreach (List<double> item in lists)
            {
                foreach (double index in item)
                {
                    if (all.Contains(index))
                        repeated.Add(index); //得到所有重复的集合的元素									
                    all.Add(index); //得到所有的集合的元素									
                }
            }


            foreach (var setkey in repeated) //循环重复的值					
            {
                List<double> templist = null; //临时						
                List<List<double>> removelist = new();
                foreach (var item in lists) //循环					
                {
                    if (item.Contains(setkey))
                    {
                        if (templist == null)
                        {
                            templist = item;
                            removelist.Add(item);
                        }
                        else
                        {
                            removelist.Add(item);
                            templist = templist.Union(item).ToList();
                        }
                    }
                }

                foreach (var item in removelist)
                {
                    lists.Remove(item);
                }

                removelist.Clear();
                lists.Add(templist);
            }

            return lists;
        }

        #endregion

        #region 中位数计算

        /// <summary>
        /// 计算中位数
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        public static double Median(IList<double> array)
        {
            if (array == null)
            {
                throw new ArgumentNullException(nameof(array));
            }

            int N = array.Count;
            array = array.OrderBy(a => a).ToArray();
            int endIndex = N / 2;
            for (int i = 0; i <= endIndex; i++)
            {
                for (int j = 0; j < N - i - 1; j++)
                {
                    if (array[j + 1] < array[j])
                    {
                        (array[j], array[j + 1]) = (array[j + 1], array[j]);
                    }
                }
            }

            if (N % 2 != 0)
            {
                return array[endIndex];
            }

            return (array[endIndex - 1] + array[endIndex]) / 2;
        }

        #endregion

        #region 查找Max

        /// <summary>
        /// 从数组查找最大值及其索引(最大值，索引)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="arr"></param>
        /// <returns></returns>
        public static (T, int) FindMax<T>(T[] arr) where T : IComparable<T>
        {
            var i_Pos = 0;
            var value = arr[0];
            for (var i = 1; i < arr.Length; ++i)
            {
                var _value = arr[i];
                if (_value.CompareTo(value) > 0)
                {
                    value = _value;
                    i_Pos = i;
                }
            }

            return (value, i_Pos);
        }

        #endregion

        #region 查找众数

        /// <summary>
        /// 泛型版本：计算数组元素的众数及其统计量
        /// </summary>
        /// <typeparam name="T">元素类型，需实现IEquatable和IComparable</typeparam>
        /// <param name="array">输入数组</param>
        /// <param name="keepNull">是否保留null（仅对可空类型有效）</param>
        /// <returns>(众数，众数的频数)</returns>
        public static (T? mode, int count) FindMode<T>(IList<T?> array, bool keepNull = true)
            where T : struct, IEquatable<T>, IComparable<T>
        {
            var dict = new Dictionary<T?, int?>();
            foreach (var item in array)
            {
                if (!keepNull && item == null) continue;
                if (dict.ContainsKey(item))
                    dict[item]++;
                else
                    dict[item] = 1;
            }

            T? mode = default;
            int maxCount = 0;
            foreach (var kv in dict)
            {
                if (kv.Value > maxCount)
                {
                    maxCount = kv.Value ?? 0;
                    mode = kv.Key;
                }
            }

            return (mode, maxCount);
        }

        /// <summary>
        /// 计算数组元素的众数及其统计量
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <returns></returns>
        public static (int?, int) FindMode(int?[] array, bool KeepNull)
        {
            //去重复计算(数值，频数)
            var (distinct, counts) = MyDistinct.distinct<int>(array, KeepNull);
            //查找值最大的索引
            var (_, max_index) = FindMax(counts);
            return (distinct[max_index], counts[max_index]);
        }

        /// <summary>
        /// 计算数组元素的众数及其统计量
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <returns></returns>
        public static (int?, int) FindMode(List<int?> array, bool keep_null)
        {
            //去重复计算(数值，频数)
            var (distinct, counts) = MyDistinct.distinct(array, keep_null);
            //查找值最大的索引
            var (_, max_index) = FindMax(counts);
            return (distinct[max_index], counts[max_index]);
        }

        /// <summary>
        /// 计算数组元素的众数及其统计量
        /// </summary>
        /// <param name="array"></param>
        /// <returns>(众数，众数的频数)</returns>
        public static (int, int) FindMode(int[] array)
        {
            //去重复计算(数值，频数)
            var (distinct, counts) = MyDistinct.distinct(array);
            //查找值最大的索引
            var (_, max_index) = FindMax(counts);
            return (distinct[max_index], counts[max_index]);
        }

        /// <summary>
        /// 计算数组元素的众数及其统计量
        /// </summary>
        /// <param name="array"></param>
        /// <returns>(众数，众数的频数)</returns>
        public static (int, int) FindMode(List<int> array)
        {
            //去重复计算(数值，频数)
            var (distinct, counts) = MyDistinct.distinct(array);
            //查找值最大的索引
            var (_, max_index) = FindMax(counts);
            return (distinct[max_index], counts[max_index]);
        }

        /// <summary>
        /// 计算数组元素的众数及其统计量
        /// </summary>
        /// <param name="array"></param>
        /// <param name="KeepNull"></param>
        /// <returns></returns>
        public static (double?, int) FindMode(double?[] array, bool keep_null)
        {
            //去重复计算(数值，频数)
            var (distinct, counts) = MyDistinct.distinct(array, keep_null);
            //查找值最大的索引
            var (_, max_index) = FindMax(counts);
            return (distinct[max_index], counts[max_index]);
        }

        /// <summary>
        /// 计算数组元素的众数及其统计量
        /// </summary>
        /// <param name="array"></param>
        /// <param name="KeepNull"></param>
        /// <returns></returns>
        public static (double?, int) FindMode(List<double?> array, bool keep_null)
        {
            //去重复计算(数值，频数)
            var (distinct, counts) = MyDistinct.distinct(array, keep_null);
            //查找值最大的索引
            var (_, max_index) = FindMax(counts);
            return (distinct[max_index], counts[max_index]);
        }

        /// <summary>
        /// 计算数组元素的众数及其统计量
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        public static (double, int) FindMode(double[] array)
        {
            //去重复计算(数值，频数)
            var (distinct, counts) = MyDistinct.distinct(array);
            //查找值最大的索引
            var (_, max_index) = FindMax(counts);
            return (distinct[max_index], counts[max_index]);
        }

        /// <summary>
        /// 计算数组元素的众数及其统计量
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        public static (double, int) FindMode(List<double> array)
        {
            //去重复计算(数值，频数)
            var (distinct, counts) = MyDistinct.distinct(array);
            //查找值最大的索引
            var (_, max_index) = FindMax(counts);
            return (distinct[max_index], counts[max_index]);
        }

        #endregion

        #region Get Segments

        /// <summary>
        /// 从1个元素数组中提取连续的段Segment，返回每个段Segment的值、起始索引、终止索引
        /// </summary>
        /// <param name="Elements"></param>
        /// <returns></returns>
        public static Tuple<List<int?>, List<int>, List<int>, List<int>> GetSegments(List<int?> Elements)
        {
            int N = Elements.Count;
            Elements.Add(int.MaxValue); //增加1个最大值，便于处理最后一个值
            int idx = -1; //元素索引
            int flag = 0; //标记，从0开始，进入下一个Segment则增加1
            List<int> Segment_flag = new();
            List<int> Segment_idx1 = new();
            List<int> Segment_idx2 = new();
            List<int> Segment_length = new();
            List<int?> Segment_code = new();
            while (true)
            {
                idx++;
                if (idx == N)
                {
                    break;
                }

                if (Elements[idx] != Elements[idx + 1])
                {
                    //当前元素与下一个元素不相同，表明出现1个新Segment，标记flag加1
                    Segment_flag.Add(flag);
                    flag++;
                    continue;
                }

                Segment_flag.Add(flag);
            }

            //找出不重复的元素
            var distinct = MyDistinct.distinct(Segment_flag);
            for (int i = 0; i < distinct.Item2.Length; i++)
            {
                int segment_idx1 = Segment_flag.FindIndex(a => a == distinct.Item1[i]);
                int segment_idx2 = Segment_flag.FindLastIndex(a => a == distinct.Item1[i]);
                Segment_idx1.Add(segment_idx1);
                Segment_idx2.Add(segment_idx2);
                Segment_length.Add(segment_idx2 - segment_idx1 + 1);
                Segment_code.Add(Elements[segment_idx1]);
            }

            return new Tuple<List<int?>, List<int>, List<int>, List<int>>(Segment_code, Segment_idx1, Segment_idx2,
                Segment_length);
        }

        /// <summary>
        /// 从1个元素数组中提取连续的段Segment，返回每个段Segment的值、起始索引、终止索引
        /// </summary>
        /// <param name="Elements"></param>
        /// <returns></returns>
        public static Tuple<List<double?>, List<int>, List<int>, List<int>> GetSegments(List<double?> Elements)
        {
            int N = Elements.Count;
            Elements.Add(double.MaxValue); //增加1个最大值，便于处理最后一个值
            int idx = -1; //元素索引
            int flag = 0; //标记，从0开始，进入下一个Segment则增加1
            List<int> Segment_flag = new();
            List<int> Segment_idx1 = new();
            List<int> Segment_idx2 = new();
            List<int> Segment_length = new();
            List<double?> Segment_code = new();
            while (true)
            {
                idx++;
                if (idx == N)
                {
                    break;
                }

                if (Elements[idx] != Elements[idx + 1])
                {
                    //当前元素与下一个元素不相同，表明出现1个新Segment，标记flag加1
                    Segment_flag.Add(flag);
                    flag++;
                    continue;
                }

                Segment_flag.Add(flag);
            }

            //找出不重复的元素
            var distinct = MyDistinct.distinct(Segment_flag);
            for (int i = 0; i < distinct.Item2.Length; i++)
            {
                int segment_idx1 = Segment_flag.FindIndex(a => a == distinct.Item1[i]);
                int segment_idx2 = Segment_flag.FindLastIndex(a => a == distinct.Item1[i]);
                Segment_idx1.Add(segment_idx1);
                Segment_idx2.Add(segment_idx2);
                Segment_length.Add(segment_idx2 - segment_idx1 + 1);
                Segment_code.Add(Elements[segment_idx1]);
            }

            return new Tuple<List<double?>, List<int>, List<int>, List<int>>(Segment_code, Segment_idx1, Segment_idx2,
                Segment_length);
        }

        #endregion

        #region print

        /// <summary>
        /// 打印数组，Mode=0,横向打印，Mode=1 纵向打印
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <param name="Mode">横向打印 or 纵向打印</param>
        /// <param name="b_print_tips">是否打印提示 --- 1D Array内容 --- </param>
        public static void Print<T>(IEnumerable<T> array, int Mode = 0, bool b_print_tips = true)
        {
            if (b_print_tips)
                Console.WriteLine(@"--- 1D Array内容 ---");
            if (Mode == 0)
            {
                for (int n = 0; n < array.Count(); n++)
                {
                    Console.Write("{0:F3}", array.ElementAt(n));
                    Console.Write("\t");
                }

                Console.Write("\n");
            }

            if (Mode == 1)
            {
                for (int n = 0; n < array.Count(); n++)
                {
                    Console.Write("{0:F3}", array.ElementAt(n));
                    Console.Write("\n");
                }
            }
        }

        #endregion

        #region read from console

        public static double[] ReadFromConsole(int SplitCode = 0)
        {
            List<double> data = new();
            string[] temp = null!;
            if (SplitCode == 0)
            {
                temp = Console.ReadLine()!.Split(new char[] { ' ' });
            }

            if (SplitCode == 1)
            {
                temp = Console.ReadLine()!.Split(new char[] { ';' });
            }

            if (SplitCode == 2)
            {
                temp = Console.ReadLine()!.Split(new char[] { ',' });
            }

            for (int i = 0; i < temp.Length; i++)
            {
                data.Add(double.Parse(temp[i]));
            }

            return data.ToArray();
        }

        #endregion

        #endregion

        #region 2d Array functions

        /// <summary>
        /// 打印二维数组到控制台
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="matrix"></param>
        public static void print<T>(T[,] array)
        {
            Console.WriteLine(@"--- 2D Array内容 ---");
            for (int i = 0; i < array.GetLength(0); i++)
            {
                for (int j = 0; j < array.GetLength(1); j++)
                {
                    Console.Write("{0:F3}", array[i, j]);
                    Console.Write("\t");
                }

                Console.WriteLine(@"	");
            }
        }

        /// <summary>
        /// 从1个2d数组中提取部分列，组成1个新数组
        /// </summary>
        public static T[,] Get2dArray_Cols<T>(T[,] input, int[] iCols)
        {
            int rows = input.GetLength(0);
            int cols = iCols.Length;
            T[,] output_2dArray = new T[rows, cols];
            for (int col = 0; col < cols; col++)
            {
                for (int row = 0; row < rows; row++)
                {
                    output_2dArray[row, col] = input[row, iCols[col]];
                }
            }

            return output_2dArray;
        }

        /// <summary>
        /// 转换数据类型，string类型 -> double类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="input_2dArray"></param>
        /// <returns></returns>
        public static double[,] convert_to_double<T>(T[,] D2_array)
        {
            try
            {
                int rows = D2_array.GetLength(0);
                int cols = D2_array.GetLength(1);
                double[,] output = new double[rows, cols];

                for (int col = 0; col < cols; col++)
                    for (int row = 0; row < rows; row++)
                        output[row, col] = Convert.ToDouble(D2_array[row, col]);

                return output;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 转换数据类型，string类型 -> float类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="input_2dArray"></param>
        /// <returns></returns>
        public static float[,] convert_to_float<T>(T[,] D2_array)
        {
            try
            {
                int rows = D2_array.GetLength(0);
                int cols = D2_array.GetLength(1);
                float[,] output = new float[rows, cols];

                for (int col = 0; col < cols; col++)
                    for (int row = 0; row < rows; row++)
                        output[row, col] = Convert.ToSingle(D2_array[row, col]);

                return output;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 替换原数组中的元素值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="input_2dArray"></param>
        /// <param name="valueMapper">原值->新值 映射</param>
        /// <returns></returns>
        public static T[,] ReplaceElement_2dArray<T>(T[,] input_2dArray, Dictionary<T, T> valueMapper)
        {
            try
            {
                int rows = input_2dArray.GetLength(0);
                int cols = input_2dArray.GetLength(1);
                T[,] output_2dArray = new T[rows, cols];
                for (int col = 0; col < cols; col++)
                {
                    for (int row = 0; row < rows; row++)
                    {
                        T key = input_2dArray[row, col];
                        if (valueMapper.ContainsKey(key)) //如果Element是valueMapper中的key，则更改该Element值
                            output_2dArray[row, col] = valueMapper[key];
                        else //否则使用原值
                            output_2dArray[row, col] = key;
                    }
                }

                return output_2dArray;
            }
            catch
            {
                return null;
            }
        }

        #endregion
    }
}