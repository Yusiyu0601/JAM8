using System.Numerics;
using JAM8.Algorithms.Geometry;
using ScottPlot;
using ScottPlot.Drawing.Colormaps;

namespace JAM8.Algorithms
{
    public partial class Form_QuickChart : Form
    {
        public Form_QuickChart(string ChartName = "")
        {
            InitializeComponent();
            Text = $"{ChartName}";
        }

        #region 绘制点图 ScatterPlot

        /// <summary>
        /// 绘制点图
        /// </summary>
        /// <param name="x_data"></param>
        /// <param name="y_data"></param>
        /// <param name="x_label"></param>
        /// <param name="y_label"></param>
        /// <param name="main_title"></param>
        public static void ScatterPlot(IEnumerable<double> x_data, IEnumerable<double> y_data,
            IEnumerable<string> point_labels = null, string x_label = "x", string y_label = "y",
            string main_title = "title")
        {
            Form_QuickChart frm = new();
            frm.Text = $"{main_title}";
            var plt = frm.formsPlot1.Plot;
            plt.Title(main_title);
            plt.XLabel(x_label);
            plt.YLabel(y_label);
            plt.Clear();
            var scatter = plt.AddScatter(x_data.ToArray(), y_data.ToArray(), Color.Black, 1, 5,
                MarkerShape.filledCircle, LineStyle.None);
            if (point_labels != null) 
                scatter.DataPointLabels = point_labels.ToArray();
            frm.formsPlot1.Refresh();
            frm.ShowDialog();
        }

        public static void ScatterPlot(IEnumerable<float> x_data, IEnumerable<float> y_data,
            IEnumerable<string> point_labels = null, string x_label = "x", string y_label = "y",
            string main_title = "title")
        {
            var x_data1 = x_data.Select(a => (double)a);
            var y_data1 = y_data.Select(a => (double)a);
            ScatterPlot(x_data1, y_data1, point_labels, x_label, y_label, main_title);
        }

        public void DrawScatter(IEnumerable<double> x_data, IEnumerable<double> y_data,
            IEnumerable<string> point_labels = null, string x_label = "x", string y_label = "y",
            string main_title = "title")
        {
            Text = $"{main_title}";
            var plt = formsPlot1.Plot;
            plt.Title(main_title);
            plt.XLabel(x_label);
            plt.YLabel(y_label);
            plt.Clear();
            var scatter = plt.AddScatter(x_data.ToArray(), y_data.ToArray(), Color.Black, 1, 5,
                MarkerShape.filledCircle, LineStyle.None);
            scatter.DataPointLabels = point_labels.ToArray();
            formsPlot1.Refresh();
        }

        #endregion

        #region 绘制线图 LinePlot

        /// <summary>
        /// 绘制线图
        /// </summary>
        /// <param name="x_data"></param>
        /// <param name="y_data"></param>
        /// <param name="x_label"></param>
        /// <param name="y_label"></param>
        /// <param name="main_title"></param>
        public static void LinePlot(IEnumerable<double> x_data, IEnumerable<double> y_data,
            IEnumerable<string> point_labels = null, string x_label = "x", string y_label = "y",
            string main_title = "title")
        {
            Form_QuickChart frm = new();
            frm.Text = $"{main_title}";
            var plt = frm.formsPlot1.Plot;
            plt.Title(main_title);
            plt.XLabel(x_label);
            plt.YLabel(y_label);
            plt.Clear();
            var scatter = plt.AddScatter(x_data.ToArray(), y_data.ToArray(), Color.Black, 1, 5, MarkerShape.none,
                LineStyle.Solid);
            scatter.DataPointLabels = point_labels.ToArray();
            frm.formsPlot1.Refresh();
            frm.Show();
        }

        public static void LinePlot(IEnumerable<float> x_data, IEnumerable<float> y_data,
            IEnumerable<string> point_labels = null, string x_label = "x", string y_label = "y",
            string main_title = "title")
        {
            var x_data1 = x_data.Select(a => (double)a);
            var y_data1 = y_data.Select(a => (double)a);
            LinePlot(x_data1, y_data1, point_labels, x_label, y_label, main_title);
        }

        public void DrawLine(IEnumerable<double> x_data, IEnumerable<double> y_data,
            IEnumerable<string> point_labels = null, string x_label = "x", string y_label = "y",
            string main_title = "title")
        {
            Text = $"{main_title}";
            var plt = formsPlot1.Plot;
            plt.Title(main_title);
            plt.XLabel(x_label);
            plt.YLabel(y_label);
            plt.Clear();
            var scatter = plt.AddScatter(x_data.ToArray(), y_data.ToArray(), Color.Black, 1, 5, MarkerShape.none,
                LineStyle.Solid);
            scatter.DataPointLabels = point_labels.ToArray();
            formsPlot1.Refresh();
        }

        #endregion

        #region 绘制条形图 BarPlot

