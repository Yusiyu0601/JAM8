using JAM8.Algorithms.Forms;
using JAM8.Algorithms.Geometry;
using JAM8.SpecificApps.建模方法;
using JAM8.Utilities;
using System.Diagnostics;

namespace JAM8.Work.建模方法
{
    public class Modeling_IDW
    {
        public static void Run()
        {
            var menu = new EasyConsole.Menu()

                .Add("退出", CommonFunctions.Cancel)
                .Add("IDW_Run", Model_IDW.IDW_Run)

                ;

            menu.Display();
        }
    }
}
