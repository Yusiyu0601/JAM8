using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using JAM8.Algorithms.Geometry;
using JAM8.Utilities;

namespace JAM8.Algorithms.Forms
{
    public partial class Form_GridCatalog : Form
    {
        private GridCatalog grid_catalog = null;
        public Grid[] selected_grids { get; internal set; }
        public GridProperty selected_gridProperty { get; internal set; }

        public Form_GridCatalog()
        {
            InitializeComponent();
            ConfigureListView(); // 配置 ListView
        }

        // 配置 ListView
        private void ConfigureListView()
        {
            listView1.View = View.Details; // 设置为细节视图
            listView1.Columns.Add("Grid Name", 150);        // 第一列显示 grid_name
            listView1.Columns.Add("Grid Structure", 200);  // 第二列显示 grid_structure
            listView1.FullRowSelect = true;                // 支持整行选择
            listView1.MultiSelect = true;
        }

        // 新建excel文件
        private void button2_Click(object sender, EventArgs e)
        {
            string path = "";
            SaveFileDialog sfd = new()
            {
                Filter = "Excel文件|*.xlsx"
            };
            if (sfd.ShowDialog() != DialogResult.OK)
                return;
            path = sfd.FileName;
            grid_catalog = GridCatalog.create(path);

            listView1.Items.Clear(); // 清空并更新 ListView 数据
            scottplot4Grid1.update_grid(null);
            this.Text = $"Form_GridCatalog[{sfd.FileName}]";
        }

        // 打开excel文件
        private void button1_Click(object sender, EventArgs e)
        {
            string path = "";
            OpenFileDialog ofd = new()
            {
                Filter = "Excel文件|*.xlsx"
            };
            if (ofd.ShowDialog() != DialogResult.OK)
                return;
            path = ofd.FileName;
            grid_catalog = GridCatalog.open(path);

            var catalog = grid_catalog.get_items();
            this.Text = $"Form_GridCatalog[{ofd.FileName}]";

            listView1.Items.Clear(); // 清空并更新 ListView 数据
            scottplot4Grid1.update_grid(null);
            foreach (var item in catalog)
            {
                var listItem = new ListViewItem(item.grid_name); // 第一列
                listItem.SubItems.Add(item.grid_structure);     // 第二列
                listView1.Items.Add(listItem);                   // 添加到 ListView 中
            }
            listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
        }

        // 添加Grid
        private void button3_Click(object sender, EventArgs e)
        {
            var (g, file_name) = Grid.create_from_gslibwin();
            if (g == null)
                return;
            var success = grid_catalog.add_item(g, file_name, -99);
            if (success)
            {
                var listItem = new ListViewItem(g.grid_name);  // 第一列
                listItem.SubItems.Add(g.gridStructure.to_string()); // 第二列
                listView1.Items.Add(listItem); // 添加到 ListView
                listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
            }
            else
                MessageBox.Show($"grid catalog已经存在grid name={g.grid_name}");
        }

        // 删除Grid
        private void button4_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0) // 检查是否有选中的项
                return;

            string grid_name = listView1.SelectedItems[0].Text; // 提取 grid_name

            // 弹出确认对话框
            DialogResult result = MessageBox.Show(
                $"是否删除 '{grid_name}'?",
                "确认删除",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if (result == DialogResult.Yes)
            {
                grid_catalog.delete_item(grid_name);  // 删除对应数据
                listView1.Items.Remove(listView1.SelectedItems[0]);  // 从 ListView 中移除选中的项
                scottplot4Grid1.update_grid(null);    // 更新显示
            }
        }

        // 选中Grid
        private void button5_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0) // 检查是否有选中的项
                return;

            selected_grids = new Grid[listView1.SelectedItems.Count]; // 初始化选中的网格数组
            for (int i = 0; i < listView1.SelectedItems.Count; i++)
            {
                string grid_name = listView1.SelectedItems[i].Text; // 提取 grid_name
                selected_grids[i] = grid_catalog.read_grid(grid_name); // 读取对应的网格数据
            }

            this.Close();  // 关闭当前窗口
            DialogResult = DialogResult.OK; // 设置对话框返回值
        }

        // 确认选择并关闭窗口
        private void button6_Click(object sender, EventArgs e)
        {
            this.Close();
            DialogResult = DialogResult.OK;
            selected_gridProperty = scottplot4Grid1._gp;
        }

        // 预览Grid
        private void button7_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0) // 检查是否有选中的项
                return;

            string grid_name = listView1.SelectedItems[0].Text; // 提取 grid_name

            scottplot4Grid1.update_grid(grid_catalog.read_grid(grid_name)); // 更新显示网格数据

            var catalog = grid_catalog.get_items();
            this.textBox1.Text = catalog.Find(a => a.grid_name == grid_name).path; // 显示路径
        }

        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (listView1.SelectedItems.Count == 0) // 检查是否有选中的项
                return;

            string grid_name = listView1.SelectedItems[0].Text; // 提取 grid_name

            scottplot4Grid1.update_grid(grid_catalog.read_grid(grid_name)); // 更新显示网格数据

            var catalog = grid_catalog.get_items();
            this.textBox1.Text = catalog.Find(a => a.grid_name == grid_name).path; // 显示路径
        }
    }
}
