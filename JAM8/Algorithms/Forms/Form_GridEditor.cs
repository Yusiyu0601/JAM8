using JAM8.Algorithms.Geometry;
using ScottPlot.Drawing.Colormaps;

namespace JAM8.Algorithms.Forms
{
    public partial class Form_GridEditor : Form
    {
        Grid g;


        public Form_GridEditor()
        {
            InitializeComponent();
            this.scottplot4GridProperty1.MouseMoveEvent += Scottplot4GridProperty1_MouseMoveEvent;
            this.scottplot4GridProperty1.MouseDownEvent += Scottplot4GridProperty1_MouseDownEvent;
        }

        private void Scottplot4GridProperty1_MouseDownEvent(int ix, int iy)
        {
            if (ModifierKeys == Keys.Control)
            {
                Console.WriteLine(ix + " " + iy);
                scottplot4GridProperty1.assign_value_in_viewGrid(ix - 1, iy - 1, (int)numericUpDown1.Value);
            }
        }

        private void Scottplot4GridProperty1_MouseMoveEvent(int ix, int iy)
        {
        }

        //打开Grid
        private void button2_Click(object sender, EventArgs e)
        {
            (g, _) = Grid.create_from_gslibwin();
            if (g == null)
                return;

            #region 加载属性列表

            listBox1.Items.Clear();
            foreach (var PropertyName in g.propertyNames)
            {
                listBox1.Items.Add(PropertyName);
            }
            label3.Text = $"[{g.Count}个属性]";

            #endregion
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex == -1 || listBox1.SelectedItem.ToString() == string.Empty)
                return;
            else
                scottplot4GridProperty1.update_gridProperty(g[listBox1.SelectedItem.ToString()]);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (g == null)
                return;
            SaveFileDialog sfd = new()
            {
                Filter = "gslib(*.out)|*.out"
            };
            if (sfd.ShowDialog() != DialogResult.OK)
                return;
            g.save_to_gslib(sfd.FileName, "grid_name", -99);
        }

        //新建Grid
        private void button3_Click(object sender, EventArgs e)
        {
            g = Grid.create(GridStructure.create_win());
            g.add_gridProperty("NewGridProperty");
            g.first_gridProperty().set_value(0);
            if (g == null)
                return;

            #region 加载属性列表

            listBox1.Items.Clear();
            foreach (var PropertyName in g.propertyNames)
            {
                listBox1.Items.Add(PropertyName);
            }
            label3.Text = $"[{g.Count}个属性]";

            #endregion
        }
    }
}
