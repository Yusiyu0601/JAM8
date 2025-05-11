using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using ExcelLibrary.BinaryFileFormat;
using JAM8.Algorithms.Geometry;
using LiteDB;
using System.Xml;
using Xfrogcn.BinaryFormatter;
using JAM8.Algorithms.Forms;
using Easy.Common.Extensions;

namespace JAM8.SpecificApps.工作空间
{
    /// <summary>
    /// 工作空间的对象
    /// </summary>
    public class MyDataItem
    {
        [BsonId]
        public int id { get; internal set; }
        public string name { get; internal set; }
        public string type_name { get; internal set; }
        public object data { get; internal set; }
    }

    /// <summary>
    /// 我的数据箱，与数据库liteDB关联，管理各种数据类型数据
    /// </summary>
    public class MyDataBox
    {
        public string db_path = "";
        public string table_name = "";

        private MyDataBox()
        {

        }

        public static MyDataBox create_from_liteDB(string db_path, string table_name)
        {
            MyDataBox ws = new()
            {
                db_path = db_path,
                table_name = table_name
            };
            return ws;
        }

        public void add_item(string name, object item)
        {
            using LiteDatabase lite = new(db_path);
            ILiteCollection<MyDataItem> items = lite.GetCollection<MyDataItem>(table_name);

            MyDataItem wi = new()
            {
                name = name,
                type_name = item.GetType().ToString(),
                data = item
            };
            var query = Query.EQ("name", name);
            var result_by_name = items.Find(query);
            if (result_by_name.Count() == 0)
                items.Insert(wi);
        }

        public void delete_item(string name)
        {
            using LiteDatabase lite = new(db_path);
            ILiteCollection<MyDataItem> items = lite.GetCollection<MyDataItem>(table_name);
            var query = Query.EQ("name", name);
            var result_by_name = items.Find(query);
            var deletedResult = items.Delete(result_by_name.FirstOrDefault().id);
        }

        public List<(string, dynamic)> get_items_by_type(string type_name)
        {
            using LiteDatabase lite = new(db_path);
            ILiteCollection<MyDataItem> lite_collection = lite.GetCollection<MyDataItem>(table_name);

            List<(string, dynamic)> result = new();
            var items = lite_collection.FindAll().ToList();
            foreach (var item in items)
            {
                if (item.type_name == type_name)
                {
                    result.Add((item.name, item));
                }
            }

            return result;
        }

        public List<(string, Grid)> get_all_grids()
        {
            MyDataBox ws = MyDataBox.create_from_liteDB(db_path, table_name);
            var items = ws.get_items_by_type(typeof(Grid).ToString());

            List<(string, Grid)> grids = new();
            foreach (var (item1, item2) in items)
            {
                var dict = (Dictionary<string, object>)item2.data;
                GridProperty gp1 = (GridProperty)dict.First().Value;
                Grid g = Grid.create(gp1.grid_structure);
                foreach (var (name, gp) in dict)
                {
                    g.add_gridProperty(name, (GridProperty)gp);
                }
                grids.Add((item1, g));
            }
            return grids;
        }

        /// <summary>
        /// win窗体显示所有items
        /// </summary>
        public void show_items_win()
        {
            Form_MyDataBoxManager frm = new(this, "预览");
            frm.Show();
        }

        /// <summary>
        /// win窗体选择items
        /// </summary>
        /// <returns></returns>
        public List<MyDataItem> select_items_win()
        {
            Form_MyDataBoxManager frm = new(this, "选择");
            if (frm.ShowDialog() != DialogResult.OK)
                return null;
            return frm.selected_items;
        }

        /// <summary>
        /// win窗体添加item
        /// </summary>
        public void add_item_win()
        {
            Form_MyDataBoxManager frm = new(this, "添加");
            if (frm.ShowDialog() != DialogResult.OK)
                return;
        }

        /// <summary>
        /// win窗体删除item
        /// </summary>
        public void delete_item_win()
        {
            Form_MyDataBoxManager frm = new(this, "删除");
            if (frm.ShowDialog() != DialogResult.OK)
                return;
        }
    }
}
