using EasyConsole;

namespace JAM8.Console.Pages
{
    class Modeling : MenuPage
    {
        public Modeling(EasyConsole.Program program)
            : base("Modeling", program,
                  new Option("Modeling_Estimate", () => program.NavigateTo<Modeling_Estimate>()),
                  new Option("Modeling_Simulate", () => program.NavigateTo<Modeling_Simulate>())
                  )
        {
        }
    }
}
