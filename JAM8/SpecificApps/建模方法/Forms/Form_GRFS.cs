﻿using ExcelLibrary.BinaryFileFormat;
using JAM8.Algorithms.Geometry;
using JAM8.Utilities;

namespace JAM8.SpecificApps.建模方法
{
    public partial class Form_GRFS : Form
    {
        private CData cd;
        private string file_name_cd;

        public Form_GRFS()
        {
            InitializeComponent();

            comboBox1.SelectedIndex = 0;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            (cd, file_name_cd) = CData.read_from_gslib_win();//cd
            comboBox2.Items.Clear();
            foreach (var item in cd.property_names)
            {
                comboBox2.Items.Add(item);
            }
            comboBox2.SelectedIndex = 0;
            textBox10.Text = file_name_cd;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            GridStructure gs = GridStructure.create_win();
            if (gs != null)
            {
                GridProperty gp_cd = cd.coarsened(gs).coarsened_grid[comboBox2.Text];
                gp_cd.show_win();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            int nx = int.Parse(tb_nx.Text);
            int ny = int.Parse(tb_ny.Text);
            int nz = int.Parse(tb_nz.Text);

            GridStructure gs = GridStructure.create_simple(nx, ny, nz);

            VariogramType vt = VariogramType.Spherical;
            var t_变差函数类型 = comboBox1.Text;
            if (t_变差函数类型 == "球状模型")
                vt = VariogramType.Spherical;
            if (t_变差函数类型 == "高斯模型")
                vt = VariogramType.Guassian;
            if (t_变差函数类型 == "指数模型")
                vt = VariogramType.Exponential;

            var t_块金 = float.Parse(tb_nugget.Text);
            var t_基台 = float.Parse(tb_sill.Text);
            var t_属性选取 = comboBox2.Text;
            var t_seed = int.Parse(tb_seed.Text);
            //var t_搜索半径 = short.Parse(tb_search_radius.Text);
            var t_cd最小数量 = int.Parse(tb_cd最小数量.Text);

            var t_range_max = double.Parse(tb_range_max.Text);
            var t_range_med = double.Parse(tb_range_med.Text);
            var t_range_min = double.Parse(tb_range_min.Text);
            var t_Azimuth = double.Parse(tb_Azimuth.Text);
            var t_Dip = double.Parse(tb_Dip.Text);
            var t_Rake = double.Parse(tb_Rake.Text);
            Variogram vm = Variogram.create(vt, t_块金, t_基台, (float)t_range_max);
            GRFS grfs = GRFS.create(gs, vm, cd, t_属性选取);
            var rot_mat = new double[] { t_Azimuth, t_Dip, t_Rake, t_range_max, t_range_med, t_range_min };
            var (result, time) = grfs.run(30, rot_mat, t_cd最小数量, t_seed);

            MyConsoleHelper.write_string_to_console($"计算用时：{time / 1000.0}秒");
            scottplot4Grid1.update_grid(result);

        }
    }
}
