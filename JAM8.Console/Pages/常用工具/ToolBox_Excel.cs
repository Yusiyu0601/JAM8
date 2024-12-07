using EasyConsole;

namespace JAM8.Console.Pages
{
    class ToolBox_Excel : MenuPage
    {
        public ToolBox_Excel(EasyConsole.Program program)
            : base(
                  "ToolBox_Excel", 
                  program,
                  new Option("Excel转换", () => Output.WriteLine("Excel转换"))
                  )
        {
        }
    }

}
