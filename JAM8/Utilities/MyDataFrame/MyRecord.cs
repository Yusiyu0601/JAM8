namespace JAM8.Utilities
{
    public class MyRecord : Dictionary<string, object>
    {
        /// <summary>
        /// 获取所有值的列表。
        /// </summary>
        /// <returns>MyRecord 的所有值组成的列表。</returns>
        public List<object> get_values()
        {
            return [.. this.Values];
        }

        /// <summary>
        /// 深拷贝当前 MyRecord。
        /// </summary>
        /// <returns>深拷贝后的 MyRecord 对象。</returns>
        public MyRecord deep_clone()
        {
            MyRecord clone = [];
            foreach (var item in this)
                clone.Add(item.Key, item.Value);
            return clone;
        }
    }
}
