using JAM8.Algorithms.MachineLearning;
using JAM8.Utilities;
using static JAM8.Algorithms.MachineLearning.MySVM;

namespace JAM8.SpecificApps.常用工具
{
    public partial class Form_SVM : Form
    {
        MyDataFrame df = null;
        SVMModel svm_model = null;

        MyDataFrame df_predict = null;

        public Form_SVM()
        {
            InitializeComponent();
        }

        //读取记录表
        private void button1_Click(object sender, EventArgs e)
        {
            df = MyDataFrame.read_from_excel();
            df.show_win("训练数据表");

            listBox1.Items.Clear();
            listBox2.Items.Clear();
            listBox3.Items.Clear();
            if (svm_model == null)
                label6.Text = "无模型";

            foreach (var item in df.series_names)
            {
                listBox1.Items.Add(item);
            }
        }

        //选择输入序列名称
        private void button2_Click(object sender, EventArgs e)
        {
            listBox2.Items.Clear();
            listBox2.Items.AddRange(listBox1.SelectedItems.Cast<string>().ToArray());
        }

        //选择输出序列名称
        private void button3_Click(object sender, EventArgs e)
        {
            listBox3.Items.Clear();
            listBox3.Items.AddRange(listBox1.SelectedItems.Cast<string>().ToArray());
        }

        //归回模式
        private void button4_Click(object sender, EventArgs e)
        {
            if (listBox2.Items.Count == 0 || listBox3.Items.Count == 0)
            {
                MessageBox.Show("选择输入和输出列名", "提示");
                return;
            }
            svm_model = MySVM.train(df,
                listBox2.Items.Cast<string>().ToArray(),
                listBox3.Items.Cast<string>().ToArray()[0],
                double.Parse(textBox1.Text),
                double.Parse(textBox2.Text),
                0);
            if (svm_model != null)
                label6.Text = "回归模型";
            MessageBox.Show("训练完成");
        }

        //分类模式
        private void button6_Click(object sender, EventArgs e)
        {
            if (listBox2.Items.Count == 0 || listBox3.Items.Count == 0)
            {
                MessageBox.Show("选择输入和输出列名", "提示");
                return;
            }
            svm_model = MySVM.train(df,
          listBox2.Items.Cast<string>().ToArray(),
          listBox3.Items.Cast<string>().ToArray()[0],
          double.Parse(textBox1.Text),
          double.Parse(textBox2.Text),
          1);
            if (svm_model != null)
                label6.Text = "分类模型";
            MessageBox.Show("训练完成");
        }


        //打开待预测数据
        private void button7_Click(object sender, EventArgs e)
        {
            df_predict = MyDataFrame.read_from_excel();
            df_predict.show_win("待预测数据记录表");

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

        //预测
        private void button5_Click(object sender, EventArgs e)
        {
            var predict = MySVM.predict(df_predict,
                listBox5.Items.Cast<string>().ToArray(), svm_model);
            predict.show_win();

            int[] actual = df.get_series<int>(listBox3.Items.Cast<string>().ToArray()[0]);
            int[] ouput = predict.get_series<int>(predict.series_names.Last());
            //ConfusionMatrix cm = new(actual, ouput, 1, 2);

            //dataGridView1.DataSource = new[] { cm };
        }

        private void label9_Click(object sender, EventArgs e)
        {
            string embeddedFilePath = "JAM8.Work.资源文件.[实例数据]seeds.xlsx";
            MyEmbeddedFileHelper.read_embedded_excel(embeddedFilePath).show_win();
        }
    }
}
