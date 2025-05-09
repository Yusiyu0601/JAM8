namespace JAM8.Utilities
{
    /// <summary>
    /// 自定义去重复类
    /// </summary>
    public class MyDistinct
    {
        #region Distinct方法 (int int?类型)

        /// <summary>
        /// 统计值及其数量，第1个参数是唯一值集合，第2个参数是值的频数
        /// </summary>
        /// <param name="array"></param>
        /// <param name="KeepNull">是否保留Null（array里可能包含Null），默认保留Null</param>
        /// <returns></returns>
        public static (int?[], int[]) distinct(IList<int?> array, bool keep_null = true)
        {
            if (keep_null)
                return distinct_keep_null(array);//假如array里有Null，要求保留Null的情况
            else
                return distinct_without_keep_null(array);//假如array里有Null，要求删除Null的情况
        }
        //统计值及其数量，第1个参数是唯一值集合（升序排列），第2个参数是值的频数
        //( NOTE: 不保留Null )
        private static (int?[], int[]) distinct_without_keep_null(IList<int?> array)
        {
            List<int> temp = new();
            for (int i = 0; i < array.Count; i++)
            {
                if (array[i] != null)//先从array里删除所有Null
                {
                    temp.Add(array[i].Value);
                }
            }
            var result = distinct(temp);
            List<int?> keys = new();
            List<int> values = new();
            for (int i = 0; i < result.Item1.Length; i++)
            {
                keys.Add(result.Item1[i]);
                values.Add(result.Item2[i]);
            }
            return (keys.ToArray(), values.ToArray());
        }
        //统计值及其数量，第1个参数是唯一值集合（升序排列），第2个参数是值的频数
        //( NOTE: 保留Null )
        private static (int?[], int[]) distinct_keep_null(IList<int?> array)
        {
            Dictionary<int?, int> dict = new();
            var keys = array.Distinct().ToList();
            int counter_null = 0;
            foreach (var key in keys)
            {
                if (key == null)
                    continue;
                dict.Add(key, 0);//key如果是null ,引发异常
            }
            for (int i = 0; i < array.Count; i++)
            {
                int? value = array[i];
                if (null == value)
                    counter_null++;
                else
                    dict[value]++;
            }
            dict = dict.OrderBy(o => o.Key).ToDictionary(o => o.Key, p => p.Value);
            List<int?> item1 = new();
            List<int> item2 = new();
            if (counter_null != 0)//如果有null
            {
                item1.Add(null);
                item1.AddRange(dict.Keys);
                item2.Add(counter_null);
                item2.AddRange(dict.Values);
            }
            else//如果没有null
            {
                item1.AddRange(dict.Keys);
                item2.AddRange(dict.Values);
            }
            return (item1.ToArray(), item2.ToArray());
        }

        /// <summary>
        /// 统计值及其数量，第1个参数是唯一值集合（升序排列），第2个参数是值的频数
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        public static (int[], int[]) distinct(IList<int> array)
        {
            Dictionary<int, int> dict = new();
            var keys = array.Distinct().ToList();
            foreach (var key in keys)
            {
                dict.Add(key, 0);
            }
            for (int i = 0; i < array.Count; i++)
            {
                int value = array[i];
                dict[value]++;
            }
            dict = dict.OrderBy(o => o.Key).ToDictionary(o => o.Key, p => p.Value);
            return (dict.Keys.ToArray(), dict.Values.ToArray());
        }

        #endregion

        #region Distinct方法 (double double?类型)

        /// <summary>
        /// 统计值及其数量，第1个参数是唯一值集合，第2个参数是值的频数
        /// </summary>
        /// <param name="array"></param>
        /// <param name="KeepNull">是否保留Null（array里可能包含Null），默认保留Null</param>
        /// <returns></returns>
        public static (double?[], int[]) distinct(IList<double?> array, bool keep_null = true)
        {
            if (keep_null)
                return distinct_keep_null(array);//假如array里有Null，要求保留Null的情况
            else
                return distinct_without_keep_null(array);//假如array里有Null，要求删除Null的情况
        }
        //统计值及其数量，第1个参数是唯一值集合（升序排列），第2个参数是值的频数
        //( NOTE: 不保留Null )
        private static (double?[], int[]) distinct_without_keep_null(IList<double?> array)
        {
            List<double> temp = new();
            for (int i = 0; i < array.Count; i++)
            {
                if (array[i] != null)//先从array里删除所有Null
                {
                    temp.Add(array[i].Value);
                }
            }
            var result = distinct(temp);
            List<double?> keys = new();
            List<int> values = new();
            for (int i = 0; i < result.Item1.Length; i++)
            {
                keys.Add(result.Item1[i]);
                values.Add(result.Item2[i]);
            }
            return (keys.ToArray(), values.ToArray());
        }
        //统计值及其数量，第1个参数是唯一值集合（升序排列），第2个参数是值的频数
        //( NOTE: 保留Null )
        private static (double?[], int[]) distinct_keep_null(IList<double?> array)
        {
            Dictionary<double?, int> dict = new();
            var keys = array.Distinct().ToList();
            int counter_null = 0;
            foreach (var key in keys)
            {
                if (key == null)
                    continue;
                dict.Add(key, 0);//key如果是null ,引发异常
            }
            for (int i = 0; i < array.Count; i++)
            {
                double? value = array[i];
                if (null == value)
                    counter_null++;
                else
                    dict[value]++;
            }
            dict = dict.OrderBy(o => o.Key).ToDictionary(o => o.Key, p => p.Value);
            List<double?> item1 = new();
            List<int> item2 = new();
            if (counter_null != 0)//如果有null
            {
                item1.Add(null);
                item1.AddRange(dict.Keys);
                item2.Add(counter_null);
                item2.AddRange(dict.Values);
            }
            else//如果没有null
            {
                item1.AddRange(dict.Keys);
                item2.AddRange(dict.Values);
            }
            return (item1.ToArray(), item2.ToArray());
        }

        /// <summary>
        /// 计算取值的频数(升序排列)
        /// </summary>
        /// <param name="array"></param>
        /// <returns>(取值，频数)</returns>
        public static (double[], int[]) distinct(IList<double> array)
        {
            Dictionary<double, int> dict = new();
            var keys = array.Distinct().ToList();
            foreach (var key in keys)
            {
                dict.Add(key, 0);
            }
            for (int i = 0; i < array.Count; i++)
            {
                double value = array[i];
                dict[value]++;
            }
            dict = dict.OrderBy(o => o.Key).ToDictionary(o => o.Key, p => p.Value);
            return (dict.Keys.ToArray(), dict.Values.ToArray());
        }

        #endregion

        #region Distinct方法 (string string?类型)

        /// <summary>
        /// 统计值及其重复次数
        /// </summary>
        /// <param name="array"></param>
        /// <returns>N：可取值的总数 values：取值 counts：取值的重复次数</returns>
        public static (int N, string[] values, int[] counts) distinct(IList<string> list)
        {
            Dictionary<string, int> dict = new();
            var keys = list.Distinct().ToList();
            int counter_null = 0;
            foreach (var key in keys)
            {
                if (key == null)
                    continue;
                dict.Add(key, 0);//key如果是null ,引发异常
            }
            for (int i = 0; i < list.Count; i++)
            {
                string value = list[i];
                if (null == value)
                    counter_null++;
                else
                    dict[value]++;
            }
            dict = dict.OrderBy(o => o.Key).ToDictionary(o => o.Key, p => p.Value);
            List<string> item1 = new();
            List<int> item2 = new();
            if (counter_null != 0)//如果有null
            {
                item1.Add(null);
                item1.AddRange(dict.Keys);
                item2.Add(counter_null);
                item2.AddRange(dict.Values);
            }
            else//如果没有null
            {
                item1.AddRange(dict.Keys);
                item2.AddRange(dict.Values);
            }
            return (item1.Count, item1.ToArray(), item2.ToArray());
        }

        #endregion
    }
}
