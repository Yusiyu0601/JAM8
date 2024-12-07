using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using JAM8.Algorithms.MachineLearning;
using JAM8.Utilities;

namespace JAM8.SpecificApps.常用工具
{
    public partial class Form_KMeans : Form
    {
        MyDataFrame df = null;

        public Form_KMeans()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            df = MyDataFrame.read_from_excel();
            if (df == null) 
                return;
            df.show_win();

            listBox1.Items.Clear();
            foreach (var item in df.series_names)
            {
                listBox1.Items.Add(item);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var result = KMeansCluster.clustering(df, listBox1.SelectedItems.Cast<string>().ToArray(), int.Parse(textBox1.Text));
            result.show_win();
        }
    }
}
