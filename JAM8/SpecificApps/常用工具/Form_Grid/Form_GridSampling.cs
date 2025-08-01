using JAM8.Algorithms.Geometry;
using JAM8.Algorithms.Numerics;
using JAM8.Utilities;

namespace JAM8.SpecificApps.常用工具
{
    public partial class Form_GridSampling : Form
    {
        private Grid g;
        private Grid g_sampling;
        private GridProperty gp_sampling;

        public Form_GridSampling()
        {
            InitializeComponent();
            checkBox1.Visible = false;
        }

        //打开
        private void button2_Click(object sender, EventArgs e)
        {
            (g, _) = Grid.create_from_gslibwin();
            if (g == null)
                return;

            #region 加载属性列表

            listBox1.Items.Clear();
            foreach (var PropertyName in g.propertyNames)
            {
                listBox1.Items.Add(PropertyName);
            }

            #endregion

            if (g.gridStructure.dim == Dimension.D2)
            {
                checkBox1.Visible = false;
                checkBox1.Checked = false;
            }
            else
            {
                checkBox1.Visible = true;
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex == -1 || listBox1.SelectedItem.ToString() == string.Empty)
                return;
            else
                scottplot4GridProperty1.update_gridProperty(g[listBox1.SelectedItem.ToString()]);
        }

        //抽样
        private void button3_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex == -1 || listBox1.SelectedItem.ToString() == string.Empty)
                return;

            g_sampling = Grid.create(g.gridStructure); //随机抽样模型
            int random_seed = int.Parse(textBox3.Text);
            int 抽样次数 = int.Parse(textBox2.Text);
            var gp = g[listBox1.SelectedItem.ToString()]; //抽样的模型来源
            int N = int.Parse(textBox1.Text);
            for (int i = 1; i <= 抽样次数; i++)
            {
                random_seed += i;
                gp_sampling = GridProperty.create(g.gridStructure); //随机抽样结果
                if (gp.grid_structure.dim == Dimension.D2)
                {
                    //随机选取井位
                    var cell_locs = MyGenerator.range(0, gp.grid_structure.N);
                    cell_locs = MyShuffleHelper.fisher_yates_shuffle(cell_locs, new MersenneTwister((uint)random_seed))
                        .shuffled;

                    //抽样
                    for (int n = 0; n < N; n++)
                        gp_sampling.set_value(cell_locs[n], gp.get_value(cell_locs[n]));
                }

                if (gp.grid_structure.dim == Dimension.D3)
                {
                    if (checkBox1.Checked == true)
                    {
                        var xy_slice = gp.get_slice(1, Algorithms.GridSliceType.xy_slice);
                        //随机选取井位
                        var well_locs = MyGenerator.range(0, xy_slice.grid_structure.N);
                        well_locs = MyShuffleHelper.fisher_yates_shuffle(well_locs, new MersenneTwister((uint)random_seed))
                            .shuffled;

                        //抽样
                        foreach (var item in well_locs)
                        {
                            var si = xy_slice.grid_structure.get_spatial_index(item);
                            for (int iz = 0; iz < gp.grid_structure.nz; iz++)
                            {
                                var si1 = SpatialIndex.create(si.ix, si.iy, iz);
                                gp_sampling.set_value(si1, gp.get_value(si1));
                            }
                        }
                    }
                    else
                    {
                        //随机选取井位
                        var cell_locs = MyGenerator.range(0, gp.grid_structure.N);
                        cell_locs = MyShuffleHelper.fisher_yates_shuffle(cell_locs, new MersenneTwister((uint)random_seed))
                            .shuffled;

                        //抽样
                        for (int n = 0; n < N; n++)
                            gp_sampling.set_value(cell_locs[n], gp.get_value(cell_locs[n]));
                    }
                }

                g_sampling.add_gridProperty($"[cd_as_grid]_{i}", gp_sampling);

                #region 更新属性列表

                listBox2.Items.Clear();
                foreach (var PropertyName in g_sampling.propertyNames)
                {
                    listBox2.Items.Add(PropertyName);
                }

                #endregion
            }

            scottplot4GridProperty2.update_gridProperty(gp_sampling);
        }

        //保存Grid
        private void button1_Click(object sender, EventArgs e)
        {
            if (g_sampling == null)
                return;
            if (checkBox2.Checked) //只保存选中的grid
            {
                Grid g_sampling_selected = Grid.create(g_sampling.gridStructure);
                g_sampling_selected.add_gridProperty(listBox2.SelectedItem.ToString(),
                    g_sampling[listBox2.SelectedItem.ToString()]);
                Grid.save_to_gslibwin(g_sampling_selected);
            }
            else
                Grid.save_to_gslibwin(g_sampling);
        }

        //保存cdata
        private void button4_Click(object sender, EventArgs e)
        {
            if (g_sampling == null)
                return;
            if (checkBox2.Checked) //只保存选中的grid
            {
                Grid g_sampling_selected = Grid.create(g_sampling.gridStructure);
                g_sampling_selected.add_gridProperty(listBox2.SelectedItem.ToString(),
                    g_sampling[listBox2.SelectedItem.ToString()]);

                SaveFileDialog sfd = new();
                if (sfd.ShowDialog() != DialogResult.OK)
                    return;

                CData cd = CData.create_from_gridProperty(g_sampling_selected[listBox2.SelectedItem.ToString()],
                    listBox2.SelectedItem.ToString(), CompareType.NotEqual, null);
                cd.save_to_gslib(sfd.FileName, -99);
            }
            else
            {
                FolderBrowserDialog fbd = new();
                if (fbd.ShowDialog() != DialogResult.OK)
                    return;

                foreach (var propertyName in g_sampling.propertyNames)
                {
                    CData cd = CData.create_from_gridProperty(g_sampling[propertyName], propertyName,
                        CompareType.NotEqual, null);
                    cd.save_to_gslib($"{fbd.SelectedPath}\\{propertyName}.dat", -99);
                }
            }
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox2.SelectedIndex == -1 || listBox2.SelectedItem.ToString() == string.Empty)
                return;
            else
                scottplot4GridProperty2.update_gridProperty(g_sampling[listBox2.SelectedItem.ToString()]);
        }

        private void Form_GridSampling_Load(object sender, EventArgs e)
        {
        }
    }
}