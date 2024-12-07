using JAM8.SpecificApps.建模方法;

namespace JAM8.Work.建模方法
{
    public class Modeling_GRFS
    {
        public static void Run()
        {
            var menu = new EasyConsole.Menu()

                .Add("退出", CommonFunctions.Cancel)
                .Add("GRFS", Model_GRFS.grfs_run)
                .Add("GRFS", Model_GRFS.GRFS1)
                .Add("GRFS2d_FFT", Model_GRFS.GRFS2d_FFT)
                .Add("GRFS3d_FFT", Model_GRFS.GRFS3d_FFT)
                .Add("GRFS_cd条件化（方案1）", Model_GRFS.GRFS_win1)
                .Add("GRFS_cd条件化（方案2）", Model_GRFS.GRFS_win2)

                ;

            menu.Display();
        }
    }
}
