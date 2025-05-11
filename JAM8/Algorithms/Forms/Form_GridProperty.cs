namespace JAM8.Algorithms.Geometry
{
    public partial class Form_GridProperty : Form
    {
        private GridProperty _gp = null;

        public Form_GridProperty(GridProperty gp)
        {
            InitializeComponent();

            _gp = gp;
            Display();
        }

        private void Display()
        {
            if (_gp == null) return;
            txt_ICount.Text = _gp.grid_structure.nx.ToString();
            txt_JCount.Text = _gp.grid_structure.ny.ToString();
            txt_KCount.Text = _gp.grid_structure.nz.ToString();
            txt_ISize.Text = _gp.grid_structure.xsiz.ToString();
            txt_JSize.Text = _gp.grid_structure.ysiz.ToString();
            txt_KSize.Text = _gp.grid_structure.zsiz.ToString();
            txt_OriginCellX.Text = _gp.grid_structure.xmn.ToString();
            txt_OriginCellY.Text = _gp.grid_structure.ymn.ToString();
            txt_OriginCellZ.Text = _gp.grid_structure.zmn.ToString();

            txt_MinValue.Text = _gp.Min.ToString();
            txt_MaxValue.Text = _gp.Max.ToString();
            txt_MeanValue.Text = _gp.buffer.Average().ToString();

            txt_CellCount.Text = _gp.grid_structure.N.ToString();
            txt_CellCountOfNull.Text = _gp.N_Nulls.ToString();


            List<double> data = [];
            for (int i = 0; i < _gp.grid_structure.N; i++)
            {
                if (_gp.get_value(i) != null)
                {
                    data.Add(_gp.get_value(i).Value);
                }
                if (_gp.get_value(i) == null)
                {
                    continue;
                }
            }

            var histogram = _gp.Histogram;
            double[] values = [.. histogram.Item1];
            double[] positions = [.. histogram.Item2];

            List<double> cdf = [];
            double sum = 0;
            for (int i = 0; i < values.Length; i++)
            {
                sum += values[i];
                cdf.Add(sum);
            }

            var plt = formsPlot1.Plot;

            var bar = plt.AddBar(values, positions, Color.DarkCyan);
            bar.YAxisIndex = plt.LeftAxis.AxisIndex;
            bar.BarWidth = (positions[1] - positions[0]) * .8;

            var scatter = plt.AddScatter(positions, [.. cdf], Color.Blue, 1, 5);
            scatter.YAxisIndex = plt.RightAxis.AxisIndex;
            plt.YAxis.Grid(false);
            plt.RightAxis.Ticks(true);
            plt.RightAxis.Grid(true);
            plt.SetAxisLimits(yMin: 0, yMax: 1, yAxisIndex: 1);

            plt.SetAxisLimits(yMin: 0);
            formsPlot1.Refresh();
        }

        private void Form_GridProperty_Load(object sender, System.EventArgs e)
        {

        }
    }
}
