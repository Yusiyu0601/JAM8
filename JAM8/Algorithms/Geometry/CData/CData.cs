using JAM8.Algorithms.Forms;
using JAM8.Algorithms.Numerics;
using JAM8.Utilities;

namespace JAM8.Algorithms.Geometry
{
    /// <summary>
    /// Conditional data class, using DataFrame as the data buffer for conditional data
    /// </summary>
    public class CData
    {
        private CData()
        {
        }

        public Dimension dim { get; internal set; }

        // Property names of the conditional data, excluding coordinates
        public string[] property_names { get; internal set; }

        // Number of conditional data items
        public int N_cdata_items => buffer.N_Record;

        //Data buffer for conditional data
        private MyDataFrame buffer;

        //The number of the x coordinate in the data buffer, starting at 0
        private int x_series_index { get; init; }

        //The number of the y coordinate in the data buffer, starting at 0
        private int y_series_index { get; init; }

        //The number of the z coordinate in the data buffer, starting at 0
        private int z_series_index { get; init; }

        /// <summary>
        /// Null value used to mark missing/invalid data. If null, null checking is disabled.
        /// This value is immutable once the object is constructed.
        /// </summary>
        private float? null_value { get; init; }

        //The target grid structure, if not empty, indicates that the conditional data
        //has been coarsened to the target grid structure
        public GridStructure target_gs { get; internal set; }

        /// <summary>
        /// Get the numerical value of the conditional data based on the sequence number and 
        /// attribute name of the conditional data
        /// </summary>
        /// <param name="idx">The serial number of the conditional data, starting from 0</param>
        /// <param name="property_name">Attribute name</param>
        /// <returns>Nullable float value</returns>
        public float? get_value(int idx, string property_name)
        {
            float? value = Convert.ToSingle(buffer[idx, property_name]);
            if (null_value.HasValue && null_value == value)
                value = null;
            return value;
        }

        /// <summary>
        /// Get the numerical value of the conditional data based on the sequence number and 
        /// property index of the conditional data
        /// </summary>
        /// <param name="idx">The serial number of the conditional data, starting from 0</param>
        /// <param name="property_idx">The index of the property, starting from 0</param>
        /// <returns>Nullable float value</returns>
        public float? get_value(int idx, int property_idx)
        {
            return get_value(idx, property_names[property_idx]);
        }

        /// <summary>
        /// Get a full record (including coordinates and all properties) by row index.
        /// </summary>
        /// <param name="idx">The record index</param>
        /// <returns>Dictionary of all field names to their values (nullable float)</returns>
        public (string[] field_names, float?[] values) get_cdata_item(int idx)
        {
            List<string> names = new();
            List<float?> values = new();

            // Add coordinates
            names.Add(buffer.series_names[x_series_index]);
            values.Add(Convert.ToSingle(buffer[idx, x_series_index]));

            names.Add(buffer.series_names[y_series_index]);
            values.Add(Convert.ToSingle(buffer[idx, y_series_index]));

            if (dim == Dimension.D3)
            {
                names.Add(buffer.series_names[z_series_index]);
                values.Add(Convert.ToSingle(buffer[idx, z_series_index]));
            }

            // Add properties
            foreach (var name in property_names)
            {
                names.Add(name);
                values.Add(get_value(idx, name));
            }

            return (names.ToArray(), values.ToArray());
        }

        /// <summary>
        /// Gets the spatial coordinate of the specified record.
        /// </summary>
        /// <param name="idx">The record index.</param>
        /// <returns>A Coord object representing the (x, y[, z]) location.</returns>
        public Coord get_coord(int idx)
        {
            float x = Convert.ToSingle(buffer[idx, x_series_index]);
            float y = Convert.ToSingle(buffer[idx, y_series_index]);

            if (dim == Dimension.D3)
            {
                float z = Convert.ToSingle(buffer[idx, z_series_index]);
                return Coord.create(x, y, z);
            }
            else
            {
                return Coord.create(x, y);
            }
        }

