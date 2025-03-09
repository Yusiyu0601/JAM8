using System.Runtime.InteropServices;
using EasyConsole;
using JAM8.Algorithms.Forms;
using JAM8.Algorithms.Geometry;

namespace FastSnesim
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
                GridProperty TI = Grid.create_from_gslibwin("Load Training Image").grid.select_gridProperty_win().grid_property;
                string is_use_cd = EasyConsole.Input.ReadString("use conditional data(2d) or not? (input Y or N) => ");
                CData cd = null;
                if (is_use_cd == "Y")
                    (cd, var _) = CData.read_from_gslibwin();
                Mould mould = Mould.create_by_ellipse(7, 7, 1);
                mould = Mould.create_by_mould(mould, 45);
                GridStructure gs = GridStructure.create_simple(250, 250, 1);
                Snesim snesim = Snesim.create();
                var (re, time) = snesim.run(TI, cd, gs, 1001, mould, 1, ratio_inverseRetrieve);
                re.showGrid_win("realization");
                Output.WriteLine(ConsoleColor.Red, $"使用时间:{time}");
            }

            #endregion

            #region 3d Example

            if (b == "3d")
            {
                Output.WriteLine(ConsoleColor.Yellow, "Load Training Image(3d)");

                GridProperty TI = Grid.create_from_gslibwin("Load Training Image").grid.select_gridProperty_win().grid_property;
                Mould mould = Mould.create_by_ellipse(15, 15, 3, 1);
                mould = Mould.create_by_mould(mould, 100);
                GridStructure gs = GridStructure.create_simple(100, 100, 50);
                Snesim snesim = Snesim.create();
                var (re, time) = snesim.run(TI, null, gs, 1001, mould, 1, ratio_inverseRetrieve);
                re.showGrid_win("realization");
                Output.WriteLine(ConsoleColor.Red, $"使用时间:{time}");
            }

            #endregion

            FreeConsole();
        }

    }
}
