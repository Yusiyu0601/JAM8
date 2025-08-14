namespace JAM8.Algorithms.Geometry
{
    public partial class Form_Scottplot4Grid : Form
    {
        private string title = null;
        public Form_Scottplot4Grid(Grid g, string title = null)
        {
            InitializeComponent();

            this.title = title;
            this.Text = $"show grid [ {title} ]";
            scottplot4Grid1.update_grid(g);
        }
    }
}
