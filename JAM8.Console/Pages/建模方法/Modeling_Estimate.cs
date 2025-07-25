﻿using System.Diagnostics;
using EasyConsole;
using JAM8.Algorithms.Forms;
using JAM8.Algorithms.Geometry;
using JAM8.SpecificApps.建模方法;
using JAM8.Utilities;

namespace JAM8.Console.Pages
{
    public class Modeling_Estimate : Page
    {
        public Modeling_Estimate(EasyConsole.Program program) : base("Modeling_Estimate", program)
        {
        }

        public override void Display()
        {
            base.Display();

            Output.WriteLine(ConsoleColor.Green, "Modeling_Estimate 功能：");

            Perform();

            System.Console.WriteLine();
            EasyConsole.Output.WriteLine(ConsoleColor.Green, "按任意键返回");
            System.Console.ReadKey();

            Program.NavigateBack();
        }

        private void Perform()
        {
            var menu = new EasyConsole.Menu()
            .Add("Back", CommonFunctions.Cancel)
            .Add("kriging插值", kriging)
            .Add("IDW插值", IDW)
           ;

            menu.Display();
        }

        private void IDW()
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
            var estimate_idw = IDW_Interpolation.Run(gs, cd, t_属性选取,t_搜索半径, t_cd最小数量);
            g.Add("estimate_idw", estimate_idw);
            g.showGrid_win("反距离加权插值模型");
            sw.Stop();
            System.Console.WriteLine($@"Time:{sw.ElapsedMilliseconds}");

        }

        private void kriging()
        {
            Form_Kriging frm = new();
            if (frm.ShowDialog() != DialogResult.OK)
                return;
        }
    }
}