        /// <summary>
        /// coarsening the conditional data to the target grid structure, and adjust the 
        /// conditional data to the grid cells of the target grid structure
        /// 将条件数据粗化到目标网格结构，实现条件数据调整至目标网格结构的网格单元上
        /// </summary>
        /// <param name="gs"></param>
        /// <returns></returns>
        public (CData coarsened_cd, Grid coarsened_grid) coarsened(GridStructure gs)
        {
            CData cd_coarsened = new()
            {
                dim = dim,
                x_series_index = x_series_index,
                y_series_index = y_series_index,
                z_series_index = z_series_index,
                null_value = null_value,
                target_gs = gs,
                property_names = property_names.Clone() as string[], //复制属性名称
                buffer = MyDataFrame.create(buffer.series_names) //复制原始数据缓冲区的结构
            };

            for (int idx_record = 0; idx_record < buffer.N_Record; idx_record++)
            {
                SpatialIndex si = null; //粗化后条件数据的空间索引
                Coord coord = null; //条件数据的坐标
                if (dim == Dimension.D2)
                {
                    float x = Convert.ToSingle(buffer[idx_record, x_series_index]);
                    float y = Convert.ToSingle(buffer[idx_record, y_series_index]);
                    coord = Coord.create(x, y);
                    si = gs.coord_to_spatial_index(coord);
                    if (si != null) //保留落在grid范围内的cdi
                    {
                        MyRecord record = buffer.get_record(idx_record); //从原始表里提取记录，然后修改
                        record[buffer.series_names[x_series_index]] = si.ix;
                        record[buffer.series_names[y_series_index]] = si.iy;
                        cd_coarsened.buffer.add_record(record);
                    }
                }

                if (dim == Dimension.D3)
                {
                    float x = Convert.ToSingle(buffer[idx_record, x_series_index]);
                    float y = Convert.ToSingle(buffer[idx_record, y_series_index]);
                    float z = Convert.ToSingle(buffer[idx_record, z_series_index]);
                    coord = Coord.create(x, y, z);
                    si = gs.coord_to_spatial_index(coord);
                    if (si != null) //保留落在grid范围内的cdi
                    {
                        MyRecord record = buffer.get_record(idx_record); //从原始表里提取记录，然后修改
                        record[buffer.series_names[x_series_index]] = si.ix;
                        record[buffer.series_names[y_series_index]] = si.iy;
                        record[buffer.series_names[z_series_index]] = si.iz;
                        cd_coarsened.buffer.add_record(record);
                    }
                }
            }

            Grid g = Grid.create(gs, "coarsened");
            foreach (var property_name in cd_coarsened.property_names)
            {
                g.add_gridProperty(property_name);
            }

            //循环将coarsened里的数据赋值给g
            for (int idx_record = 0; idx_record < cd_coarsened.buffer.N_Record; idx_record++)
            {
                SpatialIndex si = null;
                if (dim == Dimension.D2)
                {
                    int ix = Convert.ToInt32(cd_coarsened.buffer[idx_record, x_series_index]);
                    int iy = Convert.ToInt32(cd_coarsened.buffer[idx_record, y_series_index]);
                    si = SpatialIndex.create(ix, iy);
                }

                if (dim == Dimension.D3)
                {
                    int ix = Convert.ToInt32(cd_coarsened.buffer[idx_record, x_series_index]);
                    int iy = Convert.ToInt32(cd_coarsened.buffer[idx_record, y_series_index]);
                    int iz = Convert.ToInt32(cd_coarsened.buffer[idx_record, z_series_index]);
                    si = SpatialIndex.create(ix, iy, iz);
                }

                if (si != null)
                {
                    for (int j = 0; j < cd_coarsened.property_names.Length; j++)
                    {
                        g[cd_coarsened.property_names[j]].set_value(si, cd_coarsened.get_value(idx_record, j));
                    }
                }
            }

            return (cd_coarsened, g);
        }

        /// <summary>
        /// Gets the spatial boundary of the conditional data (min/max values of x, y, and z coordinates).
        /// For 2D data, the z boundary values will be null.
        /// </summary>
        /// <returns>(min_x, max_x, min_y, max_y, min_z, max_z)</returns>
        public (float min_x, float max_x, float min_y, float max_y, float? min_z, float? max_z) get_boundary()
        {
            float min_x = float.MaxValue, max_x = float.MinValue;
            float min_y = float.MaxValue, max_y = float.MinValue;
            float? min_z = null, max_z = null;

            for (int i = 0; i < N_cdata_items; i++)
            {
                float x = Convert.ToSingle(buffer[i, x_series_index]);
                float y = Convert.ToSingle(buffer[i, y_series_index]);

                if (x < min_x) min_x = x;
                if (x > max_x) max_x = x;

                if (y < min_y) min_y = y;
                if (y > max_y) max_y = y;

                if (dim == Dimension.D3)
                {
                    float z = Convert.ToSingle(buffer[i, z_series_index]);
                    if (min_z == null || z < min_z) min_z = z;
                    if (max_z == null || z > max_z) max_z = z;
                }
            }

            return (min_x, max_x, min_y, max_y, min_z, max_z);
        }

