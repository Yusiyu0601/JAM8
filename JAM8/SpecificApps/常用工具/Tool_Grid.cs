using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JAM8.Algorithms.Forms;
using JAM8.Algorithms.Geometry;
using JAM8.Utilities;

namespace JAM8.SpecificApps.常用工具
{
    public class Tool_Grid
    {
        public static void grid_filtering()
        {
            Form_GridFilter frm = new();
            frm.ShowDialog();
        }

        public static void convert_grid_to_cdata()
        {
            Form_Grid2CData frm = new();
            frm.ShowDialog();
        }

        public static void 编辑Grid()
        {
            Form_GridEditor frm = new();
            frm.ShowDialog();
        }

        public static void 将二维图像与Grid的转换()
        {
            Form_Pic2GSLIB frm = new();
            frm.ShowDialog();
        }

        public static void random_select_points()
        {
            Form_GridSampling frm = new();
            frm.ShowDialog();
        }

        public static void show_grid()
        {
            var (g, file_name) = Grid.create_from_gslibwin();
            g?.showGrid_win(FileHelper.GetFileName(file_name));
        }

        public static void show_grid_catalog()
        {
            Form_GridCatalog frm = new();
            frm.Show();
        }
    }
}
