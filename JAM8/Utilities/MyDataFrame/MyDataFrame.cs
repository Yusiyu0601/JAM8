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
        //私有构造函数
        private MyDataFrame() { }

        #region 属性

        /// <summary>
        /// 序列名称(只读)
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
        /// 记录总数(只读)
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
        /// 序列总数(只读)
        /// </summary>
        public int N_Series
        {
            get
            {
                return series_names.Length;
            }
        }

        #endregion

        #region create dataframe

        /// <summary>
        /// 根据序列数量新建 MyDataFrame 对象，序列名称为 series1, series2, ..., series{series_count}
        /// </summary>
        /// <param name="series_count">序列数量</param>
        /// <returns>新建的 MyDataFrame 对象</returns>
        public static MyDataFrame create(int series_count)
        {
            // 生成序列名称数组
            var series_names = Enumerable.Range(1, series_count)
                                          .Select(i => $"series{i}")
                                          .ToArray();

            // 调用已有的 create 方法以生成 MyDataFrame
            return create(series_names);
        }

        /// <summary>
        /// 根据序列名称新建 MyDataFrame 对象。如果列名有重复，默认修改相同列名，
        /// 可以选择保留第一个列名并删除其他重复列，或者将重复列名添加递增后缀。
        /// </summary>
        /// <param name="series_names">列名列表</param>
        /// <param name="remove_same_series_names">如果为 true，保留第一个出现的列名，删除后续出现的重复列名；如果为 false，给重复列名添加递增后缀。</param>
        /// <returns>创建的 MyDataFrame 对象</returns>
        public static MyDataFrame create(IList<string> series_names, bool remove_same_series_names = false)
        {
            // 检查输入是否为 null、空列表，或包含空字符串，若是，抛出异常
            if (series_names == null || !series_names.Any() || series_names.Any(name => string.IsNullOrEmpty(name)))
            {
                throw new ArgumentException("series_names cannot be null, empty, or contain empty strings.", nameof(series_names));
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
                        int count = seriesNameCount[name] += 1;
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
                // 去重，保留第一个出现的列名
                series_names = series_names.Distinct().ToList();
            }

            // 创建 MyDataFrame 并添加序列
            var df = new MyDataFrame { data = [] };// 初始化 data

            // 添加列并进行重复列名的检查
            foreach (var series_name in series_names)
            {
                // 添加新列到 MyDataFrame
                df.data.Add(MySeries.create(series_name));
            }

            return df;
        }

        /// <summary>
        /// 根据二维数组新建MyDataFrame对象
        /// </summary>
        /// <typeparam name="T">数组元素类型，例如 float 或 double</typeparam>
        /// <param name="series_names">列名列表</param>
        /// <param name="array">二维数组</param>
        /// <returns>新建的 MyDataFrame 对象</returns>
        /// <exception cref="ArgumentException">当列名数量与数组列数不匹配时抛出</exception>
        public static MyDataFrame create_from_array<T>(IList<string> series_names, T[,] array) where T : struct
        {
            // 检查列名数量是否与数组列数一致
            int N_Series = array.GetLength(1);
            if (series_names.Count != N_Series)
            {
                throw new ArgumentException($"列名数量 ({series_names.Count}) 与数组的列数 ({N_Series}) 不匹配。");
            }

            var df = create(series_names);

            int N_Record = array.GetLength(0);
            for (int iRecord = 0; iRecord < N_Record; iRecord++)
            {
                var record = df.new_record();
                for (int iSeries = 0; iSeries < N_Series; iSeries++)
                {
                    string seriesName = df.series_names[iSeries];
                    record[seriesName] = array[iRecord, iSeries];
                }
                df.add_record(record);
            }
            return df;
        }

        /// <summary>
        /// 根据 DataTable 新建 MyDataFrame 对象
        /// </summary>
        /// <param name="dt">输入的 DataTable</param>
        /// <returns>生成的 MyDataFrame 对象</returns>
        public static MyDataFrame create_from_datatable(DataTable dt)
        {
            if (dt == null || dt.Columns.Count == 0)
                throw new ArgumentException("DataTable 为空或没有列。");

            // 提取列名
            var seriesNames = dt.Columns.Cast<DataColumn>().Select(col => col.ColumnName).ToList();
            // 创建 MyDataFrame 对象
            MyDataFrame df = create(seriesNames);
            // 添加记录
            foreach (DataRow row in dt.Rows)
                df.add_record(row.ItemArray);

            return df;
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

        /// <summary>
        /// 根据列名获取对应的索引，如果不存在返回-1
        /// </summary>
        /// <param name="ColumnName">列名</param>
        /// <returns></returns>
        public int index_of_series(string series_name)
        {
            return series_names.IndexOf(series_name);
        }

        #endregion

        #region record操作

        /// <summary>
        /// 获取记录
        /// </summary>
        /// <param name="iRecord"></param>
        /// <returns></returns>
        public MyRecord get_record(int record_idx)
        {
            MyRecord row = [];
            //提取某行的所有列，构成MyRow
            foreach (var series_name in series_names)
            {
                row.Add(series_name, this[record_idx, series_name]);
            }
            return row;
        }

        /// <summary>
        /// (基于 MyDataFrame 的列结构) 创建一个记录对象。
        /// 如果提供了数据集合，将使用该数据填充记录；
        /// 否则，创建一个空记录，所有列的值为 null。
        /// </summary>
        /// <param name="data">可选的数据集合，用于填充记录。如果为 null，则创建空记录。</param>
        /// <returns>新创建的 MyRecord 对象。</returns>
        /// <exception cref="ArgumentException">如果数据长度与列名数量不匹配。</exception>
        public MyRecord new_record(IEnumerable<object> data = null)
        {
            // 创建记录并填充数据
            MyRecord record = [];

            if (data == null)
            {
                // 创建空记录，所有列值为 null
                foreach (var series_name in series_names)
                {
                    record.Add(series_name, null);
                }
            }
            else
            {
                // 将输入数据转为列表以支持索引访问
                var dataList = data.ToList();

                // 检查数据长度是否与列名数量一致
                if (dataList.Count != N_Series)
                {
                    throw new ArgumentException($"Input data length ({dataList.Count}) does not match the number of series ({N_Series}).", nameof(data));
                }

                for (int iSeries = 0; iSeries < N_Series; iSeries++)
                {
                    record.Add(series_names[iSeries], dataList[iSeries]);
                }
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
        public bool add_record(IEnumerable<object> record)
        {
            // 利用 new_record 创建记录对象
            var myRecord = new_record(record);

            // 调用 add_record(MyRecord) 插入记录
            add_record(myRecord);

            return true;
        }

        #endregion

        #region series操作

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

        /// <summary>
        /// 添加一个新列
        /// </summary>
        /// <param name="series_name"></param>
        public void add_series(string series_name)
        {
            // 1. 检查列名是否有效
            if (string.IsNullOrEmpty(series_name) || series_names.Contains(series_name))
            {
                throw new ArgumentException($"Column name '{series_name}' is invalid or already exists.");
            }

            // 2. 创建一个新的 MySeries 对象
            MySeries newSeries = MySeries.create(series_name);

            // 3. 为新列补充数据，假设所有其他列的行数是相同的
            for (int iRecord = 0; iRecord < N_Record; iRecord++)
            {
                newSeries.add(null);  // 添加默认值（如 null）到新列中
            }

            data.Add(newSeries);  // 新列添加到 MyDataFrame 中
        }

        /// <summary>
        /// 根据列名调整其在 data 中的位置（默认为第1列）。
        /// </summary>
        /// <param name="series_name">需要调整位置的列名。</param>
        /// <param name="newIndex">目标索引位置（默认为第1列）。</param>
        /// <exception cref="ArgumentException">如果列名不存在。</exception>
        /// <exception cref="ArgumentOutOfRangeException">如果目标索引超出范围。</exception>
        public void move_series(string series_name, int newIndex = 0)
        {
            if (string.IsNullOrEmpty(series_name))
            {
                throw new ArgumentException("Series name cannot be null or empty.", nameof(series_name));
            }

            // 检查目标索引范围是否有效
            if (newIndex < 0 || newIndex >= data.Count)
            {
                throw new ArgumentOutOfRangeException(nameof(newIndex), "Target index is out of range.");
            }

            // 找到目标列在 data 中的位置
            var seriesIndex = data.FindIndex(series => series.series_name == series_name);
            if (seriesIndex == -1)
            {
                throw new ArgumentException($"Series '{series_name}' does not exist in the data.", nameof(series_name));
            }

            // 如果当前索引与目标索引相同，则无需移动
            if (seriesIndex == newIndex)
            {
                return;
            }

            // 提取目标列
            var series = data[seriesIndex];

            // 从原位置移除目标列
            data.RemoveAt(seriesIndex);

            // 在目标索引位置插入目标列
            data.Insert(newIndex, series);
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
            MyDataFrame clone = new() { data = [] };
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
            return create_from_datatable(dt);
        }
        /// <summary>
        /// 从Excel文件流读取
        /// </summary>
        /// <param name="file_name"></param>
        /// <returns></returns>
        public static MyDataFrame read_from_excel(Stream stream, ExcelStreamType excel_stream_Type)
        {
            var dt = ExcelHelper.excel_to_dataTable(stream, excel_stream_Type);
            return create_from_datatable(dt);
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
