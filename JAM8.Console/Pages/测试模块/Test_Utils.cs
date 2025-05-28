using System.Data;
using EasyConsole;
using JAM8.Algorithms.Geometry;
using JAM8.Utilities;
using Xunit;

// using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace JAM8.Console.Pages
{
    public class Test_Utils : Page
    {
        #region 基本不用动的方法

        public Test_Utils(EasyConsole.Program program) : base("Test_Utils", program)
        {
        }

        public override void Display()
        {
            base.Display();

            Output.WriteLine(ConsoleColor.Green, "Test_Utils 功能：");

            Perform();

            System.Console.WriteLine(@"按任意键返回");
            System.Console.ReadKey();

            Program.NavigateBack();
        }

        #endregion

        private void Perform()
        {
            var menu = new EasyConsole.Menu()
                    .Add("退出", CommonFunctions.Cancel)
                    .Add("MyConsoleProgress测试", MyConsoleProgress测试)
                    .Add("MyDataFrame create测试", MyDataFrame_create测试)
                    .Add("distinct测试", distinct测试)
                    .Add("Mode众数计算测试", Mode众数计算测试)
                ;

            menu.Display();
        }

        private void Mode众数计算测试()
        {
            void PrintResult<T>(string testName, IList<T> data, bool keepNull = true)
            {
                var (mode, count) = MyArrayHelper.FindMode(data, keepNull);
                System.Console.WriteLine($"{testName}: 众数 = {(mode == null ? "null" : mode.ToString())}, 频数 = {count}");
            }

            // 测试用例
            PrintResult("TestInt", new List<int> { 1, 2, 2, 3, 3, 3, 4 });
            PrintResult("TestNullableInt keepNull=true", new int?[] { 1, 2, 2, null, 3, null, null }, true);
            PrintResult("TestNullableInt keepNull=false", new int?[] { 1, 2, 2, null, 3, null, null }, false);
            PrintResult("TestString keepNull=true",
                new List<string> { "apple", "banana", "apple", null, "banana", null }, true);
            PrintResult("TestString keepNull=false",
                new List<string> { "apple", "banana", "apple", null, "banana", null }, false);
            PrintResult("TestEmpty", new List<int>());
            PrintResult("TestAllSame", new List<string> { "same", "same", "same" });
            PrintResult("TestMultipleModes", new List<int> { 1, 1, 2, 2, 3 });
        }

        private void distinct测试()
        {
            void Test_AllDistinctCases()
            {
                // int
                {
                    var input = new List<int> { 1, 2, 2, 3, 1, 4 };
                    var (values, counts) = MyDistinct.distinct(input);
                    Assert.Equal(new[] { 1, 2, 3, 4 }, values);
                    Assert.Equal(new[] { 2, 2, 1, 1 }, counts);
                }

                // double
                {
                    var input = new List<double> { 1.1, 2.2, 2.2, 3.3, 1.1, 4.4 };
                    var (values, counts) = MyDistinct.distinct(input);
                    Assert.Equal(new[] { 1.1, 2.2, 3.3, 4.4 }, values);
                    Assert.Equal(new[] { 2, 2, 1, 1 }, counts);
                }

                // nullable int, keep null
                {
                    var input = new List<int?> { 1, 2, 2, null, 1, 4, null };
                    var (values, counts) = MyDistinct.distinct(input, true);
                    Assert.Equal(new int?[] { null, 1, 2, 4 }, values);
                    Assert.Equal(new[] { 2, 2, 2, 1 }, counts);
                }

                // nullable int, without null
                {
                    var input = new List<int?> { 1, 2, 2, null, 1, 4, null };
                    var (values, counts) = MyDistinct.distinct(input, false);
                    Assert.Equal(new int?[] { 1, 2, 4 }, values);
                    Assert.Equal(new[] { 2, 2, 1 }, counts);
                }

                // nullable double, keep null
                {
                    var input = new List<double?> { 1.1, 2.2, null, 2.2, 1.1, 4.4, null };
                    var (values, counts) = MyDistinct.distinct(input, true);
                    Assert.Equal(new double?[] { null, 1.1, 2.2, 4.4 }, values);
                    Assert.Equal(new[] { 2, 2, 2, 1 }, counts);
                }

                // nullable double, without null
                {
                    var input = new List<double?> { 1.1, 2.2, null, 2.2, 1.1, 4.4, null };
                    var (values, counts) = MyDistinct.distinct(input, false);
                    Assert.Equal(new double?[] { 1.1, 2.2, 4.4 }, values);
                    Assert.Equal(new[] { 2, 2, 1 }, counts);
                }

                // nullable struct, keep null
                {
                    var input = new List<DateTime?>
                        { new DateTime(2020, 1, 1), null, new DateTime(2020, 1, 1), new DateTime(2021, 1, 1), null };
                    var (values, counts) = MyDistinct.distinct(input, true);
                    Assert.Equal(new DateTime?[] { null, new DateTime(2020, 1, 1), new DateTime(2021, 1, 1) }, values);
                    Assert.Equal(new[] { 2, 2, 1 }, counts);
                }

                // nullable struct, without null
                {
                    var input = new List<DateTime?>
                        { new DateTime(2020, 1, 1), null, new DateTime(2020, 1, 1), new DateTime(2021, 1, 1), null };
                    var (values, counts) = MyDistinct.distinct(input, false);
                    Assert.Equal(new DateTime?[] { new DateTime(2020, 1, 1), new DateTime(2021, 1, 1) }, values);
                    Assert.Equal(new[] { 2, 1 }, counts);
                }

                // string
                {
                    var input = new List<string> { "a", "b", null, "a", "c", null };
                    var (values, counts) = MyDistinct.distinct(input);
                    Assert.Equal(4, values.Length);
                    Assert.Equal(new string?[] { null, "a", "b", "c" }, values);
                    Assert.Equal(new[] { 2, 2, 1, 1 }, counts);
                }

                // 自定义类型 SpatialIndex
                {
                    var input = new List<SpatialIndex>
                    {
                        SpatialIndex.create(1, 2),
                        SpatialIndex.create(3, 4),
                        null,
                        SpatialIndex.create(1, 2),
                        SpatialIndex.create(5, 6)
                    };
                    var (values, counts) = MyDistinct.distinct(input);
                    // Assert.Equal(
                    //     new[] { SpatialIndex.create(1, 2), SpatialIndex.create(3, 4), SpatialIndex.create(5, 6) },
                    //     values);
                    // Assert.Equal(new[] { 2, 1, 1 }, counts);
                }

                // 自定义类型 SpatialIndex
                {
                    var input = new List<SpatialIndex>
                    {
                        SpatialIndex.create(1, 2),
                        SpatialIndex.create(3, 4),
                        null,
                        SpatialIndex.create(1, 2),
                        null,
                        SpatialIndex.create(5, 6)
                    };
                    var (values, counts) = MyDistinct.distinct(input, false);
                    // Assert.Equal(
                    //     new[] { SpatialIndex.create(1, 2), SpatialIndex.create(3, 4), SpatialIndex.create(5, 6) },
                    //     values);
                    // Assert.Equal(new[] { 2, 1, 1 }, counts);
                }
            }

            Test_AllDistinctCases();

            var arr = new int?[] { 1, 2, 2, null, 3, null, 3 };
            var res = MyDistinct.distinct(arr, keep_null: true);
            // 预期 keys: [null,1,2,3], counts: [2,1,2,2]

            System.Console.WriteLine(string.Join(",", res.values.Select(v => v?.ToString() ?? "null")));
            System.Console.WriteLine(string.Join(",", res.counts));
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