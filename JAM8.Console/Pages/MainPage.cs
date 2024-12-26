using EasyConsole;

namespace JAM8.Console.Pages
{
    class MainPage : MenuPage
    {
        public MainPage(EasyConsole.Program program)
            : base("地质建模工具箱", program,
                  new Option("常用工具", () => program.NavigateTo<ToolBox>()),
                  new Option("建模方法", () => program.NavigateTo<Modeling>()),
                  new Option("其他研究", () => program.NavigateTo<Research>()),
                  new Option("测试模块", () => program.NavigateTo<Test>()),
                  new Option("帮助", () => program.NavigateTo<Help>())
                  )
        {
        }
    }

    public class DemoProgram : EasyConsole.Program
    {
        public DemoProgram()
            : base("地质建模工具箱2024 V2 (Console版)", breadcrumbHeader: true)
        {
            AddPage(new MainPage(this));

            AddPage(new ToolBox(this));
            AddPage(new ToolBox_Grid(this));
            AddPage(new ToolBox_Variogram(this));

            AddPage(new Modeling(this));
            AddPage(new Modeling_Estimate(this));
            AddPage(new Modeling_Simulate(this));


            AddPage(new Test(this));
            AddPage(new Test_Algs(this));
            AddPage(new Test_Utils(this));
            AddPage(new Test_Somethings(this));

            AddPage(new Research(this));
            AddPage(new Research_NonStationary(this));

            AddPage(new Help(this));

            SetPage<MainPage>();
        }
    }
}
