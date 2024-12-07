using JAM8.Utilities;
using ScottPlot;

namespace JAM8.SpecificApps.常用工具
{
    public partial class Form_展示测井曲线 : Form
    {
        public Form_展示测井曲线()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var dt = ExcelHelper.excel_to_dataTable(FileDialogHelper.OpenExcel());
            var df = MyDataFrame.create_from_dataTable(dt);

            var plt = formsPlot1.Plot;
            plt.Clear();
            var scatter = plt.AddScatter(df.get_series<double>(1), df.get_series<double>(0), Color.Red, 1, 5,
                MarkerShape.none,
                LineStyle.Solid);
            formsPlot1.Refresh();

            plt = formsPlot2.Plot;
            plt.Clear();
            var scatter2 = plt.AddScatter(df.get_series<double>(2), df.get_series<double>(0), Color.Green, 1, 5,
                MarkerShape.none,
                LineStyle.Solid);
            formsPlot2.Refresh();

            plt = formsPlot3.Plot;
            plt.Clear();
            var scatter3 = plt.AddScatter(df.get_series<double>(3), df.get_series<double>(0), Color.Blue, 1, 5,
                MarkerShape.none,
                LineStyle.Solid);
            formsPlot3.Refresh();

            formsPlot1.Configuration.AddLinkedControl(formsPlot2, false, true);
            formsPlot1.Configuration.AddLinkedControl(formsPlot3, false, true);
            formsPlot2.Configuration.AddLinkedControl(formsPlot1, false, true);
            formsPlot2.Configuration.AddLinkedControl(formsPlot3, false, true);
            formsPlot3.Configuration.AddLinkedControl(formsPlot1, false, true);
            formsPlot3.Configuration.AddLinkedControl(formsPlot2, false, true);
            //DataTableHelper.Show(dt);

        }

    }
}
