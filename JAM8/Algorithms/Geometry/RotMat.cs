using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExcelLibrary.CompoundDocumentFormat;

namespace JAM8.Algorithms.Geometry
{
    /// <summary>
    /// Transform Cartesian coordinates to coordinatesa ccounting for angles and anisotropy 
    /// See section 2.3 of the  GSLIB User's guide for a detailed definition
    /// </summary>
    public class RotMat
    {
        private readonly float[,] _rot = new float[3, 3];
        private const double DEG2RAD = Math.PI / 180;

        /// <summary>
        /// 各向异性旋转矩阵，用于将笛卡尔坐标投影到变换椭圆空间（参见 GSLIB 2.3）。
        /// 本类统一用于 2D 和 3D 空间的构造。
        /// 
        /// <para><b>二维使用说明（如地图或水平剖面插值）:</b></para>
        /// <para> - alpha: 主轴与 X 轴夹角（度），逆时针为正</para>
        /// <para> - beta: 固定设为 0</para>
        /// <para> - theta: 固定设为 0</para>
        /// <para> - radius: 主轴长度</para>
        /// <para> - radius1: 水平次轴长度</para>
        /// <para> - radius2: 任意正数（设为 1 即可，不参与计算）</para>
        /// 
        /// <para><b>三维使用说明（如地质建模）:</b></para>
        /// <para> - alpha: 主轴与东西方向夹角（度）</para>
        /// <para> - beta: 主轴与水平面夹角（度，向下为正）</para>
        /// <para> - theta: 次轴绕主轴旋转角（度）</para>
        /// <para> - radius: 主轴长度</para>
        /// <para> - radius1: 水平次轴长度</para>
        /// <para> - radius2: 垂向次轴长度</para>
        /// </summary>
        public RotMat(double alpha, double beta, double theta, double radius, double radius1, double radius2)
        {
            double sanis1 = radius / radius1;
            double sanis2 = radius / radius2;

            alpha = (alpha >= 0.0 && alpha < 270.0)
                ? (90 - alpha) * DEG2RAD
                : (450 - alpha) * DEG2RAD;

            double sina = Math.Sin(alpha), cosa = Math.Cos(alpha);
            double sinb = Math.Sin(-beta), cosb = Math.Cos(-beta);
            double sint = Math.Sin(theta), cost = Math.Cos(theta);

            _rot[0, 0] = (float)(cosb * cosa);
            _rot[0, 1] = (float)(cosb * sina);
            _rot[0, 2] = (float)-sinb;

            _rot[1, 0] = (float)(sanis1 * (-cost * sina + sint * sinb * cosa));
            _rot[1, 1] = (float)(sanis1 * (cost * cosa + sint * sinb * sina));
            _rot[1, 2] = (float)(sanis1 * sint * cosb);

            _rot[2, 0] = (float)(sanis2 * (sint * sina + cost * sinb * cosa));
            _rot[2, 1] = (float)(sanis2 * (-sint * cosa + cost * sinb * sina));
            _rot[2, 2] = (float)(sanis2 * cost * cosb);
        }

        /// <summary>
        /// 访问旋转矩阵的元素（局部轴 i，对应原坐标轴 j）
        /// </summary>
        public float this[int i, int j] => _rot[i, j];
    }

    /// <summary>
    /// 各向异性的距离
    /// </summary>
    public class AnisotropicDistance
    {
        /// <summary>
        /// 计算向量 (dx, dy, dz) 在各向异性空间下的距离平方。
        /// </summary>
        /// <param name="mat">各向异性旋转矩阵（RotMat）</param>
        /// <param name="dx">X 方向上的坐标差值（delta x）</param>
        /// <param name="dy">Y 方向上的坐标差值（delta y）</param>
        /// <param name="dz">Z 方向上的坐标差值（delta z）</param>
        /// <returns>将该向量映射到椭球空间后的距离平方值（无开方，适合用于距离比较）</returns>
        public static float get_distance_power2(RotMat mat, float dx, float dy, float dz)
        {
            float dist2 = 0;
            for (int i = 0; i < 3; i++)
            {
                float comp = 0;
                comp += mat[i, 0] * dx;
                comp += mat[i, 1] * dy;
                comp += mat[i, 2] * dz;
                dist2 += comp * comp;
            }

            return dist2;
        }

        /// <summary>
        /// 计算两个点在各向异性空间下的距离平方（输入为三元组坐标）。
        /// </summary>
        /// <param name="mat">各向异性旋转矩阵（RotMat）</param>
        /// <param name="p1">第一个点的坐标，格式为 (x, y, z)</param>
        /// <param name="p2">第二个点的坐标，格式为 (x, y, z)</param>
        /// <returns>两个点之间的各向异性距离的平方值（未开方，适合用于排序和比较）</returns>
        /// <remarks>
        /// 本函数适用于任何可表示为三元组 (x, y, z) 的点结构，
        /// 例如：Coord、SpatialIndex、元组、或自定义类转换为元组的结果。
        /// </remarks>
        public static float get_distance_power2(RotMat mat, (float x, float y, float z) p1,
            (float x, float y, float z) p2)
        {
            float dx = p1.x - p2.x;
            float dy = p1.y - p2.y;
            float dz = p1.z - p2.z;
            return get_distance_power2(mat, dx, dy, dz);
        }

        /// <summary>
        /// 计算某向量（以 Coord 表示）在各向异性空间下的距离平方。
        /// </summary>
        /// <param name="mat">各向异性旋转矩阵（RotMat）</param>
        /// <param name="delta">以 <see cref="Coord"/> 表示的差值向量（从某点到另一点的差）</param>
        /// <returns>各向异性空间下的距离平方（不进行平方根计算）</returns>
        public static float get_distance_power2(RotMat mat, Coord delta)
        {
            return get_distance_power2(mat, delta.x, delta.y, delta.z);
        }

        /// <summary>
        /// Demonstrates the calculation of anisotropic distance squared in both 2D and 3D contexts.
        /// </summary>
        /// <remarks>This method creates rotation matrices for 2D and 3D transformations and computes the
        /// squared anisotropic distance for given coordinate deltas. The results are printed to the console.</remarks>
        public static void Test()
        {
            var rot2d = new RotMat(30, 0, 0, 100, 50, 1); // 2D 用法
            var delta = Coord.create(10, 5);
            float d2 = AnisotropicDistance.get_distance_power2(rot2d, delta);
            Console.WriteLine($"2D distance²: {d2}");

            var rot3d = new RotMat(45, 30, 15, 100, 60, 40);
            var delta3d = Coord.create(10, 5, 3);
            float d3 = AnisotropicDistance.get_distance_power2(rot3d, delta3d);
            Console.WriteLine($"3D distance²: {d3}");
        }
    }
}