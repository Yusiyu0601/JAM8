using JAM8.Tests;

namespace JAM8.Work.模块测试
{
    class Test_somethings
    {
        public static void Run()
        {
            var menu = new EasyConsole.Menu()

                .Add("退出", CommonFunctions.Cancel)
                .Add("三个数字排序", Test_others.三个数字排序)
                .Add("ReadTxtFile对比", Test_others.ReadTxtFile对比)
                .Add("ReadLargeTxtFile", Test_others.ReadLargeTxtFile)
                .Add("速度测试", Test_others.速度测试)
                .Add("自然数的组合计算", Test_others.自然数的组合计算)
                .Add("自然数的组合计算2", Test_others.自然数的组合计算2)
                .Add("自然数的组合计算3", Test_others.自然数的组合计算3)
                .Add("Test_GLCM测试", Test_others.Test_GLCM)
                .Add("Test_提取kriging里的条件数据和粗网格点", Test_others.Test_提取kriging里的条件数据和粗网格点)
                .Add("Test_Grid转换为cd", Test_others.Test_Grid转换为cd)
                .Add("根据学生平均分给N个单次分", Test_others.根据学生平均分给N个单次分)
              ;

            menu.Display();
        }

    }
}
