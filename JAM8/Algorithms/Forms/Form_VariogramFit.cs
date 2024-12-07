using JAM8.Algorithms.Geometry;
using JAM8.Utilities;
using System.Reflection;

namespace JAM8.Algorithms.Forms
{
    public partial class Form_VariogramFit : Form
    {
        double[] h;
        double[] gamma;
        int[] N_pair;

        Variogram variogram_fit = null;
        Variogram variogram_manual = null;
        VariogramType vt = VariogramType.Guassian;

        private void dataGridView1_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            //自动编号，与数据无关
            Rectangle rectangle = new(e.RowBounds.Location.X, e.RowBounds.Location.Y, dataGridView1.RowHeadersWidth - 4, e.RowBounds.Height);
            TextRenderer.DrawText(e.Graphics,
                  (e.RowIndex + 1).ToString(),
                   dataGridView1.RowHeadersDefaultCellStyle.Font,
                   rectangle,
                   dataGridView1.RowHeadersDefaultCellStyle.ForeColor,
                   TextFormatFlags.VerticalCenter | TextFormatFlags.Right);
        }

        public Form_VariogramFit()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 构造函数，输入实验变差函数
        /// </summary>
        /// <param name="h"></param>
        /// <param name="gamma"></param>
        public Form_VariogramFit(double[] h, double[] gamma, int[] N_pair)
        {
            InitializeComponent();
            if (h == null || gamma == null || h.Length != gamma.Length || h.Length != N_pair.Length)
            {
                MessageBox.Show("输入的实验变差函数不正确");
                return;
            }
            this.h = h;
            this.gamma = gamma;
            this.N_pair = N_pair;
            Dictionary<string, double[]> dict = new()
            {
                { "h", h },
                { "gamma", gamma },
                { "N_pair",N_pair.Select(a => (double)a).ToArray() }
            };
            MyDataFrame df = MyDataFrame.create_from_multiple_series(dict);
            dataGridView1.DataSource = df.convert_to_dataTable();
            init_controls(h, gamma, N_pair);
        }

        private void Form_VariogramFit_Load(object sender, EventArgs e)
        {
            numericUpDown1.MouseWheel += NumericUpDown1_MouseWheel;
            numericUpDown2.MouseWheel += NumericUpDown2_MouseWheel;
            numericUpDown3.MouseWheel += NumericUpDown3_MouseWheel;
            numericUpDown1.Enabled = false;
            numericUpDown2.Enabled = false;
            numericUpDown3.Enabled = false;
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

        /// <summary>
        /// 根据实验变差函数拟合实现变差函数，并对所有控件信息进行更新
        /// </summary>
        /// <param name="h"></param>
        /// <param name="gamma"></param>
        void init_controls(double[] h, double[] gamma, int[] N_pair)
        {
            if (comboBox1.Text == "球状模型")
                vt = VariogramType.Spherical;
            if (comboBox1.Text == "高斯模型")
                vt = VariogramType.Guassian;
            if (comboBox1.Text == "指数模型")
                vt = VariogramType.Exponential;
            (variogram_fit, _) = Variogram.variogramFit(vt, h, gamma, N_pair);
            variogram_manual = Variogram.create(vt, variogram_fit.nugget, variogram_fit.sill, variogram_fit.range);

            if (variogram_fit.nugget < 0)
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
        void draw()
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

        //读取excel格式的实验变差函数
        private void button1_Click(object sender, EventArgs e)
        {
            MyDataFrame df = MyDataFrame.read_from_excel();
            dataGridView1.DataSource = df.convert_to_dataTable();
            double[] h = df.get_series<double>(0);
            double[] gamma = df.get_series<double>(1);
            int[] N_pair = df.get_series<double>(2).Select(a => (int)a).ToArray();
            this.h = h;
            this.gamma = gamma;
            this.N_pair = N_pair;
            if (h == null || gamma == null || N_pair == null || h.Length != gamma.Length || h.Length != N_pair.Length)
            {
                MessageBox.Show("输入的实验变差函数不正确");
                return;
            }
            init_controls(h, gamma, N_pair);
            numericUpDown1.Enabled = true;
            numericUpDown2.Enabled = true;
            numericUpDown3.Enabled = true;
        }

        //展示variogramFit_win使用的实例数据
        private void button2_Click(object sender, EventArgs e)
        {
            string embeddedFilePath = "JAM8.资源文件.实验变差函数.xls";
            string ext = Path.GetExtension(embeddedFilePath);
            using Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(embeddedFilePath);
            // 把 Stream 转换成 byte[]   
            byte[] bytes = new byte[stream.Length];
            stream.Read(bytes, 0, bytes.Length);
            // 设置当前流的位置为流的开始   
            stream.Seek(0, SeekOrigin.Begin);
            ExcelHelper.ExcelStreamType excel_stream_type = ExcelHelper.ExcelStreamType.xls;
            if (ext == ".xlsx")
                excel_stream_type = ExcelHelper.ExcelStreamType.xlsx;
            var df = MyDataFrame.read_from_excel(stream, excel_stream_type);
            df.show_win("实例数据", true);
        }

        //选用理论模型
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (h != null)
                init_controls(h, gamma, N_pair);
        }
    }
}
