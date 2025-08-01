using System.Data;
using System.Diagnostics;
using EasyConsole;
using JAM8.Algorithms.Geometry;
using JAM8.Utilities;
using Xunit;


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
                    .Add("test_MersenneTwister", test_MersenneTwister)
                    .Add("test_MySortHelper_correctness", test_MySortHelper_correctness)
                    .Add("test_MySortHelper_performance", test_MySortHelper_performance)
                    .Add("test_MyArrayHelper", test_MyArrayHelper)
                ;

            menu.Display();
        }

        private void test_MyArrayHelper()
        {
            System.Console.WriteLine("\n========== MyArrayHelper 测试 ==========");

            double[] arr1d = { 1.23456, 2.34567, 3.45678 };
            double[,] arr2d = { { 1.1, 2.2 }, { 3.3, 4.4 } };
            float[,,] arr3d = new float[,,] { { { 1.1f, 2.2f }, { 3.3f, 4.4f } }, { { 5.5f, 6.6f }, { 7.7f, 8.8f } } };

            MyArrayHelper.print<double>(arr1d, "一维数组", "F2");
            MyArrayHelper.print<double>(arr2d, "二维数组", "F1");
            MyArrayHelper.print<float>(arr3d, "三维数组", "F4");


            System.Console.WriteLine("======== 功能与效率综合测试 ========");

            // 1. 所有元素不同
            string[] arr1 = { "A", "B", "C", "D" };
            MyArrayHelper.print<string>(arr1, "所有元素不同");
            var (modes1, count1) = MyArrayHelper.find_all_modes<string>(arr1);
            System.Console.WriteLine($"众数数目：{modes1.Count}, 频数：{count1}, 众数：{string.Join(", ", modes1)}");

            // 2. 唯一众数
            int[] arr2 = { 1, 2, 2, 3 };
            MyArrayHelper.print<int>(arr2, "唯一众数");
            var (modes2, count2) = MyArrayHelper.find_all_modes<int>(arr2);
            System.Console.WriteLine($"众数：{string.Join(", ", modes2)}, 频数：{count2}");

            // 3. 多个并列众数
            int[] arr3 = { 1, 2, 1, 2, 3 };
            MyArrayHelper.print<int>(arr3, "并列众数");
            var (modes3, count3) = MyArrayHelper.find_all_modes<int>(arr3);
            System.Console.WriteLine($"众数：{string.Join(", ", modes3)}, 频数：{count3}");

            // 4. 包含 null，保留 null
            string?[] arr4 = { "A", null, "B", null, "A" };
            MyArrayHelper.print<string?>(arr4, "包含 null（保留）");
            var (modes4, count4) = MyArrayHelper.find_all_modes<string?>(arr4, keep_null: true);
            System.Console.WriteLine($"众数：{string.Join(", ", modes4.Select(x => x ?? "null"))}, 频数：{count4}");

            // 5. 包含 null，但忽略
            string?[] arr5 = { "A", null, "B", null, "A" };
            MyArrayHelper.print<string?>(arr5, "包含 null（忽略）");
            var (modes5, count5) = MyArrayHelper.find_all_modes<string?>(arr5, keep_null: false);
            System.Console.WriteLine($"众数：{string.Join(", ", modes5.Select(x => x ?? "null"))}, 频数：{count5}");

            // 6. 空数组
            int[] arr6 = Array.Empty<int>();
            MyArrayHelper.print<int>(arr6, "空数组");
            var (modes6, count6) = MyArrayHelper.find_all_modes<int>(arr6);
            System.Console.WriteLine($"众数：{string.Join(", ", modes6)}, 频数：{count6}");

            // 7. 三维数组
            int[,,] arr7 = { { { 1, 2 }, { 3, 1 } }, { { 2, 1 }, { 4, 1 } } };
            MyArrayHelper.print<int>(arr7, "三维数组");
            var (modes7, count7) = MyArrayHelper.find_all_modes<int>(arr7);
            System.Console.WriteLine($"众数：{string.Join(", ", modes7)}, 频数：{count7}");

            // 8. 大数据测试（int）
            System.Console.WriteLine("\n>> 大数据测试 int[]");
            int size = 10_000_000;
            int[] arr8 = new int[size];
            var rand = new Random(42);
            for (int i = 0; i < size; i++)
                arr8[i] = rand.Next(0, 1000);

            var sw1 = Stopwatch.StartNew();
            var (modes8, count8) = MyArrayHelper.find_all_modes<int>(arr8);
            sw1.Stop();
            System.Console.WriteLine(
                $"元素数：{size}, 众数数目：{modes8.Count}, 最大频数：{count8}, 耗时：{sw1.ElapsedMilliseconds} ms");

            // 9. 大数据测试（string）
            System.Console.WriteLine("\n>> 大数据测试 string[]");
            int size2 = 5_000_000;
            string[] arr9 = new string[size2];
            for (int i = 0; i < size2; i++)
                arr9[i] = "Item" + rand.Next(0, 1000).ToString();

            var sw2 = Stopwatch.StartNew();
            var (modes9, count9) = MyArrayHelper.find_all_modes<string>(arr9);
            sw2.Stop();
            System.Console.WriteLine(
                $"元素数：{size2}, 众数数目：{modes9.Count}, 最大频数：{count9}, 耗时：{sw2.ElapsedMilliseconds} ms");
        }

        private void test_MySortHelper_performance()
        {
            System.Console.WriteLine("\n========== 性能测试 ==========");

            int N = 1_000_000;
            Random rnd = new();
            List<int?> numbers = new();
            for (int i = 0; i < N; i++)
                numbers.Add(rnd.Next(0, 10_000));

            // 添加一些null
            for (int i = 0; i < 1000; i++)
                numbers[rnd.Next(N)] = null;

            Stopwatch sw = new();
            sw.Start();
            var sorted = MySortHelper.sort_list(numbers);
            sw.Stop();

            System.Console.WriteLine($"对 {N} 个元素排序耗时：{sw.Elapsed.TotalSeconds:F3} 秒");
            System.Console.WriteLine($"前5个元素: {string.Join(", ", sorted.GetRange(0, 5))}");
        }

        private void test_MySortHelper_correctness()
        {
            System.Console.WriteLine("【1】Dictionary 按值排序测试：");
            var dict = new Dictionary<string, int?> { ["B"] = 5, ["A"] = 3, ["C"] = null };
            var sortedDict = MySortHelper.sort_dict_by_value(dict);
            foreach (var kv in sortedDict)
                System.Console.WriteLine($"{kv.Key} => {kv.Value}");
            // 输出顺序应为：C(null), A(3), B(5)

            System.Console.WriteLine("\n【2】KeyValuePair 列表排序测试：");
            var kvpList = new List<KeyValuePair<string, double?>>
            {
                new("X", 2.5),
                new("Y", null),
                new("Z", 1.2)
            };
            var sortedKvpList = MySortHelper.sort_kvp_list_by_value(kvpList);
            foreach (var kv in sortedKvpList)
                System.Console.WriteLine($"{kv.Key} => {kv.Value}");
            // 输出顺序应为：Y(null), Z(1.2), X(2.5)

            System.Console.WriteLine("\n【3】元组列表排序测试：");
            var tupleList = new List<(string, DateTime?)>
            {
                ("now", DateTime.Now),
                ("past", null),
                ("future", DateTime.Now.AddDays(1))
            };
            var sortedTupleList = MySortHelper.sort_tuple_list_by_item2(tupleList);
            foreach (var t in sortedTupleList)
                System.Console.WriteLine($"{t.Item1} => {t.Item2}");
            // 输出顺序应为：past(null), now, future

            System.Console.WriteLine("\n【4】普通 List 排序测试（int? 含 null）：");
            var list = new List<int?> { 7, null, 3, 5 };
            var sortedList = MySortHelper.sort_list(list);
            foreach (var v in sortedList)
                System.Console.WriteLine(v);
            // 输出顺序应为：null, 3, 5, 7

            System.Console.WriteLine("\n【5】字符串排序测试（含 null）：");
            var strList = new List<string?> { "dog", null, "apple", "banana" };
            var sortedStr = MySortHelper.sort_strings(strList);
            foreach (var s in sortedStr)
                System.Console.WriteLine(s ?? "<null>");
            // 输出顺序应为：<null>, apple, banana, dog

            System.Console.WriteLine("\n【6】降序排序测试：");
            var descendingList = MySortHelper.sort_list(new List<int?> { 1, null, 3, 2 }, descending: true);
            foreach (var v in descendingList)
                System.Console.WriteLine(v);
            // 输出应为：3, 2, 1, null

            System.Console.WriteLine("\n所有测试完成。");
        }

        // 测试 Mersenne Twister 随机数生成器
        private void test_MersenneTwister()
        {
            var mt = new MersenneTwister(123456); // 固定种子

            System.Console.WriteLine("[C#] Next() uint32:");
            for (int i = 0; i < 5; i++)
                System.Console.WriteLine(mt.Next());

            System.Console.WriteLine("\n[C#] Next(min, max):");
            for (int i = 0; i < 5; i++)
                System.Console.WriteLine(mt.Next(10, 20));

            System.Console.WriteLine("\n[C#] NextDouble():");
            for (int i = 0; i < 5; i++)
                System.Console.WriteLine(mt.NextDouble());

            System.Console.WriteLine("\n[C#] NextGaussian():");
            for (int i = 0; i < 3; i++)
            {
                var (z0, z1) = mt.NextGaussian(0.0, 1.0);
                System.Console.WriteLine($"{z0}, {z1}");
            }
        }

        private void distinct测试()
        {
            void Test_AllDistinctCases()
            {
                // int
                {
                    var input = new List<int> { 1, 2, 2, 3, 1, 4 };
                    var (values, counts) = MyDistinct.distinct_by_value(input);
                    Assert.Equal(new[] { 1, 2, 3, 4 }, values);
                    Assert.Equal(new[] { 2, 2, 1, 1 }, counts);
                }

                // double
                {
                    var input = new List<double> { 1.1, 2.2, 2.2, 3.3, 1.1, 4.4 };
                    var (values, counts) = MyDistinct.distinct_by_value(input);
                    Assert.Equal(new[] { 1.1, 2.2, 3.3, 4.4 }, values);
                    Assert.Equal(new[] { 2, 2, 1, 1 }, counts);
                }

                // nullable int, keep null
                {
                    var input = new List<int?> { 1, 2, 2, null, 1, 4, null };
                    var (values, counts) = MyDistinct.distinct_by_nullable_value(input, true);
                    Assert.Equal(new int?[] { null, 1, 2, 4 }, values);
                    Assert.Equal(new[] { 2, 2, 2, 1 }, counts);
                }

                // nullable int, without null
                {
                    var input = new List<int?> { 1, 2, 2, null, 1, 4, null };
                    var (values, counts) = MyDistinct.distinct_by_nullable_value(input, false);
                    Assert.Equal(new int?[] { 1, 2, 4 }, values);
                    Assert.Equal(new[] { 2, 2, 1 }, counts);
                }

                // nullable double, keep null
                {
                    var input = new List<double?> { 1.1, 2.2, null, 2.2, 1.1, 4.4, null };
                    var (values, counts) = MyDistinct.distinct_by_nullable_value(input, true);
                    Assert.Equal(new double?[] { null, 1.1, 2.2, 4.4 }, values);
                    Assert.Equal(new[] { 2, 2, 2, 1 }, counts);
                }

                // nullable double, without null
                {
                    var input = new List<double?> { 1.1, 2.2, null, 2.2, 1.1, 4.4, null };
                    var (values, counts) = MyDistinct.distinct_by_nullable_value(input, false);
                    Assert.Equal(new double?[] { 1.1, 2.2, 4.4 }, values);
                    Assert.Equal(new[] { 2, 2, 1 }, counts);
                }

                // nullable struct, keep null
                {
                    var input = new List<DateTime?>
                        { new DateTime(2020, 1, 1), null, new DateTime(2020, 1, 1), new DateTime(2021, 1, 1), null };
                    var (values, counts) = MyDistinct.distinct_by_nullable_value(input, true);
                    Assert.Equal(new DateTime?[] { null, new DateTime(2020, 1, 1), new DateTime(2021, 1, 1) }, values);
                    Assert.Equal(new[] { 2, 2, 1 }, counts);
                }

                // nullable struct, without null
                {
                    var input = new List<DateTime?>
                        { new DateTime(2020, 1, 1), null, new DateTime(2020, 1, 1), new DateTime(2021, 1, 1), null };
                    var (values, counts) = MyDistinct.distinct_by_nullable_value(input, false);
                    Assert.Equal(new DateTime?[] { new DateTime(2020, 1, 1), new DateTime(2021, 1, 1) }, values);
                    Assert.Equal(new[] { 2, 1 }, counts);
                }

                // string
                {
                    var input = new List<string> { "a", "b", null, "a", "c", null };
                    var (values, counts) = MyDistinct.distinct_by_reference(input);
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
                    var (values, counts) = MyDistinct.distinct_by_reference(input);
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
                    var (values, counts) = MyDistinct.distinct_by_reference(input, false);
                    // Assert.Equal(
                    //     new[] { SpatialIndex.create(1, 2), SpatialIndex.create(3, 4), SpatialIndex.create(5, 6) },
                    //     values);
                    // Assert.Equal(new[] { 2, 1, 1 }, counts);
                }
            }

            Test_AllDistinctCases();

            var arr = new int?[] { 1, 2, 2, null, 3, null, 3 };
            var res = MyDistinct.distinct_by_nullable_value(arr, keep_null: true);
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