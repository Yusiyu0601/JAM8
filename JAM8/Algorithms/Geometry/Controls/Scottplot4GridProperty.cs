using System.Globalization;
using System.Windows.Forms;
using ScottPlot;
using ScottPlot.Plottable;

namespace JAM8.Algorithms.Geometry
{
    public partial class Scottplot4GridProperty : UserControl
    {
        public GridProperty _gp { get; internal set; }//输入GridProperty，2D或者3D
        private GridProperty _viewGrid;//显示GridProperty，只能是2D(2D模型本身、或者3D模型切片)
        private ScottPlot.Drawing.Colormap[] _colormaps = ScottPlot.Drawing.Colormap.GetColormaps();



        public delegate void MouseMoveHandler(int view_ix, int view_iy);
        /// <summary>
        /// 鼠标移动事件
        /// </summary>
        public event MouseMoveHandler MouseMoveEvent;

        public delegate void MouseDownHandler(int view_ix, int view_iy);
        /// <summary>
        /// 鼠标点击事件
        /// </summary>
        public event MouseDownHandler MouseDownEvent;

        public Scottplot4GridProperty()
        {
            InitializeComponent();

            comboBox2.Visible = false;
            comboBox2.SelectedIndex = 0;
            numericUpDown1.Visible = false;
            trackBar1.Visible = false;
            trackBar1.TickFrequency = 1;
            numericUpDown1.MouseWheel += NumericUpDown1_MouseWheel;


            //Set button text according to system language. 根据系统语言设置按钮文字
            bool IsChineseSystem = CultureInfo.CurrentCulture.Name.StartsWith("zh");
            if (IsChineseSystem)
            {
                label4.Text = "配色";
                button1.Text = "统计";

            }
            else
            {
                label4.Text = "CMap";
                button1.Text = "Stat";
            }
        }

        private void NumericUpDown1_MouseWheel(object sender, MouseEventArgs e)
        {
            HandledMouseEventArgs hme = e as HandledMouseEventArgs;
            if (hme != null)
                hme.Handled = true;

            try
            {
                if (e.Delta > 0)
                    numericUpDown1.Value += numericUpDown1.Increment;
                if (e.Delta < 0)
                    numericUpDown1.Value -= numericUpDown1.Increment;
            }
            catch
            {

            }
        }

        /// <summary>
        /// 更新GridProperty
        /// </summary>
        /// <param name="gp"></param>
        public void update_gridProperty(GridProperty gp)
        {
            _gp = gp;

            foreach (ScottPlot.Drawing.Colormap cmap in _colormaps)
                comboBox1.Items.Add(cmap.Name);
            comboBox1.SelectedIndex = comboBox1.Items.IndexOf("Jet");

            if (_gp.gridStructure.dim == Dimension.D3)
            {
                comboBox2.Visible = true;
                numericUpDown1.Visible = true;
                trackBar1.Visible = true;

                label5.Text = $"[nx={_gp.gridStructure.nx}  ny={_gp.gridStructure.ny}  nz={_gp.gridStructure.nz}]";
            }
            else
            {
                comboBox2.Visible = false;
                numericUpDown1.Visible = false;
                trackBar1.Visible = false;

                label5.Text = $"[nx={_gp.gridStructure.nx}  ny={_gp.gridStructure.ny}]";
            }

            re_draw_viewGrid();
        }

        /// <summary>
        /// 修改viewGrid的值
        /// </summary>
        /// <param name="view_ix"></param>
        /// <param name="view_iy"></param>
        /// <param name="value"></param>
        public void assign_value_in_viewGrid(int view_ix, int view_iy, float? value)
        {
            if (_gp.gridStructure.dim == Dimension.D2)
            {
                _gp.set_value(view_ix, view_iy, value);//对于2D，_viewGrid与_gp相同
            }
            else
            {
                _viewGrid.set_value(view_ix, view_iy, value);
                if (comboBox2.Text == "XY")
                {
                    numericUpDown1.Minimum = 1;
                    numericUpDown1.Maximum = _gp.gridStructure.nz;
                    trackBar1.Minimum = 1;
                    trackBar1.Maximum = _gp.gridStructure.nz;
                    _gp.set_slice((int)numericUpDown1.Value, GridSliceType.xy_slice, _viewGrid);
                }
                if (comboBox2.Text == "YZ")
                {
                    numericUpDown1.Minimum = 1;
                    numericUpDown1.Maximum = _gp.gridStructure.nx;
                    trackBar1.Minimum = 1;
                    trackBar1.Maximum = _gp.gridStructure.nx;
                    _gp.set_slice((int)numericUpDown1.Value, GridSliceType.yz_slice, _viewGrid);
                }
                if (comboBox2.Text == "XZ")
                {
                    numericUpDown1.Minimum = 1;
                    numericUpDown1.Maximum = _gp.gridStructure.ny;
                    trackBar1.Minimum = 1;
                    trackBar1.Maximum = _gp.gridStructure.ny;
                    _gp.set_slice((int)numericUpDown1.Value, GridSliceType.xz_slice, _viewGrid);
                }
            }
            re_draw_viewGrid();
        }

