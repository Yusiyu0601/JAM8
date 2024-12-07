using JAM8.SpecificApps.研究方法;

namespace JAM8.Work.研究方法
{
    class Research_NonStationary
    {
        public static void Run()
        {
            var menu = new EasyConsole.Menu()

                .Add("退出", CommonFunctions.Cancel)
                .Add("NonStationary_Quantitative", Quantitative_NonStationary.step1_实验变差函数2d)
                .Add("直接计算方法2d", Quantitative_NonStationary.直接计算方法2d)
                .Add("TI_PatternSize", TI_PatternSize.FindPatternSize_Entropy);
            ;

            menu.Display();
        }
    }
}
