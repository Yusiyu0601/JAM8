using System.Data;
using JAM8.Algorithms.Forms;
using JAM8.Utilities;

namespace JAM8.Algorithms.Geometry
{
    /// <summary>
    /// condition data
    /// </summary>
    public class CData : List<CDataItem>
    {
        private CData() { }

        /// <summary>
        /// 数量
        /// </summary>
        public int N
        {
            get
            {
                return Count;
            }
        }

        /// <summary>
        /// 维度
        /// </summary>
        public Dimension dim
        {
            get
            {
                return this[0].dim;
            }
        }

        /// <summary>
        /// 获取某属性[非空值]的统计量，包括最大值、最小值、平均数
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public (float max, float min, float mean, int count) get_statistics(string propertyName)
        {
            List<float> tmp_list = new();
            for (int n = 0; n < N; n++)
            {
                float? value = this[n][propertyName];
                if (value != null)
                    tmp_list.Add(value.Value);
            }
            return (tmp_list.Max(), tmp_list.Min(), tmp_list.Average(), tmp_list.Count);
        }

        /// <summary>
        /// 获取数据点集合的外边界
        /// </summary>
        /// <returns></returns>
        public (float min_x, float max_x, float min_y, float max_y, float? min_z, float? max_z) get_boundary()
        {
            float min_x = 0, max_x = 0, min_y = 0, max_y = 0;
            float? min_z = null, max_z = null;

            min_x = this.Min(a => a.coord.x);
            max_x = this.Max(a => a.coord.x);
            min_y = this.Min(a => a.coord.y);
            max_y = this.Max(a => a.coord.y);
            if (dim == Dimension.D3)
            {
                min_z = this.Min(a => a.coord.z);
                max_z = this.Max(a => a.coord.z);
            }
            return (min_x, max_x, min_y, max_y, min_z, max_z);
        }

        /// <summary>
        /// 属性名称
        /// </summary>
        public List<string> propertyNames { get; internal set; }

        /// <summary>
        /// 打印
        /// </summary>
        /// <returns></returns>
        public string view_text(int N_first_lines = 15)
        {
            var df = to_dataFrame();
            return df.view_text(N_first_lines);
        }

