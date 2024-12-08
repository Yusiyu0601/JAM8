using System.Data;
using System.Text;
using Easy.Common.Extensions;
using static JAM8.Utilities.ExcelHelper;

namespace JAM8.Utilities
{
    /// <summary>
    /// MyDataFrame.
    /// 自定义数据表，由多个数据列构成。
    /// 功能:
    /// 1.创建实例后，添加列名，不能重复
    /// 2.可以根据列名获取列号，同理可以根据列号获取列名
    /// 3.设计的越简单越好
    /// </summary>
    public class MyDataFrame
    {
        private MyDataFrame() { }

        #region 属性

        /// <summary>
        /// 序列名称
        /// </summary>
        public string[] series_names
        {
            get
            {
                return data.Select(a => a.series_name).ToArray();
            }
        }
        /// <summary>
        /// 数据(只读)
        /// </summary>
        public List<MySeries> data { get; internal set; }
        /// <summary>
        /// 记录总数
        /// </summary>
        public int N_Record
        {
            get
            {
                if (data.Count == 0)
                    return 0;
                else
                    return data[0].N_record;
            }
        }
        /// <summary>
        /// 序列总数
        /// </summary>
        public int N_Series
        {
            get
            {
                return series_names.Length;
            }
        }

        #endregion

        #region create

        /// <summary>
        /// 根据序列数量新建MyDataFrame对象，序列名称series{i}
        /// </summary>
        /// <param name="series_count"></param>
        /// <returns></returns>
        public static MyDataFrame create(int series_count)
        {
            string[] series_names = new string[series_count];
            for (int i = 1; i <= series_count; i++)
                series_names[i - 1] = $"series{i}";
            return create(series_names);
        }

        /// <summary>
        /// 根据序列名称新建MyDataFrame对象，如果名称有重复，默认修改相同列名。
        /// </summary>
        /// <param name="series_names"></param>
        /// <param name="remove_same_series_names">默认为false
        /// 当列名重复时，如果remove_same_series_names为true，则保留第1个列名，删除后续出现的列名；
        /// 如果remove_same_series_names为false，则将之后出现的列名添加后缀</param>
        /// <returns></returns>
        public static MyDataFrame create(IList<string> series_names, bool remove_same_series_names = false)
        {
            // 检查输入是否为 null 或空列表，若是，返回一个空的 MyDataFrame
            if (series_names == null || !series_names.Any())
            {
                throw new ArgumentException("series_names cannot be null or empty.", nameof(series_names));
            }

            // 如果不去重，处理列名，遇到重复的列名添加后缀
            if (!remove_same_series_names)
            {
                var seriesNameCount = new Dictionary<string, int>();
                series_names = series_names.Select(name =>
                {
                    // 如果列名已出现过，添加后缀
                    if (seriesNameCount.ContainsKey(name))
                    {
                        // 后缀递增
                        int count = seriesNameCount[name]++;
                        return $"{name}{count}";  // 为列名添加递增后缀
                    }
                    else
                    {
                        // 第一次出现该列名
                        seriesNameCount[name] = 1;
                        return name;
                    }
                }).ToList();
            }
            else
            {
                series_names = series_names.Distinct().ToList();
            }

            // 创建 MyDataFrame 并添加序列
            var df = new MyDataFrame { data = [] };
            series_names.ForEach(name => df.add_series(name)); // 假设 add_series 方法用于添加列

            return df;
        }

        /// <summary>
        /// 根据二维数组新建MyDataFrame对象
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        public static MyDataFrame create_from_array(IList<string> series_names, float[,] array)
        {
            var df = create(series_names);

            int N_Record = array.GetLength(0);
            int N_Series = array.GetLength(1);
            for (int iRecord = 0; iRecord < N_Record; iRecord++)
            {
                var record = df.new_record();
                for (int iSeries = 0; iSeries < N_Series; iSeries++)
                {
                    string series_name = df.series_names[iSeries];
                    record[series_name] = array[iRecord, iSeries];
                }
                df.add_record(record);
            }
            return df;
        }
        /// <summary>
        /// 根据二维数组新建MyDataFrame对象
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        public static MyDataFrame create_from_array(IList<string> series_names, double[,] array)
        {
            var df = create(series_names);

            int N_Record = array.GetLength(0);
            int N_Series = array.GetLength(1);
            for (int iRecord = 0; iRecord < N_Record; iRecord++)
            {
                var record = df.new_record();
                for (int iSeries = 0; iSeries < N_Series; iSeries++)
                {
                    string series_name = df.series_names[iSeries];
                    record[series_name] = array[iRecord, iSeries];
                }
                df.add_record(record);
            }
            return df;
        }

