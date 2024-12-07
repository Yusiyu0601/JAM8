using ExcelLibrary.SpreadSheet;
using JAM8.Algorithms.Geometry;
using MiniExcelLibs;
using System.Data;
using System.IO;


namespace JAM8.Utilities
{
    /// <summary>
    /// Excel读写类(xls与xlsx格式均可以使用)
    /// </summary>
    public class ExcelHelper
    {
        /// <summary>
        /// excel文件流类型，包括xlsx和xls两种
        /// </summary>
        public enum ExcelStreamType
        {
            xls,
            xlsx
        }

        /// <summary>
        /// 读取Excel文件，转换为DataTable对象(首行为标题)
        /// </summary>
        /// <param name="file_name"></param>
        /// <returns></returns>
        public static DataTable excel_to_dataTable(string file_name)
        {
            string ext = Path.GetExtension(file_name);
            if (ext == ".xls")
                return xls_to_dataTable(file_name);
            if (ext == ".xlsx")
                return xlsx_to_dataTable(file_name);
            return null;
        }

        /// <summary>
        /// 将Excel文件流转换为DataTable对象(首行为标题)
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="excel_stream_type"></param>
        /// <returns></returns>
        public static DataTable excel_to_dataTable(Stream stream, ExcelStreamType excel_stream_type)
        {
            return excel_stream_type switch
            {
                ExcelStreamType.xls => xls_to_dataTable(stream),
                ExcelStreamType.xlsx => xlsx_to_dataTable(stream),
                _ => null,
            };
        }

        /// <summary>
        /// 将DataTable对象保存为Excel文件(首行为标题)
        /// </summary>
        /// <param name="file_name"></param>
        /// <param name="dt"></param>
        public static void dataTable_to_excel(string file_name, DataTable dt)
        {
            string ext = Path.GetExtension(file_name);
            if (ext == ".xls")
                dataTable_to_xls(file_name, dt);
            if (ext == ".xlsx")
                dataTable_to_xlsx(file_name, dt);
        }

        #region xls文件读写

        /// <summary>
        /// 读取xls格式的Excel的第1个Sheet，读取为dataTable对象
        /// </summary>
        /// <param name="file_name"></param>
        /// <returns></returns>
        static DataTable xls_to_dataTable(string file_name)
        {
            string ext = FileHelper.GetFileExtension(file_name);
            if (ext != ".xls")
            { MessageBox.Show("文件后缀不是xls"); return null; }
            Workbook workbook = Workbook.Load(file_name);
            foreach (Worksheet ws in workbook.Worksheets)
            {
                if (ws.Name.Equals(workbook.Worksheets[0].Name))
                    return PopulateDataTable(ws);
            }
            return null;
        }

        /// <summary>
        /// 读取xls格式的Excel的第1个Sheet，读取为dataTable对象
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        static DataTable xls_to_dataTable(Stream stream)
        {
            Workbook workbook = Workbook.Load(stream);
            foreach (Worksheet ws in workbook.Worksheets)
            {
                if (ws.Name.Equals(workbook.Worksheets[0].Name))
                    return PopulateDataTable(ws);
            }
            return null;
        }

        /// <summary>
        /// 读取xls格式的Excel指定名称的Sheet，读取为dataTable对象
        /// </summary>
        /// <param name="file_name"></param>
        /// <param name="sheet_name"></param>
        /// <returns></returns>
        static DataTable xls_to_dataTable(string file_name, string sheet_name)
        {
            Workbook workbook = Workbook.Load(file_name);
            foreach (Worksheet ws in workbook.Worksheets)
            {
                if (ws.Name.Equals(sheet_name))
                    return PopulateDataTable(ws);
            }
            return null;
        }

        /// <summary>
        /// 读取xls格式的Excel所有Sheet，读取为dataSet对象
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        static DataSet xls_to_dataSet(string file_name)
        {
            DataSet ds = new();
            Workbook workbook = Workbook.Load(file_name);
            foreach (Worksheet ws in workbook.Worksheets)
            {
                DataTable dt = PopulateDataTable(ws);
                ds.Tables.Add(dt);
            }
            return ds;
        }

        static DataTable PopulateDataTable(Worksheet ws)
        {
            CellCollection Cells = ws.Cells;

            // Creates DataTable from a Worksheet
            // All values will be treated as Strings
            DataTable dt = new(ws.Name);

            // Extract columns
            for (int i = 0; i <= Cells.LastColIndex; i++)
                dt.Columns.Add(Cells[0, i].StringValue, typeof(string));

            // Extract data
            for (int currentRowIndex = 1; currentRowIndex <= Cells.LastRowIndex; currentRowIndex++)
            {
                DataRow dr = dt.NewRow();
                for (int currentColumnIndex = 0; currentColumnIndex <= Cells.LastColIndex; currentColumnIndex++)
                    dr[currentColumnIndex] = Cells[currentRowIndex, currentColumnIndex].StringValue;
                dt.Rows.Add(dr);
            }

            return dt;
        }

