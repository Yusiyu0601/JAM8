namespace JAM8.Algorithms.Geometry
{
    public partial class Form_Scottplot4Grid : Form
    {
        string title = null;
        public Form_Scottplot4Grid(Grid g, string title = null)
        {
            InitializeComponent();

            this.title = title;
            this.Text = $"show grid [ {title} ]";

            scottplot4Grid1.GridPropertySelectedEvent += Scottplot4Grid1_GridPropertySelectedEvent;

            scottplot4Grid1.update_grid(g);
        }

        private void Scottplot4Grid1_GridPropertySelectedEvent(string gp_name)
        {
            if (title != null)
                this.Text = $"show grid [ {title} ] [ {gp_name} ]";
            else
                this.Text = $"show grid [ {gp_name} ]";
        }
    }
}
