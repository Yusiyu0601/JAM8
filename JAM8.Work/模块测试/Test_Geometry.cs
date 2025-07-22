namespace JAM8.Work.测试
{
    public class Test_Geometry
    {
        public static void Run()
        {
            var menu = new EasyConsole.Menu()

                .Add("退出", CommonFunctions.Cancel)
                .Add("STree_Test", Tests.Test_Geometry.STree_Test)
                .Add("CData_Test", Tests.Test_Geometry.CData_Test)
                .Add("GridProperty get_region_by_range", Tests.Test_Geometry.get_region_by_range)
                .Add("calc_weights_ok", Tests.Test_Geometry.calc_weights_ok)
                .Add("calc_weights_ok(条带效应校正)", Tests.Test_Geometry.calc_weights_ok_条带效应校正)
                .Add("Mould_test", Tests.Test_Geometry.Mould_test)
                .Add("GridStructure_相等运算符重载测试", Tests.Test_Geometry.GridStructure_相等运算符重载测试)
                .Add("spatialIndex_to_coord", Tests.Test_Geometry.spatialIndex_to_coord)
                .Add("kdtree测试3", Tests.Test_Geometry.kdtree测试3)
                .Add("GridStructure设置", Tests.Test_Geometry.GridStructure设置)
                .Add("超大Grid读写", Tests.Test_Geometry.超大Grid读写)
                .Add("Scottplot4Grid_Test", Tests.Test_Geometry.Scottplot4Grid_Test)
                .Add("AnisotropicDistance_Test", Tests.Test_Geometry.AnisotropicDistance_Test)
                .Add("AnisotropicDistance3d_Test", Tests.Test_Geometry.AnisotropicDistance3d_Test)
                .Add("AnisotropicDistance3d_Test2", Tests.Test_Geometry.AnisotropicDistance3d_Test2)
                .Add("Test_SimulationPath", Tests.Test_Geometry.Test_SimulationPath)
                .Add("Test_GridCatalog", Tests.Test_Geometry.Test_GridCatalog)
                .Add("calc_dataEvent_hsim", Tests.Test_Geometry.calc_dataEvent_hsim)
                .Add("calc_time_dataEvent_ti", Tests.Test_Geometry.calc_time_dataEvent_ti)
                .Add("测试Mould创建", Tests.Test_Geometry.create_mould_by_location)
                ;

            menu.Display();
        }
    }
}
