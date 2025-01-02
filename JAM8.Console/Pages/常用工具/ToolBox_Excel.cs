using EasyConsole;
using JAM8.SpecificApps.常用工具;

namespace JAM8.Console.Pages
{
    class ToolBox_Excel : Page
    {
        public ToolBox_Excel(EasyConsole.Program program) :
            base("ToolBox_Excel", program)
        {
        }

        public override void Display()
        {
            base.Display();

            Output.WriteLine(ConsoleColor.Green, "ToolBox_Excel 功能：");

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
           .Add("格式相互转换（xls 与 xlsx）", Tool_Excel.xls与xlsx格式互相转换)
           .Add("excel单元格向下充填", Tool_Excel.excel单元格向下充填)
           .Add("去除excel指定列的单元格字符串中所有空格符", Tool_Excel.去除excel指定列的单元格字符串中所有空格符)
           .Add("N个excel文件按列合并", Tool_Excel.N个excel文件按列合并)
           .Add("N个excel文件按行合并(操作前提:excel结构相同)", Tool_Excel.N个excel文件按行合并)
           ;

            menu.Display();
        }
    }

}
