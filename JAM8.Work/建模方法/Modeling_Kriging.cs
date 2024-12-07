using JAM8.Algorithms.Geometry;
using JAM8.SpecificApps.建模方法;
using System.Diagnostics;

namespace JAM8.Work.建模方法
{
    public class Modeling_Kriging
    {
        public static void Run()
        {
            var menu = new EasyConsole.Menu()

                .Add("退出", CommonFunctions.Cancel)
                .Add("Kriging", Model_Kriging.Kriging)

                ;

            menu.Display();
        }
    }
}
