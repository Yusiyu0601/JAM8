using System.Globalization;
using JAM8.Algorithms.Geometry;

namespace JAM8.Algorithms.Forms
{
    public partial class Form_SelectGridPropertyFromGrid : Form
    {
        Grid g;

        public Form_SelectGridPropertyFromGrid(Grid g, string title = null)
        {
            InitializeComponent();

            if (g == null)
                return;
            this.g = g;
            this.Text = title;

            #region 加载属性列表

            listBox1.Items.Clear();
            foreach (var PropertyName in g.propertyNames)
            {
                listBox1.Items.Add(PropertyName);
            }
            listBox1.SelectedIndex = 0;

            #endregion

            //根据系统语言设置按钮文字
            bool IsChineseSystem = CultureInfo.CurrentCulture.Name.StartsWith("zh");
            if (IsChineseSystem)
            {
                this.Text = "从Grid里选择GridProperty";
                button1.Text = "选择GridProperty";
                button3.Text = "取消";
            }
            else
            {
                this.Text = "Select GridProperty from Grid";
                button1.Text = "Select GridProperty";
                button3.Text = "Cancel";
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
            DialogResult = DialogResult.Cancel;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
            DialogResult = DialogResult.OK;
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex == -1 || listBox1.SelectedItem.ToString() == string.Empty)
                return;

            if (g == null)
                return;
            else
                scottplot4GridProperty1.update_gridProperty(g[listBox1.SelectedItem.ToString()]);
        }
    }
}
