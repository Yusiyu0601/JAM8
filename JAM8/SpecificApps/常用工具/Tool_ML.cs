using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JAM8.SpecificApps.常用工具
{
    public class Tool_ML
    {
        public static void func_MakeTestData()
        {
            Form_TrainTestSplitData frm = new();
            frm.Show();
        }

        public static void func_RandomForest()
        {
            Form_RandomForest frm = new();
            frm.Show();
        }

        public static void func_SVM()
        {
            Form_SVM frm = new();
            frm.Show();
        }

        public static void func_KMeans()
        {
            Form_KMeans frm = new();
            frm.Show();
        }

        public static void func_MDS()
        {
            Form_MDS frm = new();
            frm.Show();
        }

    }
}
