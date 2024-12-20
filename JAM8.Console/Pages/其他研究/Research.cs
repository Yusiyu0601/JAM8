using EasyConsole;

namespace JAM8.Console.Pages
{
    class Research : MenuPage
    {
        public Research(EasyConsole.Program program)
       : base("Research", program,
             new Option("Research_NonStationary", () => program.NavigateTo<Research_NonStationary>())
             )
        {
        }
    }
}
