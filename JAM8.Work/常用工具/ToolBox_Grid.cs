using JAM8.SpecificApps.常用工具;

namespace JAM8.Work.常用工具
{
    public static class ToolBox_Grid
    {
        public static void Run()
        {
            var menu = new EasyConsole.Menu()

                .Add("退出", CommonFunctions.Cancel)
                .Add("显示Grid", Tool_Grid.show_grid)
                .Add("显示GridCatalog", Tool_Grid.show_grid_catalog)
                .Add("随机选择Grid的部分井点", Tool_Grid.random_select_points)
                .Add("将二维图片与Grid的转换", Tool_Grid.将二维图像与Grid的转换)
                .Add("编辑Grid", Tool_Grid.编辑Grid)
                .Add("根据条件过滤转换为cdata", Tool_Grid.convert_grid_to_cdata)
                .Add("根据条件过滤Grid", Tool_Grid.grid_filtering)

                ;

            menu.Display();
        }
    }
}
