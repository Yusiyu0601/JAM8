using JAM8.SpecificApps.常用工具;

namespace JAM8.Work.常用工具
{
    internal class ToolBox_Plot
    {
        public static void Run()
        {
            var menu = new EasyConsole.Menu()

                .Add("退出", CommonFunctions.Cancel)
                .Add("散点图", Tool_Plot.func_scatterplot)
                ;

            menu.Display();
        }
    }
}
