using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using JAM8.Algorithms;
using JAM8.Utilities;

namespace JAM8.SpecificApps.常用工具
{
    public partial class Form_ScatterPlot : Form
    {
        MyDataFrame df = null;

        public Form_ScatterPlot()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            df = MyDataFrame.read_from_excel();
            if (df == null)
                return;
            df.show_win("散点数据文件");

            comboBox1.Items.Clear();
            comboBox2.Items.Clear();
            comboBox3.Items.Clear();

            foreach (var item in df.series_names)
            {
                comboBox1.Items.Add(item);
                comboBox2.Items.Add(item);
                comboBox3.Items.Add(item);
            }

            comboBox1.SelectedIndex = 0;
            comboBox2.SelectedIndex = 0;
            comboBox3.SelectedIndex = 0;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form_QuickChart.ScatterPlot(df.get_series<double>(comboBox1.Text),
                df.get_series<double>(comboBox2.Text), df.get_series<string>(comboBox3.Text));
        }
    }
}
