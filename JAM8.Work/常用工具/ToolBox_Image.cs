using JAM8.SpecificApps.常用工具;

namespace JAM8.Work.常用工具
{
    internal class ToolBox_Image
    {
        public static void Run()
        {
            var menu = new EasyConsole.Menu()

                .Add("退出", CommonFunctions.Cancel)
                .Add("彩色图像转为灰度图像", Tool_Image.func_Color2Gray)
                .Add("图像二值化", Tool_Image.func_Image2Binary)
                ;

            menu.Display();
        }
    }
}
