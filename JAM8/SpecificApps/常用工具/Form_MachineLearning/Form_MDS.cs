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
using JAM8.Algorithms.MachineLearning;
using JAM8.Algorithms.Numerics;
using JAM8.Utilities;

namespace JAM8.SpecificApps.常用工具
{
    public partial class Form_MDS : Form
    {
        MyDataFrame df_距离矩阵 = null;
        MyDataFrame df_记录表 = null;

        public Form_MDS()
        {
            InitializeComponent();

            comboBox2.Items.Add("曼哈顿距离");
            comboBox2.Items.Add("欧式距离");
            comboBox2.SelectedIndex = 0;
        }

        //读取距离矩阵
        private void button1_Click(object sender, EventArgs e)
        {
            df_距离矩阵 = MyDataFrame.read_from_excel();
            dataGridView1.DataSource = df_距离矩阵.convert_to_dataTable();
        }

        //读取记录，计算距离矩阵
        private void button3_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = null;

            df_记录表 = MyDataFrame.read_from_excel();
            if (df_记录表 == null)
                return;
            df_记录表.show_win("记录表");


            listBox1.Items.Clear();
            foreach (var item in df_记录表.series_names)
            {
                listBox1.Items.Add(item);
            }

            comboBox1.Items.Clear();
            foreach (var item in df_记录表.series_names)
            {
                comboBox1.Items.Add(item);
            }
            comboBox1.SelectedIndex = 0;
        }
        //计算距离矩阵
        private void button5_Click(object sender, EventArgs e)
        {
            MyDataFrame subset = df_记录表.get_series_subset(listBox1.SelectedItems.Cast<string>().ToArray());
            MyMatrix mat = MyMatrix.create(subset.N_Record, subset.N_Record);

            for (int j = 0; j < subset.N_Record; j++)
            {
                for (int i = 0; i < subset.N_Record; i++)
                {
                    var v1 = subset.get_record(i).Select(a => Convert.ToSingle(a.Value)).ToArray();
                    var v2 = subset.get_record(j).Select(a => Convert.ToSingle(a.Value)).ToArray();
                    if (comboBox2.Text == "曼哈顿距离")
                        mat[i, j] = MyDistance.calc_manhattan(v1, v2);
                    if (comboBox2.Text == "欧式距离")
                        mat[i, j] = MyDistance.calc_euclidean(v1, v2);
                }
            }

            string[] series_names = null;
            if (checkBox2.Checked == true)//采用记录表中某列记录作为矩阵列名
            {
                series_names = df_记录表.get_series<string>(comboBox1.Text);
            }
            else
            {
                series_names = new string[subset.N_Record];
                for (int i = 1; i <= subset.N_Record; i++)
                {
                    series_names[i - 1] = $"id_{i}";
                }
            }

            df_距离矩阵 = MyDataFrame.create_from_array(series_names, mat.buffer);
            dataGridView1.DataSource = df_距离矩阵.convert_to_dataTable();
        }
        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            comboBox1.Enabled = checkBox2.Checked;
        }

        //MDS
        private void button2_Click(object sender, EventArgs e)
        {
            int N_dims = int.Parse(textBox1.Text);
            var locs = CMDSCALE.CMDSCALE_MathNet(df_距离矩阵.convert_to_double_2dArray(), N_dims);
            string[] series_names = new string[N_dims];
            for (int i = 1; i <= N_dims; i++)
                series_names[i - 1] = $"dim{i}";
            var result = MyDataFrame.create_from_array(series_names, locs);
            result.add_series("ID");
            result.move_series("ID", 0);
            result.copy_series_from(df_距离矩阵.series_names, "ID");
            result.show_win("降维后");
        }
        //二维MDS，并显示二维图像
        private void button4_Click(object sender, EventArgs e)
        {
            int N_dims = 2;//降低目标维度
            var locs = CMDSCALE.CMDSCALE_MathNet(df_距离矩阵.convert_to_double_2dArray(), N_dims);
            string[] series_names = new string[] { "dim1", "dim2" };
            var result = MyDataFrame.create_from_array(series_names, locs);
            result.add_series("ID");
            result.move_series("ID", 0);
            result.copy_series_from(df_距离矩阵.series_names, "ID");
            result.show_win("降维后");

            Form_QuickChart.ScatterPlot(result.get_series<double>("dim1"),
                result.get_series<double>("dim2"), df_距离矩阵.series_names);
        }

        private void label9_Click(object sender, EventArgs e)
        {
            string embeddedFilePath = "JAM8.资源文件.[示例数据]全国各省会之间的距离.xls";
            MyEmbeddedFileHelper.read_embedded_excel(embeddedFilePath).show_win("[示例数据]全国各省会之间的距离矩阵");
        }

        private void label5_Click(object sender, EventArgs e)
        {
            string embeddedFilePath = "JAM8.资源文件.[示例数据]城市坐标.xlsx";
            MyEmbeddedFileHelper.read_embedded_excel(embeddedFilePath).show_win("[示例数据]城市坐标记录表");
        }
    }
}
