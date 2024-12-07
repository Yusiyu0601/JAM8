namespace JAM8.Work.模块测试
{
    public class Test_TorchSharp
    {
        public static void Run()
        {
            var menu = new EasyConsole.Menu()

                .Add("退出", CommonFunctions.Cancel)
                .Add("Test_TorchSharp1", Tests.Test_TorchSharp.Test_TorchSharp1)
                .Add("Test_TorchSharp2_LinearRegression", Tests.Test_TorchSharp.Test_TorchSharp2_LinearRegression)
              ;

            menu.Display();
        }
    }
}
