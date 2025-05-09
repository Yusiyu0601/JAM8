using System.Data;
using JAM8.Utilities;
using LiteDB;

namespace JAM8.Algorithms.Geometry
{
    // GridCatalogItem 类用于表示网格目录中的一项，包含网格的基本信息
    public class GridCatalogItem
    {
        public string grid_name { get; internal set; } // 网格名称
        public string path { get; internal set; } // 网格文件路径
        public double null_value { get; internal set; } // 网格中的空值
        public string grid_structure { get; internal set; } // 网格结构描述
    }

    /// <summary>
    /// 使用 Excel 文件管理网格信息与路径，支持添加、删除、查询网格项。
    /// </summary>
    public class GridCatalog
    {
        public string file_path { get; internal set; } // 存储网格目录信息的文件路径

        // 构造函数是私有的，确保只能通过 `create` 或 `open` 方法来创建实例
        private GridCatalog() { }

        /// <summary>
        /// 创建新的 GridCatalog 文件，并将其保存到指定路径
        /// </summary>
        /// <param name="file_path">要保存的文件路径</param>
        /// <returns>返回创建的 GridCatalog 实例</returns>
        public static GridCatalog create(string file_path)
        {
            // 获取内嵌资源并保存到文件路径
            string embeddedFilePath = "JAM8.资源文件.空白excel.xlsx";
            MyEmbeddedFileHelper.save_embedded_to_filePath(embeddedFilePath, file_path);

            // 返回新的 GridCatalog 实例
            return new GridCatalog()
            {
                file_path = file_path,
            };
        }

        /// <summary>
        /// 打开现有的 GridCatalog 文件
        /// </summary>
        /// <param name="file_path">要打开的文件路径</param>
        /// <returns>返回打开的 GridCatalog 实例，如果文件不存在则返回 null</returns>
        public static GridCatalog open(string file_path)
        {
            // 如果文件不存在，返回 null
            if (FileHelper.IsExistFile(file_path) == false)
                return null;

            // 返回打开的 GridCatalog 实例
            return new GridCatalog()
            {
                file_path = file_path,
            };
        }

        /// <summary>
        /// 获取所有网格目录项
        /// </summary>
        /// <returns>返回所有网格项的列表</returns>
        public List<GridCatalogItem> get_items()
        {
            // 从 Excel 文件中读取数据并转化为 DataTable
            var dt = ExcelHelper.excel_to_dataTable(file_path);

            // 将 DataTable 转化为 GridCatalogItem 实体列表
            var list = DataTableToEntities(dt);

            return list;
        }

        /// <summary>
        /// 向网格目录中添加新的网格信息项
        /// </summary>
        /// <param name="grid">要添加的网格对象</param>
        /// <param name="grid_path">网格文件路径</param>
        /// <param name="null_value">网格中的空值</param>
        /// <returns>添加是否成功</returns>
        public bool add_item(Grid grid, string grid_path, float null_value)
        {
            // 创建新的 GridCatalogItem 实体
            GridCatalogItem gci = new()
            {
                grid_name = grid.grid_name,
                path = grid_path,
                null_value = null_value,
                grid_structure = grid.gridStructure.view_text(), // 获取网格结构的文本描述
            };

            // 获取现有的网格项列表
            var list = get_items();

            // 检查是否已有相同名称的网格项
            if (list.Exists(a => a.grid_name == gci.grid_name))
                return false; // 如果存在相同名称的网格，返回 false

            // 将新的网格项添加到列表
            list.Add(gci);

            // 将更新后的列表转化为 DataTable 并保存回 Excel 文件
            var dt = entities_to_dataTable(list);
            ExcelHelper.dataTable_to_excel(file_path, dt);

            return true; // 返回成功
        }

        /// <summary>
        /// 删除指定名称的网格信息项
        /// </summary>
        /// <param name="grid_name">要删除的网格名称</param>
        public void delete_item(string grid_name)
        {
            // 获取所有网格项列表
            var list = get_items();

            // 查找并返回指定名称的网格项
            var gci = find_item(grid_name);

            if (gci != null)
            {
                // 删除列表中与指定名称不匹配的网格项
                var removed = list.Where(a => a.grid_name != grid_name).ToList();

                // 将删除后的列表保存回 Excel 文件
                var dt = EntityHelper.entities_to_dataTable(removed);
                ExcelHelper.dataTable_to_excel(file_path, dt);
            }
        }

