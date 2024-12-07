using System.Diagnostics;

namespace JAM8.Utilities
{
    public class PowerShellHelper
    {
        /// <summary>
        /// 执行命令
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public static string ExecuteCommand(string command)
        {
            var processStartInfo = new ProcessStartInfo
            {
                FileName = "powershell.exe",
                Arguments = $"-command {command}",
                UseShellExecute = false,
                RedirectStandardOutput = true
            };

            using var process = new Process();
            process.StartInfo = processStartInfo;
            process.Start();
            string output = process.StandardOutput.ReadToEnd();
            return output;
        }
    }
}
