using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JAM8.Algorithms.Numerics;

namespace JAM8.Algorithms.Geometry
{
    /// <summary>
    /// 距离变换
    /// </summary>
    public class DistanceTransform
    {
        /// <summary>
        /// Chamfer34倒角变换
        /// </summary>
        /// <param name="property"></param>
        /// <param name="targetValue"></param>
        /// <returns></returns>
        [Obsolete]
        public static GridProperty Chamfer34(GridProperty property, double? targetValue)
        {
            GridStructure gs = property.grid_structure;
            GridProperty chamfer34 = GridProperty.create(gs);
            for (int n = 0; n < gs.N; n++)
            {
                //将不等于目标值的节点，全部赋值为null
                float? value = (property.get_value(n) != targetValue ? null : property.get_value(n));
                chamfer34.set_value(n, value);
            }

            #region

            List<double?> data = [];

            //前向处理
            for (int iy = 0; iy < gs.ny; iy++)
            {
                for (int ix = 0; ix < gs.nx; ix++)
                {
                    if (chamfer34.get_value(ix, iy) == targetValue) 
                        continue;
                    data.Clear();

                    data.Add(chamfer34.get_value(ix, iy));
                    data.Add(chamfer34.get_value(ix, iy - 1) + 3);
                    data.Add(chamfer34.get_value(ix - 1, iy) + 3);
                    data.Add(chamfer34.get_value(ix - 1, iy - 1) + 4);
                    data.Add(chamfer34.get_value(ix - 1, iy + 1) + 4);

                    chamfer34.set_value(ix, iy, (float?)data.Min());
                }
            }

            //后向处理
            for (int iy = gs.ny - 1; iy >= 0; iy--)
            {
                for (int ix = gs.nx - 1; ix >= 0; ix--)
                {
                    if (chamfer34.get_value(ix, iy) == targetValue) continue;
                    data.Clear();

                    data.Add(chamfer34.get_value(ix, iy));
                    data.Add(chamfer34.get_value(ix, iy + 1) + 3);
                    data.Add(chamfer34.get_value(ix + 1, iy) + 3);
                    data.Add(chamfer34.get_value(ix + 1, iy - 1) + 4);
                    data.Add(chamfer34.get_value(ix + 1, iy + 1) + 4);

                    chamfer34.set_value(ix, iy, (float?)data.Min());
                }
            }

            #endregion

            return chamfer34;
        }

        /// <summary>
        /// Felzenszwalb-Huttenlocher 精确欧几里得距离变换
        /// </summary>
        /// <param name="property">GridProperty对象</param>
        /// <param name="target_value">目标值</param>
        /// <returns>每个像素到目标值的欧几里得距离</returns>
        public static GridProperty EuclideanDistanceTransform(GridProperty property, float? target_value)
        {
            if (property.grid_structure.dim == Dimension.D2)
                return EuclideanDistanceTransform2D(property, target_value);
            else if (property.grid_structure.dim == Dimension.D3)
                return EuclideanDistanceTransform3D(property, target_value);
            else
                throw new Exception("不支持的维度");
        }

        /// <summary>
        /// 二维 Felzenszwalb-Huttenlocher 精确欧几里得距离变换
        /// </summary>
        /// <param name="property">二维属性字段</param>
        /// <param name="targetValue">目标值</param>
        /// <returns>每个像素到目标值的欧几里得距离</returns>
        static GridProperty EuclideanDistanceTransform2D(GridProperty property, double? targetValue)
        {
            GridStructure gs = property.grid_structure;
            GridProperty result = GridProperty.create(gs);

            int width = gs.nx;
            int height = gs.ny;

            double INF = 1e20;
            double[,] distance = new double[width, height];

            // 初始化：目标点距离为 0，其他为 INF
            for (int y = 0; y < height; y++)
                for (int x = 0; x < width; x++)
                {
                    distance[x, y] = (property.get_value(x, y) == targetValue) ? 0 : INF;
                }

            // 沿 X 方向传播
            for (int y = 0; y < height; y++)
            {
                double[] f = new double[width];
                for (int x = 0; x < width; x++)
                    f[x] = distance[x, y];
                double[] d = DistanceTransform1D(f, width);
                for (int x = 0; x < width; x++)
                    distance[x, y] = d[x];
            }

            // 沿 Y 方向传播
            for (int x = 0; x < width; x++)
            {
                double[] f = new double[height];
                for (int y = 0; y < height; y++)
                    f[y] = distance[x, y];
                double[] d = DistanceTransform1D(f, height);
                for (int y = 0; y < height; y++)
                    distance[x, y] = d[y];
            }

            // 保存结果
            for (int y = 0; y < height; y++)
                for (int x = 0; x < width; x++)
                {
                    result.set_value(x, y, (float)distance[x, y]);
                }

            return result;
        }

