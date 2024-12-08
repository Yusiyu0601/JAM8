using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Easy.Common.Extensions;
using JAM8.Algorithms;
using JAM8.Algorithms.Geometry;
using JAM8.Algorithms.MachineLearning;
using JAM8.Algorithms.Numerics;
using JAM8.Utilities;

namespace JAM8.SpecificApps.研究方法
{
    /// <summary>
    /// 样式的相关研究
    /// </summary>
    public class mould_research
    {
        /// <summary>
        /// 用MDS对mould进行降维显示
        /// </summary>
        public static void mould_dim_reduction_with_MDS()
        {
            //加载TI
            var (grid, fileName) = Grid.create_from_gslibwin();
            GridStructure gs = grid.gridStructure;
            var ti = grid.select_gridProperty_win("选择TI").grid_property;
            ti = ti.resize(GridStructure.create_simple(80, 80, 1));

            //预先提取所有样式
            var mould = Mould.create_by_rectangle(5, 5, 1);
            var pats = Patterns.create(mould, ti);//预先提取所有样式

            //绘制所有样式的图片
            Dictionary<int, Bitmap> images = new();
            GridStructure gs_pat = GridStructure.create_simple(5 * 2 + 1, 5 * 2 + 1, 1);
            foreach (var (i, pat) in pats)
            {
                var gp_pat = GridProperty.create(gs_pat);
                pat.paste_to_gridProperty(mould, SpatialIndex.create(5 + 1, 5 + 1), gp_pat);
                gp_pat = gp_pat.resize(GridStructure.create_simple(50, 50, 1));
                var image = gp_pat.draw_image_2d(Color.Gray, Algorithms.Images.ColorMapEnum.Jet);
                images.Add(i, image);
            }


            MyDataFrame df_pats = MyDataFrame.create(mould.neighbors_number);
            foreach (var (i, pat) in pats)
            {
                var record = df_pats.new_record(pat.neighbor_values.Select(a => (object)a).ToArray());
                df_pats.add_record(record);
            }

            //建立样式的距离矩阵
            var idx_labels = pats.Select(a => a.Key).ToArray();
            var labels = idx_labels.Select(a => gs.get_spatialIndex(a).view_text());
            var dismat = MyMatrix.create_dismat(df_pats, MyDistanceType.manhattan);
            var locs = CMDSCALE.CMDSCALE_MathNet(dismat, 2);
            string[] series_names = new string[] { "dim1", "dim2" };
            var result = MyDataFrame.create_from_array<float>(series_names, locs.buffer);
            result.add_series("ID");
            result.move_series("ID", 0);
            result.show_win("降维后");

            Form_QuickChart.ScatterPlot(
                result.get_series<double>("dim1"),
                result.get_series<double>("dim2"),
                labels);

            var images1 = images.Values.ToArray();
            Form_QuickChart.ImagePlot(images1, result.get_series<float>("dim1"),
                result.get_series<float>("dim2"));


        }
    }
}
