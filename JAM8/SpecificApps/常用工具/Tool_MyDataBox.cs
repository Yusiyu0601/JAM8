using System.Security.Cryptography;
using JAM8.Algorithms.Geometry;
using JAM8.SpecificApps.工作空间;

namespace JAM8.SpecificApps.常用工具
{
    public class Tool_MyDataBox
    {
        public static void create_databox()
        {
            SaveFileDialog sfd = new()
            {
                Filter = "(*.db)|*.db"
            };
            if (sfd.ShowDialog() != DialogResult.OK)
                return;
            MyDataBox ws = MyDataBox.create_from_liteDB(sfd.FileName, "TrainingImages");
            ws.show_items_win();
        }

        public static void view_items()
        {
            OpenFileDialog ofd = new()
            {
                Filter = "(*.db)|*.db"
            };
            if (ofd.ShowDialog() != DialogResult.OK)
                return;
            MyDataBox ws = MyDataBox.create_from_liteDB(ofd.FileName, "TrainingImages");
            ws.show_items_win();
        }

        public static void add_item()
        {
            OpenFileDialog ofd = new()
            {
                Filter = "(*.db)|*.db"
            };
            if (ofd.ShowDialog() != DialogResult.OK)
                return;
            MyDataBox ws = MyDataBox.create_from_liteDB(ofd.FileName, "TrainingImages");
            ws.add_item_win();
        }

        public static void delete_item()
        {
            OpenFileDialog ofd = new()
            {
                Filter = "(*.db)|*.db"
            };
            if (ofd.ShowDialog() != DialogResult.OK)
                return;
            MyDataBox ws = MyDataBox.create_from_liteDB(ofd.FileName, "TrainingImages");
            ws.delete_item_win();
        }

        public static void convert_grid_to_databox()
        {
            Grid g = Grid.create_from_gslibwin().grid;
            SaveFileDialog sfd = new()
            {
                Filter = "(*.db)|*.db"
            };
            if (sfd.ShowDialog() != DialogResult.OK)
                return;
            MyDataBox ws = MyDataBox.create_from_liteDB(sfd.FileName, "TrainingImages");
            foreach (var propertyName in g.propertyNames)
            {
                ws.add_item(propertyName, g[propertyName].convert_to_grid());
            }
        }
    }
}
