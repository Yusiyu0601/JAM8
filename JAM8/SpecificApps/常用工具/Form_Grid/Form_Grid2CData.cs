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

namespace JAM8.SpecificApps.常用工具
{
    public partial class Form_Grid2CData : Form
    {
        private Grid g;

        public Form_Grid2CData()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            (g, _) = Grid.create_from_gslibwin();
            if (g == null)
                return;

            #region 加载属性列表

            listBox1.Items.Clear();
            foreach (var PropertyName in g.propertyNames)
            {
                listBox1.Items.Add(PropertyName);
            }

            #endregion
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex == -1 || listBox1.SelectedItem.ToString() == string.Empty)
                return;
            else
            {
                string gp_name = listBox1.SelectedItem.ToString();
                float value = float.Parse(textBox1.Text);
                bool b = radioButton1.Checked == true ? true : false;
                CData cd = CData.create_from_gridProperty(g, gp_name, value, b);
                cd.save_to_gslibwin();
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex == -1 || listBox1.SelectedItem.ToString() == string.Empty)
                return;
            else
                scottplot4GridProperty1.update_gridProperty(g[listBox1.SelectedItem.ToString()]);
        }
    }
}
