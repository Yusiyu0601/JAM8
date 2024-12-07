namespace JAM8.Work.测试
{
    class Test_ML
    {
        public static void Run()
        {
            var menu = new EasyConsole.Menu()

                .Add("退出", CommonFunctions.Cancel)
                .Add("SVM_test", Tests.Test_MachineLearning.SVM_test)
                .Add("LSH_test", Tests.Test_MachineLearning.LSH_test)
              ;

            menu.Display();
        }
    }
}
