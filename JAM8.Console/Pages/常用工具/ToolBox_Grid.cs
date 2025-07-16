using EasyConsole;
using JAM8.Algorithms.Forms;
using JAM8.Algorithms.Geometry;
using JAM8.SpecificApps.常用工具;
using JAM8.Utilities;

namespace JAM8.Console.Pages
{
    class ToolBox_Grid : Page
    {
        public ToolBox_Grid(EasyConsole.Program program) :
            base(
                "ToolBox_Grid",
                program
            )
        {
        }

        public override void Display()
        {
            base.Display();

            Output.WriteLine(ConsoleColor.Green, "ToolBox_Grid 功能：");

            Perform();

            System.Console.WriteLine();
            EasyConsole.Output.WriteLine(ConsoleColor.Green, "按任意键返回");
            System.Console.ReadKey();

            Program.NavigateBack();
        }

        private void Perform()
        {
            var menu = new EasyConsole.Menu()
                    .Add("Back", CommonFunctions.Cancel)
                    .Add("Grid显示", Grid显示)
                    .Add("GridCatalog显示", GridCatalog显示)
                    .Add("Grid与Image转换", Grid与Image转换)
                    .Add("Grid手动编辑", Grid手动编辑)
                    .Add("Grid转换为CData", Grid转换为CData)
                    .Add("Grid过滤替换数值", Grid过滤替换数值)
                    .Add("Grid采样点", Grid采样点)
                    .Add("Grid拆分多个GridProperty", Grid拆分多个GridProperty)
                    .Add("多个GridProperty合并为一个Grid", 多个GridProperty合并为一个Grid)
                    .Add("GridProperty距离计算", GridProperty距离计算)
                ;

            menu.Display();
        }

        private void Grid拆分多个GridProperty()
        {
            Grid g = Grid.create_from_gslibwin().grid;
            FolderBrowserDialog fbd = new();
            if (fbd.ShowDialog() != DialogResult.OK)
                return;
            foreach (var (name, gp) in g)
            {
                Grid g1 = Grid.create(g.gridStructure);
                g1.add_gridProperty(name, gp);
                string fileName = Path.Combine(fbd.SelectedPath, $"realization_{name}.out");
                g1.save_to_gslib(fileName, g.grid_name, -99);
            }
        }

        /// <summary>
        /// 计算两两GridProperty之间的距离，并导出为距离矩阵
        /// </summary>
        private void GridProperty距离计算()
        {
            // 计算 Patterns 之间的对称样式距离
            double GetSymmetricPatternDistance(Patterns patsA, Patterns patsB)
            {
                if (patsA.Count == 0 || patsB.Count == 0)
                    return double.NaN; // 或 double.MaxValue，看你偏好如何处理空样式

                // 从 A 到 B 的平均最小距离
                double avg_A_to_B = patsA
                    .AsParallel()
                    .Select(pairA =>
                        patsB.Min(pairB =>
                            Mould.get_distance(pairA.Value, pairB.Value)
                        )
                    ).Average();

                // 从 B 到 A 的平均最小距离
                double avg_B_to_A = patsB
                    .AsParallel()
                    .Select(pairB =>
                        patsA.Min(pairA =>
                            Mould.get_distance(pairB.Value, pairA.Value)
                        )
                    ).Average();

                return (avg_A_to_B + avg_B_to_A) / 2.0;
            }

            Grid g = Grid.create_from_gslibwin().grid;

            g.showGrid_win("导入的Grid");


            Mould mould = Mould.create_by_ellipse(5, 5, 2, 1);

            // 创建对称距离矩阵
            double[,] distance_matrix = new double[g.N_gridProperties, g.N_gridProperties];

            //计算两两模型属性之间的距离
            for (int i = 0; i < g.N_gridProperties; i++)
            {
                for (int j = i; j < g.N_gridProperties; j++)
                {
                    if (i == j)
                    {
                        distance_matrix[i, j] = 0.0; // 对角线距离为 0
                        continue;
                    }

                    var pats_i = Patterns.create(mould, g[i].resize(0.3, 0.3, 0.3), true, true); // 如果支持释放
                    var pats_j = Patterns.create(mould, g[j].resize(0.3, 0.3, 0.3), true, true);
                    MyConsoleHelper.write_string_to_console($"pattern数量{pats_i.Count} {pats_j.Count}");
                    double dist = GetSymmetricPatternDistance(pats_i, pats_j);

                    MyConsoleHelper.write_string_to_console($"[{i}-{j}]距离={dist}");

                    distance_matrix[i, j] = dist;
                    distance_matrix[j, i] = dist; // 保持对称性
                }
            }

            MyDataFrame df = MyDataFrame.create_from_array([.. g.propertyNames], distance_matrix);
            df.show_win("GridProperty距离矩阵");
            MyDataFrame.write_to_excel(df, "GridProperty距离矩阵.xlsx");
        }

        private void 多个GridProperty合并为一个Grid()
        {
            try
            {
                GridStructure gs = GridStructure.create_win();
                Grid g = Grid.create(gs);
                OpenFileDialog ofd = new()
                {
                    Multiselect = true
                };

                if (ofd.ShowDialog() != DialogResult.OK)
                    return;

                var split_code = EasyConsole.Input.ReadInt("输入split_code:", 0, 4);
                var null_value = EasyConsole.Input.ReadString("输入null_value:");
                foreach (var fileName in ofd.FileNames)
                {
                    System.Console.WriteLine(fileName);

                    var g1 = Grid.create(gs);
                    g1.read_from_gslib(fileName, split_code, Convert.ToSingle(null_value));
                    g.add_gridProperty(FileHelper.GetFileName(fileName, false), g1.first_gridProperty());
                }

                g.showGrid_win();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void Grid采样点()
        {
            Form_GridSampling frm = new();
            frm.ShowDialog();
        }

        private void Grid过滤替换数值()
        {
            Form_GridFilter frm = new();
            if (frm.ShowDialog() != DialogResult.OK)
                return;
        }

        private void Grid转换为CData()
        {
            Form_Grid2CData frm = new();
            if (frm.ShowDialog() != DialogResult.OK)
                return;
        }

        private void Grid手动编辑()
        {
            Form_GridEditor frm = new();
            if (frm.ShowDialog() != DialogResult.OK)
                return;
        }

        private void Grid与Image转换()
        {
            Form_Pic2GSLIB frm = new();
            if (frm.ShowDialog() != DialogResult.OK)
                return;
        }

        private void GridCatalog显示()
        {
            Form_GridCatalog frm = new();
            if (frm.ShowDialog() != DialogResult.OK)
                return;
        }

        private void Grid显示()
        {
            Grid g = Grid.create_from_gslibwin().grid;
            if (g != null)
                g.showGrid_win();
        }
    }
}