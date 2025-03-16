using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JAM8.Utilities;

namespace JAM8.Algorithms.Geometry
{
    /// <summary>
    /// 条件数据类，采用DataFrame作为条件数据的数据缓存区
    /// </summary>
    public class CData2
    {
        private CData2() { }

        public Dimension dim { get; internal set; }//维度

        public string[] property_names { get; internal set; }//属性名称

        MyDataFrame buffer;//条件数据的数据缓存区

        int x_series_index { get; set; }//x坐标在数据缓存区里的序号，从0开始
        int y_series_index { get; set; }//y坐标在数据缓存区里的序号，从0开始
        int z_series_index { get; set; }//z坐标在数据缓存区里的序号，从0开始
        float null_value { get; set; }//null value
        bool null_value_enabled { get; set; }//设置是否启用null value

        private GridStructure target_gs { get; set; }//目标网格结构，如果不为空，则表示条件数据已经粗化到目标网格结构上

        /// <summary>
        /// 从gslib里读取CData
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

            //将cdata.buffer里所有除了x_series_index y_series_index z_series_index意外的属性名称提取出来
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
        /// 从gslib里读取CData（窗体模式）
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
        /// 根据条件数据的序号和属性名称，获取条件数据的数值
        /// </summary>
        /// <param name="idx">条件数据的序号，从0开始</param>
        /// <param name="property_name">属性名称，从0开始</param>
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
        /// 根据条件数据的序号和属性排序，获取条件数据的数值
        /// </summary>
        /// <param name="idx">条件数据的序号，从0开始</param>
        /// <param name="property_idx">属性排序，从0开始</param>
        /// <returns></returns>
        public float? this[int idx, int property_idx]
        {
            get
            {
                return this[idx, property_names[property_idx]];
            }
        }

        /// <summary>
        /// 将条件数据粗化到目标网格结构
        /// 目标即实现条件数据调整至目标网格结构的网格单元上
        /// </summary>
        /// <param name="target_gs"></param>
        /// <returns></returns>
        public (CData2 cd, Grid g) coarsened(GridStructure target_gs)
        {
            CData2 coarsened = new()
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
                    si = target_gs.coord_to_spatialIndex(coord);
                    if (si != null)//保留落在grid范围内的cdi
                    {
                        MyRecord record = buffer.get_record(idx_record);//从原始表里提取记录，然后修改
                        record[buffer.series_names[x_series_index]] = si.ix;
                        record[buffer.series_names[y_series_index]] = si.iy;
                        coarsened.buffer.add_record(record);
                    }
                }
                if (dim == Dimension.D3)
                {
                    float x = Convert.ToSingle(buffer[idx_record, x_series_index]);
                    float y = Convert.ToSingle(buffer[idx_record, y_series_index]);
                    float z = Convert.ToSingle(buffer[idx_record, z_series_index]);
                    coord = Coord.create(x, y, z);
                    si = target_gs.coord_to_spatialIndex(coord);
                    if (si != null)//保留落在grid范围内的cdi
                    {
                        MyRecord record = buffer.get_record(idx_record);//从原始表里提取记录，然后修改
                        record[buffer.series_names[x_series_index]] = si.ix;
                        record[buffer.series_names[y_series_index]] = si.iy;
                        record[buffer.series_names[z_series_index]] = si.iz;
                        coarsened.buffer.add_record(record);
                    }
                }
            }

            Grid g = Grid.create(target_gs, "coarsened");
            foreach (var property_name in coarsened.property_names)
            {
                g.add_gridProperty(property_name);
            }

            return (coarsened, g);
        }
    }
}
