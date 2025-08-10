using System.Diagnostics;
using JAM8.Algorithms.Forms;
using JAM8.Algorithms.Geometry;
using JAM8.Algorithms.Numerics;
using JAM8.SpecificApps.建模方法.Forms;
using JAM8.Utilities;

namespace JAM8.SpecificApps.建模方法
{
    public class Model_MPS
    {
        public static void DS_Run()
        {
            Form_DS frm = new();
            frm.ShowDialog();
        }

        public static void MPS_Run()
        {
            Form_MPS frm = new();
            frm.ShowDialog();
        }

        public static void Snesim_Run()
        {
            Form_GridCatalog frm = new();
            if (frm.ShowDialog() != DialogResult.OK)
                return;
            var g = frm.selected_grids.First();
            //var g = Grid.create_from_gslibwin().grid;
            var (cd, _) = CData.read_from_gslib_win();
            GridStructure gs_model = g.gridStructure;

            var snesim = Snesim.create();
            var (model, _) = snesim.simulate_multigrid(1, 1, 60, (7, 7, 1), g.first_gridProperty(), cd, gs_model);
            model.showGrid_win();
            //MyConsoleHelper.write_string_to_console("计算时间", (time / 1000.0).ToString());
        }
    }
}
