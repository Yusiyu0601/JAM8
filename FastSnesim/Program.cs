using System.Runtime.InteropServices;
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

            Form_GridCatalog frm = new();
            if (frm.ShowDialog() != DialogResult.OK)
                return;
            var g = frm.selected_grids.First();
            //var g = Grid.create_from_gslibwin().grid;
            var (cd, _) = CData.read_from_gslibwin();
            GridStructure gs_model = g.gridStructure;

            var snesim = Snesim.create();
            var model = snesim.run(1, 1, 60, (7, 7, 1), g.first_gridProperty(), cd, gs_model);
            model.showGrid_win();

            FreeConsole();
        }


    }
}
