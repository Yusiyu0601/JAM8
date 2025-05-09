namespace JAM8.SpecificApps.常用工具
{
    public partial class Form_Color2Code : Form
    {
        public delegate void delegate_OK(string GridName, Dictionary<string, double> Color2Code);
        public event delegate_OK event_OK;

        public Dictionary<string, double> Color2Code = null;
        private Bitmap m_image = null;

        public Form_Color2Code(string GridName, Bitmap image)
        {
            InitializeComponent();
            m_image = image;
            SetColor2CodeDic();

            txt_ICount.Text = image.Width.ToString();
            txt_JCount.Text = image.Height.ToString();
            txt_GridName.Text = GridName;
        }

        private void SetColor2CodeDic()
        {
            Color2Code = new Dictionary<string, double>();
            //统计图像的像素颜色值数量
            for (int y = 0; y < m_image.Height; y++)
            {
                for (int x = 0; x < m_image.Width; x++)
                {
                    Color color = m_image.GetPixel(x, y);
                    string colorString = ColorTranslator.ToHtml(color);
                    if (!Color2Code.ContainsKey(colorString))
                    {
                        Color2Code.Add(colorString, -1);
                    }
                }
            }

            int i = -1;
            foreach (var color in Color2Code.Keys)
            {
                i++;

                Label label = new Label();
                label.Name = "label" + i;
                label.Size = new System.Drawing.Size(50, 50);
                label.Location = new System.Drawing.Point(10, 70 * i + 10);
                label.BackColor = ColorTranslator.FromHtml(color);
                label.BorderStyle = BorderStyle.FixedSingle;
                panel1.Controls.Add(label);

                TextBox textBox = new TextBox();
                textBox.Name = color;
                textBox.Size = new Size(50, 50);
                textBox.Location = new System.Drawing.Point(70, 70 * i + 25);
                panel1.Controls.Add(textBox);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                //修改Color2Code中Code值
                foreach (System.Windows.Forms.Control control in panel1.Controls)
                {
                    if (control.GetType() == typeof(TextBox))
                    {
                        var textBox = control as TextBox;
                        Color2Code[textBox.Name] = Convert.ToDouble(textBox.Text);
                    }
                }
                event_OK(txt_GridName.Text, Color2Code);
                this.DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }
    }
}
