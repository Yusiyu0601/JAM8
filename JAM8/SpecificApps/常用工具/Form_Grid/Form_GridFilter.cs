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
    public partial class Form_GridFilter : Form
    {
        Grid g;
        GridProperty gp_filtered;//过滤后的gp

        public Form_GridFilter()
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

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex == -1 || listBox1.SelectedItem.ToString() == string.Empty)
                return;
            else
                scottplot4GridProperty1.update_gridProperty(g[listBox1.SelectedItem.ToString()]);
        }

        //过滤计算
        private void button3_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex == -1 || listBox1.SelectedItem.ToString() == string.Empty)
                return;
            else
            {
                float 阈值 = float.Parse(textBox1.Text);
                float 新数值 = float.Parse(textBox2.Text);
                string gp_name = listBox1.SelectedItem.ToString();
                if (checkBox1.Checked == false)//重新过滤
                    gp_filtered = g[gp_name].deep_clone();
                else//持续过滤
                {
                    if (gp_filtered == null)//如果是第一次过滤，gp_filtered需要赋初值
                        gp_filtered = g[gp_name].deep_clone();
                }
                if (rb_大于.Checked)
                    gp_filtered.set_values(MyCompareType.greater_than, 阈值, 新数值);
                if (rb_大于等于.Checked)
                    gp_filtered.set_values(MyCompareType.greater_equal_than, 阈值, 新数值);
                if (rb_小于.Checked)
                    gp_filtered.set_values(MyCompareType.less_than, 阈值, 新数值);
                if (rb_小于等于.Checked)
                    gp_filtered.set_values(MyCompareType.less_equal_than, 阈值, 新数值);
                if (rb_等于.Checked)
                    gp_filtered.set_values(MyCompareType.equal, 阈值, 新数值);
                if (rb_不等于.Checked)
                    gp_filtered.set_values(MyCompareType.not_equal, 阈值, 新数值);

                scottplot4GridProperty2.update_gridProperty(gp_filtered);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            g.add_gridProperty("filtered", gp_filtered);
            Grid.save_to_gslibwin(g);
        }
    }
}
