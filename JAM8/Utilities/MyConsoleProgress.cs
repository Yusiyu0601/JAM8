namespace JAM8.Utilities
{
    /// <summary>
    /// 控制台进度显示
    /// </summary>
    public class MyConsoleProgress
    {
        #region 静态方法

        private static string preview = "";
        private static readonly object lockObj = new();

        /// <summary>
        /// 打印进度(结束时根据 nextline_at_end 参数决定是否换行)
        /// </summary>
        /// <param name="progress">进度百分比</param>
        /// <param name="text">进度条说明文本</param>
        /// <param name="tag">可选标签</param>
        /// <param name="nextline_at_end">是否在结束时换行</param>
        public static void Print(double progress, string text, string tag = null, bool nextline_at_end = true)
        {
            // 进度值范围检查
            if (progress < 0 || progress > 100)
                throw new ArgumentOutOfRangeException(nameof(progress), "Progress must be between 0 and 100.");

            string progress_str = progress.ToString("0.0");  // 一位小数

            lock (lockObj) // 确保线程安全
            {
                if (progress == 100 && nextline_at_end)
                {
                    Console.WriteLine($@" [{text}] progress = {progress_str} %");
                    return;
                }

                if (preview == progress_str) return;

                // 更新静态预览值
                preview = progress_str;

                string output = tag == null
                    ? $"\r [{text}] progress = {progress_str} %   "
                    : $"\r [{text}] progress = {progress_str} %  [{tag}]   ";
                Console.Write(output);
            }
        }

        /// <summary>
        /// 打印进度(结束时根据 nextline_at_end 参数决定是否换行)
        /// </summary>
        /// <param name="current">当前位置</param>
        /// <param name="max">总数</param>
        /// <param name="text">任务描述文本</param>
        /// <param name="tag">可选标签</param>
        /// <param name="nextline_at_end">是否在结束时换行</param>
        public static void Print(long current, long max, string text, string tag = null, bool nextline_at_end = true)
        {
            if (max <= 0)
                throw new ArgumentException("The total count (max) must be greater than 0.", nameof(max));

            // 计算进度百分比
            string progressStr = Math.Round((double)current / max * 100, 1).ToString("0.0");

            lock (lockObj) // 确保线程安全
            {
                // 如果进度已达到最大值并且 nextline_at_end 为 true，换行
                if (current == max && nextline_at_end)
                {
                    Console.WriteLine($@" [{text}] progress = {progressStr} %");
                    return;
                }
                // 如果进度未变化，则不进行更新
                if (preview == progressStr) return;

                // 更新静态预览值
                preview = progressStr;

                // 根据是否有标签，格式化输出
                string output = tag == null
                    ? $"\r [{text}] progress = {progressStr} %   "
                    : $"\r [{text}] progress = {progressStr} %  [{tag}]   ";
                Console.Write(output);
            }
        }

        #endregion

        #region 实例方法，可打印剩余时间

        private DateTime time_start = DateTime.Now;//起始时间
        private DateTime time_last = DateTime.Now;//上一次调用print的时间
        private DateTime time_last_100;//前N次调用print的时间
        private TimeSpan time_span_100;//N次时间间隔
        private int flag = 0;
        public MyConsoleProgress()
        {
            time_last = DateTime.Now;
            time_start = DateTime.Now;
        }

        /// <summary>
        /// 打印进度，可以显示剩余时间
        /// </summary>
        /// <param name="current"></param>
        /// <param name="max"></param>
        /// <param name="text"></param>
        /// <param name="tag"></param>
        public void PrintWithRemainTime(long current, long max, string text, string tag = null)
        {
            flag++;
            if (flag % 100 == 0)//计算N次的间隔时间
            {
                time_last_100 = time_last;
                time_last = DateTime.Now;
                time_span_100 = time_last - time_last_100;
            }
            double time_remain = (max - current) * time_span_100.TotalMilliseconds / 100;
            Print(current, max, text, $"{tag} --- 已用:{(int)(time_last - time_start).TotalSeconds}秒 " +
                $"剩余:{Convert.ToInt32(time_remain / 1000)}秒");

        }

        #endregion
    }
}
