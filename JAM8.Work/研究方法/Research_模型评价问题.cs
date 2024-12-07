using JAM8.Algorithms.Geometry;
using JAM8.SpecificApps.研究方法;
using JAM8.Utilities;

namespace JAM8.Work.研究方法
{
    internal class Research_模型评价问题
    {
        public static void Run()
        {
            var menu = new EasyConsole.Menu()

                .Add("退出", CommonFunctions.Cancel)
                .Add("随机实现与cd匹配率", Model_Evaluation.随机实现与cd匹配率)
                .Add("cd在随机实现中表现为噪点的比例", Model_Evaluation.cd在随机实现中表现为噪点的比例)
                .Add("cd在随机实现中表现为噪点的比例（批量计算）", Model_Evaluation.cd在随机实现中表现为噪点的比例_批量计算)
                .Add("随机实现与TI的变差函数比较", Model_Evaluation.随机实现与TI的理论变差函数比较)
                .Add("Re_TI_entropy_of_pattern_size", Model_Evaluation.Re_TI_entropy_of_pattern_size)
                ;

            menu.Display();
        }
    }
}
