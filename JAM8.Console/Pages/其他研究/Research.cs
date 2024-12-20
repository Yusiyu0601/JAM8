using EasyConsole;

namespace JAM8.Console.Pages
{
    class Research : MenuPage
    {
        public Research(EasyConsole.Program program)
       : base("Research", program,
             new Option("Modeling_Estimate", () => program.NavigateTo<Modeling_Estimate>())
             )
        {
        }
    }
}
