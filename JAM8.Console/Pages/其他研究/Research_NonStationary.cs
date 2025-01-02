using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyConsole;
using JAM8.SpecificApps.研究方法;

namespace JAM8.Console.Pages
{
    public class Research_NonStationary : Page
    {
        public Research_NonStationary(EasyConsole.Program program) : base("Research_NonStationary", program)
        {
        }

        public override void Display()
        {
            base.Display();

            Output.WriteLine(ConsoleColor.Green, "Research_NonStationary 功能：");

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
            .Add("直接计算方法2d", Quantitative_NonStationary.直接计算方法2d)
            .Add("step1_模式相似度", Quantitative_NonStationary.step1_模式相似度)
            .Add("Test_GetAnchorsDistanceModel", Quantitative_NonStationary.Test_GetAnchorsDistanceModel)
           ;

            menu.Display();
        }
    }
}
