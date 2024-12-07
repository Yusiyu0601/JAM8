namespace JAM8.Algorithms.Geometry
{
    public partial class Form_Image : Form
    {
        public Form_Image(Bitmap bitmap)
        {
            InitializeComponent();
            this.pictureBox1.Image = bitmap;
        }
    }
}
