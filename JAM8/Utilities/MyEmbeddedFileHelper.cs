using System.Reflection;
using System.Text;

namespace JAM8.Utilities
{
    public class MyEmbeddedFileHelper
    {
        /// <summary>
        /// 读取嵌入的txt文件，返回字符串格式的txt内容
        /// </summary>
        /// <param name="embeddedFilePath">例如:"JAM8.资源文件.demo_largetrain.out"</param>
        /// <returns></returns>
        public static string read_embedded_txt(string embedded_filePath)
        {
            string ext = Path.GetExtension(embedded_filePath);
            using Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(embedded_filePath);
            // 把 Stream 转换成 byte[]   
            byte[] bytes = new byte[stream.Length];
            stream.Read(bytes, 0, bytes.Length);
            // 设置当前流的位置为流的开始   
            stream.Seek(0, SeekOrigin.Begin);
            string txt = Encoding.Default.GetString(bytes);
            return txt;
        }

        /// <summary>
        /// 读取"JAM8.Work.资源文件"嵌入的excel文件，返回MyDataFrame对象
        /// </summary>
        /// <param name="embeddedFilePath">例如:"JAM8.Work.资源文件.ABC.xlsx"</param>
        /// <returns></returns>
        public static MyDataFrame read_embedded_excel(string embedded_filePath)
        {
            string ext = Path.GetExtension(embedded_filePath);
            using Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(embedded_filePath);
            // 把 Stream 转换成 byte[]   
            byte[] bytes = new byte[stream.Length];
            stream.Read(bytes, 0, bytes.Length);
            // 设置当前流的位置为流的开始
            stream.Seek(0, SeekOrigin.Begin);
            ExcelHelper.ExcelStreamType excel_stream_type = ExcelHelper.ExcelStreamType.xls;
            if (ext == ".xlsx")
                excel_stream_type = ExcelHelper.ExcelStreamType.xlsx;
            var df = MyDataFrame.read_from_excel(stream, excel_stream_type);
            return df;
        }

        public static void save_embedded_to_filePath(string embedded_filePath, string target_filePath)
        {
            using Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(embedded_filePath);
            if (stream == null)
            {
                throw new FileNotFoundException($"嵌入资源 '{embedded_filePath}' 未找到。");
            }

            // 写入到指定路径
            using FileStream fileStream = new(target_filePath, FileMode.Create, FileAccess.Write);
            stream.CopyTo(fileStream);
        }

    }
}
