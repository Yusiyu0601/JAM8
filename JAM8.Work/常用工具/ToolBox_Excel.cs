using JAM8.SpecificApps.常用工具;

namespace JAM8.Work.常用工具
{
    internal class ToolBox_Excel
    {
        public static void Run()
        {
            var menu = new EasyConsole.Menu()

                .Add("退出", CommonFunctions.Cancel)
                .Add("xls 与 xlsx 格式相互转换", Tool_Excel.xls与xlsx格式互相转换)
                .Add("excel单元格向下充填", Tool_Excel.excel单元格向下充填)
                .Add("去除excel指定列的单元格字符串中所有空格符", Tool_Excel.去除excel指定列的单元格字符串中所有空格符)
                .Add("N个excel文件按列合并", Tool_Excel.N个excel文件按列合并)
                .Add("N个excel文件按行合并(操作前提:excel结构相同)", Tool_Excel.N个excel文件按行合并)
                ;

            menu.Display();
        }
    }
}