        /// <summary>
        /// 查找指定名称的网格信息项
        /// </summary>
        /// <param name="grid_name">要查找的网格名称</param>
        /// <returns>返回找到的网格项，如果未找到则返回 null</returns>
        public GridCatalogItem find_item(string grid_name)
        {
            var list = get_items();
            var gci = list.Find(a => a.grid_name == grid_name);
            return gci;
        }

        /// <summary>
        /// 根据网格名称读取对应的 Grid 对象
        /// </summary>
        /// <param name="grid_name">网格名称</param>
        /// <returns>返回读取的 Grid 对象</returns>
        public Grid read_grid(string grid_name)
        {
            var list = get_items();
            var result = list.Find(a => a.grid_name == grid_name);
            if (result == null)
                return null;

            // 创建网格结构并初始化网格对象
            GridStructure gs = GridStructure.create(result.grid_structure);
            Grid g = Grid.create(gs, result.grid_name);

            // 从文件中创建网格数据
            g.read_from_gslib(result.path, 1, (float)result.null_value, result.grid_name);
            return g;
        }

        /// <summary>
        /// 将 DataTable 转化为 GridCatalogItem 实体列表
        /// </summary>
        /// <param name="dt">包含网格信息的 DataTable</param>
        /// <returns>返回转化后的 GridCatalogItem 实体列表</returns>
        public static List<GridCatalogItem> DataTableToEntities(DataTable dt)
        {
            if (dt == null || dt.Rows.Count == 0)
                return new List<GridCatalogItem>(); // 如果 DataTable 为空或没有数据，返回空列表

            var entityList = new List<GridCatalogItem>();

            // 遍历 DataTable 中的每一行
            foreach (DataRow row in dt.Rows)
            {
                try
                {
                    // 创建新的 GridCatalogItem 实体并从 DataRow 中提取数据
                    var entity = new GridCatalogItem
                    {
                        grid_name = row["grid_name"] != DBNull.Value ? row["grid_name"].ToString() : string.Empty,
                        path = row["path"] != DBNull.Value ? row["path"].ToString() : string.Empty,
                        null_value = row["null_value"] != DBNull.Value ? Convert.ToDouble(row["null_value"]) : double.NaN,
                        grid_structure = row["grid_structure"] != DBNull.Value ? row["grid_structure"].ToString() : string.Empty
                    };

                    // 将实体添加到列表中
                    entityList.Add(entity);
                }
                catch (Exception ex)
                {
                    // 记录错误日志或执行其他错误处理
                    Console.WriteLine($@"Error processing row: {ex.Message}");
                }
            }

            return entityList; // 返回填充完成的实体列表
        }

        /// <summary>
        /// 将 GridCatalogItem 实体列表转化为 DataTable
        /// </summary>
        /// <param name="entityList">包含网格信息的实体列表</param>
        /// <returns>返回转化后的 DataTable</returns>
        public static DataTable entities_to_dataTable(List<GridCatalogItem> entityList)
        {
            // 创建一个新的 DataTable 实例
            DataTable dt = new DataTable();

            // 如果实体列表为空，直接返回空 DataTable
            if (entityList == null || entityList.Count == 0)
                return dt;

            // 创建 DataTable 的列，列名对应 GridCatalogItem 类的属性
            dt.Columns.Add("grid_name", typeof(string));
            dt.Columns.Add("path", typeof(string));
            dt.Columns.Add("null_value", typeof(double));
            dt.Columns.Add("grid_structure", typeof(string));

            // 遍历实体列表，将每个实体添加到 DataTable 中
            foreach (var entity in entityList)
            {
                // 创建一行，并填充属性值
                DataRow row = dt.NewRow();
                row["grid_name"] = entity.grid_name;
                row["path"] = entity.path;
                row["null_value"] = entity.null_value;
                row["grid_structure"] = entity.grid_structure;

                // 将这一行添加到 DataTable
                dt.Rows.Add(row);
            }

            // 返回转换后的 DataTable
            return dt;
        }

    }
}
