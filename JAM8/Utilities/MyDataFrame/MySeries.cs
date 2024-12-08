namespace JAM8.Utilities
{
    /// <summary>
    /// 列数据
    /// </summary>
    public class MySeries
    {
        private MySeries() { }

        /// <summary>
        /// 列名
        /// </summary>
        public string series_name { get; internal set; }

        /// <summary>
        /// 数据缓冲区
        /// </summary>
        public List<object> buffer { get; internal set; }

        /// <summary>
        /// 记录数量，即行数
        /// </summary>
        public int N_record
        {
            get
            {
                return buffer.Count;
            }
        }

        /// <summary>
        /// 新建MySeries
        /// </summary>
        /// <param name="columnName"></param>
        /// <returns></returns>
        internal static MySeries create(string series_name)
        {
            MySeries series = new()
            {
                series_name = series_name,
                buffer = []
            };

            return series;
        }

        public new string ToString()
        {
            return series_name;
        }

        public void add(object value)
        {
            buffer.Add(value);
        }

        public object this[int idx_record]
        {
            get
            {
                return buffer[idx_record];
            }
            set
            {
                buffer[idx_record] = value;
            }
        }

        public MySeries deep_clone()
        {
            MySeries clone = create(series_name);
            clone.buffer = new();
            for (int i = 0; i < N_record; i++)
            {
                clone.buffer.Add(this[i]);
            }
            return clone;
        }
    }
}
