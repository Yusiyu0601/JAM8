using System.Text;

namespace JAM8.Utilities
{
    public class StringHelper
    {
        /// <summary>
        /// 打印输出时中英文混杂时候对齐
        /// https://www.cnblogs.com/chenjiahong/articles/2705437.html
        /// c#中控制字符串的格式可以使用String.Format()方法。在中英文混杂的字符串中，
        /// 如果要对齐，由于中文字符所占的宽度比西文字符宽，采用字符串.PadLeft()或PadRight()
        /// 可以补齐字符长度。但是这种方法没法解决中文字符占位比较宽的问题。
        /// 
        /// 使用方法:
        /// Console.WriteLine("{0}{1}", padRightEx(s1, 20), "hello");
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <param name="totalByteCount"></param>
        /// <returns></returns>
        public static string padRightEx(string str, int totalByteCount)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);//注册Nuget包System.Text.Encoding.CodePages中的编码到.NET Core
            Encoding coding = Encoding.GetEncoding("GB2312");
            int dcount = 0;
            foreach (char ch in str.ToCharArray())
            {
                if (coding.GetByteCount(ch.ToString()) == 2)
                    dcount++;
            }
            string w = str.PadRight(totalByteCount - dcount);
            return w;
        }
    }
}
