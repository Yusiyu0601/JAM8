using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Easy.Common.Extensions;
using ExcelLibrary.CompoundDocumentFormat;
using JAM8.Algorithms.Geometry;
using JAM8.Utilities;
using LiteDB;
using ScottPlot.Statistics;

namespace JAM8.SpecificApps.工作空间
{
    public partial class Form_MyDataBoxManager : Form
    {
        private string operate_type = "";
        private MyDataBox my_dataBox = null;
        public List<MyDataItem> selected_items = null;

        public Form_MyDataBoxManager()
        {
            InitializeComponent();
        }

        public Form_MyDataBoxManager(MyDataBox my_dataBox, string operate_type)
        {
            InitializeComponent();

            this.my_dataBox = my_dataBox;
            this.operate_type = operate_type;
            if (operate_type == "预览")
            {
                button2.Visible = false;
                comboBox1.Visible = false;
            }
            if (operate_type == "选择")
            {
                button2.Text = "选择";
                comboBox1.Visible = false;
            }
            if (operate_type == "添加")
            {
                button2.Text = "添加";
                comboBox1.Visible = true;
                comboBox1.SelectedIndex = 0;
            }
            if (operate_type == "删除")
            {
                button2.Text = "删除";
                comboBox1.Visible = false;
            }

            using LiteDatabase lite = new(my_dataBox.db_path);
            update_listview(lite);
        }

        private void update_listview(LiteDatabase lite)
        {
            ILiteCollection<MyDataItem> lite_collection = lite.GetCollection<MyDataItem>("TrainingImages");
            var items = lite_collection.FindAll().ToList();

            listView1.Items.Clear();
            foreach (var item in items)
            {
                ListViewItem lvi = new()
                {
                    Text = item.name
                };
                lvi.SubItems.Add(item.type_name.Split('.').Last());
                listView1.Items.Add(lvi);
            }
        }

        //操作
        private void button2_Click(object sender, EventArgs e)
        {
            using LiteDatabase lite = new(my_dataBox.db_path);
            ILiteCollection<MyDataItem> items = lite.GetCollection<MyDataItem>("TrainingImages");

            if (operate_type == "选择")
            {
                List<MyDataItem> selected_items = new();
                for (int i = 0; i < listView1.SelectedItems.Count; i++)
                {
                    string item_name = listView1.SelectedItems[i].SubItems[0].Text;
                    var query = Query.EQ("name", item_name);
                    selected_items.Add(items.Find(query));
                }
                this.selected_items = selected_items;
                this.Close();
                DialogResult = DialogResult.OK;
            }
            if (operate_type == "添加")
            {
                if (comboBox1.Text == "Grid")
                {
                    var (g, name) = Grid.create_from_gslibwin();
                    if (g == null)
                        return;
                    MyDataItem wi = new()
                    {
                        name = FileHelper.GetFileName(name, false),
                        type_name = g.GetType().ToString(),
                        data = g
                    };
                    var query = Query.EQ("name", wi.name);
                    var result_by_name = items.Find(query);
                    if (result_by_name.Count() == 0)
                    {
                        items.Insert(wi);
                        update_listview(lite);
                    }
                    else
                        MessageBox.Show("记录已存在");
                }
            }
            if (operate_type == "删除")
            {
                if (listView1.SelectedItems.Count != 0)
                {
                    var query = Query.EQ("name", listView1.SelectedItems[0].Text);
                    var result_by_name = items.Find(query);
                    var deletedResult = items.Delete(result_by_name.FirstOrDefault().id);// 删除数据
                    update_listview(lite);
                }
            }
        }

        //预览
        private void button1_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count != 0)
            {
                using LiteDatabase lite = new(my_dataBox.db_path);
                ILiteCollection<MyDataItem> lite_collection = lite.GetCollection<MyDataItem>("TrainingImages");
                var items = lite_collection.FindAll().ToList();

                string selected_name = listView1.SelectedItems[0].Text;
                MyDataItem item = items.Find(a => a.name == selected_name);

                if (item.type_name == typeof(Grid).ToString())
                {
                    var dict = (Dictionary<string, object>)item.data;
                    GridProperty gp1 = (GridProperty)dict.First().Value;
                    Grid g = Grid.create(gp1.gridStructure);
                    foreach (var (name, gp) in dict)
                    {
                        g.add_gridProperty(name, (GridProperty)gp);
                    }
                    g.showGrid_win();
                }
            }
        }

        private void Form_MyDataBoxManager_Load(object sender, EventArgs e)
        {
            this.listView1.ListViewItemSorter = new ListViewColumnSorter();
            this.listView1.ColumnClick += new ColumnClickEventHandler(ListViewHelper.ListView_ColumnClick);
        }
    }
}
