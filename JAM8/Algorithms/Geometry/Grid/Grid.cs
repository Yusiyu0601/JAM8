using System.Text;
using JAM8.Algorithms.Forms;
using JAM8.Utilities;
using LiteDB;

namespace JAM8.Algorithms.Geometry
{
    /// <summary>
    /// Grid Class
    /// </summary>
    public class Grid : Dictionary<string, GridProperty>
    {
        private Grid()
        {
        }

        #region attribute 属性

        public string grid_name { get; set; } = "grid_name";

        public GridStructure gridStructure { get; internal set; }

        public List<string> propertyNames
        {
            get { return Keys.ToList(); }
        }

        /// <summary>
        /// 索引器，范围为[0,N-1]
        /// </summary>
        /// <param name="idx">propertyName索引</param>
        /// <returns></returns>
        public GridProperty this[int idx]
        {
            get
            {
                if (idx >= 0 && idx < Count)
                    return this[propertyNames[idx]];
                else
                    return null;
            }
        }

        public object tag { get; set; }

        /// <summary>
        /// Number of GridProperties
        /// </summary>
        public int N_gridProperties
        {
            get { return Keys.Count; }
        }

        #endregion

        #region instance function 实例函数

        #region add_gridProperty & delete_gridProperty & replace_propertyName

        /// <summary>
        /// 修改属性名称
        /// </summary>
        /// <param name="oldName"></param>
        /// <param name="newName"></param>
        public void replace_propertyName(string oldName, string newName)
        {
            if (ContainsKey(oldName)) //存在旧名称的属性
            {
                if (!ContainsKey(newName)) //不存在新名称的属性
                {
                    var gp = base[oldName];
                    Remove(oldName);
                    Add(newName, gp);
                }
            }
        }

        /// <summary>
        /// 增加GridProperty，如果已经存在propertyName，则不会添加，也不返回提示
        /// </summary>
        /// <param name="propertyName"></param>
        public void add_gridProperty(string propertyName, GridProperty gp = null)
        {
            //如果没有以propertyName为名的属性，则增加propertyName，并扩充Buffer
            if (!ContainsKey(propertyName))
            {
                if (gp == null) //如果没有提供了属性，则新建一个属性
                    Add(propertyName, GridProperty.create(gridStructure));
                else //更新gp
                {
                    if (gp.grid_structure == gridStructure)
                        this[propertyName] = gp;
                    else
                        Console.WriteLine(GridStructure.Exception_NotEquals);
                }
            }
        }

        /// <summary>
        /// 删除Property
        /// </summary>
        /// <param name="PropertyName"></param>
        public void delete_gridProperty(string propertyName)
        {
            if (ContainsKey(propertyName))
                Remove(propertyName);
        }

        /// <summary>
        /// 返回第1个gridProperty
        /// </summary>
        /// <returns></returns>
        public GridProperty first_gridProperty()
        {
            return this[0];
        }

        /// <summary>
        /// 返回最后1个gridProperty
        /// </summary>
        /// <returns></returns>
        public GridProperty last_gridProperty()
        {
            return this[N_gridProperties - 1];
        }

        #endregion

        #region get_value & set_value

        /// <summary>
        /// 根据arrayIndex获取指定propertyName的网格单元值
        /// </summary>
        /// <param name="arrayIndex">取值范围[1,gs.N]</param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public float? get_value(int arrayIndex, string propertyName)
        {
            return this[propertyName].get_value(arrayIndex);
        }

        /// <summary>
        /// 根据spatialIndex获取指定propertyName的网格单元值
        /// </summary>
        /// <param name="si">ix iy iz取值从1开始</param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public float? get_value(SpatialIndex si, string propertyName)
        {
            return this[propertyName].get_value(si);
        }

        /// <summary>
        /// 根据arrayIndex给指定propertyName的网格单元值赋值
        /// </summary>
        /// <param name="arrayIndex">取值范围[1,gs.N]</param>
        /// <param name="propertyName"></param>
        /// <param name="value"></param>
        public void set_value(int arrayIndex, string propertyName, float? value)
        {
            this[propertyName].set_value(arrayIndex, value);
        }

        /// <summary>
        /// 根据spatialIndex给指定propertyName的网格单元值赋值
        /// </summary>
        /// <param name="si">ix iy iz取值从1开始</param>
        /// <param name="propertyName"></param>
        /// <param name="value"></param>
        public void set_value(SpatialIndex si, string propertyName, float? value)
        {
            this[propertyName].set_value(si, value);
        }

        #endregion

        #region other functions

