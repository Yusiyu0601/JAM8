using JAM8.SpecificApps.常用工具;

namespace JAM8.Work.常用工具
{
    internal class ToolBox_MachineLearning
    {
        public static void Run()
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