        /// <summary>
        /// 三维 Felzenszwalb-Huttenlocher 精确欧几里得距离变换
        /// </summary>
        /// <param name="property">三维属性字段</param>
        /// <param name="targetValue">目标值</param>
        /// <returns>每个像素到目标值的欧几里得距离</returns>
        static GridProperty EuclideanDistanceTransform3D(GridProperty property, double? targetValue)
        {
            GridStructure gs = property.grid_structure;
            GridProperty result = GridProperty.create(gs);

            int width = gs.nx;
            int height = gs.ny;
            int depth = gs.nz;

            double INF = 1e20;
            double[,,] distance = new double[width, height, depth];

            // 初始化：目标点距离为 0，其他为 INF
            for (int z = 0; z < depth; z++)
                for (int y = 0; y < height; y++)
                    for (int x = 0; x < width; x++)
                    {
                        distance[x, y, z] = (property.get_value(x, y, z) == targetValue) ? 0 : INF;
                    }

            // 沿 X 方向传播
            for (int z = 0; z < depth; z++)
                for (int y = 0; y < height; y++)
                {
                    double[] f = new double[width];
                    for (int x = 0; x < width; x++) f[x] = distance[x, y, z];
                    double[] d = DistanceTransform1D(f, width);
                    for (int x = 0; x < width; x++) distance[x, y, z] = d[x];
                }

            // 沿 Y 方向传播
            for (int z = 0; z < depth; z++)
                for (int x = 0; x < width; x++)
                {
                    double[] f = new double[height];
                    for (int y = 0; y < height; y++) f[y] = distance[x, y, z];
                    double[] d = DistanceTransform1D(f, height);
                    for (int y = 0; y < height; y++) distance[x, y, z] = d[y];
                }

            // 沿 Z 方向传播（关键修复：跨层距离计算）
            for (int y = 0; y < height; y++)
                for (int x = 0; x < width; x++)
                {
                    double[] f = new double[depth];
                    for (int z = 0; z < depth; z++) f[z] = distance[x, y, z];
                    double[] d = DistanceTransform1D(f, depth);
                    for (int z = 0; z < depth; z++) distance[x, y, z] = Math.Sqrt(d[z]); // 最终欧几里得距离
                }

            // 保存结果
            for (int z = 0; z < depth; z++)
                for (int y = 0; y < height; y++)
                    for (int x = 0; x < width; x++)
                    {
                        result.set_value(x, y, z, (float)distance[x, y, z]);
                    }

            return result;
        }

        /// <summary>
        /// 一维距离变换（Felzenszwalb-Huttenlocher）
        /// </summary>
        /// <param name="f"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        private static double[] DistanceTransform1D(double[] f, int n)
        {
            int[] v = new int[n];
            double[] z = new double[n + 1];
            double[] d = new double[n];

            int k = 0;
            v[0] = 0;
            z[0] = double.NegativeInfinity;
            z[1] = double.PositiveInfinity;

            for (int q = 1; q < n; q++)
            {
                double s = ((f[q] + q * q) - (f[v[k]] + v[k] * v[k])) / (2.0 * q - 2.0 * v[k]);
                while (s <= z[k])
                {
                    k--;
                    s = ((f[q] + q * q) - (f[v[k]] + v[k] * v[k])) / (2.0 * q - 2.0 * v[k]);
                }

                k++;
                v[k] = q;
                z[k] = s;
                z[k + 1] = double.PositiveInfinity;
            }

            k = 0;
            for (int q = 0; q < n; q++)
            {
                while (z[k + 1] < q) k++;
                d[q] = (q - v[k]) * (q - v[k]) + f[v[k]];
            }

            return d;
        }
    }
}