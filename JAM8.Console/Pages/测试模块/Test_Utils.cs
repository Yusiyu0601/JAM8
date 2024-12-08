using EasyConsole;
using JAM8.Utilities;

namespace JAM8.Console.Pages
{
    public class Test_Utils : Page
    {
        public Test_Utils(EasyConsole.Program program) : base("Test_Utils", program) { }

        public override void Display()
        {
            base.Display();

            Output.WriteLine(ConsoleColor.Green, "Test_Utils 功能：");

            Perform();

            System.Console.WriteLine("按任意键返回");
            System.Console.ReadKey();

            Program.NavigateBack();
        }

        private void Perform()
        {
            var menu = new EasyConsole.Menu()

           .Add("退出", CommonFunctions.Cancel)
           .Add("MyConsoleProgress测试", MyConsoleProgress测试)
           .Add("MyDataFrame create测试", MyDataFrame_create测试)
           ;

            menu.Display();
        }

        private void MyDataFrame_create测试()
        {
            MyDataFrame df = MyDataFrame.create(["A", "A", "B", "B", "A"]);
            MyDataFrame df2 = MyDataFrame.create(["A", "A", "B", "B", "A"], true);
        }

        private void MyConsoleProgress测试()
        {
            int N = 20000000;
            for (int i = 0; i <= N; i++)
            {
                //var m = Math.Sqrt(i);
                //Thread.Sleep(32);
                MyConsoleProgress.Print(i / (double)N * 100, "测试");
                //MyConsoleProgress.Print(i, N, "测试");
            }

            long totalSteps = N;
            MyConsoleProgress progress = new();
            Random random = new Random();
            for (int i = 1; i <= totalSteps; i++)
            {
                //int delay = random.Next(1, 5); // 生成 5 到 50 毫秒的随机延迟
                //Thread.Sleep(delay); // 模拟不平稳的工作时间
                progress.PrintWithRemainTime(i, totalSteps, "测试RemainTime", $"Step {i}");
            }
        }
    }
}
