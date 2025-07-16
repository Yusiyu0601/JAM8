using EasyConsole;

namespace JAM8.Console.Pages
{
    class MainPage : MenuPage
    {
        public MainPage(EasyConsole.Program program)
            : base("地质建模工具箱", program,
                new Option("工具(variogram、grid、excel)", () => program.NavigateTo<ToolBox>()),
                new Option("建模(estimate、simulation)", () => program.NavigateTo<Modeling>()),
                new Option("研究(non_stationary)", () => program.NavigateTo<Research>()),
                new Option("测试(Algorithms、Utilities)", () => program.NavigateTo<Test>()),
                new Option("帮助", () => program.NavigateTo<Help>())
            )
        {
        }
    }

    public class DemoProgram : EasyConsole.Program
    {
        public DemoProgram()
            : base("地质建模工具箱2025 V1 (Console版)", breadcrumbHeader: true)
        {
            AddPage(new MainPage(this));

            AddPage(new ToolBox(this));
            AddPage(new ToolBox_Grid(this));
            AddPage(new ToolBox_Variogram(this));
            AddPage(new ToolBox_Excel(this));
            AddPage(new ToolBox_MachineLearning(this));

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