        /// <summary>
        /// 绘制线图
        /// </summary>
        /// <param name="x_data"></param>
        /// <param name="y_data"></param>
        /// <param name="x_label"></param>
        /// <param name="y_label"></param>
        /// <param name="main_title"></param>
        public static void BarPlot(IEnumerable<double> values, IEnumerable<double> positions, string x_label = "x",
            string y_label = "y", string main_title = "title")
        {
            Form_QuickChart frm = new()
            {
                Text = $"{main_title}"
            };
            var plt = frm.formsPlot1.Plot;
            plt.Title(main_title);
            plt.XLabel(x_label);
            plt.YLabel(y_label);
            plt.Clear();
            var bar = plt.AddBar(values.ToArray(), positions.ToArray());
            double[] positions_ = positions.ToArray();
            double[] values_ = values.ToArray();
            bar.BarWidth = (positions_[1] - positions_[0]) * .8;
            frm.formsPlot1.Refresh();
            frm.Show();
        }

        public static void BarPlot(IEnumerable<float> values, IEnumerable<float> positions, string x_label = "x",
            string y_label = "y", string main_title = "title")
        {
            var values_ = values.Select(a => (double)a);
            var positions_ = positions.Select(a => (double)a);
            BarPlot(values_, positions_, x_label, y_label, main_title);
        }

        public void DrawBar(IEnumerable<double> values, IEnumerable<double> positions, string x_label = "x",
            string y_label = "y", string main_title = "title")
        {
            Text = $"{main_title}";
            var plt = formsPlot1.Plot;
            plt.Title(main_title);
            plt.XLabel(x_label);
            plt.YLabel(y_label);
            plt.Clear();

            double[] positions_ = positions.ToArray();
            double[] values_ = values.ToArray();
            var bar = plt.AddBar(values.ToArray(), positions.ToArray());
            bar.BarWidth = (positions_[1] - positions_[0]) * .8;
            formsPlot1.Refresh();
        }

        #endregion

        /// <summary>
        /// 绘制图像
        /// </summary>
        /// <param name="b"></param>
        /// <param name="x_label"></param>
        /// <param name="y_label"></param>
        /// <param name="main_title"></param>
        public static void ImagePlot(Bitmap b, string x_label = "x", string y_label = "y", string main_title = "title")
        {
            Form_QuickChart frm = new();
            frm.Text = $"{main_title}";
            var plt = frm.formsPlot1.Plot;
            plt.Title(main_title);
            plt.XLabel(x_label);
            plt.YLabel(y_label);
            plt.Clear();
            var img = plt.AddImage(b, 0, 0);
            img.HeightInAxisUnits = 5;
            img.WidthInAxisUnits = 5;
            plt.AxisScaleLock(true);
            frm.formsPlot1.Refresh();
            frm.Show();
        }

        public static void ImagePlot(IList<Bitmap> images, IList<float> xs, IList<float> ys,
            string x_label = "x", string y_label = "y", string main_title = "title")
        {
            Form_QuickChart frm = new();
            frm.Text = $"{main_title}";
            var plt = frm.formsPlot1.Plot;
            plt.Title(main_title);
            plt.XLabel(x_label);
            plt.YLabel(y_label);
            plt.Clear();

            var images1 = images.ToArray();
            var xs1 = xs.ToArray();
            var ys1 = ys.ToArray();
            for (int i = 0; i < images.Count(); i++)
            {
                var img = plt.AddImage(images1[i], xs1[i], ys1[i], scale: 2);
            }

            plt.AxisScaleLock(true);
            frm.formsPlot1.Refresh();
            frm.Show();
        }

        /// <summary>
        /// 绘制二维数组
        /// </summary>
        /// <param name="array"></param>
        /// <param name="x_label"></param>
        /// <param name="y_label"></param>
        /// <param name="main_title"></param>
        public static void ArrayPlot(double[,] array, string x_label = "x", string y_label = "y",
            string main_title = "title")
        {
            Form_QuickChart frm = new()
            {
                Text = $"{main_title}"
            };
            var plt = frm.formsPlot1.Plot;
            plt.Title(main_title);
            plt.XLabel(x_label);
            plt.YLabel(y_label);
            plt.Clear();
            int n_dim0 = array.GetLength(0);
            int n_dim1 = array.GetLength(1);
            Bitmap b = new(n_dim0, n_dim1);
            for (int idx_dim0 = 0; idx_dim0 < n_dim0; idx_dim0++)
            {
                for (int idx_dim1 = 0; idx_dim1 < n_dim1; idx_dim1++)
                {
                    int value = (int)array[idx_dim0, idx_dim1];
                    b.SetPixel(idx_dim0, idx_dim1, Color.FromArgb(value, value, value));
                }
            }

            var img = plt.AddImage(b, 0, 0);
            img.HeightInAxisUnits = 5;
            img.WidthInAxisUnits = 5;
            plt.AxisScaleLock(true);
            frm.formsPlot1.Refresh();
            frm.Show();
        }

