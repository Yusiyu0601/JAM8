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
        // ------------------- 私有字段 -------------------
        private string file_name = string.Empty;
        private GridCatalog grid_catalog = null;

        // ------------------- 公共返回属性 -------------------
        public (Grid grid, string path)[] selected_grids_with_path { get; internal set; }
        public (Grid grid, string file_name) selected_grid_with_path { get; internal set; }
        public GridProperty selected_gridProperty { get; internal set; }


        public Form_GridCatalog(bool allowMultiSelect = true)
        {
            InitializeComponent();

            // 初始禁用所有按钮
            btnSelectGrid.Enabled = false;
            btnSelectGridProperty.Enabled = false;
            btnSelectGrids.Enabled = false;

            button3.Enabled = false;
            button4.Enabled = false;

            // 如果不允许多选，就隐藏“选择Grids”按钮
            btnSelectGrids.Visible = allowMultiSelect;

            // 配置 ListView
            listView1.View = View.Details; // 设置为细节视图
            listView1.Columns.Add("Grid Name", 150); // 第一列显示 grid_name
            listView1.Columns.Add("Grid Structure", 200); // 第二列显示 grid_structure
            listView1.FullRowSelect = true; // 支持整行选择
            listView1.MultiSelect = allowMultiSelect;
        }

        // 新建管理grid catalog的excel文件
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

            button3.Enabled = true;
            button4.Enabled = true;

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
            file_name = ofd.FileName;

            button3.Enabled = true;
            button4.Enabled = true;

            listView1.Items.Clear(); // 清空并更新 ListView 数据
            scottplot4Grid1.update_grid(null);
            foreach (var item in catalog)
            {
                var listItem = new ListViewItem(item.grid_name); // 第一列
                listItem.SubItems.Add(item.grid_structure); // 第二列
                listView1.Items.Add(listItem); // 添加到 ListView 中
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
                var listItem = new ListViewItem(g.grid_name); // 第一列
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
                grid_catalog.delete_item(grid_name); // 删除对应数据
                listView1.Items.Remove(listView1.SelectedItems[0]); // 从 ListView 中移除选中的项
                scottplot4Grid1.update_grid(null); // 更新显示
            }
        }

        /// <summary>
        /// 双击 ListView 中的某一项时触发，
        /// 加载对应网格并在界面中显示，同时在状态栏显示其文件路径。
        /// </summary>
        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            // 如果没有选中任何项，则不执行任何操作
            if (listView1.SelectedItems.Count == 0)
                return;

            // 获取选中的网格名称（第一列文本）
            string grid_name = listView1.SelectedItems[0].Text;

            // 从 GridCatalog 中读取对应的 Grid 对象，并更新 scottplot 显示区域
            scottplot4Grid1.update_grid(grid_catalog.read_grid(grid_name));

            // 获取当前 catalog 项目列表（用于查找路径信息）
            var catalog = grid_catalog.get_items();

            // 根据 grid_name 查找对应的文件路径，并显示在状态栏中
            this.toolStripStatusLabel1.Text = catalog.Find(a => a.grid_name == grid_name).path;
        }

        // 选中Grid
        private void button5_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0) // 检查是否有选中的项
                return;

            string grid_name = listView1.SelectedItems[0].Text;

            // 赋值给 selected_grid，而不是数组
            selected_grid_with_path = (grid_catalog.read_grid(grid_name), this.toolStripStatusLabel1.Text);

            this.Close(); // 关闭当前窗口
            DialogResult = DialogResult.OK; // 设置对话框返回值
        }

        //选择多个Grids
        private void button7_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0)
                return;

            var catalog = grid_catalog.get_items();
            var results = new List<(Grid grid, string path)>();

            foreach (ListViewItem item in listView1.SelectedItems)
            {
                string grid_name = item.Text;
                var grid = grid_catalog.read_grid(grid_name);
                var path = catalog.Find(a => a.grid_name == grid_name)?.path ?? "";
                results.Add((grid, path));
            }

            selected_grids_with_path = results.ToArray();

            this.Close();
            DialogResult = DialogResult.OK;
        }

        // 选择GridProperty
        private void button6_Click(object sender, EventArgs e)
        {
            selected_gridProperty = scottplot4Grid1._gp;

            this.Close();
            DialogResult = DialogResult.OK;
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int count = listView1.SelectedItems.Count;

            if (count == 1)
            {
                btnSelectGrid.Enabled = true;
                btnSelectGridProperty.Enabled = true;
                btnSelectGrids.Enabled = false;

                //更新状态栏文本为选中项的路径
                string grid_name = listView1.SelectedItems[0].Text;
                toolStripStatusLabel1.Text = grid_catalog.get_items()
                    .Find(a => a.grid_name == grid_name)?.path ?? "";
            }
            else if (count > 1)
            {
                btnSelectGrid.Enabled = false;
                btnSelectGridProperty.Enabled = true;
                btnSelectGrids.Enabled = true;

                //多选时状态栏提示
                toolStripStatusLabel1.Text = $"已选中 {count} 个网格";
            }
            else
            {
                //没有选中项时清空
                btnSelectGrid.Enabled = false;
                btnSelectGridProperty.Enabled = false;
                btnSelectGrids.Enabled = false;

                toolStripStatusLabel1.Text = "未选中任何网格";
            }
        }

        private void toolStripStatusLabel1_DoubleClick(object sender, EventArgs e)
        {
            // 多选状态下不执行复制
            if (listView1.SelectedItems.Count > 1)
                return;

            string text = toolStripStatusLabel1.Text;
            if (!string.IsNullOrWhiteSpace(text))
            {
                try
                {
                    Clipboard.SetText(text); // 主线程已是 STA，不需额外线程
                    MessageBox.Show($"已复制路径到剪贴板:\n{text}", "提示");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"复制失败: {ex.Message}", "错误");
                }
            }
        }
    }
}