using JAM8.Algorithms.Numerics;
using JAM8.Utilities;

namespace JAM8.Algorithms.Geometry
{
    /// <summary>
    /// Conditional data class, using DataFrame as the data buffer for conditional data
    /// 条件数据类，采用DataFrame作为条件数据的数据缓存区
    /// </summary>
    public class CData2
    {
        private CData2() { }

        public Dimension dim { get; internal set; }//维度

        public string[] property_names { get; internal set; }//属性名称

        private MyDataFrame buffer;//Data buffer for conditional data 条件数据的数据缓存区

        //The number of the x coordinate in the data buffer, starting at 0
        //x坐标在数据缓存区里的序号，从0开始
        private int x_series_index { get; set; }

        //The number of the y coordinate in the data buffer, starting at 0
        //y坐标在数据缓存区里的序号，从0开始
        private int y_series_index { get; set; }

        //The number of the z coordinate in the data buffer, starting at 0
        //z坐标在数据缓存区里的序号，从0开始
        private int z_series_index { get; set; }

        private float null_value { get; set; }//null value

        private bool null_value_enabled { get; set; }//Set whether to enable null values 设置是否启用null value

        //The target grid structure, if not empty, indicates that the conditional data
        //has been coarsened to the target grid structure
        //目标网格结构，如果不为空，则表示条件数据已经粗化到目标网格结构上
        public GridStructure target_gs { get; internal set; }

        /// <summary>
        /// Get the numerical value of the conditional data based on the sequence number and 
        /// attribute name of the conditional data
        /// 根据条件数据的序号和属性名称，获取条件数据的数值
        /// </summary>
        /// <param name="idx">The serial number of the conditional data, starting from 0
        /// 条件数据的序号，从0开始</param>
        /// <param name="property_name">attribute name
        /// 属性名称</param>
        /// <returns></returns>
        public float? this[int idx, string property_name]
        {
            get
            {
                float? value = Convert.ToSingle(buffer[idx, property_name]);
                if (null_value_enabled && null_value == value)
                    value = null;
                return value;
            }
        }

        /// <summary>
        /// Sort by the ordinal number and attributes of the conditional data to obtain 
        /// the numerical value of the conditional data
        /// 根据条件数据的序号和属性排序，获取条件数据的数值
        /// </summary>
        /// <param name="idx">The serial number of the conditional data, starting from 0
        /// 条件数据的序号，从0开始</param>
        /// <param name="property_idx">The serial number of the property, starting from 0
        /// 属性的序号，从0开始</param>
        /// <returns></returns>
        public float? this[int idx, int property_idx]
        {
            get
            {
                return this[idx, property_names[property_idx]];
            }
        }

        /// <summary>
        /// coarsening the conditional data to the target grid structure, and adjust the 
        /// conditional data to the grid cells of the target grid structure
        /// 将条件数据粗化到目标网格结构，实现条件数据调整至目标网格结构的网格单元上
        /// </summary>
        /// <param name="target_gs"></param>
        /// <returns></returns>
        public (CData2 coarsened_cd, Grid coarsened_grid) coarsened(GridStructure target_gs)
        {
            CData2 cd_coarsened = new()
            {
                dim = dim,
                x_series_index = x_series_index,
                y_series_index = y_series_index,
                z_series_index = z_series_index,
                null_value = null_value,
                null_value_enabled = null_value_enabled,
                target_gs = target_gs,
                property_names = property_names.Clone() as string[],//复制属性名称
                buffer = MyDataFrame.create(buffer.series_names)//复制原始数据缓冲区的结构
            };

            for (int idx_record = 0; idx_record < buffer.N_Record; idx_record++)
            {
                SpatialIndex si = null;//粗化后条件数据的空间索引
                Coord coord = null;//条件数据的坐标
                if (dim == Dimension.D2)
                {
                    float x = Convert.ToSingle(buffer[idx_record, x_series_index]);
                    float y = Convert.ToSingle(buffer[idx_record, y_series_index]);
                    coord = Coord.create(x, y);
                    si = target_gs.coord_to_spatial_index(coord);
                    if (si != null)//保留落在grid范围内的cdi
                    {
                        MyRecord record = buffer.get_record(idx_record);//从原始表里提取记录，然后修改
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
                    si = target_gs.coord_to_spatial_index(coord);
                    if (si != null)//保留落在grid范围内的cdi
                    {
                        MyRecord record = buffer.get_record(idx_record);//从原始表里提取记录，然后修改
                        record[buffer.series_names[x_series_index]] = si.ix;
                        record[buffer.series_names[y_series_index]] = si.iy;
                        record[buffer.series_names[z_series_index]] = si.iz;
                        cd_coarsened.buffer.add_record(record);
                    }
                }
            }

            Grid g = Grid.create(target_gs, "coarsened");
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
                        g[cd_coarsened.property_names[j]].set_value(si, cd_coarsened[idx_record, j]);
                    }
                }
            }

