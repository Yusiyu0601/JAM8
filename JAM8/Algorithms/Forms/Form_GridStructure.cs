using System.Globalization;
using System.Text.RegularExpressions;

namespace JAM8.Algorithms.Geometry
{
    public partial class Form_GridStructure : Form
    {
        private string _dim = "";
        public List<string> paras { get; internal set; }

        public Form_GridStructure(GridStructure gs = null, string title = null)
        {
            InitializeComponent();
            //根据系统语言设置按钮文字
            bool IsChineseSystem = CultureInfo.CurrentCulture.Name.StartsWith("zh");
            if (IsChineseSystem)
            {
                this.Text = "GridStructure设置";
                label10.Text = "维度";
                button1.Text = "确定";
                button2.Text = "取消";
            }
            else
            {
                this.Text = "GridStructure Setting";
                label10.Text = "Dim";
                button1.Text = "OK";
                button2.Text = "Cancel";
            }

            if (title != null)
                this.Text = title;

            comboBox2.SelectedIndex = 0;
            if (gs != null)
            {
                tb_ICount.Text = gs.nx.ToString();
                tb_JCount.Text = gs.ny.ToString();
                tb_KCount.Text = gs.nz.ToString();
                tb_ISize.Text = gs.xsiz.ToString();
                tb_JSize.Text = gs.ysiz.ToString();
                tb_KSize.Text = gs.zsiz.ToString();
                tb_OriginCellX.Text = gs.xmn.ToString();
                tb_OriginCellY.Text = gs.ymn.ToString();
                tb_OriginCellZ.Text = gs.zmn.ToString();

                if (gs.dim == Dimension.D2)
                    comboBox2.SelectedIndex = 0;
                else
                    comboBox2.SelectedIndex = 1;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            bool b1 = RegexInteger(tb_ICount.Text);
            bool b2 = RegexInteger(tb_JCount.Text);
            bool b3 = RegexInteger(tb_KCount.Text);
            if (b1 && b2 && b3)
            {
                paras = new()
                {
                    tb_ICount.Text,
                    tb_JCount.Text,
                    tb_KCount.Text,
                    tb_ISize.Text,
                    tb_JSize.Text,
                    tb_KSize.Text,
                    tb_OriginCellX.Text,
                    tb_OriginCellY.Text,
                    tb_OriginCellZ.Text,
                    _dim
                };
                DialogResult = DialogResult.OK;
            }
            else
                MessageBox.Show("检查输入数字是否正确");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        public static bool RegexInteger(string IInteger)
        {
            Regex g = new(@"^[0-9]\d*$");
            return g.IsMatch(IInteger);
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox2.SelectedIndex == 0)
            {
                tb_KCount.Visible = false;
                tb_KSize.Visible = false;
                tb_OriginCellZ.Visible = false;
                label3.Visible = false;
                label6.Visible = false;
                label7.Visible = false;
                _dim = "D2";
            }

            if (comboBox2.SelectedIndex == 1)
            {
                tb_KCount.Visible = true;
                tb_KSize.Visible = true;
                tb_OriginCellZ.Visible = true;
                label3.Visible = true;
                label6.Visible = true;
                label7.Visible = true;
                _dim = "D3";
            }
        }
    }
}