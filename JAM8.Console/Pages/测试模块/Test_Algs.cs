using EasyConsole;
using JAM8.Algorithms.Forms;
using JAM8.Algorithms.Geometry;
using JAM8.Algorithms.Numerics;
using JAM8.SpecificApps.常用工具;
using JAM8.Utilities;

namespace JAM8.Console.Pages
{
    internal class Test_Algs : Page
    {
        public Test_Algs(EasyConsole.Program program) : base("Test_Algs", program) { }

        public override void Display()
        {
            base.Display();

            Output.WriteLine(ConsoleColor.Green, "Test_Algs 功能：");

            Perform();

            System.Console.WriteLine("按任意键返回");
            System.Console.ReadKey();

            Program.NavigateBack();
        }

        private void Perform()
        {
            var menu = new EasyConsole.Menu()

           .Add("退出", CommonFunctions.Cancel)
           .Add("Snesim测试（正逆查询树）", Snesim测试)
           .Add("统计", 统计)
           ;

            menu.Display();
        }

        private void 统计()
        {
            MyDataFrame df = MyDataFrame.read_from_excel();
            df.show_win("", true);

            // 创建一个包含90002个元素的数组（示例数据）
            int totalElements = df.N_Record;
            int[] arr = new int[totalElements];
            Random rand = new Random();
            for (int i = 0; i < df.N_Record; i++)
            {
                var value = df[i, 0];
                arr[i] = Convert.ToInt32(value);
            }
            // 定义分块数目为101（包括边界）
            int numChunks = 101;
            double[] means = new double[numChunks];

            // 分块计算
            for (int i = 0; i < numChunks; i++)
            {
                // 确定当前分块的范围
                int start = i * (totalElements / numChunks);
                int end = (i == numChunks - 1) ? totalElements : (i + 1) * (totalElements / numChunks);

                // 计算该分块的均值
                double sum = 0;
                for (int j = start; j < end; j++)
                {
                    sum += arr[j];
                }

                means[i] = sum / (end - start);
            }

            MyDataFrame new_df = MyDataFrame.create(["NodeCountAverage"]);
            for (int i = 0; i < means.Length; i++)
            {
                new_df.add_record([means[i]]);
            }
            new_df.show_win("NodeCountAverage", true);
        }

        private void Snesim测试()
        {
            Output.WriteLine(ConsoleColor.Yellow, "加载训练图像");
            Grid g = null;
            Form_GridCatalog frm = new();
            if (frm.ShowDialog() != DialogResult.OK)
            {
                g = Grid.create_from_gslibwin().grid;
            }
            else
            {
                g = frm.selected_grids.FirstOrDefault();
            }

            if (g == null)
                return;

            var ti = g.first_gridProperty();
            Mould mould = ti.gridStructure.dim == Dimension.D2 ? Mould.create_by_ellipse(10, 10, 1) :
                Mould.create_by_ellipse(7, 7, 2, 1);
            mould = Mould.create_by_mould(mould, 20);

            //手动测试
            int progress_for_inverse_retrieve = EasyConsole.Input.ReadInt("逆向查询占比", 0, 100);
            Snesim snesim = Snesim.create();
            var (re, time) = snesim.run(ti, null, GridStructure.create_simple(100, 100, 1), 1001, mould, 1, progress_for_inverse_retrieve);
            //re?.showGrid_win();
            Output.WriteLine(ConsoleColor.Red, $"{time}毫秒");

            //批量测试
            //MyDataFrame df = MyDataFrame.create(["占比", "时间"]);
            //for (int i = 0; i <= 100; i += 5)
            //{
            //    int progress_for_inverse_retrieve = i;

            //    Snesim snesim = Snesim.create();
            //    var (re, time) = snesim.run(ti, null, ti.gridStructure, 1001, mould, 1, progress_for_inverse_retrieve);
            //    //re?.showGrid_win();
            //    Output.WriteLine(ConsoleColor.Red, $"{time}毫秒");

            //    df.add_record([i, time]);
            //}
            //df.show_win("逆向查询占比与时间", true);
        }
    }
}
