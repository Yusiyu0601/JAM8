using System.Data;
using System.Diagnostics;
using JAM8.Algorithms.Geometry;
using JAM8.Algorithms.Numerics;
using JAM8.Utilities;

namespace JAM8.SpecificApps.常用工具
{
    public partial class Form_MultiVariograms : Form
    {
        DataTable dataTable = null;

        public Form_MultiVariograms()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var dt = ExcelHelper.excel_to_dataTable(FileDialogHelper.OpenExcel());

            dataGridView1.DataSource = dt;
            dataTable = dt;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                listBox1.Items.Add($"行序:{i}");
            }
            int N = 0;
            if (EasyMath.IsOddNumber(dt.Columns.Count))
                N = dt.Columns.Count;
            else
                N++;
            numericUpDown1.Value = N;
            numericUpDown2.Value = N + 1;
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            textBox1.Clear();
            for (int i = 1; i <= numericUpDown1.Value; i++)
            {
                if (EasyMath.IsOddNumber(i))
                    textBox1.Text += $"{i},";
            }
            textBox1.Text = textBox1.Text.Trim(',');
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            textBox2.Clear();
            for (int i = 1; i <= numericUpDown2.Value; i++)
            {
                if (EasyMath.IsEvenNumber(i))
                    textBox2.Text += $"{i},";
            }
            textBox2.Text = textBox2.Text.Trim(',');
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            List<int> Xs = textBox1.Text.Split(new char[] { ',' }).Select(int.Parse).ToList();
            List<int> Ys = textBox2.Text.Split(new char[] { ',' }).Select(int.Parse).ToList();
            if (Xs.Count != Ys.Count)
                throw new Exception("X与Y数量不相同");

            if (listBox1.SelectedIndex == -1)
                return;
            int row_select = Convert.ToInt32(listBox1.SelectedItem.ToString().
                Split(new char[] { ':' }).Last());

            var str_row = dataTable.Rows[row_select];
            int N = Xs.Count;
            double[] h = new double[N];
            double[] gamma = new double[N];
            int[] w = new int[N];
            for (int i = 0; i < N; i++)
            {
                int XCol = Xs[i] - 1;
                int YCol = Ys[i] - 1;

                h[i] = Convert.ToDouble(str_row[XCol]);
                gamma[i] = Convert.ToDouble(str_row[YCol]);
                w[i] = 1;
            }
            Variogram.variogramFit_win(h, gamma, w);

        }

        private void button2_Click(object sender, EventArgs e)
        {
            List<int> Xs = textBox1.Text.Split(new char[] { ',' }).Select(int.Parse).ToList();
            List<int> Ys = textBox2.Text.Split(new char[] { ',' }).Select(int.Parse).ToList();
            if (Xs.Count != Ys.Count)
                throw new Exception("X与Y数量不相同");

            MyDataFrame df = MyDataFrame.create(new string[4] { "range", "sill", "c0", "loss" });

            Stopwatch sw = new();
            sw.Start();
            for (int k = 0; k < listBox1.Items.Count; k++)
            {
                int row_select = Convert.ToInt32(listBox1.Items[k].ToString().
                    Split(new char[] { ':' }).Last());

                var str_row = dataTable.Rows[row_select];
                int N = Xs.Count;
                double[] h = new double[N];
                double[] gamma = new double[N];
                int[] w = new int[N];
                for (int i = 0; i < N; i++)
                {
                    int XCol = Xs[i] - 1;
                    int YCol = Ys[i] - 1;

                    h[i] = Convert.ToDouble(str_row[XCol]);
                    gamma[i] = Convert.ToDouble(str_row[YCol]);
                    w[i] = 1;
                }
                //Console.WriteLine(k);

                var record = df.new_record();
                try
                {
                    var (fitted, loss) = Variogram.variogramFit(VariogramType.Spherical, h, gamma, w);
                    record["range"] = fitted.range;
                    record["sill"] = fitted.sill;
                    record["c0"] = fitted.nugget;
                    record["loss"] = loss;
                }
                catch
                {
                    record["range"] = -99.99;
                    record["sill"] = -99.99;
                    record["c0"] = -99.99;
                    record["loss"] = -99.99;
                }
                finally
                {
                    df.add_record(record);
                }
            }
            sw.Stop();
            Console.WriteLine(sw.ElapsedMilliseconds);
            df.show_win();
        }
    }
}