        /// <summary>
        /// 将CData赋值于指定GridStructure(注意：可能出现多个cdi位于同一个网格节点中)，
        /// 所有CDataItem的Coord调整到指定GridStructure框架中，越界的cdi，直接删除。
        /// </summary>
        /// <param name="gs"></param>
        /// <returns></returns>
        public (Grid grid_assigned, CData cd_assigned) assign_to_grid(GridStructure gs, string grid_name = "cdata")
        {
            Grid grid_assigned = Grid.create(gs, grid_name);//根据输入gs创建grid
            CData cd_assigned = new()//创建新的cd
            {
                propertyNames = []
            };
            for (int i = 0; i < propertyNames.Count; i++)
                cd_assigned.propertyNames.Add(propertyNames[i]);

            foreach (var propertyName in propertyNames)
            {
                GridProperty gp = GridProperty.create(gs);
                for (int i = 0; i < N; i++)//将条件数据赋值于网格
                {
                    SpatialIndex si = gs.coord_to_spatialIndex(this[i].coord);//坐标—>索引
                    if (si != null)
                        gp.set_value(si, this[i][propertyName]);
                }
                grid_assigned.add_gridProperty(propertyName, gp);
            }
            foreach (var cdi in this)
            {
                var si = gs.coord_to_spatialIndex(cdi.coord);
                if (si != null)//保留在grid范围内的cdi
                {
                    var cdi_adjust = cdi.deep_clone();
                    cdi_adjust.coord = gs.spatialIndex_to_coord(si);
                    cd_assigned.Add(cdi_adjust);
                }
            }

            return (grid_assigned, cd_assigned);
        }

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
        public static CData read_from_gslib(string fileName, Dimension dim,
            int col_x, int col_y, int col_z,
            bool enable_nullValue, float nullValue)
        {
            MyDataFrame df = GSLIB.gslib_to_df(fileName);
            CData cd = new()
            {
                propertyNames = new()
            };
            //提取属性名列表
            for (int iCol = 0; iCol < df.N_Series; iCol++)
            {
                if (dim == Dimension.D2 && iCol != col_x - 1 && iCol != col_y - 1)
                    cd.propertyNames.Add(df.series_names[iCol]);
                if (dim == Dimension.D3 && iCol != col_x - 1 && iCol != col_y - 1 && iCol != col_z - 1)
                    cd.propertyNames.Add(df.series_names[iCol]);
            }
            //根据属性名列表提取行数据
            for (int iRow = 0; iRow < df.N_Record; iRow++)
            {
                float x, y, z;
                Coord c;
                Dictionary<string, float?> values;
                if (dim == Dimension.D2)
                {
                    x = Convert.ToSingle(df[iRow, col_x - 1]);
                    y = Convert.ToSingle(df[iRow, col_y - 1]);
                    c = Coord.create(x, y);
                }
                else
                {
                    x = Convert.ToSingle(df[iRow, col_x - 1]);
                    y = Convert.ToSingle(df[iRow, col_y - 1]);
                    z = Convert.ToSingle(df[iRow, col_z - 1]);
                    c = Coord.create(x, y, z);
                }
                values = new();
                foreach (var propertyName in cd.propertyNames)
                {
                    float? value = Convert.ToSingle(df[iRow, propertyName]);
                    if (enable_nullValue && nullValue == value)
                        value = null;
                    values.Add(propertyName, value);
                }
                cd.Add(CDataItem.create(c, values));
            }

            return cd;
        }

        /// <summary>
        /// 从gslib里读取CData
        /// </summary>
        /// <returns></returns>
        public static (CData cdata, string file_name) read_from_gslibwin(string title = null)
        {
            Form_ReadConditionData frm = new(title);
            if (frm.ShowDialog() != DialogResult.OK)
                return (null, null);
            var paras = frm.paras;

            var fileName = paras[0];
            var dim = (Dimension)Enum.Parse(typeof(Dimension), paras[1]);
            int col_x = int.Parse(paras[2]);
            int col_y = int.Parse(paras[3]);
            int col_z = int.Parse(paras[4]);
            bool enable_nullValue = bool.Parse(paras[5]);
            float nullValue = float.Parse(paras[6]);

            return (read_from_gslib(fileName, dim, col_x, col_y, col_z, enable_nullValue, nullValue), fileName);
        }

        /// <summary>
        /// 将gridProperty中满足条件的节点，提取出来作为cdi
        /// </summary>
        /// <param name="g"></param>
        /// <param name="property_name"></param>
        /// <param name="value"></param>
        /// <param name="equal_or_exclude">为true，则保留等于value的节点;为false，则排除等于value的节点</param>
        /// <returns></returns>
        public static CData create_from_gridProperty(Grid g, string property_name, float? value, bool equal_or_exclude)
        {
            CData cd = new()
            {
                propertyNames = []
            };
            cd.propertyNames.Add(property_name);
            for (int n = 0; n < g.gridStructure.N; n++)
            {
                Coord c = g.gridStructure.arrayIndex_to_coord(n);
                float? value1 = g[property_name].get_value(n);
                if (equal_or_exclude)//为true，则保留等于value的节点
                {
                    if (value1 == value)
                    {
                        Dictionary<string, float?> values = new()
                        {
                            { property_name, value1 }
                        };
                        CDataItem cdi = CDataItem.create(c, values);
                        cd.Add(cdi);
                    }
                }
                else//为false，则排除等于value的节点
                {
                    if (value1 != value)
                    {
                        Dictionary<string, float?> values = new()
                        {
                            { property_name, value1 }
                        };
                        CDataItem cdi = CDataItem.create(c, values);
                        cd.Add(cdi);
                    }
                }

            }
            return cd;
        }

