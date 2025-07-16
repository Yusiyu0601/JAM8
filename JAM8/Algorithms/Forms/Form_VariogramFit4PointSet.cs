using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using JAM8.Algorithms.Geometry;
using JAM8.Algorithms.Numerics;
using JAM8.Utilities;
using MathNet.Numerics.Statistics;
using ScottPlot;

namespace JAM8.Algorithms.Forms
{
    public partial class Form_VariogramFit4PointSet : Form
    {
        private Grid g = null;

        private double[] h;
        private double[] gamma;
        private int[] N_pair;

        private Variogram variogram_fit = null;
        private Variogram variogram_manual = null;
        private VariogramType vt = VariogramType.Spherical;

        public Form_VariogramFit4PointSet()
        {
            InitializeComponent();
        }

        //加载Grid
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

        //选择GridProperty
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

                trackBar_N_lag.TickFrequency = 1;
                trackBar_azimuth.TickFrequency = 1;
                numericUpDown_N_lag.Increment = 1;
                numericUpDown_azimuth.Increment = 1;
            }
        }

        //更新实验变差函数
        private void update_experimental_variogram()
        {
            if (g == null || listBox1.SelectedIndex == -1)
                return;

            var gp = g[listBox1.SelectedItem.ToString()];
            if (gp == null) return;

            // 读取参数
            double azimuth = (double)numericUpDown_azimuth.Value;
            int N_lag = (int)numericUpDown_N_lag.Value;
            double lag_unit = (double)numericUpDown_lag_unit.Value;
            double bandwidth = (double)numericUpDown_bandwidth.Value;
            double azimuth_tolerance = (double)numericUpDown_azimuth_tolerance.Value;

            // 计算实验变差函数
            (h, gamma, N_pair) = Variogram.calc_variogram_from_grid_valid_cells(
                gp, azimuth, N_lag, lag_unit, bandwidth, azimuth_tolerance);

            // 显示实验变差函数数据表
            MyDataFrame df = MyDataFrame.create(["h", "gamma", "N_pairs"]);
            for (int i = 0; i < h.Length; i++)
                df.add_record([h[i], gamma[i], N_pair[i]]);
            dataGridView1.DataSource = df.convert_to_dataTable();

            // 更新变差函数曲线
            draw_variogram_lines();
            // 绘制搜索窗
            draw_search_window();
        }

        //重新绘制所有曲线，包括实验变程函数、拟合、手动调整
        private void draw_variogram_lines()
        {
            formsPlot1.Plot.Clear();
            formsPlot1.Plot.XAxis.Label("滞后距", Color.Black, size: 22, fontName: "宋体");
            formsPlot1.Plot.XAxis.TickLabelStyle(fontSize: 18);
            formsPlot1.Plot.YAxis.Label("变差函数", Color.Black, size: 22, fontName: "宋体");
            formsPlot1.Plot.YAxis.TickLabelStyle(fontSize: 18);

            formsPlot1.Plot.AddScatter(h, gamma, Color.Gray, lineStyle: ScottPlot.LineStyle.None,
                markerShape: ScottPlot.MarkerShape.filledCircle, label: "实验变差函数");

            // if (comboBox1.Text == "球状模型")
            //     vt = VariogramType.Spherical;
            // if (comboBox1.Text == "高斯模型")
            //     vt = VariogramType.Guassian;
            // if (comboBox1.Text == "指数模型")
            //     vt = VariogramType.Exponential;
            // // 自动拟合变差函数
            // (variogram_fit, _) = Variogram.variogramFit(vt, h, gamma, N_pair);
            // List<double> x_fit = [];
            // List<double> y_fit = [];
            // double step = (h.Max() - h.Min()) / 100.0; //步长
            // double maxLagDistance = h.Last();
            // //根据拟合的模型进行等间距重采样
            // for (double lag = 0; lag < maxLagDistance * 1.05; lag += step)
            // {
            //     x_fit.Add(lag);
            //     y_fit.Add(variogram_fit.calc_variogram((float)lag));
            // }
            //
            // formsPlot1.Plot.AddScatter(x_fit.ToArray(), y_fit.ToArray(), Color.Green, lineWidth: 2, markerSize: 0,
            //     label: "球状模型");

            formsPlot1.Refresh();
        }

        /// <summary>
        /// 绘制当前GridProperty的搜索窗与数据点（用于实验变差函数方向分析）
        /// </summary>
        private void draw_search_window()
        {
            if (g == null || listBox1.SelectedIndex == -1)
                return;

            var gp = g[listBox1.SelectedItem.ToString()];
            if (gp == null) return;

            var plt = formsPlot2.Plot;
            plt.Clear();

            var (coord_array_indexes, _) = gp.get_values_by_condition(null, CompareType.NotEqual);

            var coords = gp.grid_structure.array_indexes_to_coords(coord_array_indexes);

            // 中心点选择为图像中心
            double cx = gp.grid_structure.nx / 2;
            double cy = g.gridStructure.ny / 2;

            // 获取界面参数
            double azimuthDeg = (double)numericUpDown_azimuth.Value;
            double bandwidth = (double)numericUpDown_bandwidth.Value;
            double lagUnit = (double)numericUpDown_lag_unit.Value;
            int nLag = (int)numericUpDown_N_lag.Value;

            // 角度转弧度
            double theta = azimuthDeg * Math.PI / 180.0;

            // 主方向单位向量
            double dx = Math.Cos(theta);
            double dy = Math.Sin(theta);

            // 垂直方向单位向量
            double px = -dy;
            double py = dx;

            // 起点左下角
            double cx1 = cx - px * bandwidth / 2.0;
            double cy1 = cy - py * bandwidth / 2.0;
            double lag_total = lagUnit * nLag;

            // 四角
            double p0x = cx1;
            double p0y = cy1;
            double p1x = cx1 + dx * lag_total;
            double p1y = cy1 + dy * lag_total;
            double p2x = p1x + px * bandwidth;
            double p2y = p1y + py * bandwidth;
            double p3x = p0x + px * bandwidth;
            double p3y = p0y + py * bandwidth;

            // 边框
            plt.AddScatter(
                [p0x, p1x, p2x, p3x, p0x],
                [p0y, p1y, p2y, p3y, p0y],
                color: Color.Blue, lineWidth: 1, label: "搜索窗"
            );

            // 滞后线
            for (int i = 1; i < nLag; i++)
            {
                double bx = cx1 + dx * (lagUnit * i);
                double by = cy1 + dy * (lagUnit * i);
                double ex = bx + px * bandwidth;
                double ey = by + py * bandwidth;
                plt.AddLine(bx, by, ex, ey, color: Color.Blue, lineWidth: 1);
            }

            // 中心点
            plt.AddPoint(cx, cy, color: Color.Red, size: 10, MarkerShape.filledCircle);

            // 所有数据点（黑方块）
            foreach (var c in coords)
            {
                plt.AddPoint(c.x, c.y, color: Color.Black, size: 6, MarkerShape.filledSquare);
            }

            // --- 添加容差角边界线 ---
            double deltaDeg = (double)numericUpDown_azimuth_tolerance.Value; // 用户设定的容差角
            double angle1 = (azimuthDeg + deltaDeg) * Math.PI / 180.0;
            double angle2 = (azimuthDeg - deltaDeg) * Math.PI / 180.0;
            double radius = lag_total * 1.2; // 容差角线的长度稍大于滞后窗

            // 边界线1：+delta
            double x1 = cx + Math.Cos(angle1) * radius;
            double y1 = cy + Math.Sin(angle1) * radius;
            plt.AddLine(cx, cy, x1, y1, color: Color.Orange, lineWidth: 1);

            // 边界线2：-delta
            double x2 = cx + Math.Cos(angle2) * radius;
            double y2 = cy + Math.Sin(angle2) * radius;
            plt.AddLine(cx, cy, x2, y2, color: Color.Orange, lineWidth: 1);

            // 可选：添加注释
            plt.AddText("+容差", x1, y1, size: 10, color: Color.Orange);
            plt.AddText("-容差", x2, y2, size: 10, color: Color.Orange);


            plt.AxisAuto();
            formsPlot2.Refresh();
        }

        private void trackBar_N_lag_Scroll(object sender, EventArgs e)
        {
            numericUpDown_N_lag.Value = trackBar_N_lag.Value;
        }

        private void numericUpDown_N_lag_ValueChanged(object sender, EventArgs e)
        {
            trackBar_N_lag.Value = (int)numericUpDown_N_lag.Value;
            update_experimental_variogram();
        }

        private void trackBar_azimuth_Scroll(object sender, EventArgs e)
        {
            numericUpDown_azimuth.Value = trackBar_azimuth.Value;
        }

        private void numericUpDown_azimuth_ValueChanged(object sender, EventArgs e)
        {
            trackBar_azimuth.Value = (int)numericUpDown_azimuth.Value;
            update_experimental_variogram();
        }

        private void trackBar_lag_unit_Scroll(object sender, EventArgs e)
        {
            numericUpDown_lag_unit.Value = (decimal)trackBar_lag_unit.Value; // 例如每单位0.1
        }

        private void numericUpDown_lag_unit_ValueChanged(object sender, EventArgs e)
        {
            trackBar_lag_unit.Value = (int)(numericUpDown_lag_unit.Value); // 精度转换回去
            update_experimental_variogram();
        }

        private void trackBar_bandwidth_Scroll(object sender, EventArgs e)
        {
            numericUpDown_bandwidth.Value = (decimal)trackBar_bandwidth.Value;
        }

        private void numericUpDown_bandwidth_ValueChanged(object sender, EventArgs e)
        {
            trackBar_bandwidth.Value = (int)(numericUpDown_bandwidth.Value);
            update_experimental_variogram();
        }

        private void trackBar_azimuth_tolerance_Scroll(object sender, EventArgs e)
        {
            numericUpDown_azimuth_tolerance.Value = trackBar_azimuth_tolerance.Value;
        }

        private void numericUpDown_azimuth_tolerance_ValueChanged(object sender, EventArgs e)
        {
            trackBar_azimuth_tolerance.Value = (int)numericUpDown_azimuth_tolerance.Value;
            update_experimental_variogram();
        }

        //设置变差函数理论模型
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            update_experimental_variogram();
        }
    }
}