        private void formsPlot1_MouseMove(object sender, MouseEventArgs e)
        {
            if (_gp == null)
                return;
            var coord = formsPlot1.GetMouseCoordinates();
            int ix = (int)Math.Ceiling(coord.x);
            int iy = (int)Math.Ceiling(coord.y);
            var v = _viewGrid!.get_value(ix - 1, iy - 1);
            string s = v == null ? "null" : v.Value.ToString("f4");
            label1.Text = $"ix={ix}";
            label2.Text = $"iy={iy}";
            label3.Text = $"value={s}";
            MouseMoveEvent?.Invoke(ix, iy);
        }

        private void formsPlot1_MouseDown(object sender, MouseEventArgs e)
        {
            if (_gp == null)
                return;
            var coord = formsPlot1.GetMouseCoordinates();
            int ix = (int)Math.Ceiling(coord.x);
            int iy = (int)Math.Ceiling(coord.y);
            var v = _viewGrid!.get_value(ix - 1, iy - 1);
            string s = v == null ? "null" : v.Value.ToString("f4");
            label1.Text = $"ix={ix}";
            label2.Text = $"iy={iy}";
            label3.Text = $"value={s}";
            MouseDownEvent?.Invoke(ix, iy);
        }

        private static double?[,] Grid2Intensities(GridProperty gp)
        {
            //grid网格顺时针旋转90度
            double?[,] intensities = new double?[gp.gridStructure.ny, gp.gridStructure.nx];
            for (int j = 0; j < gp.gridStructure.ny; j++)
            {
                for (int i = 0; i < gp.gridStructure.nx; i++)
                {
                    int I = gp.gridStructure.ny - j - 1;
                    int J = i;
                    intensities[I, J] = gp.get_value(i, j);
                }
            }
            return intensities;
        }

        private Heatmap hm = null;

        private Colorbar cb = null;
        //重新绘制可显示的图像
        private void re_draw_viewGrid()
        {
            if (_gp == null) return;
            ScottPlot.Drawing.Colormap cmap;
            get_viewGrid();
            double?[,] intensities = Grid2Intensities(_viewGrid!);
            cmap = _colormaps[comboBox1.SelectedIndex >= 0 ? comboBox1.SelectedIndex : 0];

            if (hm == null)
            {
                this.hm = formsPlot1.Plot.AddHeatmap(intensities, cmap, lockScales: checkBox1.Checked);
                this.cb = formsPlot1.Plot.AddColorbar(hm);
            }
            else
            {
                this.hm.Update(intensities, cmap);
                this.cb.UpdateColormap(hm.Colormap);
            }

            formsPlot1.Refresh();
        }

        //根据实际情况，获取实际显示的模型
        private void get_viewGrid()
        {
            if (_gp.gridStructure.dim == Dimension.D2)
            {
                _viewGrid = _gp;
            }
            else
            {
                if (comboBox2.Text == "XY")
                {
                    numericUpDown1.Minimum = 1;
                    numericUpDown1.Maximum = _gp.gridStructure.nz;
                    trackBar1.Minimum = 1;
                    trackBar1.Maximum = _gp.gridStructure.nz;
                    _viewGrid = _gp.get_slice((int)numericUpDown1.Value, GridSliceType.xy_slice);
                }
                if (comboBox2.Text == "YZ")
                {
                    numericUpDown1.Minimum = 1;
                    numericUpDown1.Maximum = _gp.gridStructure.nx;
                    trackBar1.Minimum = 1;
                    trackBar1.Maximum = _gp.gridStructure.nx;
                    _viewGrid = _gp.get_slice((int)numericUpDown1.Value, GridSliceType.yz_slice);
                }
                if (comboBox2.Text == "XZ")
                {
                    numericUpDown1.Minimum = 1;
                    numericUpDown1.Maximum = _gp.gridStructure.ny;
                    trackBar1.Minimum = 1;
                    trackBar1.Maximum = _gp.gridStructure.ny;
                    _viewGrid = _gp.get_slice((int)numericUpDown1.Value, GridSliceType.xz_slice);
                }
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            re_draw_viewGrid();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form_GridProperty frm = new(_gp!);
            frm.Show();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            formsPlot1.Plot.AxisScaleLock(checkBox1.Checked);
            formsPlot1.Refresh();
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            trackBar1.Value = (int)numericUpDown1.Value;
            re_draw_viewGrid();
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            numericUpDown1.Value = trackBar1.Value;
            re_draw_viewGrid();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            re_draw_viewGrid();
        }
    }
}
