using JAM8.Utilities;
using Microsoft.VisualBasic;
using System.Globalization;
using System.Security.Cryptography;
using System.Windows.Forms;

namespace JAM8.Algorithms.Geometry
{
    public partial class Form_ReadConditionData : Form
    {
        private string fileName = "";
        private string Dim = Dimension.D2.ToString();
        private int ColX, ColY, ColZ;
        private double? nullValue;
        private bool enable_nullValue;

        public List<string> paras { get; internal set; }

        public Form_ReadConditionData(string title = null)
        {
            InitializeComponent();
            if (title != null)
                Text = title;
            comboBox2.SelectedIndex = 0;
            groupBox1.Enabled = false;

            bool IsChineseSystem = CultureInfo.CurrentCulture.Name.StartsWith("zh");
            if (IsChineseSystem)
            {
                button3.Text = "打开文件";
                label1.Text = "预览";
                button4.Text = "示例数据";
                button1.Text = "确定";
                button2.Text = "取消";
                label5.Text = "维度";
                label2.Text = "X-表列序";
                label3.Text = "Y-表列序";
                label4.Text = "Z-表列序";
                label6.Text = "注意:\r\n1.列序从0开始;\r\n2.每行分隔符只能是空格";
            }
            else
            {
                button3.Text = "Open the file";
                label1.Text = "Preview";
                button4.Text = "Example data";
                button1.Text = "OK";
                button2.Text = "Cancel";
                label5.Text = "Dimension";
                label2.Text = "X-Column";
                label3.Text = "Y-Column";
                label4.Text = "Z-Column";
                label6.Text = "Note:\r\n1.Column starts from 0;\r\n2.Each line separator can only be a space";
            }
        }

        //打开文件
        private void button3_Click(object sender, EventArgs e)
        {
            var ofd = new OpenFileDialog()
            {
                Filter = "gslib格式(*.dat;*.out)|*.dat;*.out"
            };
            if (ofd.ShowDialog() != DialogResult.OK) return;
            fileName = ofd.FileName;
            预览();
        }

        private void textBox1_DragEnter(object sender, DragEventArgs e)
        {
            var data = e.Data.GetData(DataFormats.FileDrop);
            if (data != null)
            {
                var fileNames = data as string[];
                if (fileNames.Length > 0)
                    fileName = fileNames[0];
            }
            预览();
        }

        //前30行数据预览
        private void 预览()
        {
            groupBox1.Enabled = true;
            textBox1.Text = fileName;
            textBox2.Clear();
            using var sr = new StreamReader(fileName);
            string s = "";
            int flag = -1;
            while (sr.Peek() > -1 && flag <= 50)
            {
                s += sr.ReadLine() + "\r\n";
                flag++;
            }
            textBox2.Text = s;//GSLIB文件前50行预览
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (t_Check空值.Checked)
                t_NoDataValue.ReadOnly = false;
            else
                t_NoDataValue.ReadOnly = true;
        }

        //确定
        private void button1_Click(object sender, EventArgs e)
        {
            ColX = int.Parse(t_ColX.Text);
            ColY = int.Parse(t_ColY.Text);
            ColZ = int.Parse(t_ColZ.Text);

            enable_nullValue = t_Check空值.Checked;
            nullValue = double.Parse(t_NoDataValue.Text);

            paras = new()
            {
                fileName,
                Dim.ToString(),
                ColX.ToString(),
                ColY.ToString(),
                ColZ.ToString(),
                enable_nullValue.ToString(),
                nullValue.ToString()
            };
            DialogResult = DialogResult.OK;
        }

        //取消
        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        //demo数据
        private void button4_Click_1(object sender, EventArgs e)
        {
            string embeddedFilePath = "JAM8.资源文件.demo_cluster.out";
            string txt = MyEmbeddedFileHelper.read_embedded_txt(embeddedFilePath);
            Form_showTxt frm = new(txt, "示例数据");
            frm.Show();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            //提醒，这里修改后，其他之前用1 2 3地方会出错！！！
            t_ColX.Text = "0";
            t_ColY.Text = "1";
            t_ColZ.Text = "2";
            if (comboBox2.SelectedIndex == 0)
            {
                label4.Visible = false;
                t_ColZ.Visible = false;
                Dim = Dimension.D2.ToString();
            }
            if (comboBox2.SelectedIndex == 1)
            {
                label4.Visible = true;
                t_ColZ.Visible = true;
                Dim = Dimension.D3.ToString();
            }
        }
    }
}
