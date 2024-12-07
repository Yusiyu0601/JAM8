using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Easy.Common;
using JAM8.Algorithms.Geometry;
using JAM8.Utilities;

namespace JAM8.SpecificApps.研究方法
{
    public class TI_PatternSize
    {
        #region 平均熵值法

        //计算单个模型（不同尺寸模式分解方式）的平均熵序列
        public static void FindPatternSize_Entropy()
        {
            GridProperty Property = Grid.
                create_from_gslibwin().grid.
                select_gridProperty_win().grid_property;

            Property.show_win();
            List<double> result = new();
            List<int> radiusCollect = MyGenerator.range(1, 40, 1);

            MyDataFrame df = MyDataFrame.create(new string[] { "radius", "entropy" });
            for (int i = 0; i < radiusCollect.Count; i++)
            {
                var record = df.new_record();
                int radius = radiusCollect[i];
                var mould = Mould.create_by_ellipse(radius, radius, 1);
                var pats = Patterns.create(mould, Property);
                double TI_Entropy = PatternEntropy(pats);

                result.Add(TI_Entropy);
                Console.WriteLine(TI_Entropy);

                record["radius"] = radius;
                record["entropy"] = TI_Entropy;

                df.add_record(record);
            }
            df.show_win();
        }

        /// <summary>
        /// 根据Pattern模式库计算模型Model（某个模板尺寸分解）的平均熵
        /// </summary>
        /// <param name="mouldInstances"></param>
        /// <returns></returns>
        static double PatternEntropy(Patterns pats)
        {
            double entropyMean = 0;
            foreach (var (_, mouldInstance) in pats)
            {
                double entropy = 0.0;
                var distinct = mouldInstance.neighbor_values.Distinct().ToList();
                Dictionary<double?, int> temp = new();
                for (int i = 0; i < distinct.Count; i++)
                {
                    temp.Add(distinct[i], 0);
                }

                for (int i = 0; i < mouldInstance.mould.neighbors_number; i++)
                {
                    double? value = mouldInstance[i];
                    for (int j = 0; j < distinct.Count; j++)
                    {
                        double? key = temp.Keys.ToList()[j];
                        if (value == key)
                        {
                            temp[key]++;
                        }
                    }
                }
                for (int i = 0; i < distinct.Count; i++)
                {
                    double? key = temp.Keys.ToList()[i];
                    int n = temp[key];
                    double p = (double)n / mouldInstance.mould.neighbors_number;//概率
                    entropy += -p * Math.Log(p, 2);//典型熵的定义的底数为2
                }
                entropyMean += entropy;
            }
            entropyMean /= pats.Count;//取均值
            return entropyMean;
        }

        #endregion
    }
}
