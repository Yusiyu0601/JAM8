namespace JAM8.Algorithms.Geometry
{
    public partial class Form_WriteConditionData : Form
    {
        public List<string> paras { get; internal set; }

        public Form_WriteConditionData(CData cd, string title = null)
        {
            InitializeComponent();

            if (title != null)
                Text = title;

            t_ColX.Enabled = false;
            t_ColY.Enabled = false;
            t_ColZ.Enabled = false;
            comboBox2.Enabled = false;
            if (cd.dim == Dimension.D2)
            {
                comboBox2.SelectedIndex = 0;
                label4.Visible = false;
                t_ColZ.Visible = false;
            }
            if (cd.dim == Dimension.D3)
            {
                comboBox2.SelectedIndex = 1;
                label4.Visible = true;
                t_ColZ.Visible = true;
            }

            textBox2.Text = cd.view_text();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var sfd = new SaveFileDialog()
            {
                Filter = "gslib格式(*.dat)|*.dat"
            };
            if (sfd.ShowDialog() != DialogResult.OK)
                return;
            txt_FileName.Text = sfd.FileName;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (txt_FileName.Text == "file name" || txt_FileName.Text == "")
            {
                MessageBox.Show("警告:\n set file path please ! ", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            paras = new()
            {
                txt_FileName.Text,
                t_NoDataValue.Text
            };

            DialogResult = DialogResult.OK;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void t_Check空值_CheckedChanged(object sender, EventArgs e)
        {
            if (t_Check空值.Checked)
                t_NoDataValue.ReadOnly = false;
            else
                t_NoDataValue.ReadOnly = true;
        }
    }
}
