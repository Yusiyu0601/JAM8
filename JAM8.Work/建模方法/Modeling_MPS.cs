using JAM8.Algorithms.Geometry;
using JAM8.SpecificApps.建模方法;

namespace JAM8.Work.建模方法
{
    internal class Modeling_MPS
    {
        public static void Run()
        {
            var menu = new EasyConsole.Menu()

                .Add("退出", CommonFunctions.Cancel)
                .Add("MPS", Model_MPS.MPS_Run)
                .Add("Snesim", Model_MPS.Snesim_Run)
                .Add("DS", Model_MPS.DS_Run)
                ;

            menu.Display();
        }
    }
}
