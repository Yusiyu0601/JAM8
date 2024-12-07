namespace JAM8.Work.测试
{
    public class Test_Utilities
    {
        public static void Run()
        {
            var menu = new EasyConsole.Menu()

                .Add("退出", CommonFunctions.Cancel)
                .Add("MyDataFrame_Test", Tests.Test_Utilities.MyDataFrame_Test)
                .Add("Excel_Test", Tests.Test_Utilities.Excel_Test)
                .Add("GenericCopier_Test", Tests.Test_Utilities.GenericCopier_Test)
                .Add("SortHelper_RandomNotRepeatNum", Tests.Test_Utilities.SortHelper_RandomNotRepeatNum)
                .Add("EntityHelper_Test", Tests.Test_Utilities.EntityHelper_Test)
                .Add("Linespace", Tests.Test_Utilities.Linespace)
                .Add("Test_MyDataFrame", Tests.Test_Utilities.Test_MyDataFrame)
                ;

            menu.Display();
        }
    }
}
