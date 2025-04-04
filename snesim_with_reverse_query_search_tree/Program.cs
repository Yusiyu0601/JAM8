using System.Globalization;
using System.Runtime.InteropServices;
using EasyConsole;
using JAM8.Algorithms.Forms;
using JAM8.Algorithms.Geometry;

namespace snesim_with_reverse_query_search_tree
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

            //start FastSnesim simulation by using inverse retrieve search tree
            Output.WriteLine("start FastSnesim simulation by using inverse retrieve search tree");

            //1. Choose Example Dimension
            string b = EasyConsole.Input.ReadString("Choose Example Dimension (input 2d or 3d) => ");

            //2. Set ratio of inverse retrieve search tree
            int ratio_inverseRetrieve = Input.ReadInt("set ratio of inverse retrieve search tree (input 0 ~ 100) => ", 0, 100);

            #region 2d Example

            if (b == "2d")
            {
                //3. Load Training Image
                Output.WriteLine(ConsoleColor.Yellow, "\nLoad Training Image(2d)");
                var (input_grid, input_fileName) = Grid.create_from_gslibwin("Load Training Image");
                GridProperty TI = input_grid.select_gridProperty_win("Select Property as Training Image").grid_property;
                Output.WriteLine($"\n\tfileName  {input_fileName}");
                Output.WriteLine(TI.gridStructure.view_text());

                //4. Set Simulation Grid Size
                Output.WriteLine(ConsoleColor.Yellow, "Set Simulation Grid Size");
                GridStructure re_gs = GridStructure.create_simple(100, 100, 1);
                re_gs = GridStructure.create_win(re_gs, "Set Simulation Grid Size");
                Output.WriteLine(re_gs.view_text());

                //5. Load Conditional Data
                string is_use_cd = EasyConsole.Input.ReadString("use conditional data(2d) or not? (input Y or N) => ");
                CData2 cd = null;
                CData2 coarsened_cd = null;
                Grid coarsened_grid = null;
                if (is_use_cd == "Y")
                {
                    string cd_fileName = "";
                    (cd, cd_fileName) = CData2.read_from_gslib_win();
                    Output.WriteLine($"\n\tconditional data fileName  {cd_fileName}\n");
                    //coarsened conditional data and show coarsened grid
                    (coarsened_cd, coarsened_grid) = cd.coarsened(re_gs);
                    coarsened_grid.showGrid_win();
                }

                //6. Run FastSnesim Simulation
                Snesim snesim = Snesim.create();
                var (re, time) = snesim.run(1001, 3, 60, (7, 7, 0), TI, cd, re_gs, ratio_inverseRetrieve);

                //7. Show Simulation Result and Show Simulation Time
                re.showGrid_win("realization");
                Output.WriteLine(ConsoleColor.Red, $"\nTime {time} milliseconds");
            }

            #endregion

            #region 3d Example

            if (b == "3d")
            {
                //3. Load Training Image
                Output.WriteLine(ConsoleColor.Yellow, "\nLoad Training Image(3d)");
                var (input_grid, input_fileName) = Grid.create_from_gslibwin("Load Training Image");
                GridProperty TI = input_grid.select_gridProperty_win("Select Property as Training Image").grid_property;
                Output.WriteLine($"\n\tfileName  {input_fileName}");
                Output.WriteLine(TI.gridStructure.view_text());

                //4. Set Simulation Grid Size
                Output.WriteLine(ConsoleColor.Yellow, "Set Simulation Grid Size");
                GridStructure re_gs = GridStructure.create_simple(100, 100, 30);
                re_gs = GridStructure.create_win(re_gs, "Set Simulation Grid Size");
                Output.WriteLine(re_gs.view_text());

                //5. Load Conditional Data
                string is_use_cd = EasyConsole.Input.ReadString("use conditional data(3d) or not? (input Y or N) => ");
                CData2 cd = null;
                CData2 coarsened_cd = null;
                Grid coarsened_grid = null;
                if (is_use_cd == "Y")
                {
                    string cd_fileName = "";
                    (cd, cd_fileName) = CData2.read_from_gslib_win();
                    Output.WriteLine($"\n\tconditional data fileName  {cd_fileName}\n");
                    //coarsened conditional data and show coarsened grid
                    (coarsened_cd, coarsened_grid) = cd.coarsened(re_gs);
                    coarsened_grid.showGrid_win();
                }

                //6. Run FastSnesim Simulation
                Snesim snesim = Snesim.create();
                var (re, time) = snesim.run(1001, 1, 80, (7, 7, 3), TI, cd, re_gs, ratio_inverseRetrieve);

                //7. Show Simulation Result and Show Simulation Time
                re.showGrid_win("realization");
                Output.WriteLine(ConsoleColor.Red, $"\nTime {time} milliseconds");
            }

            #endregion

            Console.ReadKey();

            FreeConsole();
        }

    }
}
