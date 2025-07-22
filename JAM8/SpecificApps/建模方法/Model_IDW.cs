using JAM8.Algorithms.Forms;
using JAM8.Algorithms.Geometry;
using JAM8.Utilities;
using System.Diagnostics;

namespace JAM8.SpecificApps.建模方法
{
    public class Model_IDW
    {
        public static void IDW_Run()
        {
            var (cd, _) = CData.read_from_gslib_win("打开条件数据");
            Form_SelectPropertyFromCData frm = new(cd, "选择条件数据属性");
            if (frm.ShowDialog() != DialogResult.OK)
                return;
            var t_属性选取 = frm.selected_property_name;

            var t_搜索半径 = MyConsoleHelper.read_int_from_console("搜索半径");
            var t_cd最小数量 = MyConsoleHelper.read_int_from_console("cd最小数量");

            GridStructure gs = GridStructure.create_win(null, "设置模型网格尺寸");
            Grid g = Grid.create(gs);

            GridProperty gp_cd = cd.coarsened(gs).coarsened_grid[t_属性选取];
            g.add_gridProperty("井数据", gp_cd);
            Stopwatch sw = new();
            sw.Start();
            var estimate_idw = IDW_Interpolation.Run(gs, cd, t_属性选取, t_搜索半径, t_cd最小数量);
            g.Add("estimate_idw", estimate_idw);
            g.showGrid_win("反距离加权插值模型");
            sw.Stop();
            Console.WriteLine($@"Time:{sw.ElapsedMilliseconds}");
        }
    }
}