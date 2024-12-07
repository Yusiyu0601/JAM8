using System.Data;
using System.Text;
using ScottPlot;

namespace JAM8.Utilities
{
    public class DataTableHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="Title"></param>
        /// <param name="is_showDialog"></param>
        public static void show_win(DataTable dt, string Title = "数据表", bool is_showDialog = false)
        {
            Form_DataTable frm = new(dt)
            {
                Text = Title
            };
            if (is_showDialog == false)
                frm.Show();
            else
                frm.ShowDialog();
        }

        /// <summary>
        /// 根据datatable获得列名
        /// </summary>
        /// <param name="dt">表对象</param>
        /// <returns>返回结果的数据列数组</returns>
        public static string[] get_column_names(DataTable dt)
        {
            List<string> column_names = new();
            foreach (DataColumn col in dt.Columns)
            {
                column_names.Add(col.ColumnName);//获取到DataColumn列对象的列名
            }
            return column_names.ToArray();
        }

    }
}