        /// <summary>
        /// 绘制二维数组(复数)
        /// </summary>
        /// <param name="array"></param>
        /// <param name="type">0:实部 1:虚部 2:Magnitude</param>
        /// <param name="x_label"></param>
        /// <param name="y_label"></param>
        /// <param name="main_title"></param>
        public static void ArrayPlot(Complex[,] array, int type = 2, string x_label = "x", string y_label = "y",
            string main_title = "title")
        {
            int n_dim0 = array.GetLength(0);
            int n_dim1 = array.GetLength(1);
            double[,] array1 = new double[n_dim0, n_dim1];
            for (int idx_dim0 = 0; idx_dim0 < n_dim0; idx_dim0++)
            {
                for (int idx_dim1 = 0; idx_dim1 < n_dim1; idx_dim1++)
                {
                    if (type == 0)
                        array1[idx_dim0, idx_dim1] = (int)array[idx_dim0, idx_dim1].Real;
                    if (type == 1)
                        array1[idx_dim0, idx_dim1] = (int)array[idx_dim0, idx_dim1].Imaginary;
                    if (type == 2)
                        array1[idx_dim0, idx_dim1] = (int)array[idx_dim0, idx_dim1].Magnitude;
                }
            }

            ArrayPlot(array1, x_label, y_label, main_title);
        }

        /// <summary>
        /// 绘制二维数组
        /// </summary>
        public static void ArrayPlot2(double[,] array)
        {
            int n_dim0 = array.GetLength(0);
            int n_dim1 = array.GetLength(1);
            GridProperty gp = GridProperty.create(GridStructure.create_simple(n_dim0, n_dim1, 1));
            for (int idx_dim0 = 0; idx_dim0 < array.GetLength(0); idx_dim0++)
            {
                for (int idx_dim1 = 0; idx_dim1 < array.GetLength(1); idx_dim1++)
                {
                    gp.set_value(idx_dim0, idx_dim1, (float?)array[idx_dim0, idx_dim1]);
                }
            }

            gp.show_win();
        }

        /// <summary>
        /// 绘制二维数组(复数)
        /// </summary>
        /// <param name="array"></param>
        /// <param name="type">0:实部 1:虚部 2:Magnitude</param>
        public static void ArrayPlot2(Complex[,] array, int type = 2)
        {
            int n_dim0 = array.GetLength(0);
            int n_dim1 = array.GetLength(1);
            double[,] array1 = new double[n_dim0, n_dim1];
            for (int idx_dim0 = 0; idx_dim0 < n_dim0; idx_dim0++)
            {
                for (int idx_dim1 = 0; idx_dim1 < n_dim1; idx_dim1++)
                {
                    if (type == 0)
                        array1[idx_dim0, idx_dim1] = array[idx_dim0, idx_dim1].Real;
                    if (type == 1)
                        array1[idx_dim0, idx_dim1] = array[idx_dim0, idx_dim1].Imaginary;
                    if (type == 2)
                        array1[idx_dim0, idx_dim1] = array[idx_dim0, idx_dim1].Magnitude;
                }
            }

            ArrayPlot2(array1);
        }

        /// <summary>
        /// 绘制三维数组
        /// </summary>
        public static void ArrayPlot2(double[,,] array)
        {
            int n_dim0 = array.GetLength(0);
            int n_dim1 = array.GetLength(1);
            int n_dim2 = array.GetLength(2);
            GridProperty gp = GridProperty.create(GridStructure.create_simple(n_dim0, n_dim1, n_dim2));
            for (int idx_dim2 = 0; idx_dim2 < array.GetLength(2); idx_dim2++)
            {
                for (int idx_dim1 = 0; idx_dim1 < array.GetLength(1); idx_dim1++)
                {
                    for (int idx_dim0 = 0; idx_dim0 < array.GetLength(0); idx_dim0++)
                    {
                        gp.set_value(idx_dim0, idx_dim1, idx_dim2, (float?)array[idx_dim0, idx_dim1, idx_dim2]);
                    }
                }
            }

            gp.show_win();
        }

        /// <summary>
        /// 绘制三维数组(复数)
        /// </summary>
        /// <param name="array"></param>
        /// <param name="type">0:实部 1:虚部 2:Magnitude</param>
        public static void ArrayPlot2(Complex[,,] array, int type = 2)
        {
            int n_dim0 = array.GetLength(0);
            int n_dim1 = array.GetLength(1);
            int n_dim2 = array.GetLength(2);
            double[,,] array1 = new double[n_dim0, n_dim1, n_dim2];
            for (int idx_dim2 = 0; idx_dim2 < array.GetLength(2); idx_dim2++)
            {
                for (int idx_dim1 = 0; idx_dim1 < array.GetLength(1); idx_dim1++)
                {
                    for (int idx_dim0 = 0; idx_dim0 < array.GetLength(0); idx_dim0++)
                    {
                        if (type == 0)
                            array1[idx_dim0, idx_dim1, idx_dim2] = array[idx_dim0, idx_dim1, idx_dim2].Real;
                        if (type == 1)
                            array1[idx_dim0, idx_dim1, idx_dim2] = array[idx_dim0, idx_dim1, idx_dim2].Imaginary;
                        if (type == 2)
                            array1[idx_dim0, idx_dim1, idx_dim2] = array[idx_dim0, idx_dim1, idx_dim2].Magnitude;
                    }
                }
            }

            ArrayPlot2(array1);
        }
    }
}