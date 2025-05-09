using System.Globalization;
using JAM8.Algorithms.Images;
using ScottPlot.Plottable;

namespace JAM8.Algorithms.Geometry
{
    public partial class Scottplot4Grid : UserControl
    {
        //输入Grid,2D 或者 3D
        public Grid _g { get; internal set; }
        //选中的GridProperty，2D或者3D
        public GridProperty _gp { get; internal set; }
        //实际显示GridProperty，只能是2D(2D模型的深度复制体、或者3D模型切片)
        private GridProperty _view_2d_gp;
        //过滤字符串
        private string _filter_string = null;

        private readonly ScottPlot.Drawing.Colormap[] _colormaps =
            ScottPlot.Drawing.Colormap.GetColormaps();

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

        public delegate void GridPropertySelectedHandler(string gp_name);
        /// <summary>
        /// 选择网格属性事件
        /// </summary>
        public event GridPropertySelectedHandler GridPropertySelectedEvent;

        public Scottplot4Grid()
        {
            InitializeComponent();

            foreach (ScottPlot.Drawing.Colormap cmap in _colormaps)
                cb_colorMap.Items.Add(cmap.Name);
            cb_colorMap.Text = "Jet";

            cb_xy_yz_zx.Items.Add("XY");
            cb_xy_yz_zx.Items.Add("YZ");
            cb_xy_yz_zx.Items.Add("XZ");
            cb_xy_yz_zx.Visible = false;
            cb_xy_yz_zx.Text = cb_xy_yz_zx.Items[0].ToString();

            numericUpDown1.Visible = false;

            trackBar1.Visible = false;
            trackBar1.TickFrequency = 1;

            numericUpDown1.MouseWheel += NumericUpDown1_MouseWheel;

            //根据系统语言设置按钮文字
            bool IsChineseSystem = CultureInfo.CurrentCulture.Name.StartsWith("zh");
            if (IsChineseSystem)
            {
                label6.Text = "属性";
                label4.Text = "配色";
                toolStripDropDownButton1.Text = "操作";
                toolStripLabel3.Text = "过滤";

            }
            else
            {
                label6.Text = "Prop";
                label4.Text = "CMap";
                toolStripDropDownButton1.Text = "Action";
                toolStripLabel3.Text = "Filter";
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
        /// 更新Grid对象
        /// </summary>
        /// <param name="g">如果g是null，则清空绘图区</param>
        public void update_grid(Grid g)
        {
            if (g == null)
            {
                this.formsPlot1.Plot.Clear();
                this.formsPlot1.Refresh();
                hm = null;//hm必须变为空
                return;
            }
            _g = g;
            cb_gridProperty.Items.Clear();
            foreach (var propertyName in g.propertyNames)
            {
                cb_gridProperty.Items.Add(propertyName);
            }
            cb_gridProperty.SelectedIndex = 0;
        }

        //选择GridProperty
        private void cb_gridProperty_SelectedIndexChanged(object sender, EventArgs e)
        {
            _gp = _g[cb_gridProperty.Text];

            if (_gp.gridStructure.dim == Dimension.D3)
            {
                cb_xy_yz_zx.Visible = true;
                numericUpDown1.Visible = true;
                trackBar1.Visible = true;

                toolStripLabel1.Text = $"[nx={_gp.gridStructure.nx}  ny={_gp.gridStructure.ny}  nz={_gp.gridStructure.nz}]";
            }
            else
            {
                cb_xy_yz_zx.Visible = false;
                numericUpDown1.Visible = false;
                trackBar1.Visible = false;

                toolStripLabel1.Text = $"[nx={_gp.gridStructure.nx}  ny={_gp.gridStructure.ny}]";
            }

            redraw_viewGrid();

            GridPropertySelectedEvent?.Invoke(cb_gridProperty.Text);
        }

        private void formsPlot1_MouseMove(object sender, MouseEventArgs e)
        {
            if (_gp == null)
                return;
            var coord = formsPlot1.GetMouseCoordinates();
            int ix = (int)Math.Ceiling(coord.x);
            int iy = (int)Math.Ceiling(coord.y);
            var v = _view_2d_gp!.get_value(ix - 1, iy - 1);
            string s = v == null ? "null" : v.Value.ToString("f4");
            toolStripLabel2.Text = $"[ix={ix}  iy={iy}  value={s}]";
            MouseMoveEvent?.Invoke(ix, iy);
        }

        private void formsPlot1_MouseDown(object sender, MouseEventArgs e)
        {
            if (_gp == null)
                return;
            var coord = formsPlot1.GetMouseCoordinates();
            int ix = (int)Math.Ceiling(coord.x);
            int iy = (int)Math.Ceiling(coord.y);
            var v = _view_2d_gp!.get_value(ix - 1, iy - 1);
            string s = v == null ? "null" : v.Value.ToString("f4");
            toolStripLabel2.Text = $"[ix={ix}  iy={iy}  value={s}]";
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

        //更改颜色映射并重绘
        private void cb_ColorMap_SelectedIndexChanged(object sender, EventArgs e)
        {
            redraw_viewGrid();
        }

        //更改坐标轴比例并重绘
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                if (_view_2d_gp.gridStructure.nx > _view_2d_gp.gridStructure.ny)
                    formsPlot1.Plot.AxisScaleLock(true, ScottPlot.EqualScaleMode.PreserveY);
                if (_view_2d_gp.gridStructure.nx < _view_2d_gp.gridStructure.ny)
                    formsPlot1.Plot.AxisScaleLock(true, ScottPlot.EqualScaleMode.PreserveX);
                if (_view_2d_gp.gridStructure.nx == _view_2d_gp.gridStructure.ny)
                    formsPlot1.Plot.AxisScaleLock(true, ScottPlot.EqualScaleMode.ZoomOut);
            }
            else
            {
                formsPlot1.Plot.AxisScaleLock(false);
                formsPlot1.Plot.AxisAuto();
            }
            formsPlot1.Refresh();
        }

        //更新三维切片数据并重绘
        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            trackBar1.Value = (int)numericUpDown1.Value;
            redraw_viewGrid();
        }
        //更新三维切片数据并重绘
        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            numericUpDown1.Value = trackBar1.Value;
            redraw_viewGrid();
        }
        //更新三维切片数据并重绘
        private void cb_xy_yz_zx_SelectedIndexChanged(object sender, EventArgs e)
        {
            redraw_viewGrid();
        }

