using EasyConsole;
using JAM8.Algorithms.Forms;
using JAM8.Algorithms.Geometry;
using JAM8.SpecificApps.常用工具;

namespace JAM8.Console.Pages
{
    class ToolBox_Grid : Page
    {
        public ToolBox_Grid(EasyConsole.Program program) :
            base(
                "ToolBox_Grid",
                program
                  )
        {
        }

        public override void Display()
        {
            base.Display();

            Output.WriteLine(ConsoleColor.Green, "Test1 功能：");

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
           .Add("Grid显示", Grid显示)
           .Add("GridCatalog显示", GridCatalog显示)
           .Add("Grid与Image转换", Grid与Image转换)
           .Add("Grid手动编辑", Grid手动编辑)
           .Add("Grid转换为CData", Grid转换为CData)
           .Add("Grid过滤替换数值", Grid过滤替换数值)
           .Add("Grid采样点", Grid采样点)
           ;

            menu.Display();
        }

        private void Grid采样点()
        {
            Form_GridSampling frm = new();
            frm.ShowDialog();
        }

        private void Grid过滤替换数值()
        {
            Form_GridFilter frm = new();
            if (frm.ShowDialog() != DialogResult.OK)
                return;
        }

        private void Grid转换为CData()
        {
            Form_Grid2CData frm = new();
            if (frm.ShowDialog() != DialogResult.OK)
                return;
        }

        private void Grid手动编辑()
        {
            Form_GridEditor frm = new();
            if (frm.ShowDialog() != DialogResult.OK)
                return;
        }

        private void Grid与Image转换()
        {
            Form_Pic2GSLIB frm = new();
            if (frm.ShowDialog() != DialogResult.OK)
                return;
        }

        private void GridCatalog显示()
        {
            Form_GridCatalog frm = new();
            if (frm.ShowDialog() != DialogResult.OK)
                return;
        }

        private void Grid显示()
        {
            Grid g = Grid.create_from_gslibwin().grid;
            if (g != null)
                g.showGrid_win();
        }
    }
}
