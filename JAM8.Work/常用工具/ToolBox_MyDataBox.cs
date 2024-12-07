using JAM8.SpecificApps.常用工具;

namespace JAM8.Work.常用工具
{
    public class ToolBox_MyDataBox
    {
        public static void Run()
        {
            var menu = new EasyConsole.Menu()

                .Add("退出", CommonFunctions.Cancel)
                .Add("新建DataBox", Tool_MyDataBox.create_databox)
                .Add("预览item", Tool_MyDataBox.view_items)
                .Add("添加item", Tool_MyDataBox.add_item)
                .Add("删除item", Tool_MyDataBox.delete_item)
                .Add("提取Grid所有的GridProperty，并保存至DataBox", Tool_MyDataBox.convert_grid_to_databox)
                ;

            menu.Display();
        }
    }
}