            return (cd_coarsened, g);
        }

        /// <summary>
        /// Deep copy
        /// </summary>
        /// <returns></returns>
        public CData2 deep_clone()
        {
            CData2 cd = new()
            {
                dim = dim,
                null_value = null_value,
                null_value_enabled = null_value_enabled,

                x_series_index = x_series_index,
                y_series_index = y_series_index,
                z_series_index = z_series_index,

                target_gs = target_gs,//这个不需要深度复制，因为是占用空间较大，而且只是引用

                buffer = buffer.deep_clone(),
                property_names = [.. property_names]
            };

            return cd;
        }

        /// <summary>
        /// Read CData from gslib. 从gslib里读取CData
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="dim"></param>
        /// <param name="sep"></param>
        /// <param name="col_x"></param>
        /// <param name="col_y"></param>
        /// <param name="col_z"></param>
        /// <param name="enable_nullValue"></param>
        /// <param name="nullValue"></param>
        /// <returns></returns>
        public static CData2 read_from_gslib(string file_name, float null_value, bool null_value_enabled,
            int x_series_index, int y_series_index, int z_series_index = -1)
        {
            CData2 cdata = new()
            {
                buffer = GSLIB.gslib_to_df(file_name),
                x_series_index = x_series_index,
                y_series_index = y_series_index,
                z_series_index = z_series_index,
                null_value = null_value,
                null_value_enabled = null_value_enabled,
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
        /// Read CData from gslib (winform mode). 从gslib里读取CData（窗体模式）
        /// </summary>
        /// <returns></returns>
        public static (CData2 cdata, string file_name) read_from_gslib_win(string title = null)
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
            bool null_value_enabled = bool.Parse(paras[5]);//启用null value
            float null_value = float.Parse(paras[6]);
            if (dim == Dimension.D2)
                return (read_from_gslib(file_name, null_value, null_value_enabled, x_series_index, y_series_index, -1), file_name);
            if (dim == Dimension.D3)
                return (read_from_gslib(file_name, null_value, null_value_enabled, x_series_index, y_series_index, z_series_index), file_name);
            return (null, file_name);
        }

        /// <summary>
        /// Use the nodes in gridProperty that satisfy the condition as the condition data
        /// 将gridProperty中满足条件的节点，作为条件数据
        /// </summary>
        /// <param name="gp"></param>
        /// <param name="grid_property_name"></param>
        /// <param name="compare_type"></param>
        /// <param name="compared_value"></param>
        /// <returns></returns>
        public static CData2 create_from_gridProperty(GridProperty gp, string grid_property_name, CompareType compare_type, float? compared_value)
        {
            CData2 cd = new()
            {
                property_names = [grid_property_name],
                target_gs = gp.grid_structure
            };
            if (gp.grid_structure.dim == Dimension.D2)
            {
                cd.dim = Dimension.D2;
                cd.x_series_index = 0;
                cd.y_series_index = 1;
                cd.buffer = MyDataFrame.create(["x", "y", grid_property_name]);
            }
            if (gp.grid_structure.dim == Dimension.D3)
            {
                cd.dim = Dimension.D3;
                cd.x_series_index = 0;
                cd.y_series_index = 1;
                cd.z_series_index = 2;
                cd.buffer = MyDataFrame.create(["x", "y", "z", grid_property_name]);
            }

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
