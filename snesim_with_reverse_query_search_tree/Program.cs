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

        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            ComWrappers.RegisterForMarshalling(WinFormsComInterop.WinFormsComWrappers.Instance);
            AllocConsole();//开启控制台

            Output.WriteLine("start FastSnesim simulation by using inverse retrieve search tree");
            string b = EasyConsole.Input.ReadString("Choose Example Dimension (input 2d or 3d) => ");

            int ratio_inverseRetrieve = EasyConsole.Input.ReadInt("set ratio of inverse retrieve search tree (input 0 ~ 100) => ", 0, 100);

            #region 2d Example

            if (b == "2d")
            {
                Output.WriteLine(ConsoleColor.Yellow, "Load Training Image(2d)");
                GridProperty TI = Grid.create_from_gslibwin("Load Training Image").grid
                    .select_gridProperty_win("Select Property as Training Image").grid_property;
                string is_use_cd = EasyConsole.Input.ReadString("use conditional data(2d) or not? (input Y or N) => ");
                CData cd = null;
                if (is_use_cd == "Y")
                    (cd, var _) = CData.read_from_gslibwin();
                Output.WriteLine(ConsoleColor.Yellow, "Set Simulation Grid Size");
                GridStructure gs = GridStructure.create_simple(500, 500, 1);
                gs = GridStructure.create_win(gs, "Set Simulation Grid Size");
                Snesim snesim = Snesim.create();
                var (re, time) = snesim.run(1001, 1, 60, (7, 7, 0), TI, cd, gs, ratio_inverseRetrieve);
                re.showGrid_win("realization");
                Output.WriteLine(ConsoleColor.Red, $"使用时间:{time}");
            }

            #endregion

            #region 3d Example

            if (b == "3d")
            {
                Output.WriteLine(ConsoleColor.Yellow, "Load Training Image(3d)");
                GridProperty TI = Grid.create_from_gslibwin("Load Training Image").grid
                    .select_gridProperty_win("Select Property as Training Image").grid_property;
                string is_use_cd = EasyConsole.Input.ReadString("use conditional data(3d) or not? (input Y or N) => ");
                CData cd = null;
                if (is_use_cd == "Y")
                    (cd, var _) = CData.read_from_gslibwin();
                Output.WriteLine(ConsoleColor.Yellow, "Set Simulation Grid Size");
                GridStructure gs = GridStructure.create_simple(100, 100, 30);
                gs = GridStructure.create_win(gs, "Set Simulation Grid Size");
                Snesim snesim = Snesim.create();
                var (re, time) = snesim.run(1001, 1, 85, (7, 7, 3), TI, cd, gs, ratio_inverseRetrieve);
                re.showGrid_win("realization");
                Output.WriteLine(ConsoleColor.Red, $"使用时间:{time}");
            }

            #endregion

            Console.ReadKey();

            FreeConsole();
        }

    }
}