        /// <summary>
        /// 计算所有GridProperty的点对点平均数
        /// </summary>
        /// <returns></returns>
        public GridProperty get_EType()
        {
            var result = GridProperty.create(gridStructure);
            for (int n = 0; n < gridStructure.N; n++) //逐网格单元计算
            {
                float? sum = 0.0f;
                for (int m = 0; m < N_gridProperties; m++)
                    sum += this[m].get_value(n);
                result.set_value(n, sum /= N_gridProperties);
            }

            return result;
        }

        /// <summary>
        /// 打印
        /// </summary>
        /// <returns></returns>
        public string view_text()
        {
            string str = $"\n * * * Name = {grid_name}\tN_gridProperties = {N_gridProperties} * * *\n";
            str += gridStructure.to_string();
            for (int i = 0; i < N_gridProperties; i++)
                str += $"\n\t{i + 1} [{propertyNames[i]}]\n";
            str += $"\n *     *     *     *     *     *     *\n";
            return str;
        }

        #endregion

        #region from_gslib & to_gslib

        /// <summary>
        /// 从GSLIB里读取Grid的快速方法
        /// </summary>
        /// <param name="file_name"></param>
        /// <param name="split_code"></param>
        /// <param name="null_value"></param>
        /// <param name="grid_name"></param>
        public void read_from_gslib(string file_name, int split_code, float null_value, string grid_name = null)
        {
            // 输出空行，便于视觉分隔
            Console.WriteLine();

            string[] split_strs = new Dictionary<int, string[]>
            {
                { 0, ["\t"] },
                { 1, [" "] },
                { 2, [";"] },
                { 3, [","] }
            }.GetValueOrDefault(split_code, null);

            // 获取文件总字节数
            long totalFileSize = new FileInfo(file_name).Length;

            const int bufferSize = 8192; // 8KB的缓冲区大小
            byte[] buffer = new byte[bufferSize];
            long bytesRead = 0;

            string remainingBuffer = ""; // 用于保存跨块数据（如果有）
            int flag = -1; //读取第几行
            int NGridProperty = 0; //网格属性数量
            List<string> propertyNames = [];

            using (FileStream fileStream = new(file_name, FileMode.Open, FileAccess.Read))
            using (BinaryReader reader = new(fileStream))
            {
                while (reader.BaseStream.Position < reader.BaseStream.Length)
                {
                    int bytesInCurrentRead = reader.Read(buffer, 0, bufferSize); // 读取数据
                    bytesRead += bytesInCurrentRead;

                    // 将上次残余的部分与当前读取的数据拼接
                    string data = remainingBuffer + Encoding.UTF8.GetString(buffer, 0, bytesInCurrentRead);

                    int start = 0;
                    int end;

                    // 逐行解析数据
                    while ((end = data.IndexOf('\n', start)) != -1)
                    {
                        string line = data[start..end]; // 提取当前行
                        start = end + 1; // 更新 start，跳过换行符

                        // 去除 '\r' 回车符（如果有）
                        if (!string.IsNullOrEmpty(line) && line[^1] == '\r')
                        {
                            line = line[..^1];
                        }

                        flag++;

                        // 第1行是Grid名称
                        if (flag == 0)
                        {
                            string s = line.TrimEnd(['\r']); //Grid名称
                            //如果grid_name为空，则使用GSLIB文件的第1行作为名称
                            if (grid_name == null)
                                this.grid_name = s.Split(['{', '('])[0];
                        }

                        // 第2行是网格属性数量
                        if (flag == 1)
                        {
                            NGridProperty = int.Parse(line); // 转换为整数
                        }

                        // 根据网格属性名称添加到grid里
                        if (flag > 1 && flag <= NGridProperty + 1)
                        {
                            add_gridProperty(line);
                            propertyNames.Add(line);
                        }

                        // 处理网格数据
                        if (flag > NGridProperty + 1)
                        {
                            var values = line.Split(split_strs, StringSplitOptions.RemoveEmptyEntries);
                            for (int i = 0; i < values.Length; i++)
                            {
                                if (float.TryParse(values[i], out float value) && value != null_value)
                                {
                                    string propertyName = propertyNames[i];
                                    set_value(flag - NGridProperty - 2, propertyName, value);
                                }
                            }
                        }
                    }

                    // 将剩余部分保留，等待下一次读取
                    remainingBuffer = data[start..];

                    // 计算并输出进度
                    MyConsoleProgress.Print(bytesRead, totalFileSize, "Load Grid（Gslib format）");
                }
            }

            // 处理剩余的部分（如果有）
            if (!string.IsNullOrEmpty(remainingBuffer))
            {
                Console.WriteLine(remainingBuffer);
            }
        }