        /// <summary>
        /// dataTable对象保存为xls格式的Excel
        /// </summary>
        /// <param name="file_name"></param>
        /// <param name="dt"></param>
        static void dataTable_to_xls(string file_name, DataTable dt)
        {
            Workbook workbook = new();
            Worksheet worksheet = new("Sheet1");
            //构建表头
            for (int col = 0; col < dt.Columns.Count; col++)
            {
                worksheet.Cells[0, col] = new Cell(dt.Columns[col].ColumnName);
            }

            for (int rowIndex = 0; rowIndex < dt.Rows.Count; rowIndex++)
            {
                for (int colIndex = 0; colIndex < dt.Columns.Count; colIndex++)
                {
                    worksheet.Cells[rowIndex + 1, colIndex] = new Cell(dt.Rows[rowIndex][colIndex].ToString());
                }
            }
            //设置默认宽度
            if (worksheet.Cells.LastRowIndex > 1)
            {
                for (int n = 0; n < dt.Columns.Count; n++)
                {
                    if (worksheet.Cells[1, n].Value.ToString().Length >= 50)
                        worksheet.Cells.ColumnWidth[(ushort)n] = 10000;
                    else if (worksheet.Cells[1, n].Value.ToString().Length >= 10)
                        worksheet.Cells.ColumnWidth[(ushort)n] = (ushort)(worksheet.Cells[1, n].Value.ToString().Length * 300);
                    else
                        worksheet.Cells.ColumnWidth[(ushort)n] = 3000;
                }
            }
            else
                worksheet.Cells.ColumnWidth[0, (ushort)(dt.Columns.Count - 1)] = 3000;

            workbook.Worksheets.Add(worksheet);
            workbook.Save(file_name);
        }

        /// <summary>
        /// dataSet对象保存为xls格式的Excel
        /// </summary>
        /// <param name="file_name"></param>
        /// <param name="ds"></param>
        static void dataSet_to_xls(string file_name, DataSet ds)
        {
            if (ds.Tables.Count == 0)
                throw new ArgumentException("DataSet needs to have at least one DataTable", "dataset");

            Workbook workbook = new();
            foreach (DataTable dt in ds.Tables)
            {
                Worksheet worksheet = new(dt.TableName);
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    // Add column header
                    worksheet.Cells[0, i] = new Cell(dt.Columns[i].ColumnName);

                    // Populate row data
                    for (int j = 0; j < dt.Rows.Count; j++)
                        worksheet.Cells[j + 1, i] = new Cell(dt.Rows[j][i]);
                }
                workbook.Worksheets.Add(worksheet);
            }
            workbook.Save(file_name);
        }

        /// <summary>
        /// dataTable对象保存为xls格式的文件流
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="ds"></param>
        /// <exception cref="ArgumentException"></exception>
        static void dataSet_to_stream(Stream stream, DataSet ds)
        {
            if (ds.Tables.Count == 0)
                throw new ArgumentException("DataSet needs to have at least one DataTable", "dataset");

            Workbook workbook = new();
            foreach (DataTable dt in ds.Tables)
            {
                Worksheet worksheet = new(dt.TableName);
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    // Add column header
                    worksheet.Cells[0, i] = new Cell(dt.Columns[i].ColumnName);

                    // Populate row data
                    for (int j = 0; j < dt.Rows.Count; j++)
                        worksheet.Cells[j + 1, i] = new Cell(dt.Rows[j][i]);
                }
                workbook.Worksheets.Add(worksheet);
            }
            workbook.SaveToStream(stream);
        }

        #endregion

        #region xlsx文件读写

        /// <summary>
        /// 读取xlsx格式的Excel的第1个Sheet，读取为dataTable对象
        /// </summary>
        /// <param name="file_name"></param>
        /// <param name="useHeaderRow"></param>
        static DataTable xlsx_to_dataTable(string file_name)
        {
            try
            {
                using var stream = File.OpenRead(file_name);
                var dt = MiniExcel.QueryAsDataTable(stream, useHeaderRow: true);
                return dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            return null;
        }

        /// <summary>
        /// 读取xlsx格式的Excel的第1个Sheet，读取为dataTable对象
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        static DataTable xlsx_to_dataTable(Stream stream)
        {
            var dt = MiniExcel.QueryAsDataTable(stream, useHeaderRow: true);
            return dt;
        }

        /// <summary>
        /// dt对象保存为xlsx格式的Excel
        /// </summary>
        /// <param name="file_name"></param>
        /// <param name="dt"></param>
        static void dataTable_to_xlsx(string file_name, DataTable dt)
        {
            MiniExcel.SaveAs(file_name, dt, printHeader: true, overwriteFile: true);
        }

        #endregion
    }
}
