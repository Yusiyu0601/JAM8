using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using JAM8.Algorithms.Images;

namespace JAM8.SpecificApps.常用工具
{
    public partial class Form_Image2Binary : Form
    {
        private Bitmap b = null;
        private Bitmap b_binary = null;
        public Form_Image2Binary()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            b = ImageProcess.from_file_faster() as Bitmap;
            pictureBox1.Image = b;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            b_binary = new Bitmap(b.Width, b.Height);
            ImageProcess.wellner_adaptive_threshold(b, b_binary);
            pictureBox2.Image = b_binary;
        }
    }
}
