using Easy.Common.Extensions;
using JAM8.SpecificApps.常用工具;
using JAM8.Utilities;
using System.Data;

namespace JAM8.Work.常用工具
{
    public class ToolBox_WellLog
    {
        public static void Run()
        {
            var menu = new EasyConsole.Menu()

                .Add("退出", CommonFunctions.Cancel)
                .Add("计算层位内的测井属性均值", Tool_WellLog.计算层位内的测井属性均值)
                .Add("展示测井曲线", Tool_WellLog.展示测井曲线)
                .Add("提取测井曲线部分列", Tool_WellLog.提取测井曲线部分列)
                ;

            menu.Display();
        }
    }
}
