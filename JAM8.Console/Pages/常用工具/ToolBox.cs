using EasyConsole;

namespace JAM8.Console.Pages
{
    class ToolBox : MenuPage
    {
        public ToolBox(EasyConsole.Program program)
            : base(
                  "ToolBox",
                  program,
                  new Option("ToolBox_Grid", () => program.NavigateTo<ToolBox_Grid>()),
                  new Option("ToolBox_Variogram", () => program.NavigateTo<ToolBox_Variogram>()),
                  new Option("ToolBox_Variogram", () => program.NavigateTo<ToolBox_Variogram>())
                  )
        {
        }
    }
}
