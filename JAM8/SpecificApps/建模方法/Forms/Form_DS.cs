using JAM8.Algorithms.Geometry;
using JAM8.Utilities;

namespace JAM8.SpecificApps.建模方法
{
    public partial class Form_DS : Form
    {
        Grid g_ti;
        CData cd;
        Grid g_re;
        string file_name_ti;
        string file_name_cd;

        public Form_DS()
        {
            InitializeComponent();

#if DEBUG
            button6.Visible = true;
#elif RELEASE
            button6.Visible = false;
#endif
        }

        private void button2_Click(object sender, EventArgs e)
        {
            (g_ti, file_name_ti) = Grid.create_from_gslibwin();//ti
            if (g_ti == null)
                return;

            textBox9.Text = file_name_ti;

            #region 加载属性列表

            listBox1.Items.Clear();
            foreach (var PropertyName in g_ti.propertyNames)
            {
                listBox1.Items.Add(PropertyName);
            }

            #endregion
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex == -1 || listBox1.SelectedItem.ToString() == string.Empty)
                return;
            else
                scottplot4GridProperty1.update_gridProperty(g_ti[listBox1.SelectedItem.ToString()]);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            (cd, file_name_cd) = CData.read_from_gslibwin();//cd

            textBox10.Text = file_name_cd;

            #region 加载属性列表

            listBox2.Items.Clear();
            foreach (var PropertyName in cd.propertyNames)
            {
                listBox2.Items.Add(PropertyName);
            }

            #endregion
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox2.SelectedIndex == -1 || listBox2.SelectedItem.ToString() == string.Empty)
                return;
            else
            {
                var boundary = cd.get_boundary();
                int nx = 100;
                int ny = 100;
                int nz = cd.dim == Dimension.D2 ? 1 : 10;
                float xsiz = (boundary.max_x - boundary.min_x) / 100;
                float ysiz = (boundary.max_y - boundary.min_y) / 100;
                float? zsiz = cd.dim == Dimension.D2 ? 1.0f : (boundary.max_z - boundary.min_z) / 10;
                float xmn = boundary.min_x;
                float ymn = boundary.min_y;
                float? zmn = cd.dim == Dimension.D2 ? 0.5f : boundary.min_z;
                GridStructure gs = GridStructure.create(nx, ny, nz, xsiz, ysiz, zsiz.Value, xmn, ymn, zmn.Value);
                var g_cd = cd.assign_to_grid(gs);
                scottplot4GridProperty2.update_gridProperty(g_cd.grid_assigned[listBox2.SelectedItem.ToString()]);
            }
        }

        //模拟
        private void button3_Click(object sender, EventArgs e)
        {
            radioButton1.Checked = false;
            radioButton1.Checked = false;

            if (listBox1.SelectedIndex == -1 || listBox1.SelectedItem.ToString() == string.Empty)
                return;

            int nx = int.Parse(textBox1.Text);
            int ny = int.Parse(textBox2.Text);
            int nz = int.Parse(textBox3.Text);
            var gs = GridStructure.create_with_old_size_origin(nx, ny, nz, g_ti.gridStructure);//re
            DirectSampling ds = null;
            if (cd != null)
                ds = DirectSampling.create(gs, g_ti[listBox1.SelectedItem.ToString()], cd, cd.propertyNames[0]);
            else
                ds = DirectSampling.create(gs, g_ti[listBox1.SelectedItem.ToString()]);

            int search_radius = int.Parse(textBox5.Text);
            float max_fraction = float.Parse(textBox4.Text);
            int max_number = int.Parse(textBox7.Text);
            float distance_threshold = float.Parse(textBox6.Text);
            int random_seed = int.Parse(textBox8.Text);
            g_re = ds.run(search_radius, max_number, max_fraction, distance_threshold, random_seed);
            radioButton1.Checked = true;
        }

        //保存
        private void button1_Click(object sender, EventArgs e)
        {
            Grid.save_to_gslibwin(g_re);
        }

        //显示re
        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (g_re == null)
                return;
            scottplot4GridProperty3.update_gridProperty(g_re["re"]);
        }

        //显示cd
        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (g_re == null)
                return;
            scottplot4GridProperty3.update_gridProperty(g_re["cd"]);
        }

        //批量模拟
        private void button4_Click(object sender, EventArgs e)
        {
            Console.WriteLine("输入批量模拟的次数");
            int N = int.Parse(Console.ReadLine());
            Console.WriteLine("设置保存路径");
            FolderBrowserDialog fbd = new();
            if (fbd.ShowDialog() != DialogResult.OK)
                return;

            Random rnd = new(int.Parse(textBox8.Text));
            string name_ti = FileHelper.GetFileName(file_name_ti, false);
            string name_cd = FileHelper.GetFileName(file_name_cd, false);
            for (int i = 0; i < N; i++)
            {
                textBox8.Text = rnd.Next().ToString();
                button3_Click(sender, e);
                string file_name = $"{fbd.SelectedPath}\\ti[{name_ti}]_cd[{name_cd}]_{i}.out";
                string grid_name = $"ti[{name_ti}]_cd[{name_cd}]";
                scottplot4GridProperty3.update_gridProperty(g_re["re"]);
                g_re.save_to_gslib(file_name, grid_name, -99);
            }
        }

        //隐藏功能:批量模拟2
        private void button6_Click(object sender, EventArgs e)
        {
            Console.WriteLine("程序目标:实现批量导入不同cd，用相同ti进行建模，其中建模参数相同");
            Grid g_ti = Grid.create_from_gslibwin("打开训练图像").grid;
            GridProperty gp_ti = g_ti.select_gridProperty_win("选择作为训练图像的gridProperty").grid_property;
            Grid g_cd = Grid.create_from_gslibwin("打开Grid形式的cdata").grid;
            g_cd.showGrid_win("Grid形式的cdata");

            Console.WriteLine("设置保存路径");
            FolderBrowserDialog fbd = new();
            if (fbd.ShowDialog() != DialogResult.OK)
                return;

            var gs = GridStructure.create_win(g_ti.gridStructure);//re
            Grid g_res = Grid.create(gs);
            foreach (var cd_gp_name in g_cd.propertyNames)
            {
                CData cd = CData.create_from_gridProperty(g_cd, cd_gp_name, null, false);
                DirectSampling ds = null;
                if (cd != null)
                    ds = DirectSampling.create(gs, gp_ti, cd, cd.propertyNames[0]);
                else
                    ds = DirectSampling.create(gs, gp_ti);

                var g_re = ds.run(search_radius: 20, maximum_number: 15, maximum_fraction: 0.5, distance_threshold: 0.01, random_seed: 12131);
                foreach (var (gp_name, gp) in g_re)
                {
                    g_res.add_gridProperty($"{cd_gp_name}_{gp_name}", gp);
                }

                string file_name = $"{fbd.SelectedPath}\\{cd_gp_name}.out";
                string grid_name = $"{cd_gp_name}";
                g_re.save_to_gslib(file_name, grid_name, -99);
            }
            g_res.showGrid_win("模拟结果[不同cd，相同ti]");
        }
    }
}
