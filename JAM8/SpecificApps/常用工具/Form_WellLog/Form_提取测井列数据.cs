using JAM8.Utilities;

namespace JAM8.SpecificApps.常用工具
{
    public partial class Form_提取测井列数据 : Form
    {
        Dictionary<string, string> fileNames_dic = null;

        MyDataFrame df_current = null;//当前显示的表

        public Form_提取测井列数据()
        {
            InitializeComponent();
        }

        //打开测井数据文件(txt)
        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new()
            {
                Multiselect = true,
                Filter = "txt文件|*.txt"
            };
            if (ofd.ShowDialog() != DialogResult.OK)
                return;

            listBox1.Items.Clear();
            fileNames_dic = new();
            List<string> fileNames = ofd.FileNames.ToList();
            List<string> fileNames_justName = fileNames.Select(a => FileHelper.GetFileName(a, false)).ToList();
            for (int i = 0; i < fileNames.Count; i++)
            {
                fileNames_dic.Add(fileNames_justName[i], fileNames[i]);
                listBox1.Items.Add(fileNames_justName[i]);
            }

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex == -1)
                return;
            string fileName_justName = listBox1.SelectedItem.ToString();
            df_current = MyDataFrame.read_from_txt(fileNames_dic[fileName_justName], "\t", true);
            dataGridView1.DataSource = df_current.convert_to_dataTable();

            listBox2.Items.Clear();
            foreach (var item in df_current.series_names)
            {
                listBox2.Items.Add(item);
            }
        }

        //选取
        private void button4_Click(object sender, EventArgs e)
        {
            if (listBox2.SelectedIndex != -1)
            {
                if (!listBox3.Items.Contains(listBox2.SelectedItem))
                    listBox3.Items.Add(listBox2.SelectedItem);
            }
        }

        //撤销
        private void button5_Click(object sender, EventArgs e)
        {
            if (listBox3.SelectedIndex != -1)
            {
                listBox3.Items.RemoveAt(listBox3.SelectedIndex);
            }
        }

        private void listBox1_MouseClick(object sender, MouseEventArgs e)
        {
            if (listBox1.IndexFromPoint(e.X, e.Y) == -1) listBox1.ClearSelected();
        }

        private void dataGridView1_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            //自动编号，与数据无关
            Rectangle rectangle = new(e.RowBounds.Location.X,
               e.RowBounds.Location.Y,
               dataGridView1.RowHeadersWidth - 4,
               e.RowBounds.Height);

            TextRenderer.DrawText(e.Graphics,
                  (e.RowIndex + 1).ToString(),
                   dataGridView1.RowHeadersDefaultCellStyle.Font,
                   rectangle,
                   dataGridView1.RowHeadersDefaultCellStyle.ForeColor,
                   TextFormatFlags.VerticalCenter | TextFormatFlags.Right);
        }

        //导出
        private void button3_Click(object sender, EventArgs e)
        {
            List<string> curve_names = new();
            for (int i = 0; i < listBox3.Items.Count; i++)
            {
                curve_names.Add(listBox3.Items[i].ToString());
            }
            if (checkBox1.Checked == false)//仅导出当前井
            {
                SaveFileDialog sfd = new()
                {
                    Filter = "xlsx文件|*.xlsx",
                    FileName = $"{listBox1.SelectedItem}_new",
                    InitialDirectory = Path.GetFullPath(fileNames_dic[listBox1.SelectedItem.ToString()])
                };
                if (sfd.ShowDialog() != DialogResult.OK)
                    return;
                var df = df_current.get_series_subset(curve_names);
                MyDataFrame.write_to_excel(df, sfd.FileName);
                Console.WriteLine($"保存完成({listBox1.SelectedItem})");
            }
            else
            {
                FolderBrowserDialog fbd = new()
                {
                    InitialDirectory = Path.GetFullPath(fileNames_dic[listBox1.SelectedItem.ToString()])
                };
                if (fbd.ShowDialog() != DialogResult.OK)
                    return;
                foreach (var (fileName_justName, fileName) in fileNames_dic)
                {
                    df_current = MyDataFrame.read_from_txt(fileName, "\t", true);//先读取
                    dataGridView1.DataSource = df_current.convert_to_dataTable();

                    string save_path = $"{fbd.SelectedPath}\\{fileName_justName}.xlsx";
                    var df = df_current.get_series_subset(curve_names);
                    if (df == null)
                    {
                        List<string> not_found = new(curve_names);
                        not_found.RemoveAll(a => df_current.series_names.Contains(a));
                        string tip = string.Empty;
                        for (int i = 0; i < not_found.Count; i++)
                        {
                            tip += not_found[i] + " ";
                        }
                        Console.WriteLine($"{fileName_justName}缺少列:{tip}");
                        continue;
                    }
                    MyDataFrame.write_to_excel(df, save_path);
                    Console.WriteLine($"保存完成({fileName_justName})");
                }
            }
        }

        private void Form_WellLog_txt2excel_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {
            string embeddedFilePath = "JAM8.资源文件.[实例数据]测井曲线_板3.txt";
            string txt = MyEmbeddedFileHelper.read_embedded_txt(embeddedFilePath);
            Form_showTxt frm = new(txt);
            frm.Show();
        }
    }
}

