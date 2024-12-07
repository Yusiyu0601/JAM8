using EasyConsole;

namespace JAM8.Console.Pages
{
    class ToolBox_Variogram : MenuPage
    {
        public ToolBox_Variogram(EasyConsole.Program program)
            : base("ToolBox_Variogram", program,
                  new Option("AutoFitting", () => Output.WriteLine("自动拟合变差函数"))
                  )
        {

        }
    }
}
