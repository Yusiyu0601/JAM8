using JAM8.Utilities;
using System.Security.Cryptography;
using System.Windows.Forms;

namespace JAM8.Algorithms.Geometry
{
    public partial class Form_ReadConditionData : Form
    {
        string fileName = "";
        string Dim = Dimension.D2.ToString();
        int ColX, ColY, ColZ;
        double? nullValue;
        bool enable_nullValue;

        public List<string> paras { get; internal set; }

        public Form_ReadConditionData(string title = null)
        {
            InitializeComponent();
            if (title != null)
                Text = title;
            comboBox2.SelectedIndex = 0;
            groupBox1.Enabled = false;
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
        void 预览()
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
            if (comboBox2.SelectedIndex == 0)
            {
                t_ColX.Text = "1";
                t_ColY.Text = "2";
                label4.Visible = false;
                t_ColZ.Visible = false;
                Dim = Dimension.D2.ToString();
            }
            if (comboBox2.SelectedIndex == 1)
            {
                t_ColX.Text = "1";
                t_ColY.Text = "2";
                t_ColZ.Text = "3";
                label4.Visible = true;
                t_ColZ.Visible = true;
                Dim = Dimension.D3.ToString();
            }
        }
    }
}
