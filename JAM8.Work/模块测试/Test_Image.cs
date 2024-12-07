namespace JAM8.Work.模块测试
{
    internal class Test_Image
    {
        public static void Run()
        {
            var menu = new EasyConsole.Menu()

            .Add("退出", CommonFunctions.Cancel)
            .Add("fft", Tests.Test_Image.fft)
            ;

            menu.Display();
        }
    }
}
