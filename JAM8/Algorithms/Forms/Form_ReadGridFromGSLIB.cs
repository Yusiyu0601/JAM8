using JAM8.Utilities;

namespace JAM8.Algorithms.Geometry
{
    public partial class Form_ReadGridFromGSLIB : Form
    {
        public List<string> paras { get; internal set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text">说明文字</param>
        public Form_ReadGridFromGSLIB(string title = null)
        {
            InitializeComponent();
            if (title != null)
                this.Text = title;
        }

        private void btn_OpenFile_Click(object sender, EventArgs e)
        {
            var ofd = new OpenFileDialog()
            {
                Filter = "gslib格式(*.dat;*.out)|*.dat;*.out"
            };
            if (ofd.ShowDialog() != DialogResult.OK)
                return;

            txt_FileName.Text = ofd.FileName;
            txt_GridName.Text = FileHelper.GetFileName(ofd.FileName, false);
            
            using var sr = new StreamReader(ofd.FileName);
            string s = "";
            int flag = -1;
            while (sr.Peek() > -1 && flag <= 50)
            {
                s += sr.ReadLine() + "\r\n";
                if (flag == 0)//解析GSLIB第一行
                {
                    if (s.Contains("="))
                    {
                        string[] temp1 = s.Split('=', StringSplitOptions.TrimEntries);
                        txt_GridName.Text = temp1[0];
                        GridStructure gs = GridStructure.create(temp1[1]);
                        txt_ICount.Text = gs.nx.ToString();
                        txt_JCount.Text = gs.ny.ToString();
                        txt_KCount.Text = gs.nz.ToString();
                        txt_ISize.Text = gs.xsiz.ToString();
                        txt_JSize.Text = gs.ysiz.ToString();
                        txt_KSize.Text = gs.zsiz.ToString();
                        txt_OriginCellX.Text = gs.xmn.ToString();
                        txt_OriginCellY.Text = gs.ymn.ToString();
                        txt_OriginCellZ.Text = gs.zmn.ToString();
                    }
                }
                flag++;
            }
            textBox1.Text = s;//GSLIB文件前50行预览
        }

        private void btn_OK_Click(object sender, EventArgs e)
        {
            if (txt_FileName.Text == "file name")
            {
                MessageBox.Show("警告：\n set file path please ! ", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            paras = new()
            {
                txt_FileName.Text,
                txt_GridName.Text,
                txt_ValueOfNull.Text,
                txt_ICount.Text,
                txt_JCount.Text,
                txt_KCount.Text,
                txt_ISize.Text,
                txt_JSize.Text,
                txt_KSize.Text,
                txt_OriginCellX.Text,
                txt_OriginCellY.Text,
                txt_OriginCellZ.Text
            };
            DialogResult = DialogResult.OK;
        }

        private void btn_Cancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string embeddedFilePath = "JAM8.资源文件.demo_largetrain.out";
            string txt = MyEmbeddedFileHelper.read_embedded_txt(embeddedFilePath);
            Form_showTxt frm = new(txt, "示例数据");
            frm.Show();
        }
    }
}