        #region 重绘函数

        private Heatmap hm = null;

        private Colorbar cb = null;
        //重新绘制可显示的图像
        private void redraw_viewGrid()
        {
            if (_gp == null)
                return;

            ScottPlot.Drawing.Colormap cmap;

            //根据实际情况，获取实际显示的模型
            void get_viewGrid()
            {
                if (_gp.gridStructure.dim == Dimension.D2)
                {
                    _view_2d_gp = _gp.deep_clone();
                }
                else
                {
                    if (cb_xy_yz_zx.Text == "XY")
                    {
                        numericUpDown1.Minimum = 0;
                        numericUpDown1.Maximum = _gp.gridStructure.nz - 1;
                        trackBar1.Minimum = 0;
                        trackBar1.Maximum = _gp.gridStructure.nz - 1;
                        _view_2d_gp = _gp.get_slice((int)numericUpDown1.Value, GridSliceType.xy_slice);
                    }
                    if (cb_xy_yz_zx.Text == "YZ")
                    {
                        numericUpDown1.Minimum = 0;
                        numericUpDown1.Maximum = _gp.gridStructure.nx - 1;
                        trackBar1.Minimum = 0;
                        trackBar1.Maximum = _gp.gridStructure.nx - 1;
                        _view_2d_gp = _gp.get_slice((int)numericUpDown1.Value, GridSliceType.yz_slice);
                    }
                    if (cb_xy_yz_zx.Text == "XZ")
                    {
                        numericUpDown1.Minimum = 0;
                        numericUpDown1.Maximum = _gp.gridStructure.ny - 1;
                        trackBar1.Minimum = 0;
                        trackBar1.Maximum = _gp.gridStructure.ny - 1;
                        _view_2d_gp = _gp.get_slice((int)numericUpDown1.Value, GridSliceType.xz_slice);
                    }
                }
            }

            get_viewGrid();//调用函数

            try
            {
                if (_filter_string != null && _filter_string != "")
                {
                    string[] str = _filter_string.Split(',');
                    float min = float.Parse(str[0]);
                    float max = float.Parse(str[1]);
                    for (int n = 0; n < _view_2d_gp.gridStructure.N; n++)
                    {
                        if (_view_2d_gp.get_value(n) >= min &&
                            _view_2d_gp.get_value(n) <= max)
                        {
                        }
                        else
                        {
                            _view_2d_gp.set_value(n, null);
                        }
                    }
                }
            }
            catch
            {
                MessageBox.Show("输入格式应该是 0.2,0,8 ");
            }

            double?[,] intensities = Grid2Intensities(_view_2d_gp!);//数据转换
            cmap = _colormaps[cb_colorMap.SelectedIndex >= 0 ? cb_colorMap.SelectedIndex : 0];

            if (hm == null)
            {
                hm = formsPlot1.Plot.AddHeatmap(intensities, cmap);
                cb = formsPlot1.Plot.AddColorbar(hm);
            }
            else
            {
                hm.Update(intensities, cmap);
                cb.UpdateColormap(hm.Colormap);
            }
            formsPlot1.Refresh();
        }

        #endregion

        #region grid操作

