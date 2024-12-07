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
        private float[,] rotmat = new float[3, 3];

        private const double DEG2RAD = System.Math.PI / 180;

        /// <summary>
        /// 以三个角度初始化旋转椭圆
        /// </summary>
        /// <param name="alpha">angle between the major axis of anisotropy and the E-W axis. Note: Counter clockwise is positive.</param>
        /// <param name="beta">angle between major axis and the horizontal plane.(The dip of the ellipsoid measured positive down)</param>
        /// <param name="theta">Angle of rotation of minor axis about the major axis of the ellipsoid.</param>
        /// <param name="radius">主轴</param>
        /// <param name="radius1">平面次轴</param>
        /// <param name="radius2">垂向次轴</param>
        public RotMat(double alpha, double beta, double theta, double radius, double radius1, double radius2)
        {
            double sanis1 = radius / radius1;
            double sanis2 = radius / radius2;
            if (alpha >= 0.0 && alpha < 270.0)
                alpha = (90 - alpha) * DEG2RAD;
            else
                alpha = (450 - alpha) * DEG2RAD;
            double sina = System.Math.Sin(alpha);
            double sinb = System.Math.Sin(-beta);
            double sint = System.Math.Sin(theta);
            double cosa = System.Math.Cos(alpha);
            double cosb = System.Math.Cos(-beta);
            double cost = System.Math.Cos(theta);

            rotmat[0, 0] = (float)(cosb * cosa);
            rotmat[0, 1] = (float)(cosb * sina);
            rotmat[0, 2] = (float)-sinb;
            rotmat[1, 0] = (float)(sanis1 * (-cost * sina + sint * sinb * cosa));
            rotmat[1, 1] = (float)(sanis1 * (cost * cosa + sint * sinb * sina));
            rotmat[1, 2] = (float)(sanis1 * (sint * cosb));
            rotmat[2, 0] = (float)(sanis2 * (sint * sina + cost * sinb * cosa));
            rotmat[2, 1] = (float)(sanis2 * (-sint * cosa + cost * sinb * sina));
            rotmat[2, 2] = (float)(sanis2 * (cost * cosb));
        }

        /// <summary>
        /// 获取笛卡尔坐标轴到椭圆局部坐标轴的投影
        /// </summary>
        /// <param name="local">RorMat Coordinatesa</param>
        /// <param name="world">Cartesian coordinates</param>
        /// <returns></returns>
        public float GetRotMatScale(int local, int world)
        {
            return rotmat[local, world];
        }
    }

    /// <summary>
    /// 坐标轴：
    /// </summary>
    //public enum RotMatAxis
    //{
    //    /// <summary>
    //    /// 主轴
    //    /// </summary>
    //    Main = 0,
    //    /// <summary>
    //    /// 平面上次轴
    //    /// </summary>
    //    Two = 1,
    //    /// <summary>
    //    /// 垂直于平面的第三个轴
    //    /// </summary>
    //    Three = 2,
    //}

    /// <summary>
    /// 各向异性的距离
    /// </summary>
    public class AnisotropicDistance
    {
        /// <summary>
        /// 椭圆约束下某点与中心点的距离
        /// 如何理解？
        /// 此处距离是指各项异性空间中的点（输入参数coord）在校正到各向同性空间后的距离
        /// </summary>
        /// <param name="mat"></param>
        /// <returns></returns>
        public static float calc_anis_dist(RotMat mat, Coord coord)
        {
            double dis = 0;
            for (int i = 0; i < 3; i++)
            {
                //RotMatAxis axis = (RotMatAxis)i;
                double cont = 0;
                cont += mat.GetRotMatScale(i, 0) * coord.x;
                cont += mat.GetRotMatScale(i, 1) * coord.y;
                cont += mat.GetRotMatScale(i, 2) * coord.z;
                dis += cont * cont;
            }
            return (float)Math.Sqrt(dis);
        }

        /// <summary>
        /// 椭圆约束下两个点之间的距离
        /// </summary>
        /// <param name="mat"></param>
        /// <param name="c1"></param>
        /// <param name="c2"></param>
        /// <returns></returns>
        public static float calc_anis_dist(RotMat mat, Coord c1, Coord c2)
        {
            Coord dc = Coord.create(c1.x - c2.x, c1.y - c2.y, c1.z - c2.z);
            return calc_anis_dist(mat, dc);
        }

        #region MyRegion

        /// <summary>
        /// 计算si与原点的各向异性距离平方
        /// </summary>
        /// <param name="rotmat"></param>
        /// <param name="si"></param>
        /// <returns></returns>
        public static float calc_anis_distance_power2(RotMat rotmat, SpatialIndex si)
        {
            return calc_anis_distance_power2(rotmat, si.ix, si.iy, si.iz);
        }
        /// <summary>
        /// 计算si与原点的各向异性距离平方
        /// </summary>
        /// <param name="rotmat"></param>
        /// <param name="si1"></param>
        /// <param name="si2"></param>
        /// <returns></returns>
        public static float calc_anis_distance_power2(RotMat rotmat, SpatialIndex si1, SpatialIndex si2)
        {
            return calc_anis_distance_power2(rotmat, si1.ix - si2.ix, si1.iy - si2.iy, si1.iz - si2.iz);
        }
        /// <summary>
        /// 计算si与原点的各向异性距离平方
        /// </summary>
        /// <param name="rotmat"></param>
        /// <param name="delta_ix"></param>
        /// <param name="delta_iy"></param>
        /// <param name="delta_iz"></param>
        /// <returns></returns>
        public static float calc_anis_distance_power2(RotMat rotmat, int delta_ix, int delta_iy, int delta_iz)
        {
            float dist_power2 = 0;
            for (int i = 0; i < 3; i++)
            {
                float cont = 0;
                cont += rotmat.GetRotMatScale(i, 0) * delta_ix;
                cont += rotmat.GetRotMatScale(i, 1) * delta_iy;
                cont += rotmat.GetRotMatScale(i, 2) * delta_iz;
                dist_power2 += cont * cont;
            }
            return dist_power2;
        }

        #endregion
    }

}
