using ExcelLibrary.BinaryFileFormat;
using JAM8.Utilities;

namespace JAM8.Algorithms.Geometry
{
    public partial class Form_WriteGridToGSLIB : Form
    {
        public List<string> paras { get; internal set; }
        public Form_WriteGridToGSLIB(Grid g, string title = null)
        {
            InitializeComponent();

            if (title != null)
                Text = title;

            GridStructure gs = g.gridStructure;
            txt_FileName.Text = "file_name";
            txt_GridName.Text = g.grid_name;
            txt_ValueOfNull.Text = "-99";
            txt_ICount.Text = gs.nx.ToString();
            txt_JCount.Text = gs.ny.ToString();
            txt_KCount.Text = gs.nz.ToString();
            txt_ISize.Text = gs.xsiz.ToString();
            txt_JSize.Text = gs.ysiz.ToString();
            txt_KSize.Text = gs.zsiz.ToString();
            txt_OriginCellX.Text = gs.xmn.ToString();
            txt_OriginCellY.Text = gs.ymn.ToString();
            txt_OriginCellZ.Text = gs.zmn.ToString();

            textBox1.Text = get_first_50_lines(g);
        }

        private void btn_SaveFile_Click(object sender, EventArgs e)
        {
            var sfd = new SaveFileDialog
            {
                Filter = "GSLIB数据格式(*.out)|*.out|GSLIB数据格式(*.dat)|*.dat|所有文件|*.*"
            };
            if (sfd.ShowDialog() != DialogResult.OK) return;
            txt_FileName.Text = sfd.FileName;
        }

        private void btn_OK_Click(object sender, EventArgs e)
        {
            if (txt_FileName.Text == "file name")
            {
                MessageBox.Show("警告:\n set file path please ! ", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            paras =new()
            {
                txt_FileName.Text,
                txt_GridName.Text,
                txt_ValueOfNull.Text
            };

            DialogResult = DialogResult.OK;
            Close();
        }

        private void btn_Cancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        string get_first_50_lines(Grid g)
        {
            string text = "";
            string gridSize = g.gridStructure.view_text().Trim('\n').Trim('\t');
            text += "gridName" + gridSize + "\r\n";//输出GSLIB数据的标题
            text += g.N_gridProperties + "\r\n";//输出变量数目
            for (int i = 0; i < g.N_gridProperties; i++)
            {
                text += g.propertyNames[i] + "\r\n";//输出属性名称
            }
            int N = g.gridStructure.N > 200 ? 200 : g.gridStructure.N;
            for (int n = 0; n < N; n++)//逐行输出数据
            {
                string line_str = string.Empty;
                for (int col = 0; col < g.N_gridProperties; col++)//逐列输出数据
                {
                    string temp = string.Empty;
                    float? value = g.get_value(n, g.propertyNames[col]);
                    temp = value == null ?
                        "-99" : value.Value.ToString("E3");
                    line_str += temp;
                    if (col < g.N_gridProperties - 1)
                        line_str += " ";
                }
                text += line_str + "\r\n";
            }
            return text;
        }
    }
}
