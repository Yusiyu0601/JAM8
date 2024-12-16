using System.Data;
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
            df.add_series("C");
            df.add_record([1, 2, 3, 4, 5, 6]);
            df.move_series("C");
            var record = df.new_record();
            df.add_record(record);
            var objs = df.get_record(0).get_values();
            df.show_win();

            MyDataFrame df2 = MyDataFrame.create(["A", "A", "B", "B", "A"], true);

            MyDataFrame df3 = MyDataFrame.create(5);

            var seriesNames = new List<string> { "Col1", "Col2" };
            var data = new float[,] { { 1.0f, 2.0f }, { 3.0f, 4.0f } };

            var df4 = MyDataFrame.create_from_array(seriesNames, data);
            DataTable dt = ExcelHelper.excel_to_dataTable();
            var df5 = MyDataFrame.create_from_datatable(dt);
            df5.add_series("C");
            df5.show_win();

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
