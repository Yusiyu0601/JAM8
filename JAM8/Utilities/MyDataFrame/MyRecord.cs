namespace JAM8.Utilities
{
    public class MyRecord : Dictionary<string, object>
    {
        public MyRecord deep_clone()
        {
            MyRecord clone = new();
            foreach (var item in this)
                clone.Add(item.Key, item.Value);
            return clone;
        }
    }
}
