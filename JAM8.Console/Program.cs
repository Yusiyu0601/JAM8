using System.Runtime.InteropServices;
using JAM8.Console.Pages;

namespace JAM8.Console
{
    internal static class Program
    {
        [System.Runtime.InteropServices.DllImport("kernel32.dll", SetLastError = true)]
        [return: System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.Bool)]
        static extern bool AllocConsole();

        [System.Runtime.InteropServices.DllImport("Kernel32")]
        public static extern void FreeConsole();

        // 导入系统API
        [DllImport("user32.dll")]
        private static extern bool SetProcessDPIAware();

        [DllImport("user32.dll")]
        private static extern bool SetProcessDpiAwarenessContext(int dpiFlag);

        private const int DpiAwarenessContext_PerMonitorAwareV2 = 34;

        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // 启用高DPI感知（Windows 10+推荐）
            if (Environment.OSVersion.Version.Major >= 6) // Windows Vista及以上
            {
                SetProcessDPIAware(); // Win7/Win8
                SetProcessDpiAwarenessContext((int)DpiAwarenessContext_PerMonitorAwareV2); // Win10 1703+
            }

            ComWrappers.RegisterForMarshalling(WinFormsComInterop.WinFormsComWrappers.Instance);

            AllocConsole();//开启控制台

            new DemoProgram().Run();

            FreeConsole();
        }


    }
}