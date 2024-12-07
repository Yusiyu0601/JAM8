namespace JAM8.Work.测试
{
    public class Test_Numerics
    {
        public static void Run()
        {
            var menu = new EasyConsole.Menu()

                .Add("退出", CommonFunctions.Cancel)
                .Add("Matrix_Test1", Tests.Test_Numerics.Matrix_Test1)
                .Add("ExcelHelper_Test1", Tests.Test_Numerics.ExcelHelper_Test1)

                .Add("Coord_Test", Tests.Test_Numerics.Coord_Test)
                .Add("OK_Test", Tests.Test_Numerics.OK_Test)
                .Add("Mould_Test", Tests.Test_Numerics.Mould_Test)
                .Add("KDQuery_Test", Tests.Test_Numerics.KDQuery_Test)
                .Add("HNSW_Test", Tests.Test_Numerics.HNSW_Test)
                .Add("MyConsoleProgress_Test", Tests.Test_Numerics.MyConsoleProgress_Test)
                .Add("SimulationPath_Test", Tests.Test_Numerics.SimulationPath_Test)
                .Add("Quantile", Tests.Test_Numerics.Quantile_Test)
                .Add("FFT_Test1", Tests.Test_Numerics.FFT_Test1)
                .Add("FFT_Test2", Tests.Test_Numerics.FFT_Test2)
                .Add("FFT2_Test", Tests.Test_Numerics.FFT2_Test)
                .Add("QuickChart_Test", Tests.Test_Numerics.QuickChart_Test)
                .Add("FFTShift_Test", Tests.Test_Numerics.FFTShift_Test)
                .Add("NelderMeadSimplex_Test", Tests.Test_Numerics.NelderMeadSimplex_Test)
                .Add("NelderMeadSimplex_球状模型拟合", Tests.Test_Numerics.NelderMeadSimplex_球状模型拟合)
                .Add("LevenbergMarquardtSolver_Test", Tests.Test_Numerics.LevenbergMarquardtSolver_Test)
                .Add("DataMapper_Test", Tests.Test_Numerics.DataMapper_Test)
                ;

            menu.Display();
        }
    }
}
