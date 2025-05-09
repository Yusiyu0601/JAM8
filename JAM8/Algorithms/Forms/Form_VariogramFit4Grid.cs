using JAM8.Algorithms.Geometry;
using JAM8.Utilities;
using System.Data;
using System.Reflection;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace JAM8.Algorithms.Forms
{
    public partial class Form_VariogramFit4Grid : Form
    {
        private Grid g = null;

        private double[] h;
        private double[] gamma;
        private int[] N_pair;

        private Variogram variogram_fit = null;
        private Variogram variogram_manual = null;
        private VariogramType vt = VariogramType.Spherical;

        public Form_VariogramFit4Grid()
        {
            InitializeComponent();
        }

        private void Form_VariogramFit4Grid_Load(object sender, EventArgs e)
        {
            numericUpDown1.MouseWheel += NumericUpDown1_MouseWheel;
            numericUpDown2.MouseWheel += NumericUpDown2_MouseWheel;
            numericUpDown3.MouseWheel += NumericUpDown3_MouseWheel;

            numericUpDown4.MouseWheel += NumericUpDown4_MouseWheel;
            numericUpDown5.MouseWheel += NumericUpDown5_MouseWheel;

            comboBox1.SelectedIndex = 0;
        }


        #region 修改NumericUpDown，人工调整后的变差函数，进行draw重绘

        private void NumericUpDown3_MouseWheel(object sender, MouseEventArgs e)
        {
            HandledMouseEventArgs hme = e as HandledMouseEventArgs;
            if (hme != null)
            {
                hme.Handled = true;
            }

            if (e.Delta > 0)
            {
                decimal dd = numericUpDown3.Value + numericUpDown3.Increment;
                if (dd <= numericUpDown3.Maximum)
                {
                    numericUpDown3.Value = dd;
                }
            }
            else if (e.Delta < 0)
            {
                decimal dd = numericUpDown3.Value - numericUpDown3.Increment;
                if (dd >= numericUpDown3.Minimum)
                {
                    numericUpDown3.Value = dd;
                }
            }
        }

        private void NumericUpDown2_MouseWheel(object sender, MouseEventArgs e)
        {
            HandledMouseEventArgs hme = e as HandledMouseEventArgs;
            if (hme != null)
            {
                hme.Handled = true;
            }

            if (e.Delta > 0)
            {
                decimal dd = numericUpDown2.Value + numericUpDown2.Increment;
                if (dd <= numericUpDown2.Maximum)
                {
                    numericUpDown2.Value = dd;
                }
            }
            else if (e.Delta < 0)
            {
                decimal dd = numericUpDown2.Value - numericUpDown2.Increment;
                if (dd >= numericUpDown2.Minimum)
                {
                    numericUpDown2.Value = dd;
                }
            }
        }

        private void NumericUpDown1_MouseWheel(object sender, MouseEventArgs e)
        {
            HandledMouseEventArgs hme = e as HandledMouseEventArgs;
            if (hme != null)
            {
                hme.Handled = true;
            }

            if (e.Delta > 0)
            {
                decimal dd = numericUpDown1.Value + numericUpDown1.Increment;
                if (dd <= numericUpDown1.Maximum)
                {
                    numericUpDown1.Value = dd;
                }
            }
            else if (e.Delta < 0)
            {
                decimal dd = numericUpDown1.Value - numericUpDown1.Increment;
                if (dd >= numericUpDown1.Minimum)
                {
                    numericUpDown1.Value = dd;
                }
            }
        }

        private void NumericUpDown4_MouseWheel(object sender, MouseEventArgs e)
        {
            HandledMouseEventArgs hme = e as HandledMouseEventArgs;
            if (hme != null)
            {
                hme.Handled = true;
            }

            if (e.Delta > 0)
            {
                decimal dd = numericUpDown4.Value + numericUpDown4.Increment;
                if (dd <= numericUpDown4.Maximum)
                {
                    numericUpDown4.Value = dd;
                }
            }
            else if (e.Delta < 0)
            {
                decimal dd = numericUpDown4.Value - numericUpDown4.Increment;
                if (dd >= numericUpDown4.Minimum)
                {
                    numericUpDown4.Value = dd;
                }
            }
        }

        private void NumericUpDown5_MouseWheel(object sender, MouseEventArgs e)
        {
            HandledMouseEventArgs hme = e as HandledMouseEventArgs;
            if (hme != null)
            {
                hme.Handled = true;
            }

            if (e.Delta > 0)
            {
                decimal dd = numericUpDown5.Value + numericUpDown5.Increment;
                if (dd <= numericUpDown5.Maximum)
                {
                    numericUpDown5.Value = dd;
                }
            }
            else if (e.Delta < 0)
            {
                decimal dd = numericUpDown5.Value - numericUpDown5.Increment;
                if (dd >= numericUpDown5.Minimum)
                {
                    numericUpDown5.Value = dd;
                }
            }
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            if (variogram_manual != null)
            {
                textBox4.Text = numericUpDown1.Value.ToString();
                textBox5.Text = numericUpDown2.Value.ToString();
                textBox6.Text = numericUpDown3.Value.ToString();
            }
            draw();
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            if (variogram_manual != null)
            {
                textBox4.Text = numericUpDown1.Value.ToString();
                textBox5.Text = numericUpDown2.Value.ToString();
                textBox6.Text = numericUpDown3.Value.ToString();
            }
            draw();
        }

        private void numericUpDown3_ValueChanged(object sender, EventArgs e)
        {
            if (variogram_manual != null)
            {
                textBox4.Text = numericUpDown1.Value.ToString();
                textBox5.Text = numericUpDown2.Value.ToString();
                textBox6.Text = numericUpDown3.Value.ToString();
            }
            draw();
        }

        #endregion

        #region 调整参数，重新提取实验变差函数

        //调整滞后距数量
        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            numericUpDown4.Value = trackBar1.Value;//trackbar的值，赋给numericUpDown后，numericUpDown会主动调用修改值事件函数
        }

        //调整主方位角
        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            numericUpDown5.Value = trackBar2.Value;//trackbar的值，赋给numericUpDown后，numericUpDown会主动调用修改值事件函数
        }

        //调整滞后距数量
        private void numericUpDown4_ValueChanged(object sender, EventArgs e)
        {
            trackBar1.Value = (int)numericUpDown4.Value;
            int N_lag = trackBar1.Value;
            double azimuth = trackBar2.Value;
            var gp = g[listBox1.SelectedItem.ToString()];
            if (gp.gridStructure.dim == Dimension.D2)
            {
                (h, gamma, N_pair) = Variogram.calc_experiment_variogram(gp, azimuth, N_lag, 2);
            }
            else if (gp.gridStructure.dim == Dimension.D3)
            {
                (h, gamma, N_pair) = Variogram.calc_3d_horizontal_experiment_variogram(gp, azimuth, N_lag, 2);
            }
            init_controls(h, gamma, N_pair);
        }

        //调整主方位角
        private void numericUpDown5_ValueChanged(object sender, EventArgs e)
        {
            trackBar2.Value = (int)numericUpDown5.Value;
            int N_lag = trackBar1.Value;
            double azimuth = trackBar2.Value;
            var gp = g[listBox1.SelectedItem.ToString()];
            if (gp.gridStructure.dim == Dimension.D2)
            {
                (h, gamma, N_pair) = Variogram.calc_experiment_variogram(gp, azimuth, N_lag, 2);
            }
            else if (gp.gridStructure.dim == Dimension.D3)
            {
                (h, gamma, N_pair) = Variogram.calc_3d_horizontal_experiment_variogram(gp, azimuth, N_lag, 2);
            }
            init_controls(h, gamma, N_pair);
        }


        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (h != null)
                init_controls(h, gamma, N_pair);
        }

        #endregion

        //加载模型
        private void button1_Click(object sender, EventArgs e)
        {
            (g, _) = Grid.create_from_gslibwin();
            if (g == null) return;

            #region 加载属性列表

            listBox1.Items.Clear();
            foreach (var PropertyName in g.propertyNames)
            {
                listBox1.Items.Add(PropertyName);
            }

            #endregion
        }

        //选择模型属性
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex == -1 || listBox1.SelectedItem.ToString() == string.Empty)
                return;
            else
            {
                scottplot4GridProperty1.update_gridProperty(g[listBox1.SelectedItem.ToString()]);
                GridStructure gs = g.gridStructure;
                if (gs.dim == Dimension.D3)
                    button5.Enabled = true;
                else if (gs.dim == Dimension.D2)
                    button5.Enabled = false;
                int 对角线 = (int)Math.Sqrt(gs.nx * gs.nx + gs.ny * gs.ny);
                trackBar1.Maximum = 对角线;
                trackBar2.Maximum = 180;
                trackBar1.Minimum = 10;

                numericUpDown4.Maximum = trackBar1.Maximum;
                numericUpDown4.Minimum = trackBar1.Minimum;
                numericUpDown5.Maximum = trackBar2.Maximum;

                trackBar1.TickFrequency = 1;
                trackBar2.TickFrequency = 1;
                numericUpDown4.Increment = 1;
                numericUpDown5.Increment = 1;

                numericUpDown4_ValueChanged(sender, e);
            }
        }

        /// <summary>
        /// 根据实验变差函数拟合实现变差函数，并对所有控件信息进行更新
        /// </summary>
        /// <param name="h"></param>
        /// <param name="gamma"></param>
        private void init_controls(double[] h, double[] gamma, int[] N_pair)
        {
            if (comboBox1.Text == "球状模型")
                vt = VariogramType.Spherical;
            if (comboBox1.Text == "高斯模型")
                vt = VariogramType.Guassian;
            if (comboBox1.Text == "指数模型")
                vt = VariogramType.Exponential;
            (variogram_fit, _) = Variogram.variogramFit(vt, h, gamma, N_pair);
            variogram_manual = Variogram.create(VariogramType.Spherical, variogram_fit.nugget, variogram_fit.sill, variogram_fit.range);

            Dictionary<string, double[]> dict = new()
            {
                { "h", h },
                { "gamma", gamma },
                { "N_pair",N_pair.Select(a => (double)a).ToArray() }
            };

            MyDataFrame df = MyDataFrame.create(["h", "gamma", "N_pairs"]);
            for (int i = 0; i < h.Length; i++)
                df.add_record([h[i], gamma[i], N_pair[i]]);

            dataGridView1.DataSource = df.convert_to_dataTable();
            if (variogram_fit.nugget < 0)//偶尔存在这种情况
                variogram_fit.nugget = 0;
            textBox1.Text = variogram_fit.range.ToString();
            textBox2.Text = variogram_fit.sill.ToString();
            textBox3.Text = variogram_fit.nugget.ToString();
            textBox4.Text = variogram_fit.range.ToString();
            textBox5.Text = variogram_fit.sill.ToString();
            textBox6.Text = variogram_fit.nugget.ToString();
            numericUpDown1.Maximum = (decimal)(variogram_fit.range > h.Last() ? variogram_fit.range * 2 : h.Last());//人工修改变程的最大值
            numericUpDown2.Maximum = (decimal)(variogram_fit.sill * 1.2);//基台值显示范围
            numericUpDown3.Maximum = (decimal)(gamma.Max() * 1.2);//nugget显示范围
            numericUpDown1.DecimalPlaces = 3;
            numericUpDown2.DecimalPlaces = 3;
            numericUpDown3.DecimalPlaces = 3;
            numericUpDown1.Value = (decimal)variogram_fit.range;
            numericUpDown2.Value = (decimal)variogram_fit.sill;
            numericUpDown3.Value = (decimal)variogram_fit.nugget;
            numericUpDown1.Increment = (decimal)(variogram_fit.range / 50);
            numericUpDown2.Increment = (decimal)(variogram_fit.sill / 50);
            numericUpDown3.Increment = (decimal)(variogram_fit.sill / 50);
            draw();
        }

        //重新绘制所有曲线，包括实验变程函数、拟合、手动调整
        private void draw()
        {
            formsPlot1.Plot.Clear();
            formsPlot1.Plot.XAxis.Label("滞后距", Color.Black, size: 22, fontName: "宋体");
            formsPlot1.Plot.XAxis.TickLabelStyle(fontSize: 18);
            formsPlot1.Plot.YAxis.Label("变差函数", Color.Black, size: 22, fontName: "宋体");
            formsPlot1.Plot.YAxis.TickLabelStyle(fontSize: 18);

            formsPlot1.Plot.AddScatter(h, gamma, Color.Gray, lineStyle: ScottPlot.LineStyle.None,
                markerShape: ScottPlot.MarkerShape.filledCircle, label: "实验变差函数");

            //自动拟合
            List<double> x_fit = new();
            List<double> y_fit = new();
            double step = (h.Max() - h.Min()) / 100.0;//步长
            double maxLagDistance = h.Last();
            //根据拟合的模型进行等间距重采样
            for (double lag = 0; lag < maxLagDistance * 1.05; lag += step)
            {
                x_fit.Add(lag);
                y_fit.Add(variogram_fit.calc_variogram((float)lag));
            }
            formsPlot1.Plot.AddScatter(x_fit.ToArray(), y_fit.ToArray(), Color.Green, lineWidth: 2, markerSize: 0, label: "球状模型");

            //手工调节
            List<double> x_manual = new();
            List<double> y_manual = new();
            step = variogram_fit.range / 100.0;
            maxLagDistance = h.Last();
            variogram_manual = Variogram.create(vt, (float)numericUpDown3.Value,
                (float)numericUpDown2.Value, (float)numericUpDown1.Value);
            //根据拟合的模型进行等间距重采样
            for (double lag = 0; lag < maxLagDistance * 1.05; lag += step)
            {
                x_manual.Add(lag);
                y_manual.Add(variogram_manual.calc_variogram((float)lag));
            }
            formsPlot1.Plot.AddScatter(x_manual.ToArray(), y_manual.ToArray(), Color.Blue, lineWidth: 2, markerSize: 0, label: "球状模型");

            formsPlot1.Refresh();
        }

        //保存实验变差函数
        private void button3_Click(object sender, EventArgs e)
        {
            ExcelHelper.dataTable_to_excel(FileDialogHelper.SaveExcel(), dataGridView1.DataSource as DataTable);
        }
        //保存拟合变差函数
        private void button4_Click(object sender, EventArgs e)
        {
            MessageBox.Show("自己动手复制吧");
        }

        //垂向实验变差函数
        private void button5_Click(object sender, EventArgs e)
        {
            var gp = g[listBox1.SelectedItem.ToString()];
            int N_lag = gp.gridStructure.nz;
            var (h, gamma, N_pair) = Variogram.calc_3d_vertical_experiment_variogram(gp, N_lag, 1);
            Form_VariogramFit frm = new(h, gamma, N_pair);
            frm.Show();
        }

    }
}
