using System.Windows.Forms;
using JAM8.Algorithms.Geometry;

namespace JAM8.Algorithms.Forms
{
    public partial class Form_SelectPropertyFromCData : Form
    {
        CData cd;

        public string selected_property_name
        {
            get
            {
                return listBox1.Text;
            }
        }

        public Form_SelectPropertyFromCData(CData cd, string title = null)
        {
            InitializeComponent();

            if (title != null)
                Text = title;

            this.cd = cd;

            #region 加载属性列表

            listBox1.Items.Clear();
            foreach (var PropertyName in cd.propertyNames)
            {
                listBox1.Items.Add(PropertyName);
            }
            listBox1.SelectedIndex = 0;

            #endregion
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex == -1 || listBox1.SelectedItem.ToString() == string.Empty)
                return;

            if (cd == null)
                return;
            else
            {
                var boundary = cd.get_boundary();
                int nx = 100;
                int ny = 100;
                int nz = cd.dim == Dimension.D2 ? 1 : 10;
                float xsiz = (boundary.max_x - boundary.min_x) / 100;
                float ysiz = (boundary.max_y - boundary.min_y) / 100;
                float? zsiz = cd.dim == Dimension.D2 ? 1.0f : (boundary.max_z - boundary.min_z) / 10;
                float xmn = boundary.min_x;
                float ymn = boundary.min_y;
                float? zmn = cd.dim == Dimension.D2 ? 0.5f : boundary.min_z;
                GridStructure gs = GridStructure.create(nx, ny, nz, xsiz, ysiz, zsiz.Value, xmn, ymn, zmn.Value);
                var g_cd = cd.assign_to_grid(gs);
                scottplot4GridProperty1.update_gridProperty(g_cd.grid_assigned[listBox1.SelectedItem.ToString()]);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
            DialogResult = DialogResult.OK;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
            DialogResult = DialogResult.Cancel;
        }
    }
}
