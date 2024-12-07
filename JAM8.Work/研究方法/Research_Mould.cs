using JAM8.SpecificApps.研究方法;

namespace JAM8.Work.研究方法
{
    public class Research_Mould
    {
        public static void Run()
        {
            var menu = new EasyConsole.Menu()

                .Add("退出", CommonFunctions.Cancel)
                .Add("mould_dim_reduction_with_MDS", mould_research.mould_dim_reduction_with_MDS)
            ;

            menu.Display();
        }
    }
}
