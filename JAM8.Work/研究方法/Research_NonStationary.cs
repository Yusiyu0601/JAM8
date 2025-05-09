using JAM8.SpecificApps.研究方法;

namespace JAM8.Work.研究方法
{
    internal class ResearchNonStationary
    {
        public static void run()
        {
            var menu = new EasyConsole.Menu()

                .Add("退出", CommonFunctions.Cancel)
                .Add("NonStationary_Quantitative", QuantitativeNonStationary.step1_实验变差函数2d)
                .Add("直接计算方法2d", QuantitativeNonStationary.直接计算方法2d)
                .Add("TI_PatternSize", TrainImage_PatternSize.FindPatternSize_Entropy);
            ;

            menu.Display();
        }
    }
}