        private void 统计ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form_GridProperty frm = new(_gp!);
            frm.Show();
        }

        private void 保存ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void 二维图片ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_g.gridStructure.dim == Dimension.D2)
            {
                FolderBrowserDialog fbd = new();
                if (fbd.ShowDialog() != DialogResult.OK)
                    return;

                foreach (var item in _g)
                {
                    string save_path = $"{fbd.SelectedPath}\\{item.Key}.bmp";

                    Bitmap b = null;
                    if (this.cb_colorMap.Text == "Grayscale")
                        b = _view_2d_gp.draw_image_2d(Color.White, ColorMapEnum.Gray);
                    if (this.cb_colorMap.Text == "GrayscaleR")
                        b = _view_2d_gp.draw_image_2d(Color.White, ColorMapEnum.GrayR);
                    else
                        b = _view_2d_gp.draw_image_2d(Color.White, ColorMapEnum.Jet);
                    b.Save(save_path);
                }
                MessageBox.Show("保存完成");
            }
        }

        private void 二维图片当前显示属性ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_g.gridStructure.dim == Dimension.D2)
            {
                SaveFileDialog sfd = new()
                {
                    Filter = "bmp文件|*.bmp"
                };
                if (sfd.ShowDialog() != DialogResult.OK)
                    return;

                Bitmap b = null;
                if (this.cb_colorMap.Text == "Grayscale")
                    b = _view_2d_gp.draw_image_2d(Color.White, ColorMapEnum.Gray);
                if (this.cb_colorMap.Text == "GrayscaleR")
                    b = _view_2d_gp.draw_image_2d(Color.White, ColorMapEnum.GrayR);
                else
                    b = _view_2d_gp.draw_image_2d(Color.White, ColorMapEnum.Jet);
                b.Save(sfd.FileName);
                MessageBox.Show("保存完成");
            }
        }

        private void eTypeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var EType = _g.get_EType();
            EType.show_win();
        }

        private void 缩放ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GridStructure gs_resized = GridStructure.create_win(_g.gridStructure);
            var g_resized = Grid.create(gs_resized);
            foreach (var (gp_name, gp) in _g)
            {
                g_resized.add_gridProperty($"{gp_name}(resized)", gp.resize(gs_resized));
            }
            g_resized.showGrid_win("resized");
        }

        private void toolStripTextBox1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                _filter_string = toolStripTextBox1.Text;
                redraw_viewGrid();
            }
            if (e.KeyData == Keys.ShiftKey)
            {
                toolStripTextBox1.Clear();
                _filter_string = null;
                redraw_viewGrid();
            }
        }

        private void 保存GridToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Grid.save_to_gslibwin(_g);
        }

        private void 保存View2dGridToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Grid g = Grid.create(_view_2d_gp.gridStructure);
            g.add_gridProperty("view2d_gp", _view_2d_gp);
            Grid.save_to_gslibwin(g);
        }

        private void 保存ToolStripMenuItem1_Click(object sender, EventArgs e)
        {

            Grid g = Grid.create(_gp.gridStructure);
            g.add_gridProperty(cb_gridProperty.Text, _gp);
            Grid.save_to_gslibwin(g);
        }

        #endregion

        #region combox 提示

        private void cb_gridProperty_DrawItem(object sender, DrawItemEventArgs e)
        {
            // 绘制背景
            e.DrawBackground();
            //绘制列表项目
            e.Graphics.DrawString(cb_gridProperty.Items[e.Index].ToString(), e.Font, System.Drawing.Brushes.Black, e.Bounds);
            //将高亮的列表项目的文字传递到toolTip1(之前建立ToolTip的一个实例)
            if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
                toolTip1.Show(cb_gridProperty.Items[e.Index].ToString(), cb_gridProperty, e.Bounds.X + e.Bounds.Width, e.Bounds.Y + e.Bounds.Height);
            e.DrawFocusRectangle();
        }

        private void cb_gridProperty_DropDownClosed(object sender, EventArgs e)
        {
            toolTip1.Hide(cb_gridProperty);
        }

        private void cb_colorMap_DrawItem(object sender, DrawItemEventArgs e)
        {
            // 绘制背景
            e.DrawBackground();
            //绘制列表项目
            e.Graphics.DrawString(cb_colorMap.Items[e.Index].ToString(), e.Font, System.Drawing.Brushes.Black, e.Bounds);
            //将高亮的列表项目的文字传递到toolTip1(之前建立ToolTip的一个实例)
            if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
                toolTip1.Show(cb_colorMap.Items[e.Index].ToString(), cb_colorMap, e.Bounds.X + e.Bounds.Width, e.Bounds.Y + e.Bounds.Height);
            e.DrawFocusRectangle();
        }

        private void cb_colorMap_DropDownClosed(object sender, EventArgs e)
        {
            toolTip1.Hide(cb_colorMap);
        }

        #endregion


    }
}
