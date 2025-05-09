using System.Data;
using JAM8.Utilities;

namespace JAM8.SpecificApps.常用工具
{
    public partial class Form_层位测井均值 : Form
    {
        private Dictionary<string, string> log_fileName_dic = null;//测井数据文件集
        private List<string> log_names = null;//测井曲线属性名称集

        private MyDataFrame df_层位数据 = null;
        public Form_层位测井均值()
        {
            InitializeComponent();
        }

        //打开层位数据文件
        private void button1_Click(object sender, EventArgs e)
        {
            DataTable dt = ExcelHelper.excel_to_dataTable(FileDialogHelper.OpenExcel());
            dataGridView1.DataSource = dt;
            df_层位数据 = MyDataFrame.create_from_datatable(dt);

            comboBox1.Items.Clear();
            comboBox2.Items.Clear();
            comboBox3.Items.Clear();
            for (int i = 0; i < df_层位数据.series_names.Length; i++)
            {
                comboBox1.Items.Add(df_层位数据.series_names[i]);
                comboBox2.Items.Add(df_层位数据.series_names[i]);
                comboBox3.Items.Add(df_层位数据.series_names[i]);
            }
            comboBox1.SelectedIndex = 0;
            comboBox2.SelectedIndex = 0;
            comboBox3.SelectedIndex = 0;
        }

        //打开测井文件
        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new()
            {
                Multiselect = true,
                Filter = "xlsx文件|*.xlsx"
            };
            if (ofd.ShowDialog() != DialogResult.OK)
                return;

            listBox1.Items.Clear();
            log_fileName_dic = new();
            List<string> fileNames = ofd.FileNames.ToList();
            List<string> fileNames_justName = fileNames.Select(a => FileHelper.GetFileName(a, false)).ToList();
            for (int i = 0; i < fileNames.Count; i++)
            {
                log_fileName_dic.Add(fileNames_justName[i], fileNames[i]);
                listBox1.Items.Add(fileNames_justName[i]);
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex == -1)
                return;
            string fileName_justName = listBox1.SelectedItem.ToString();
            var df_current = MyDataFrame.read_from_excel(log_fileName_dic[fileName_justName]);
            dataGridView2.DataSource = df_current.convert_to_dataTable();

            listBox2.Items.Clear();
            log_names = new();
            foreach (var item in df_current.series_names)
            {
                log_names.Add(item);
                comboBox4.Items.Add(item);
                listBox2.Items.Add(item);
            }
            comboBox4.SelectedIndex = 0;
        }

        //计算层位测井均值
        private void button3_Click(object sender, EventArgs e)
        {
            //读取所有测井文件
            Dictionary<string, MyDataFrame> dfs_log = new();
            int flag = 0;
            foreach (var (log_fileName_justName, log_fileName) in log_fileName_dic)
            {
                MyConsoleProgress.Print(flag++, log_fileName_dic.Count, "读取所有测井文件");
                if (log_fileName.Contains("xlsx"))//仅处理xlsx文件
                    dfs_log.Add(log_fileName_justName, MyDataFrame.read_from_excel(log_fileName));
            }

            string str_depth_top = comboBox1.SelectedItem.ToString();//顶深的列名
            string str_depth_bottom = comboBox2.SelectedItem.ToString();//底深的列名
            string str_well_name = comboBox3.SelectedItem.ToString();//井名的列名

            if (log_names == null)
            {
                MessageBox.Show("选择测井文件，显示测井属性集");
                return;
            }

            //添加测井曲线
            List<string> series_names = new() { str_well_name, str_depth_bottom, str_depth_top };
            log_names = log_names.Take(new Range(1, ^0)).ToList();//排除掉depth列
            series_names.AddRange(log_names);
            //创建一个新DataFrame，包括井名列、深度列、测井曲线列
            MyDataFrame df_result = MyDataFrame.create(series_names.ToArray());

            for (int i = 0; i < df_层位数据.N_Record; i++)//逐行解释数据
            {
                MyConsoleProgress.Print(i, df_层位数据.N_Record, "计算层位测井均值");

                string str_top = df_层位数据[i, str_depth_top].ToString().Trim();
                string str_bottom = df_层位数据[i, str_depth_bottom].ToString().Trim();
                double depth_top = double.Parse(str_top);//获取顶深
                double depth_bottom = double.Parse(str_bottom);//获取底深
                string well_name = df_层位数据[i, str_well_name].ToString();//获取井名

                var record = df_result.new_record();//结果表创建新记录
                //将原始层位数据复制到新表记录中
                record[str_well_name] = df_层位数据[i, str_well_name];
                record[str_depth_top] = df_层位数据[i, str_depth_top];
                record[str_depth_bottom] = df_层位数据[i, str_depth_bottom];

                if (!log_fileName_dic.ContainsKey(well_name))
                {
                    Console.WriteLine($@"{well_name}文件不存在");
                    df_result.add_record(record);
                    continue;
                }

                var df_log = dfs_log[well_name];//根据井名得到测井数据表
                Dictionary<string, double> sum_log_深度段内 = new();
                foreach (var log_name in log_names)
                    sum_log_深度段内.Add(log_name, 0);//初始化
                int count_samples = 0;//深度段内的样本数量
                //从测井曲线表里提取指定深度段的测井数据，并求取平均值
                for (int j = 0; j < df_log.N_Record; j++)
                {
                    double depth = double.Parse(df_log[j, comboBox4.Text].ToString());
                    double tolerance_thickness = 0.125;//厚度容差
                    if (depth > depth_top - tolerance_thickness && depth < depth_bottom + tolerance_thickness)
                    {
                        count_samples += 1;//样本数累加
                        foreach (var log_name in log_names)
                            sum_log_深度段内[log_name] += double.Parse(df_log[j, log_name].ToString());//属性值累加
                    }
                }

                if (count_samples > 0)//计算平均值作为该段曲线的数值
                {
                    foreach (var log_name in log_names)
                    {
                        double mean = sum_log_深度段内[log_name] / count_samples;
                        record[log_name] = Math.Round(mean, 2);
                    }
                }
                else//实在查找不到，就用-9999
                {
                    foreach (var log_name in log_names)
                        record[log_name] = -9999;
                }

                df_result.add_record(record);
            }
            df_result.show_win();
        }

        private void label1_Click(object sender, EventArgs e)
        {
            string embeddedFilePath = "JAM8.资源文件.[实例数据]带层位的试油数据.xlsx";
            MyEmbeddedFileHelper.read_embedded_excel(embeddedFilePath).show_win();
        }

        private void label2_Click(object sender, EventArgs e)
        {
            string embeddedFilePath = "JAM8.资源文件.[实例数据]测井曲线_庄389.xlsx";
            MyEmbeddedFileHelper.read_embedded_excel(embeddedFilePath).show_win();
        }
    }
}