        /// <summary>
        /// Deep copy
        /// </summary>
        /// <returns></returns>
        public CData deep_clone()
        {
            CData cd = new()
            {
                dim = dim,
                null_value = null_value,

                x_series_index = x_series_index,
                y_series_index = y_series_index,
                z_series_index = z_series_index,

                target_gs = target_gs, //这个不需要深度复制，因为是占用空间较大，而且只是引用

                buffer = buffer.deep_clone(),
                property_names = [.. property_names]
            };

            return cd;
        }

        /// <summary>
        /// Read CData from gslib.
        /// </summary>
        /// <param name="file_name"></param>
        /// <param name="x_series_index"></param>
        /// <param name="y_series_index"></param>
        /// <param name="z_series_index"></param>
        /// <param name="null_value"></param>
        /// <returns></returns>
        public static CData read_from_gslib(string file_name, int x_series_index, int y_series_index,
            int z_series_index, float? null_value)
        {
            CData cdata = new()
            {
                buffer = GSLIB.gslib_to_df(file_name),
                x_series_index = x_series_index,
                y_series_index = y_series_index,
                z_series_index = z_series_index,
                null_value = null_value,
                dim = z_series_index == -1 ? Dimension.D2 : Dimension.D3
            };

            //将cdata.buffer里所有除了x_series_index y_series_index z_series_index以外的属性名称提取出来
            List<string> property_names = [];
            for (int i = 0; i < cdata.buffer.series_names.Length; i++)
            {
                if (cdata.dim == Dimension.D2 && i != x_series_index && i != y_series_index)
                {
                    property_names.Add(cdata.buffer.series_names[i]);
                }

                if (cdata.dim == Dimension.D3 && i != x_series_index && i != y_series_index && i != z_series_index)
                {
                    property_names.Add(cdata.buffer.series_names[i]);
                }
            }

            cdata.property_names = [.. property_names];

            return cdata;
        }

        /// <summary>
        /// Read CData from gslib (winform mode).
        /// </summary>
        /// <returns></returns>
        public static (CData cdata, string file_name) read_from_gslib_win(string title = null)
        {
            Form_ReadConditionData frm = new(title);
            if (frm.ShowDialog() != DialogResult.OK)
                return (null, null);
            var paras = frm.paras;

            var file_name = paras[0];
            var dim = (Dimension)Enum.Parse(typeof(Dimension), paras[1]);
            int x_series_index = int.Parse(paras[2]);
            int y_series_index = int.Parse(paras[3]);
            int z_series_index = int.Parse(paras[4]);
            bool null_value_enabled = bool.Parse(paras[5]);
            float? null_value = float.Parse(paras[6]);

            if (null_value_enabled == false) //如果不启用null值，则设置为null
                null_value = null;

            if (dim == Dimension.D2)
                return (read_from_gslib(file_name, x_series_index, y_series_index, -1, null_value),
                    file_name);
            if (dim == Dimension.D3)
                return (read_from_gslib(file_name, x_series_index, y_series_index, z_series_index, null_value),
                    file_name);
            //如果dim不是D2或D3，则返回null
            return (null, file_name);
        }

        /// <summary>
        /// 保存至gslib
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="null_value"></param>
        public void save_to_gslib(string fileName, float null_value)
        {
            GSLIB.df_to_gslib(fileName, null_value, buffer);
        }

        /// <summary>
        /// 保存至gslib
        /// </summary>
        public void save_to_gslibwin(string title = null)
        {
            Form_WriteConditionData frm = new(this, title);
            if (frm.ShowDialog() != DialogResult.OK)
                return;
            var paras = frm.paras;
            var fileName = paras[0];
            var null_value = float.Parse(paras[1]); //null值
            save_to_gslib(fileName, null_value);
        }

