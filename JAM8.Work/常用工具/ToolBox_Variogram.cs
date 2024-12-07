using JAM8.Algorithms.Forms;
using JAM8.Algorithms.Geometry;
using JAM8.SpecificApps.常用工具;

namespace JAM8.Work.常用工具
{
    internal class ToolBox_Variogram
    {
        public static void Run()
        {
            var menu = new EasyConsole.Menu()

                .Add("退出", CommonFunctions.Cancel)
                .Add("根据实验变差函数自动拟合", Tool_Variogram.根据实验变差函数自动拟合)
                .Add("根据实验变差函数自动拟合（多组计算）", Tool_Variogram.根据实验变差函数自动拟合_多组计算)
                .Add("计算GridProperty的实验变差函数", Tool_Variogram.计算GridProperty的实验变差函数)
                .Add("根据理论变差函数模型计算实验变差函数", Tool_Variogram.根据理论变差函数模型计算实验变差函数)

                ;

            menu.Display();
        }
    }
}
