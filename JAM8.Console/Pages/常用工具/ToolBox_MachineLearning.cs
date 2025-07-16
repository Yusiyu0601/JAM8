using EasyConsole;
using JAM8.SpecificApps.常用工具;

namespace JAM8.Console.Pages
{
    class ToolBox_MachineLearning : Page
    {
        public ToolBox_MachineLearning(EasyConsole.Program program) :
            base("ToolBox_MachineLearning", program)
        {
        }

        public override void Display()
        {
            base.Display();

            Output.WriteLine(ConsoleColor.Green, "ToolBox_Excel 功能：");

            Perform();

            System.Console.WriteLine();
            EasyConsole.Output.WriteLine(ConsoleColor.Green, "按任意键返回");
            System.Console.ReadKey();

            Program.NavigateBack();
        }

        private void Perform()
        {
            var menu = new EasyConsole.Menu()

                .Add("退出", CommonFunctions.Cancel)
                .Add("制作测试数据(80%训练、20%测试)", Tool_ML.func_MakeTestData)
                .Add("多维尺度分析", Tool_ML.func_MDS)
                .Add("K均值聚类", Tool_ML.func_KMeans)
                .Add("支持向量机", Tool_ML.func_SVM)
                .Add("随机森林", Tool_ML.func_RandomForest)
           ;

            menu.Display();
        }

    }

}