        /// <summary>
        /// 输出Grid到GSLIB
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="nullValue"></param>
        public void save_to_gslib(string fileName, string gridName, float nullValue)
        {
            Console.Write($"\n写入GSLIB文件路径: {fileName}\n"); //打印文件路径
            using StreamWriter sw = new(fileName);
            string gridSize = gridStructure.to_string().Trim('\n').Trim('\t');
            sw.WriteLine($"{gridName} = {gridSize}"); //输出GSLIB数据的标题
            sw.WriteLine(N_gridProperties); //输出变量数目
            for (int i = 0; i < N_gridProperties; i++)
            {
                sw.WriteLine(propertyNames[i]); //输出属性名称
            }

            for (int n = 0; n < gridStructure.N; n++) //逐行输出数据
            {
                MyConsoleProgress.Print(n, gridStructure.N - 1, "输出数据:Grid => GSLIB");
                string line_str = string.Empty;
                for (int col = 0; col < N_gridProperties; col++) //逐列输出数据
                {
                    string temp = string.Empty;
                    float? value = get_value(n, propertyNames[col]);
                    temp = value == null ? nullValue.ToString("E3") : value.Value.ToString("E3");
                    line_str += temp;
                    if (col < N_gridProperties - 1)
                        line_str += " ";
                }

                sw.WriteLine(line_str);
            }
        }

        #endregion

        /// <summary>
        /// 窗体模式绘制Grid
        /// </summary>
        public void showGrid_win(string title = null)
        {
            try
            {
                Thread mThread = new(delegate()
                {
                    Form_Scottplot4Grid frm = new(this, title);
                    frm.ShowDialog();//这里存在一个bug
                });
                mThread.SetApartmentState(ApartmentState.STA);
                mThread.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        /// <summary>
        /// 窗体模式选择gridProperty
        /// </summary>
        public (GridProperty grid_property, string property_name) select_gridProperty_win(string title = null)
        {
            Form_SelectGridPropertyFromGrid frm = new(this, title);
            if (frm.ShowDialog() != DialogResult.OK)
                return (null, null);
            string gp_name = frm.listBox1.SelectedItem.ToString();
            GridProperty gp = this[gp_name];
            return (gp, gp_name);
        }

        #endregion

        #region static function 静态函数

        /// <summary>
        /// 创建Grid
        /// </summary>
        /// <param name="gs"></param>
        /// <param name="gridName"></param>
        /// <returns></returns>
        public static Grid create(GridStructure gs, string grid_name = null)
        {
            Grid g = new()
            {
                gridStructure = gs,
                grid_name = grid_name ?? "grid_name"
            };
            return g;
        }

        /// <summary>
        /// Create the Grid and initialize it with property_names 创建Grid，并使用property_names初始化
        /// </summary>
        /// <param name="gs"></param>
        /// <param name="property_names"></param>
        /// <param name="grid_name"></param>
        /// <returns></returns>
        public static Grid create(GridStructure gs, IEnumerable<string> property_names, string grid_name)
        {
            Grid g = new()
            {
                gridStructure = gs,
                grid_name = grid_name ?? "grid_name"
            };
            foreach (var property_name in property_names)
            {
                g.add_gridProperty(property_name, null);
            }

            return g;
        }

        /// <summary>
        /// Read Grid from GSLIB 从GSLIB里读取Grid
        /// </summary>
        public static (Grid grid, string fileName) create_from_gslibwin(string title = null)
        {
            Form_ReadGridFromGSLIB frm = new(title);
            if (frm.ShowDialog() != DialogResult.OK)
                return (null, "");
            var paras = frm.paras;
            string fileName = paras[0];
            string gridName = paras[1];
            float nullValue = float.Parse(paras[2]);
            int nx = int.Parse(paras[3]);
            int ny = int.Parse(paras[4]);
            int nz = int.Parse(paras[5]);
            float xsiz = float.Parse(paras[6]);
            float ysiz = float.Parse(paras[7]);
            float zsiz = float.Parse(paras[8]);
            float xmn = float.Parse(paras[9]);
            float ymn = float.Parse(paras[10]);
            float zmn = float.Parse(paras[11]);

            //根据快速窗体的信息读取Grid数据
            GridStructure gs = GridStructure.create(nx, ny, nz, xsiz, ysiz, zsiz, xmn, ymn, zmn);
            Grid g = create(gs, gridName);
            g.read_from_gslib(fileName, 1, nullValue, gridName);
            return new(g, fileName);
        }

        /// <summary>
        /// 输出Grid到GSLIB
        /// </summary>
        public static void save_to_gslibwin(Grid g)
        {
            Form_WriteGridToGSLIB frm = new(g);
            if (frm.ShowDialog() != DialogResult.OK)
                return;
            var paras = frm.paras;

            string fileName = paras[0];
            string gridName = paras[1];
            float nullValue = float.Parse(paras[2]);

            //根据快速窗体的信息读取Grid数据
            g.save_to_gslib(fileName, gridName, nullValue);
        }

        #endregion
    }
}