        /// <summary>
        /// Use the nodes in gridProperty that satisfy the condition as the condition data
        /// </summary>
        /// <param name="gp"></param>
        /// <param name="grid_property_name"></param>
        /// <param name="compare_type"></param>
        /// <param name="compared_value"></param>
        /// <returns></returns>
        public static CData create_from_gridProperty(GridProperty gp, string grid_property_name,
            CompareType compare_type, float? compared_value)
        {
            CData cd = new()
            {
                property_names = [grid_property_name],
                target_gs = gp.grid_structure,
                dim = gp.grid_structure.dim,
                x_series_index = 0,
                y_series_index = 1,
                z_series_index = gp.grid_structure.dim == Dimension.D3 ? 2 : -1,
                buffer = MyDataFrame.create(
                    gp.grid_structure.dim == Dimension.D2
                        ? new[] { "x", "y", grid_property_name }
                        : new[] { "x", "y", "z", grid_property_name }
                )
            };

            //遍历所有节点，判断是否满足条件
            for (int n = 0; n < gp.grid_structure.N; n++)
            {
                Coord c = gp.grid_structure.array_index_to_coord(n);
                float? currentValue = gp.get_value(n);

                //根据compare_type，判断是否保留等于compared_value的节点，用switch语法
                // 判断当前条件是否满足
                bool shouldReplace = compare_type switch
                {
                    CompareType.NoCompared => true,
                    CompareType.Equals => currentValue == compared_value,
                    CompareType.NotEqual => currentValue != compared_value,
                    CompareType.GreaterThan => currentValue > compared_value,
                    CompareType.GreaterEqualsThan => currentValue >= compared_value,
                    CompareType.LessThan => currentValue < compared_value,
                    CompareType.LessEqualsThan => currentValue <= compared_value,
                    _ => false
                };

                // 如果当前条件满足，使用该条件的Value
                if (shouldReplace)
                {
                    if (cd.dim == Dimension.D2)
                        cd.buffer.add_record([c.x, c.y, currentValue]);
                    if (cd.dim == Dimension.D3)
                        cd.buffer.add_record([c.x, c.y, c.z, currentValue]);
                }
            }

            return cd;
        }

        /// <summary>
        /// 窗体模式选择cd_propertyName
        /// </summary>
        public string select_by_property_name_win(string title = null)
        {
            Form_SelectPropertyFromCData frm = new(this, title);
            if (frm.ShowDialog() != DialogResult.OK)
                return null;
            return frm.selected_property_name;
        }

        /// <summary>
        /// Returns the first few lines of the data buffer as a preview.
        /// </summary>
        /// <param name="N_first_lines">The number of lines to retrieve from the start of the buffer. Defaults to 15 if not specified.</param>
        /// <returns>A string containing the first <paramref name="N_first_lines"/> lines of the text buffer.</returns>
        public string view_text(int N_first_lines = 15)
        {
            return buffer.view_text(N_first_lines);
        }

        /// <summary>
        /// Prints the contents of the data buffer.
        /// </summary>
        /// <remarks>Delegates to the underlying buffer's print method.</remarks>
        public void print(int N_first_lines = 15)
        {
            MyConsoleHelper.write_string_to_console($"CData\n{view_text()}");
        }

        public override string ToString()
        {
            string dim_str = dim == Dimension.D2 ? "2D" : "3D";
            string props = property_names == null || property_names.Length == 0
                ? "none"
                : string.Join(", ", property_names);
            return $"CData2 [{dim_str}] — {buffer.N_Record} records, {buffer.N_Series} fields | Properties: [{props}]";
        }

        /// <summary>
        /// 检查条件数据是否与网格属性匹配
        /// </summary>
        /// <param name="gp"></param>
        /// <param name="cd_property_name"></param>
        /// <returns></returns>
        public (int not_match_number, int[] not_match_array_index) check_match(GridProperty gp, string cd_property_name)
        {
            if (target_gs == null)
            {
                return (-1, null);
            }

            if (target_gs != gp.grid_structure)
                return (-1, null);

            int not_match_number = 0;
            List<int> not_match_array_index = [];
            for (int i_Record = 0; i_Record < buffer.N_Record; i_Record++)
            {
                float? value = null;
                if (dim == Dimension.D2)
                {
                    int ix = Convert.ToInt32(buffer[i_Record, x_series_index]);
                    int iy = Convert.ToInt32(buffer[i_Record, y_series_index]);
                    value = gp.get_value(ix, iy);
                }

                if (dim == Dimension.D3)
                {
                    int ix = Convert.ToInt32(buffer[i_Record, x_series_index]);
                    int iy = Convert.ToInt32(buffer[i_Record, y_series_index]);
                    int iz = Convert.ToInt32(buffer[i_Record, z_series_index]);
                    value = gp.get_value(ix, iy, iz);
                }

                float? cd_value = Convert.ToSingle(buffer[i_Record, cd_property_name]);
                if (cd_value != value)
                {
                    not_match_number++;
                    not_match_array_index.Add(i_Record);
                }
            }

            return (not_match_number, not_match_array_index.ToArray());
        }
    }
}