        /// <summary>
        /// 根据DataTable新建MyDataFrame对象
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static MyDataFrame create_from_dataTable(DataTable dt)
        {
            if (dt == null)
                return null;
            List<string> series_names = new();
            foreach (DataColumn Column in dt.Columns)
                series_names.Add(Column.ColumnName);
            MyDataFrame df = create(series_names.ToArray());

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                MyRecord record = df.new_record();//新建1行
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    string series_name = df.series_names[j];
                    record[series_name] = dt.Rows[i][j];
                }
                df.add_record(record);
            }
            return df;
        }

        /// <summary>
        /// 根据单个序列的数组新建MyDataFrame对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="series_name">序列名称</param>
        /// <param name="series_array">序列数组</param>
        /// <returns></returns>
        public static MyDataFrame create_from_one_series<T>(string series_name, T[] series_array)
        {
            MyDataFrame df = create(new string[] { series_name });
            for (int i = 0; i < series_array.Length; i++)
            {
                var record = df.new_record();
                record[series_name] = series_array[i];
                df.add_record(record);
            }
            return df;
        }

        /// <summary>
        /// 根据多个不重名序列的数组新建MyDataFrame对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="series"></param>
        /// <returns></returns>
        public static MyDataFrame create_from_multiple_series<T>(Dictionary<string, T[]> series_dict)
        {
            int count_max = series_dict.Max(a => a.Value.Length);//所有序列，其中记录数量的最大值
            MyDataFrame df = create(series_dict.Select(a => a.Key).ToArray());

            for (int i = 0; i < count_max; i++)
            {
                var record = df.new_record();

                foreach (var series_name in df.series_names)
                {
                    record[series_name] = series_dict[series_name][i];
                }

                df.add_record(record);
            }

            return df;
        }

        /// <summary>
        /// 根据已有df创建新的df，根据new_series_names需要添加新series。
        /// 注意：不添加重名的series_name
        /// </summary>
        /// <param name="df"></param>
        /// <param name="new_series_names"></param>
        /// <returns></returns>
        public static MyDataFrame create_from_mydf(MyDataFrame df, string[] new_series_names)
        {
            List<string> series_names = new(df.series_names);
            series_names.AddRange(new_series_names);
            MyDataFrame new_df = create([.. series_names], true);//删除重名的series_name
            for (int iRecord = 0; iRecord < df.N_Record; iRecord++)
            {
                var record = new_df.new_record();
                foreach (var series_name in df.series_names)
                {
                    record[series_name] = df[iRecord, series_name];
                }
                new_df.add_record(record);
            }
            return new_df;
        }

        /// <summary>
        /// 根据已有df创建新的df，根据new_series_names需要添加新series。
        /// 注意：不添加重名的series_name
        /// </summary>
        /// <param name="df"></param>
        /// <param name="new_series_name"></param>
        /// <param name="to_first"></param>
        /// <returns></returns>
        public static MyDataFrame create_from_mydf(MyDataFrame df, string new_series_name, bool to_first = false)
        {
            List<string> series_names = new();
            if (to_first)//将新列名排在第1位置
            {
                series_names.Add(new_series_name);
                series_names.AddRange(df.series_names);
            }
            else
            {
                series_names.AddRange(df.series_names);
                series_names.Add(new_series_name);
            }

            MyDataFrame new_df = create(series_names.ToArray(), true);//删除重名的series_name
            for (int iRecord = 0; iRecord < df.N_Record; iRecord++)
            {
                var record = new_df.new_record();
                foreach (var series_name in df.series_names)
                {
                    record[series_name] = df[iRecord, series_name];
                }
                new_df.add_record(record);
            }
            return new_df;
        }

        #endregion

        #region this索引器

        /// <summary>
        /// 索引器
        /// </summary>
        /// <param name="idx_record">记录序号</param>
        /// <param name="idx_series">序列序号</param>
        /// <returns></returns>
        public object this[int record_idx, int series_idx]
        {
            get
            {
                return data[series_idx][record_idx];
            }
            set
            {
                data[series_idx][record_idx] = value;
            }
        }

        /// <summary>
        /// 索引器
        /// </summary>
        /// <param name="idx_record">记录序号</param>
        /// <param name="series_name">序列名称</param>
        /// <returns></returns>
        public object this[int record_idx, string series_name]
        {
            get
            {
                return data[index_of_series(series_name)][record_idx];
            }
            set
            {
                data[index_of_series(series_name)][record_idx] = value;
            }
        }

        #endregion

        /// <summary>
        /// 根据列名获取对应的索引，如果不存在返回-1
        /// </summary>
        /// <param name="ColumnName">列名</param>
        /// <returns></returns>
        public int index_of_series(string series_name)
        {
            return series_names.IndexOf(series_name);
        }

        #region get record

        /// <summary>
        /// 获取记录
        /// </summary>
        /// <param name="iRecord"></param>
        /// <returns></returns>
        public MyRecord get_record(int record_idx)
        {
            MyRecord row = new();
            //提取某行的所有列，构成MyRow
            foreach (var column_name in series_names)
            {
                row.Add(column_name, this[record_idx, column_name]);
            }
            return row;
        }

        #endregion

        #region get series

        /// <summary>
        /// 获取Series
        /// </summary>
        /// <param name="iSeries"></param>
        /// <returns></returns>
        public MySeries get_series(int series_idx)
        {
            return data[series_idx];
        }
        /// <summary>
        /// 获取Series
        /// </summary>
        /// <param name="series_name"></param>
        /// <returns></returns>
        public MySeries get_series(string series_name)
        {
            int idx_series = index_of_series(series_name);
            if (idx_series == -1)
                return null;
            return data[idx_series];
        }
        /// <summary>
        /// 获取Series，并转换为T类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="series_name"></param>
        /// <returns></returns>
        public T[] get_series<T>(int series_idx)
        {
            var series = get_series(series_idx);
            T[] result = new T[series.N_record];
            for (int i = 0; i < series.N_record; i++)
                result[i] = (T)Convert.ChangeType(series[i], typeof(T));
            return result;
        }
        /// <summary>
        /// 获取Series，并转换为T类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="series_name"></param>
        /// <returns></returns>
        public T[] get_series<T>(string series_name)
        {
            int idx_series = index_of_series(series_name);
            return get_series<T>(idx_series);
        }

        #endregion

        //添加列对象，如果有重复，返回false
        bool add_series(string series_name)
        {
            if (series_name == string.Empty)
                return false;
            if (series_names.Contains(series_name))
                return false;

            data.Add(MySeries.create(series_name));

            return true;
        }

        #region 添加记录

        /// <summary>
        /// (基于MyDataFrame的列结构)创建1个空记录
        /// </summary>
        public MyRecord new_record()
        {
            MyRecord record = new();
            foreach (var series_name in series_names)
            {
                record.Add(series_name, null);
            }
            return record;
        }
        /// <summary>
        /// (基于MyDataFrame的列结构)创建1个空记录，并使用输入参数充填record
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public MyRecord new_record(object[] data)
        {
            MyRecord record = new();
            int flag = -1;
            foreach (var series_name in series_names)
            {
                flag++;
                record.Add(series_name, data[flag]);
            }
            return record;
        }
        public MyRecord new_record(IList<object> data)
        {
            MyRecord record = new();
            int flag = -1;
            foreach (var series_name in series_names)
            {
                flag++;
                record.Add(series_name, data[flag]);
            }
            return record;
        }

        /// <summary>
        /// 添加记录
        /// </summary>
        public void add_record(MyRecord record)
        {
            foreach (var (series_name, value) in record)
                get_series(series_name).add(value);
        }
        /// <summary>
        /// 添加记录，列数必须相同
        /// </summary>
        /// <param name="record"></param>
        /// <returns></returns>
        public bool add_record(object[] record)
        {
            if (record.Length != N_Series)
                return false;

            for (int i = 0; i < N_Series; i++)
            {
                string series_name = series_names[i];
                get_series(series_name).add(record[i]);
            }
            return true;
        }

        #endregion

        #region copy 

        /// <summary>
        /// 从df中复制指定列的列数据到本df，前提条件是本df与目标df的记录数量相同，列名在两个df均存在。
        /// </summary>
        /// <param name="df"></param>
        /// <param name="series_name"></param>
        /// <returns></returns>
        public bool copy_series_from(MyDataFrame df, string series_name)
        {
            if (N_Record != df.N_Record)
                return false;
            if (index_of_series(series_name) < 0)
                return false;
            if (df.index_of_series(series_name) < 0)
                return false;

            for (int iRecord = 0; iRecord < N_Record; iRecord++)
            {
                this[iRecord, series_name] = df[iRecord, series_name];
            }
            return true;
        }
        /// <summary>
        /// 从df中复制指定列的列数据到本df，前提条件是本df与目标df的记录数量相同，列名在两个df均存在。
        /// </summary>
        /// <param name="df"></param>
        /// <param name="series_names"></param>
        /// <returns></returns>
        public bool copy_series_from(MyDataFrame df, string[] series_names)
        {
            if (N_Record != df.N_Record)//检查行数相同
                return false;
            foreach (var series_name in series_names)//检查列名是否均存在
            {
                if (index_of_series(series_name) < 0)
                    return false;
                if (df.index_of_series(series_name) < 0)
                    return false;
            }

            for (int iRecord = 0; iRecord < N_Record; iRecord++)
            {
                foreach (var series_name in series_names)
                {
                    this[iRecord, series_name] = df[iRecord, series_name];
                }
            }
            return true;
        }
        /// <summary>
        /// 将series_data复制到本df，前提条件是本df的记录数量与series_data的长度相同，列名在本df中存在。
        /// </summary>
        /// <param name="series_data"></param>
        /// <param name="series_name"></param>
        /// <returns></returns>
        public bool copy_series_from<T>(T[] series_data, string series_name)
        {
            if (N_Record != series_data.Length)
                return false;
            if (index_of_series(series_name) < 0)
                return false;

            for (int iRecord = 0; iRecord < N_Record; iRecord++)
            {
                this[iRecord, series_name] = series_data[iRecord];
            }
            return true;
        }

        /// <summary>
        /// 深度复制，包括数据
        /// </summary>
        /// <returns></returns>
        public MyDataFrame deep_clone()
        {
            MyDataFrame clone = new()
            {
                data = new()
            };
            for (int i = 0; i < N_Series; i++)
                clone.data.Add(get_series(i).deep_clone());
            return clone;
        }

        #endregion

        #region convert to other data strutures

        /// <summary>
        /// 转换为二维数组，前提要求数据项都是数值型
        /// </summary>
        public float[,] convert_to_float_2dArray(float null_value = -99.99f)
        {
            float[,] array = new float[N_Record, N_Series];

            for (int iRecord = 0; iRecord < N_Record; iRecord++)//遍历数据的所有行
            {
                for (int iSeries = 0; iSeries < N_Series; iSeries++)//赋值row
                {
                    float value = null_value;
                    float.TryParse(this[iRecord, iSeries].ToString(), out value);
                    array[iRecord, iSeries] = value;
                }
            }

            return array;
        }
        /// <summary>
        /// 转换为二维数组，前提要求数据项都是数值型
        /// </summary>
        public double[,] convert_to_double_2dArray(double null_value = -99.99d)
        {
            double[,] array = new double[N_Record, N_Series];

            for (int iRecord = 0; iRecord < N_Record; iRecord++)//遍历数据的所有行
            {
                for (int iSeries = 0; iSeries < N_Series; iSeries++)//赋值row
                {
                    double value = null_value;
                    double.TryParse(this[iRecord, iSeries].ToString(), out value);
                    array[iRecord, iSeries] = value;
                }
            }

            return array;
        }

        /// <summary>
        /// 转换为交错数组，前提要求数据项都是数值型
        /// </summary>
        /// <returns></returns>
        public float[][] convert_to_float_jagged_array(float null_value = -99.99f)
        {
            float[][] jagged_array = new float[N_Record][];

            for (int iRecord = 0; iRecord < N_Record; iRecord++)//遍历数据的所有行
            {
                List<float> record = new();
                foreach (var series_name in series_names)
                {
                    float value = null_value;
                    float.TryParse(this[iRecord, series_name].ToString(), out value);
                    record.Add(value);
                }
                jagged_array[iRecord] = record.ToArray();
            }
            return jagged_array;
        }
        /// <summary>
        /// 转换为交错数组，前提要求数据项都是数值型
        /// </summary>
        /// <returns></returns>
        public double[][] convert_to_double_jagged_array(float null_value = -99.99f)
        {
            double[][] jagged_array = new double[N_Record][];

            for (int iRecord = 0; iRecord < N_Record; iRecord++)//遍历数据的所有行
            {
                List<double> record = new();
                foreach (var series_name in series_names)
                {
                    float value = null_value;
                    float.TryParse(this[iRecord, series_name].ToString(), out value);
                    record.Add(value);
                }
                jagged_array[iRecord] = record.ToArray();
            }
            return jagged_array;
        }

        /// <summary>
        /// 转换为DataTable
        /// </summary>
        /// <returns>DataTable.</returns>
        public DataTable convert_to_dataTable()
        {
            DataTable dt = new();

            //添加数据表的列对象
            foreach (MySeries series in data)
                dt.Columns.Add(series.series_name);

            for (int r = 0; r < N_Record; r++)//遍历数据的所有行
            {
                DataRow row = dt.NewRow();//新建row
                for (int c = 0; c < N_Series; c++)//赋值row
                {
                    row[c] = this[r, c];
                }
                dt.Rows.Add(row);//把row添加到DataTable
            }

            return dt;
        }

        #endregion

        #region subset操作

        /// <summary>
        /// 获取部分记录record（行）
        /// </summary>
        /// <param name="iRecord_start"></param>
        /// <param name="iRecord_end"></param>
        /// <returns></returns>
        public MyDataFrame get_record_subset(int record_start_idx, int record_end_idx)
        {
            List<int> record_idxes = new();
            for (int i = record_start_idx; i <= record_end_idx; i++)
                record_idxes.Add(i);
            return get_record_subset(record_idxes);
        }

        /// <summary>
        /// 获取部分记录record（行）
        /// </summary>
        /// <param name="iRecords"></param>
        /// <returns></returns>
        public MyDataFrame get_record_subset(IList<int> record_idxes)
        {
            record_idxes = record_idxes.Distinct().ToList();//首先对行序去重复
            MyDataFrame subset = create(series_names);//新建1个新的空表
            foreach (var iRecord in record_idxes)
            {
                if (iRecord >= 0 && iRecord < N_Record)//行序必须有效
                {
                    subset.add_record(get_record(iRecord).deep_clone());//提取行数据，添加到新表 
                }
            }
            return subset;
        }

        /// <summary>
        /// 获取部分series（列）
        /// </summary>
        /// <param name="iSeries"></param>
        /// <returns></returns>
        public MyDataFrame get_series_subset(IList<int> series_idxes)
        {
            MyDataFrame subset = new() { data = new() };
            foreach (var series_idx in series_idxes)
                subset.data.Add(get_series(series_idx).deep_clone());
            return subset;
        }

        /// <summary>
        /// 获取部分series（列），如果待获取的序列名称不存在，返回null
        /// </summary>
        /// <param name="series_names"></param>
        /// <returns></returns>
        public MyDataFrame get_series_subset(IList<string> series_names)
        {
            List<string> not_found = new();
            foreach (var series_name in series_names)//检查是否缺失列名
            {
                if (index_of_series(series_name) == -1)
                {
                    not_found.Add(series_name);
                }
            }
            if (not_found.Count != 0)//如果存在缺失列
            {
                return null;
            }
            else//所有列都有
            {
                MyDataFrame subset = new() { data = new() };
                foreach (var series_name in series_names)
                    subset.data.Add(get_series(series_name).deep_clone());
                return subset;
            }
        }

        #endregion

        #region txt读写

        /// <summary>
        /// 从Txt文件读取
        /// </summary>
        /// <param name="file_name"></param>
        /// <param name="split">分隔符</param>
        /// <param name="first_header">首行是列名</param>
        /// <returns></returns>
        public static MyDataFrame read_from_txt(string file_name, string split = "\t", bool first_header = true)
        {
            int N_Rows = (int)TxtHelper.get_row_count(file_name);//总行数
            Console.WriteLine();
            MyDataFrame df;
            if (first_header == true)
            {
                using FileStream fs = new(file_name, FileMode.Open, FileAccess.Read);
                using StreamReader sr = new(fs, Encoding.Default);
                string strLine = "";//记录每次读取的一行记录
                strLine = sr.ReadLine();//首行为列名
                var series_names = strLine.Split(split);//分隔符
                df = create(series_names);

                int progress = 0;
                while ((strLine = sr.ReadLine()) != null)//逐行读取txt中的数据
                {
                    progress++;
                    MyConsoleProgress.Print(progress, N_Rows, "读取txt格式，转换为DataFrame对象");
                    var data_list = strLine.Split(split);//tab分隔符
                    MyRecord record = df.new_record(data_list);
                    df.add_record(record);
                }
            }
            else
            {
                using FileStream fs = new(file_name, FileMode.Open, FileAccess.Read);
                using StreamReader sr = new(fs, Encoding.Default);
                string strLine = "";//记录每次读取的一行记录
                strLine = sr.ReadLine();//首行为列名
                var series_names = strLine.Split(split);//分隔符
                for (int i = 0; i < series_names.Length; i++)
                {
                    series_names[i] = $"series{i + 1}";
                }
                df = create(series_names);

                using FileStream fs1 = new(file_name, FileMode.Open, FileAccess.Read);
                using StreamReader sr1 = new(fs1, Encoding.Default);//新建1个文件读取流

                int progress = 0;
                while ((strLine = sr1.ReadLine()) != null)//逐行读取txt中的数据
                {
                    progress++;
                    MyConsoleProgress.Print(progress, N_Rows, "读取txt格式，转换为DataFrame对象");

                    var data_list = strLine.Split(split);//tab分隔符
                    MyRecord record = df.new_record(data_list);
                    df.add_record(record);
                }
            }
            return df;
        }
        /// <summary>
        /// 从Txt文件读取
        /// </summary>
        /// <param name="split"></param>
        /// <param name="first_header"></param>
        /// <returns></returns>
        public static MyDataFrame read_from_txt(string split = "\t", bool first_header = true)
        {
            OpenFileDialog ofd = new()
            {
                Filter = "(*.txt)|*.txt"
            };
            if (ofd.ShowDialog() != DialogResult.OK)
                return null;
            return read_from_txt(ofd.FileName, split, first_header);
        }
        /// <summary>
        /// 写入Txt文件
        /// </summary>
        /// <param name="df"></param>
        /// <param name="file_name"></param>
        /// <param name="split">分隔符</param>
        public static void write_to_txt(MyDataFrame df, string file_name, string split = "\t")
        {
            string dir = FileHelper.GetParentDirectory(file_name);
            if (false == Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            using StreamWriter sw = new(file_name, false, Encoding.UTF8);

            string first_row = string.Empty;
            for (int i = 0; i < df.N_Series; i++)
            {
                first_row += df.series_names[i];
                if (i == df.N_Series - 1)
                    continue;
                else
                    first_row += split;
            }
            sw.WriteLine(first_row);

            for (int r = 0; r < df.N_Record; r++)//遍历数据的所有行
            {
                string row = string.Empty;
                for (int c = 0; c < df.N_Series; c++)//赋值row
                {
                    row += df[r, c];
                    if (c == df.N_Series - 1)
                        continue;
                    else
                        row += split;
                }
                sw.WriteLine(row);
            }
        }
        /// <summary>
        /// 写入Txt文件
        /// </summary>
        /// <param name="df"></param>
        /// <param name="split"></param>
        public static void write_to_txt(MyDataFrame df, string split = "\t")
        {
            SaveFileDialog sfd = new()
            {
                Filter = "(*.txt)|*.txt"
            };
            if (sfd.ShowDialog() != DialogResult.OK)
                return;
            write_to_txt(df, sfd.FileName, split);
        }

        #endregion

        #region csv读写

        /// <summary>
        /// 从csv文件读取
        /// </summary>
        /// <param name="file_name"></param>
        /// <param name="first_header">首行是列名</param>
        /// <returns></returns>
        public static MyDataFrame read_from_csv(string file_name, bool first_header = true)
        {
            return read_from_txt(file_name, ",", first_header);
        }
        /// <summary>
        /// 从csv文件读取
        /// </summary>
        /// <param name="first_header"></param>
        /// <returns></returns>
        public static MyDataFrame read_from_csv(bool first_header = true)
        {
            OpenFileDialog ofd = new()
            {
                Filter = "(*.csv)|*.csv"
            };
            if (ofd.ShowDialog() != DialogResult.OK)
                return null;
            return read_from_csv(ofd.FileName, first_header);
        }
        /// <summary>
        /// 写入csv文件
        /// </summary>
        /// <param name="df"></param>
        /// <param name="file_name"></param>
        public static void write_to_csv(MyDataFrame df, string file_name)
        {
            write_to_txt(df, file_name, ",");
        }
        /// <summary>
        /// 写入csv文件
        /// </summary>
        /// <param name="df"></param>
        public static void write_to_csv(MyDataFrame df)
        {
            SaveFileDialog sfd = new()
            {
                Filter = "(*.csv)|*.csv"
            };
            if (sfd.ShowDialog() != DialogResult.OK)
                return;
            write_to_csv(df, sfd.FileName);
        }

        #endregion

        #region excel读写

        /// <summary>
        /// 从Excel文件读取
        /// </summary>
        /// <param name="file_name"></param>
        /// <param name="first_header"></param>
        /// <returns></returns>
        public static MyDataFrame read_from_excel(string file_name)
        {
            var dt = excel_to_dataTable(file_name);
            return create_from_dataTable(dt);
        }
        /// <summary>
        /// 从Excel文件流读取
        /// </summary>
        /// <param name="file_name"></param>
        /// <returns></returns>
        public static MyDataFrame read_from_excel(Stream stream, ExcelStreamType excel_stream_Type)
        {
            var dt = ExcelHelper.excel_to_dataTable(stream, excel_stream_Type);
            return create_from_dataTable(dt);
        }
        /// <summary>
        /// 从Excel文件读取
        /// </summary>
        /// <returns></returns>
        public static MyDataFrame read_from_excel()
        {
            return read_from_excel(FileDialogHelper.OpenExcel());
        }
        /// <summary>
        /// 写入Excel文件
        /// </summary>
        /// <param name="df"></param>
        /// <param name="file_name"></param>
        public static void write_to_excel(MyDataFrame df, string file_name)
        {
            string dir = FileHelper.GetParentDirectory(file_name);
            if (false == Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            var dt = df.convert_to_dataTable();
            ExcelHelper.dataTable_to_excel(file_name, dt);
        }
        /// <summary>
        /// 写入Excel文件
        /// </summary>
        /// <param name="df"></param>
        public static void write_to_excel(MyDataFrame df)
        {
            write_to_excel(df, FileDialogHelper.SaveExcel());
        }

        #endregion

        #region show data

        /// <summary>
        /// 打印
        /// </summary>
        /// <param name="N_first_lines"></param>
        /// <returns></returns>
        public string view_text(int N_first_lines = 15)
        {
            string text = "";
            text += ($"MyDataFrame[{N_Record}行,{N_Series}列]");
            text += (Environment.NewLine);
            for (int iSeries = 0; iSeries < N_Series; iSeries++)
            {
                text += string.Format("{0}", StringHelper.padRightEx(series_names[iSeries], 20));
            }
            text += (Environment.NewLine);
            N_first_lines = Math.Min(N_first_lines, N_Record);
            for (int iRecord = 0; iRecord < N_first_lines; iRecord++)
            {
                for (int iSeries = 0; iSeries < N_Series; iSeries++)
                {
                    string text_cell = this[iRecord, iSeries] == null ? "null" : this[iRecord, iSeries].ToString();
                    text += string.Format("{0}", StringHelper.padRightEx(text_cell, 20));
                }
                text += (Environment.NewLine);
            }
            return text;
        }
        /// <summary>
        /// 控制台展示
        /// </summary>
        public void show_console()
        {
            Console.Write(view_text());
        }

        /// <summary>
        /// 窗体展示
        /// </summary>
        /// <param name="is_showDialog">是否为模式窗体，使用ShowDialog方法显示窗体，当它在显示时，
        /// 如果作为激活窗体，则其它窗体不可用，只有将其关闭后，其它窗体才能恢复可用状态。</param>
        public void show_win(string title = "data frame", bool is_showDialog = true)
        {
            DataTable dt = convert_to_dataTable();
            DataTableHelper.show_win(dt, title, is_showDialog);
        }

        #endregion
    }
}
