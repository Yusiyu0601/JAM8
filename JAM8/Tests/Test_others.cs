using System.Diagnostics;
using System.Text;
using JAM8.Algorithms.Geometry;
using JAM8.Algorithms.Numerics;
using JAM8.Utilities;

namespace JAM8.Tests
{
    public class Test_others
    {
        public static void Test_GLCM()
        {
            OpenFileDialog ofd = new();
            if (ofd.ShowDialog() != DialogResult.OK)
                return;
        }

        public static void Test_提取kriging里的条件数据和粗网格点()
        {
            Grid g = Grid.create_from_gslibwin().grid;
            GridProperty gp = GridProperty.create(g.select_gridProperty_win().grid_property,
                (0.5f, 0, CompareType.LessThan),
                (0.5f, 1, CompareType.GreaterThan)
                );
            gp.show_win();

        }

        public static void Test_Grid转换为cd()
        {
            Grid g = Grid.create_from_gslibwin().grid;
            GridProperty gp = g.select_gridProperty_win().grid_property;
            CData cd = CData.create_from_gridProperty(gp, null, false);
            cd.save_to_gslibwin();

        }


        public static void 自然数的组合计算3()
        {
            int[] a = new int[10];
            a[0] = 3;
            void comb(int m, int k)
            {
                int i, j;
                for (i = m; i >= k; i--)
                {
                    a[k] = i;
                    if (k > 1)
                        comb(i - 1, k - 1);
                    else
                    {
                        for (j = a[0]; j > 0; j--)
                            Console.Write("{0},", a[j]);
                        Console.Write("\n");
                    }
                }
            }
            comb(5, 3);
        }


        public static void 自然数的组合计算2()
        {
            int n = 6; // 自然数范围
            int r = 3; // 取几个数
                       // 数组用来保存组合结果
            int[] arr = new int[r];
            Console.Write("组合结果为:\n");
            combrecur(n, r, arr, 0);
        }

        private static void combrecur(int n, int r, int[] arr, int index)
        {
            if (r == 0)
            {
                for (int i = 0; i < index; i++)
                {
                    Console.Write(arr[i] + ",");
                }
                Console.Write("\n");
                return;
            }
            if (n <= 0 || n < r)
            {
                return;
            }
            // 选择当前数并继续递归
            arr[index] = n;
            combrecur(n - 1, r - 1, arr, index + 1);
            // 不选择当前数并继续递归
            combrecur(n - 1, r, arr, index);
        }

        private static int[] a = new int[100];
        public static void 自然数的组合计算()
        {
            int m = 6, k = 3;
            for (int i = m; i >= k; i--)
            {
                function(i, k);
            }
        }

        private static void function(int m, int k)
        {
            if (k == 1)
            {
                a[k - 1] = m;
                int j = 0;
                while (a[j] != 0)
                    j += 1;
                for (int i = j - 1; i >= 0; i--)
                {
                    Console.Write(a[i] + ",");
                }
                Console.Write("\n");
            }
            else
            {
                a[k - 1] = m;
                for (int j = m - 1; j >= k - 1; j--)
                {
                    function(j, k - 1);
                }
            }
        }

        public static void 速度测试()
        {
            Stopwatch sw = new();
            sw.Start();

            double result = 0;
            Parallel.For(0, 1000000000, i =>
            {
                double r = i * 3.14 / 180;
                result = Math.Sqrt(Math.Sin(r) * Math.Cos(r));
            });

            sw.Stop();
            Console.WriteLine(sw.ElapsedMilliseconds);
        }

