using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using JAM8.Utilities;

namespace JAM8.SpecificApps.常用工具
{
    public partial class Form_TrainTestSplitData : Form
    {
        MyDataFrame df;
        public Form_TrainTestSplitData()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            df = MyDataFrame.read_from_excel();
            df.show_win("数据集");

            comboBox1.Items.Clear();
            comboBox1.Items.AddRange(df.series_names);
            comboBox1.SelectedIndex = 0;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            try
            {
                double v1 = double.Parse(textBox1.Text);
                textBox2.Text = (1 - v1).ToString();
            }
            catch
            {
            }
        }

        //Split数据集
        private void button2_Click(object sender, EventArgs e)
        {
            double ratio_train = double.Parse(textBox1.Text);//训练集占比
            if (checkBox1.Checked == false)//
            {
                int N_train = (int)(ratio_train * df.N_Record);
                var indexes = MyGenerator.range(0, df.N_Record, 1);
                var indexes_train = SortHelper.RandomSelect(indexes, N_train, new Random());
                MyDataFrame df_train = df.get_record_subset(indexes_train);
                df_train.show_win("训练数据集");

                var indexes_test = new List<int>(indexes);
                indexes_test.RemoveAll(indexes_train.Contains);
                MyDataFrame df_test = df.get_record_subset(indexes_test);
                df_test.show_win("测试数据集");
            }
            else//保持比例
            {
                string series_name_categories = comboBox1.Text;//属类别的序列名称
                var series_output = df.get_series<string>(series_name_categories);
                var (_, categories, counts) = MyDistinct.distinct(series_output);//类别去重
                Dictionary<string, List<int>> dict = new();
                foreach (var item in categories)
                    dict.Add(item, new List<int>());

                for (int iRecord = 0; iRecord < df.N_Record; iRecord++)
                {
                    var category = df[iRecord, series_name_categories].ToString();
                    dict[category].Add(iRecord);
                }
                //为每个类提取给定比例的记录作为训练集和测试集
                List<int> indexes_train = new();
                List<int> indexes_test = new();
                foreach (var (category, indexes_category) in dict)
                {
                    int N_train_category = (int)(ratio_train * indexes_category.Count);//该类的训练集
                    //随机抽样
                    var indexes_train_category = SortHelper.RandomSelect(indexes_category, N_train_category, new Random());
                    indexes_train.AddRange(indexes_train_category);

                    var indexes_test_category = new List<int>(indexes_category);//该类的测试集
                    indexes_test_category.RemoveAll(indexes_train_category.Contains);
                    indexes_test.AddRange(indexes_test_category);
                }
                MyDataFrame df_train = df.get_record_subset(indexes_train);
                df_train.show_win("训练数据集");
                MyDataFrame df_test = df.get_record_subset(indexes_test);
                df_test.show_win("测试数据集");
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            comboBox1.Enabled = checkBox1.Checked;
        }
    }
}