        /// <summary>
        /// 将gridProperty中满足条件的节点，提取出来作为cdi,属性名称默认为"cd"
        /// </summary>
        /// <param name="gp"></param>
        /// <param name="value"></param>
        /// <param name="equal_or_exclude">为true，则保留等于value的节点;为false，则排除等于value的节点</param>
        /// <returns></returns>
        public static CData create_from_gridProperty(GridProperty gp, float? value, bool equal_or_exclude)
        {
            string property_name = "cd";
            CData cd = new()
            {
                propertyNames = []
            };
            cd.propertyNames.Add(property_name);
            for (int n = 0; n < gp.gridStructure.N; n++)
            {
                Coord c = gp.gridStructure.arrayIndex_to_coord(n);
                float? value1 = gp.get_value(n);
                if (equal_or_exclude)//为true，则保留等于value的节点
                {
                    if (value1 == value)
                    {
                        Dictionary<string, float?> values = new()
                        {
                            { property_name, value1 }
                        };
                        CDataItem cdi = CDataItem.create(c, values);
                        cd.Add(cdi);
                    }
                }
                else//为false，则排除等于value的节点
                {
                    if (value1 != value)
                    {
                        Dictionary<string, float?> values = new()
                        {
                            { property_name, value1 }
                        };
                        CDataItem cdi = CDataItem.create(c, values);
                        cd.Add(cdi);
                    }
                }

            }
            return cd;
        }

        /// <summary>
        /// 保存至gslib
        /// </summary>
        /// <param name="fileName"></param>
        public void save_to_gslib(string fileName, float null_value)
        {
            GSLIB.df_to_gslib(fileName, null_value, to_dataFrame());
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
            var null_value = float.Parse(paras[1]);//null值
            save_to_gslib(fileName, null_value);
        }

        /// <summary>
        /// 转换为MyDataFrame
        /// </summary>
        /// <returns></returns>
        public MyDataFrame to_dataFrame()
        {
            List<string> series_names = new();
            if (dim == Dimension.D2)
                series_names.AddRange(new List<string>() { "X", "Y" });
            if (dim == Dimension.D3)
                series_names.AddRange(new List<string>() { "X", "Y", "Z" });
            series_names.AddRange(propertyNames);
            var df = MyDataFrame.create(series_names.ToArray());
            foreach (var cdi in this)
            {
                var record = df.new_record();
                record["X"] = cdi.coord.x;
                record["Y"] = cdi.coord.y;
                if (dim == Dimension.D3)
                    record["Z"] = cdi.coord.z;
                foreach (var (key, value) in cdi)
                    record[key] = value;
                df.add_record(record);
            }
            return df;
        }

        /// <summary>
        /// 计算与Coord的距离，返回结果距离由小到大
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public List<(double distance, CDataItem cdi)> order_by_distance(Coord c)
        {
            List<(double distance, CDataItem)> ordered = new();
            foreach (var item in this)
                ordered.Add((Coord.get_distance(item.coord, c), item));
            var result = ordered.OrderBy(a => a.distance).ToList();
            return result;
        }

        /// <summary>
        /// 深度复制
        /// </summary>
        /// <returns></returns>
        public CData deep_clone()
        {
            CData cd = new()
            {
                propertyNames = new()
            };

            for (int i = 0; i < propertyNames.Count; i++)
                cd.propertyNames.Add(propertyNames[i]);
            for (int i = 0; i < N; i++)
                cd.Add(this[i].deep_clone());

            return cd;
        }

        /// <summary>
        /// 窗体模式选择cd_propertyName
        /// </summary>
        public string select_cd_propertyName_win(string title = null)
        {
            Form_SelectPropertyFromCData frm = new(this, title);
            if (frm.ShowDialog() != DialogResult.OK)
                return null;
            return frm.selected_property_name;
        }
    }
}
