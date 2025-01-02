using System.Data;
using System.Text;

namespace JAM8.Utilities
{
    public partial class Form_DataTable : Form
    {
        public Form_DataTable(DataTable dt)
        {
            InitializeComponent();
            this.dataGridView1.DataSource = dt;
        }

        private void dataGridView1_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            //自动编号，与数据无关
            Rectangle rectangle = new(e.RowBounds.Location.X, e.RowBounds.Location.Y,
               dataGridView1.RowHeadersWidth - 4, e.RowBounds.Height);
            TextRenderer.DrawText(e.Graphics, (e.RowIndex + 1).ToString(),
                   dataGridView1.RowHeadersDefaultCellStyle.Font,
                   rectangle,
                   dataGridView1.RowHeadersDefaultCellStyle.ForeColor,
                   TextFormatFlags.VerticalCenter | TextFormatFlags.Right);
        }

        private void 保存ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ExcelHelper.dataTable_to_excel(FileDialogHelper.SaveExcel(), dataGridView1.DataSource as DataTable);
        }
    }
}