        public static void ReadLargeTxtFile()
        {
            string fileName = FileDialogHelper.OpenText();

            //方式1
            //Stopwatch sw = new();
            //sw.Start();
            //using StreamReader sr = new(fileName);//读取所有行数据
            //string[] all_lines = sr.ReadToEnd().Split(new char[] { '\n' });
            //sw.Stop();
            //Console.WriteLine(sw.ElapsedMilliseconds );

            //方式2
            //string g = String.Join("", Enumerable.Repeat(new Guid().ToString(), 200000));
            List<string> array = new();
            using FileStream fs = File.Open(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            using BufferedStream bs = new(fs, 1024 * 1000);
            using StreamReader sr1 = new(bs);
            string s;

            Stopwatch sw1 = new();
            sw1.Start();
            while ((s = sr1.ReadLine()) != null)
            {
                array.Add(s);
            }
            sw1.Stop();
            Console.WriteLine(sw1.ElapsedMilliseconds);

        }

        public static void ReadTxtFile对比()
        {
            DateTime end;
            DateTime start = DateTime.Now;
            Console.WriteLine("### Overall Start Time: " + start.ToLongTimeString());
            Console.WriteLine();
            TestReadingLinesFromFile((int)Math.Floor((double)(Int32.MaxValue / 500)), 5);
            TestReadingLinesFromFile((int)Math.Floor((double)(Int32.MaxValue / 500)), 10);
            TestReadingLinesFromFile((int)Math.Floor((double)(Int32.MaxValue / 500)), 25);
            TestReadingLinesFromFile((int)Math.Floor((double)(Int32.MaxValue / 1000)), 5);
            TestReadingLinesFromFile((int)Math.Floor((double)(Int32.MaxValue / 1000)), 10);
            TestReadingLinesFromFile((int)Math.Floor((double)(Int32.MaxValue / 1000)), 25);
            TestReadingLinesFromFile((int)Math.Floor((double)(Int32.MaxValue / 10000)), 5);
            TestReadingLinesFromFile((int)Math.Floor((double)(Int32.MaxValue / 10000)), 10);
            TestReadingLinesFromFile((int)Math.Floor((double)(Int32.MaxValue / 10000)), 25);
            end = DateTime.Now;
            Console.WriteLine();
            Console.WriteLine("### Overall End Time: " + end.ToLongTimeString());
            Console.WriteLine("### Overall Run Time: " + (end - start));
            Console.WriteLine();
            Console.WriteLine(@"Hit Enter to Exit");
            Console.ReadLine();
        }
        //####################################################
        //Does a comparison of reading all the lines in from a file. Which way is fastest?
        private static void TestReadingLinesFromFile(int numberOfLines, int numTimesGuidRepeated)
        {
            Console.WriteLine("######## " + System.Reflection.MethodBase.GetCurrentMethod().Name);
            Console.WriteLine("######## Number of lines in file: " + numberOfLines);
            Console.WriteLine("######## Number of times Guid repeated on each line: " + numTimesGuidRepeated);
            Console.WriteLine(@"###########################################################");
            Console.WriteLine();
            string g = String.Join("", Enumerable.Repeat(new Guid().ToString(), numTimesGuidRepeated));
            string[] AllLines = null;
            string fileName = "Performance_Test_File.txt";
            int MAX = numberOfLines;
            DateTime end;
            DateTime start = DateTime.Now;
            //Create the file populating it with GUIDs
            Console.WriteLine("Generating file: " + start.ToLongTimeString());
            using (StreamWriter sw = File.CreateText(fileName))
            {
                for (int x = 0; x < MAX; x++)
                {
                    sw.WriteLine(g);
                }
            }
            end = DateTime.Now;
            Console.WriteLine("Finished at: " + end.ToLongTimeString());
            Console.WriteLine("Time: " + (end - start));
            Console.WriteLine();



            /*               1               */
            GC.Collect();
            Thread.Sleep(1000);     //give disk hardware time to recover
            //Just read everything into one string
            Console.WriteLine(@"Reading file reading to end into string: ");
            start = DateTime.Now;
            try
            {
                using (StreamReader sr = File.OpenText(fileName))
                {
                    string s = sr.ReadToEnd();
                    //Obviously you'd then have to process the string
                }
                end = DateTime.Now;
                Console.WriteLine("Finished at: " + end.ToLongTimeString());
                Console.WriteLine("Time: " + (end - start));
                Console.WriteLine();
            }
            catch (OutOfMemoryException)
            {
                end = DateTime.Now;
                Console.WriteLine(@"Not enough memory. Couldn't perform this test.");
                Console.WriteLine("Finished at: " + end.ToLongTimeString());
                Console.WriteLine("Time: " + (end - start));
                Console.WriteLine();
            }
            catch (Exception)
            {
                end = DateTime.Now;
                Console.WriteLine(@"EXCEPTION. Couldn't perform this test.");
                Console.WriteLine("Finished at: " + end.ToLongTimeString());
                Console.WriteLine("Time: " + (end - start));
                Console.WriteLine();
            }



            /*               2               */
            GC.Collect();
            Thread.Sleep(1000);     //give disk hardware time to recover
            //Read the entire contents into a StringBuilder object
            Console.WriteLine(@"Reading file reading to end into stringbuilder: ");
            start = DateTime.Now;
            try
            {
                using (StreamReader sr = File.OpenText(fileName))
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append(sr.ReadToEnd());
                    //Obviously you'd then have to process the string
                }
                end = DateTime.Now;
                Console.WriteLine("Finished at: " + end.ToLongTimeString());
                Console.WriteLine("Time: " + (end - start));
                Console.WriteLine();
            }
            catch (OutOfMemoryException)
            {
                end = DateTime.Now;
                Console.WriteLine(@"Not enough memory. Couldn't perform this test.");
                Console.WriteLine("Finished at: " + end.ToLongTimeString());
                Console.WriteLine("Time: " + (end - start));
                Console.WriteLine();
            }
            catch (Exception)
            {
                end = DateTime.Now;
                Console.WriteLine(@"EXCEPTION. Couldn't perform this test.");
                Console.WriteLine("Finished at: " + end.ToLongTimeString());
                Console.WriteLine("Time: " + (end - start));
                Console.WriteLine();
            }



            /*               3               */
            GC.Collect();
            Thread.Sleep(1000);     //give disk hardware time to recover
            //Standard and probably most common way of reading a file. 
            Console.WriteLine(@"Reading file assigning each line to string: ");
            start = DateTime.Now;
            using (StreamReader sr = File.OpenText(fileName))
            {
                string s = String.Empty;
                while ((s = sr.ReadLine()) != null)
                {
                    //we're just testing read speeds
                }
            }
            end = DateTime.Now;
            Console.WriteLine("Finished at: " + end.ToLongTimeString());
            Console.WriteLine("Time: " + (end - start));
            Console.WriteLine();



            /*               4               */
            GC.Collect();
            Thread.Sleep(1000);     //give disk hardware time to recover
            //Doing it the most common way, but using a Buffered Reader now.
            Console.WriteLine(@"Buffered reading file assigning each line to string: ");
            start = DateTime.Now;
            using (FileStream fs = File.Open(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (BufferedStream bs = new BufferedStream(fs))
            using (StreamReader sr = new StreamReader(bs))
            {
                string s;
                while ((s = sr.ReadLine()) != null)
                {
                    //we're just testing read speeds
                }
            }
            end = DateTime.Now;
            Console.WriteLine("Finished at: " + end.ToLongTimeString());
            Console.WriteLine("Time: " + (end - start));
            Console.WriteLine();


            /*               5               */
            GC.Collect();
            Thread.Sleep(1000);     //give disk hardware time to recover
            //Reading each line using a buffered reader again, but setting the buffer size since we know what it will be.
            Console.WriteLine(@"Buffered reading with preset buffer size assigning each line to string: ");
            start = DateTime.Now;
            using (FileStream fs = File.Open(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (BufferedStream bs = new BufferedStream(fs, System.Text.ASCIIEncoding.Unicode.GetByteCount(g)))
            using (StreamReader sr = new StreamReader(bs))
            {
                string s;
                while ((s = sr.ReadLine()) != null)
                {
                    //we're just testing read speeds
                }
            }
            end = DateTime.Now;
            Console.WriteLine("Finished at: " + end.ToLongTimeString());
            Console.WriteLine("Time: " + (end - start));
            Console.WriteLine();


            /*               6               */
            GC.Collect();
            Thread.Sleep(1000);     //give disk hardware time to recover
            //Read every line of the file reusing a StringBuilder object to save on string memory allocation times
            Console.WriteLine(@"Reading file assigning each line to StringBuilder: ");
            start = DateTime.Now;
            using (StreamReader sr = File.OpenText(fileName))
            {
                StringBuilder sb = new StringBuilder();
                while (sb.Append(sr.ReadLine()).Length > 0)
                {
                    //we're just testing read speeds
                    sb.Clear();
                }
            }
            end = DateTime.Now;
            Console.WriteLine("Finished at: " + end.ToLongTimeString());
            Console.WriteLine("Time: " + (end - start));
            Console.WriteLine();



            /*               7               */
            GC.Collect();
            Thread.Sleep(1000);     //give disk hardware time to recover
            //Reading each line into a StringBuilder, but setting the StringBuilder object to an initial
            //size since we know how long the longest line in the file is.
            Console.WriteLine(@"Reading file assigning each line to preset size StringBuilder: ");
            start = DateTime.Now;
            using (StreamReader sr = File.OpenText(fileName))
            {
                StringBuilder sb = new StringBuilder(g.Length);
                while (sb.Append(sr.ReadLine()).Length > 0)
                {
                    //we're just testing read speeds
                    sb.Clear();
                }
            }
            end = DateTime.Now;
            Console.WriteLine("Finished at: " + end.ToLongTimeString());
            Console.WriteLine("Time: " + (end - start));
            Console.WriteLine();



            /*               8               */
            GC.Collect();
            Thread.Sleep(1000);     //give disk hardware time to recover
            //Read each line into an array index. 
            Console.WriteLine(@"Reading each line into string array: ");
            start = DateTime.Now;
            try
            {
                AllLines = new string[MAX];    //only allocate memory here
                using (StreamReader sr = File.OpenText(fileName))
                {
                    int x = 0;
                    while (!sr.EndOfStream)
                    {
                        //we're just testing read speeds
                        AllLines[x] = sr.ReadLine();
                        x += 1;
                    }
                }
                end = DateTime.Now;
                Console.WriteLine("Finished at: " + end.ToLongTimeString());
                Console.WriteLine("Time: " + (end - start));
                Console.WriteLine();
            }
            catch (OutOfMemoryException)
            {
                end = DateTime.Now;
                Console.WriteLine(@"Not enough memory. Couldn't perform this test.");
                Console.WriteLine("Finished at: " + end.ToLongTimeString());
                Console.WriteLine("Time: " + (end - start));
                Console.WriteLine();
            }
            catch (Exception)
            {
                end = DateTime.Now;
                Console.WriteLine(@"EXCEPTION. Couldn't perform this test.");
                Console.WriteLine("Finished at: " + end.ToLongTimeString());
                Console.WriteLine("Time: " + (end - start));
                Console.WriteLine();
            }
            finally
            {
                if (AllLines != null)
                {
                    Array.Clear(AllLines, 0, AllLines.Length);
                    AllLines = null;
                }
            }



            /*               9               */
            GC.Collect();
            Thread.Sleep(1000);
            //Read the entire file using File.ReadAllLines. 
            Console.WriteLine(@"Performing File ReadAllLines into array: ");
            start = DateTime.Now;
            try
            {
                AllLines = new string[MAX];    //only allocate memory here
                AllLines = File.ReadAllLines(fileName);
                end = DateTime.Now;
                Console.WriteLine("Finished at: " + end.ToLongTimeString());
                Console.WriteLine("Time: " + (end - start));
                Console.WriteLine();
            }
            catch (OutOfMemoryException)
            {
                end = DateTime.Now;
                Console.WriteLine(@"Not enough memory. Couldn't perform this test.");
                Console.WriteLine("Finished at: " + end.ToLongTimeString());
                Console.WriteLine("Time: " + (end - start));
                Console.WriteLine();
            }
            catch (Exception)
            {
                end = DateTime.Now;
                Console.WriteLine(@"EXCEPTION. Couldn't perform this test.");
                Console.WriteLine("Finished at: " + end.ToLongTimeString());
                Console.WriteLine("Time: " + (end - start));
                Console.WriteLine();
            }
            finally
            {
                if (AllLines != null)
                {
                    Array.Clear(AllLines, 0, AllLines.Length);
                    AllLines = null;
                }
            }
            File.Delete(fileName);
            fileName = null;
            GC.Collect();
        }


        public static void 三个数字排序()
        {
            int x = 10, y = 30, z = 30;
            Dictionary<string, double> temp = new();
            Stopwatch sw = new();
            sw.Start();
            for (z = 0; z < 100; z++)
            {
                for (y = 0; y < 500; y++)
                {
                    for (x = 0; x < 500; x++)
                    {
                        (x, y, z) = sorted(x, y, z);
                        string key = $"{x}_{y}_{z}";
                        double dist = Math.Sqrt(x * x + y * y + z * z);
                        temp.TryAdd(key, dist);
                    }
                }
            }
            sw.Stop();



            Console.WriteLine($@"{sw.ElapsedMilliseconds}");
        }

        private static (int x, int y, int z) sorted(int x, int y, int z)
        {
            int t;
            if (x > y)
            {
                t = x;
                x = y;
                y = t;
            }

            if (x > z)
            {
                t = x;
                x = z;
                z = t;
            }

            if (y > z)
            {
                t = y;
                y = z;
                z = t;
            }
            return (x, y, z);
        }

        public static void 根据学生平均分给N个单次分()
        {
            MyConsoleHelper.write_string_to_console("Excel文件至少包含 <姓名> 和 <平时成绩> 两列");
            //导入excel
            MyDataFrame df = MyDataFrame.read_from_excel();
            df.show_win();

            List<string> series_names = new();
            series_names.Add("姓名");
            series_names.Add("平时成绩");

            int N = MyConsoleHelper.read_int_from_console("输入次数="); ;
            for (int i = 1; i <= N; i++)
            {
                series_names.Add($"{i}");
            }
            MyDataFrame result = MyDataFrame.create(series_names);

            Random rnd = new(11);
            //
            for (int idx_record = 0; idx_record < df.N_Record; idx_record++)
            {
                var record = result.new_record();
                record["姓名"] = df[idx_record, "姓名"];
                var 平时成绩 = float.Parse(df[idx_record, "平时成绩"].ToString());
                record["平时成绩"] = df[idx_record, "平时成绩"];

                while (true)
                {
                    Gaussian gau = new(平时成绩, 2);
                    var array = gau.sample(N, rnd);

                    var 平均值 = array.Average(a => (int)a);
                    if (平均值 != 平时成绩)
                        continue;
                    else
                    {
                        for (int i = 1; i <= N; i++)
                        {
                            record[$"{i}"] = (int)array[i - 1];
                        }
                        break;
                    }
                }

                MyConsoleHelper.write_string_to_console(record["姓名"].ToString());
                result.add_record(record);
            }
            result.show_win();
        }
    }
}
