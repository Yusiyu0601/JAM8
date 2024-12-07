using EasyConsole;

namespace JAM8.Console.Pages
{
    public class Test : MenuPage
    {
        public Test(EasyConsole.Program program)
    : base(
          "Test", program,
          new Option("Test_Algs", () => program.NavigateTo<Test_Algs>()),
          new Option("Test_Utils", () => program.NavigateTo<Test_Utils>())
          )
        {
        }
    }
}
