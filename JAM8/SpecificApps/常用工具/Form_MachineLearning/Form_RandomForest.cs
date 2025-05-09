using System.Data;
using JAM8.Algorithms.MachineLearning;
using JAM8.Utilities;

namespace JAM8.SpecificApps.常用工具
{
    public partial class Form_RandomForest : Form
    {
        private MyDataFrame df_train = null;
        private MyDataFrame df_predict = null;

        private MyRandomForest my_rf = null;

        public Form_RandomForest()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            df_train = MyDataFrame.read_from_excel();
            df_train.show_win("训练数据表");

            listBox1.Items.Clear();
            listBox2.Items.Clear();
            listBox3.Items.Clear();
            listBox4.Items.Clear();
            listBox5.Items.Clear();
            if (my_rf == null)
                label6.Text = "无模型";

            foreach (var item in df_train.series_names)
            {
                listBox1.Items.Add(item);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            listBox2.Items.Clear();
            listBox2.Items.AddRange(listBox1.SelectedItems.Cast<string>().ToArray());

        }

        private void button3_Click(object sender, EventArgs e)
        {
            listBox3.Items.Clear();
            listBox3.Items.AddRange(listBox1.SelectedItems.Cast<string>().ToArray());

        }

        private void button7_Click(object sender, EventArgs e)
        {
            df_predict = MyDataFrame.read_from_excel();
            df_predict.show_win("待预测数据记录表");

            listBox4.Items.Clear();
            listBox5.Items.Clear();
            foreach (var item in df_predict.series_names)
            {
                listBox4.Items.Add(item);
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            listBox5.Items.Clear();
            listBox5.Items.AddRange(listBox4.SelectedItems.Cast<string>().ToArray());

        }

        //训练
        private void button6_Click(object sender, EventArgs e)
        {
            if (listBox2.Items.Count == 0 || listBox3.Items.Count == 0)
            {
                MessageBox.Show("选择输入和输出列名", "提示");
                return;
            }
            my_rf = MyRandomForest.create(df_train, listBox2.Items.Cast<string>().ToArray(), listBox3.Items.Cast<string>().ToArray()[0]);
            if (my_rf != null)
                label6.Text = "分类模型";
            MessageBox.Show("训练完成");
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (my_rf == null)
            {
                MessageBox.Show("随机森林模型未建立");
            }
            var predict = my_rf.predict(df_predict, listBox5.Items.Cast<string>().ToArray());
            predict.show_win();


            //int[] actual = df_predict.get_series<int>(listBox3.Items.Cast<string>().ToArray()[0]);
            //int[] ouput = predict.get_series<int>(predict.series_names.Last());
            //ConfusionMatrix cm = new(actual, ouput, 1, 2);

            //dataGridView1.DataSource = new[] { cm };
        }

        private void label4_Click(object sender, EventArgs e)
        {
            string embeddedFilePath = "JAM8.资源文件.[实例数据]seeds.xlsx";
            MyEmbeddedFileHelper.read_embedded_excel(embeddedFilePath).show_win();
        }
    }
}
