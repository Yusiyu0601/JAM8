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
    public class TrainImage_PatternSize
    {
        #region 平均熵值法

        //计算单个模型（不同尺寸模式分解方式）的平均熵序列
        public static void FindPatternSize_Entropy()
        {
            var property = Grid.create_from_gslibwin().grid.select_gridProperty_win().grid_property;

            property.show_win();
            var result = new List<double>();
            var radius_collect = MyGenerator.range(1, 40, 1);

            var df = MyDataFrame.create(["radius", "entropy"]);
            foreach (var radius in radius_collect)
            {
                var record = df.new_record();
                var mould = Mould.create_by_ellipse(radius, radius, 1);
                var pats = Patterns.create(mould, property);
                var train_image_entropy = pattern_entropy(pats);

                result.Add(train_image_entropy);
                Console.WriteLine(train_image_entropy);

                record["radius"] = radius;
                record["entropy"] = train_image_entropy;

                df.add_record(record);
            }

            df.show_win();
        }

        /// <summary>
        /// 根据Pattern模式库计算模型Model（某个模板尺寸分解）的平均熵
        /// </summary>
        /// <param name="pats"></param>
        /// <returns></returns>
        private static double pattern_entropy(Patterns pats)
        {
            double entropy_mean = 0;
            foreach (var (_, mould_instance) in pats)
            {
                var entropy = 0.0;
                var distinct = mould_instance.neighbor_values.Distinct().ToList();
                Dictionary<double?, int> temp = new();
                for (var i = 0; i < distinct.Count; i++)
                {
                    temp.Add(distinct[i], 0);
                }

                for (var i = 0; i < mould_instance.mould.neighbors_number; i++)
                {
                    double? value = mould_instance[i];
                    for (var j = 0; j < distinct.Count; j++)
                    {
                        var key = temp.Keys.ToList()[j];
                        if (value == key)
                        {
                            temp[key]++;
                        }
                    }
                }

                for (var i = 0; i < distinct.Count; i++)
                {
                    var key = temp.Keys.ToList()[i];
                    var n = temp[key];
                    var p = (double)n / mould_instance.mould.neighbors_number; //概率
                    entropy += -p * Math.Log(p, 2); //典型熵的定义的底数为2
                }

                entropy_mean += entropy;
            }

            entropy_mean /= pats.Count; //取均值
            return entropy_mean;
        }

        #endregion
    }
}