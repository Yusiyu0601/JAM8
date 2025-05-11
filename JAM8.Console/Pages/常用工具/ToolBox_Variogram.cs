using EasyConsole;
using JAM8.Algorithms.Geometry;

namespace JAM8.Console.Pages
{
    class ToolBox_Variogram : Page
    {
        public ToolBox_Variogram(EasyConsole.Program program)
            : base(
                "ToolBox_Variogram",
                program
            )
        {
        }

        public override void Display()
        {
            base.Display();

            Output.WriteLine(ConsoleColor.Green, "ToolBox_Variogram 功能：");

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
                    .Add("计算GridProperty的实验变差函数", 计算GridProperty的实验变差函数)
                ;

            menu.Display();
        }

        private void 计算GridProperty的实验变差函数()
        {
            Variogram.variogramFit4Grid_win();
        }
    }
}