using JAM8.Algorithms.Forms;
using JAM8.Algorithms.Geometry;
using JAM8.Utilities;

namespace JAM8.SpecificApps.建模方法.Forms
{
    public partial class Form_MPS : Form
    {
        private Grid g_ti; //训练图像
        private Grid g_re; //模拟网格

        private CData cd;

        private string file_name_ti;
        private string file_name_cd;

        public Form_MPS()
        {
            InitializeComponent();
        }

        //导入TI
        private void button2_Click(object sender, EventArgs e)
        {
            (g_ti, file_name_ti) = Grid.create_from_gslibwin(); //ti
            if (g_ti == null)
                return;

            scottplot4Grid2.update_grid(g_ti);

            tb_file_name.Text = file_name_ti;

            mps_nx.Text = g_ti.gridStructure.nx.ToString();
            mps_ny.Text = g_ti.gridStructure.ny.ToString();
            mps_nz.Text = g_ti.gridStructure.nz.ToString();
        }

        //导入TI
        private void button7_Click(object sender, EventArgs e)
        {
            Form_GridCatalog frm = new();
            if (frm.ShowDialog() != DialogResult.OK)
                return;
            g_ti = frm.selected_grids.FirstOrDefault();
            scottplot4Grid2.update_grid(g_ti);

            mps_nx.Text = g_ti.gridStructure.nx.ToString();
            mps_ny.Text = g_ti.gridStructure.ny.ToString();
            mps_nz.Text = g_ti.gridStructure.nz.ToString();
        }

        private void textBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Clipboard.SetDataObject(textBox1.Text);
        }

        //导入cdata
        private void button1_Click(object sender, EventArgs e)
        {
            (cd, var filename) = CData.read_from_gslib_win("导入条件数据");
            textBox2.Text = filename;
        }

        //预览cdata
        private void button4_Click(object sender, EventArgs e)
        {
            int nx = int.Parse(tb_cd_nx.Text);
            int ny = int.Parse(tb_cd_ny.Text);
            int nz = int.Parse(tb_cd_nz.Text);
            float xsiz = float.Parse(tb_cd_xsiz.Text);
            float ysiz = float.Parse(tb_cd_ysiz.Text);
            float zsiz = float.Parse(tb_cd_zsiz.Text);
            float xmn = float.Parse(tb_cd_xmn.Text);
            float ymn = float.Parse(tb_cd_ymn.Text);
            float zmn = float.Parse(tb_cd_zmn.Text);
            GridStructure gs = GridStructure.create(nx, ny, nz, xsiz, ysiz, zsiz, xmn, ymn, zmn);

            this.scottplot4Grid3.update_grid(cd.coarsened(gs).coarsened_grid);
        }

        //SIMPAT
        private void button3_Click(object sender, EventArgs e)
        {
            int multigrid = int.Parse(tb_multigrid.Text);
            int random_seed = int.Parse(mps_randomSeed.Text);
            int N = int.Parse(mps_N.Text);
            int nx = int.Parse(mps_nx.Text);
            int ny = int.Parse(mps_ny.Text);
            int nz = int.Parse(mps_nz.Text);
            GridStructure gs_re = GridStructure.create_simple(nx, ny, nz);
            int template_rx = int.Parse(tb_template_rx.Text);
            int template_ry = int.Parse(tb_template_ry.Text);
            int template_rz = int.Parse(tb_template_rz.Text);


            textBox1.Text = $"{FileHelper.GetFileName(tb_file_name.Text, false)} [{N}re]";

            Simpat simpat = Simpat.create(random_seed, multigrid, (template_rx, template_ry, template_rz),
                scottplot4Grid2._gp, cd, gs_re, N);
            var re = simpat.run(1);
            scottplot4Grid1.update_grid(re);
        }

        //SNESIM
        private void button5_Click(object sender, EventArgs e)
        {
            var snesim = Snesim.create();
            int multigrid = int.Parse(tb_multigrid.Text);
            int random_seed = int.Parse(mps_randomSeed.Text);
            int max_number = int.Parse(snesim_max_number.Text);
            int N = int.Parse(mps_N.Text);
            int nx = int.Parse(mps_nx.Text);
            int ny = int.Parse(mps_ny.Text);
            int nz = int.Parse(mps_nz.Text);
            GridStructure gs_re = GridStructure.create_simple(nx, ny, nz);
            int template_rx = int.Parse(tb_template_rx.Text);
            int template_ry = int.Parse(tb_template_ry.Text);
            int template_rz = int.Parse(tb_template_rz.Text);

            var (model, _) = snesim.run(random_seed, multigrid, max_number, (template_rx, template_ry, template_rz),
                g_ti.first_gridProperty(), cd, gs_re, 35);

            scottplot4Grid1.update_grid(model);
        }

        //DS
        private void button6_Click(object sender, EventArgs e)
        {
            int random_seed = int.Parse(mps_randomSeed.Text);
            int N = int.Parse(mps_N.Text);
            int nx = int.Parse(mps_nx.Text);
            int ny = int.Parse(mps_ny.Text);
            int nz = int.Parse(mps_nz.Text);
            GridStructure gs_re = GridStructure.create_simple(nx, ny, nz);
            int search_radius = int.Parse(ds_搜索半径.Text);
            float max_fraction = float.Parse(ds_搜索比例.Text);
            int max_number = int.Parse(ds_最大条件数.Text);
            float distance_threshold = float.Parse(ds_距离阈值.Text);

            DirectSampling ds = null;
            if (cd != null)
                ds = DirectSampling.create(gs_re, g_ti.first_gridProperty(), cd, cd.property_names[0]);
            else
                ds = DirectSampling.create(gs_re, g_ti.first_gridProperty());

            g_re = ds.run(search_radius, max_number, max_fraction, distance_threshold, random_seed);
            scottplot4Grid1.update_grid(g_re);
        }
    }
}