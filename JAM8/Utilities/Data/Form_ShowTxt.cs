using System.Text;

namespace JAM8.Utilities
{
    public partial class Form_showTxt : Form
    {
        public Form_showTxt(string Txt, string title = null)
        {
            InitializeComponent();
            this.textBox1.Text = Txt;
            this.Text = title;
        }

        private void 保存ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string file_name = FileDialogHelper.SaveText();
            using FileStream fileStream = new(file_name, FileMode.OpenOrCreate);
            using StreamWriter streamWriter = new(fileStream, Encoding.Default);
            streamWriter.Write(this.textBox1.Text);
            streamWriter.Flush();
        }
    }
}
