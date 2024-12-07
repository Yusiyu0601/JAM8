using System.Drawing;
using MathNet.Numerics;
using ScottPlot.Palettes;

namespace JAM8.Algorithms.Geometry
{
    /// <summary>
    /// Coord类
    /// </summary>
    public class Coord
    {
        private Coord() { }

        #region 属性

        public Dimension dim { get; internal set; }
        public float x { get; internal set; }
        public float y { get; internal set; }
        public float z { get; internal set; }

        #endregion

        #region create

        /// <summary>
        /// 创建2d Coord
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static Coord create(float x, float y)
        {
            Coord c = new()
            {
                x = x,
                y = y,
                dim = Dimension.D2
            };
            return c;
        }
        /// <summary>
        /// 创建3d Coord
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <returns></returns>
        public static Coord create(float x, float y, float z)
        {
            Coord c = new()
            {
                x = x,
                y = y,
                z = z,
                dim = Dimension.D3
            };
            return c;
        }
        /// <summary>
        /// 复制 Coord
        /// </summary>
        /// <param name="coord"></param>
        /// <returns></returns>
        public static Coord create(Coord coord)
        {
            Coord c = new()
            {
                x = coord.x,
                y = coord.y,
                z = coord.z,
                dim = coord.dim
            };
            return c;
        }

        #endregion

        /// <summary>
        /// 打印
        /// </summary>
        /// <returns></returns>
        public string view_text()
        {
            return $"Coord {dim}_[{x}_{y}_{z}]";
        }

        public override string ToString()
        {
            return view_text();
        }

        #region 判断相等

        //判断==
        public static bool operator ==(Coord left, Coord right)
        {
            if (left.dim != right.dim) return false;//维度不同
            if (left.x != right.x) return false;
            if (left.y != right.y) return false;
            if (left.z != right.z) return false;
            return true;
        }
        //判断!=
        public static bool operator !=(Coord left, Coord right)
        {
            return !(left == right);
        }

        public override bool Equals(object obj)
        {
            if (obj is not Coord)
            {
                return false;
            }
            return x == ((Coord)obj).x && y == ((Coord)obj).y && z == ((Coord)obj).z;
        }

        public override int GetHashCode()
        {
            return x.GetHashCode() ^ y.GetHashCode() ^ z.GetHashCode();
        }

        #endregion

        #region offset偏移计算

        /// <summary>
        /// 偏移(2D & 3D)
        /// </summary>
        /// <param name="delta"></param>
        /// <returns></returns>
        public Coord offset(Coord delta)
        {
            if (dim != delta.dim)
                return null;
            if (delta.dim == Dimension.D2)
                return offset(delta.x, delta.y);
            if (delta.dim == Dimension.D3)
                return offset(delta.x, delta.y, delta.z);
            return null;
        }
        /// <summary>
        /// 偏移(2D)
        /// </summary>
        /// <param name="delta_x"></param>
        /// <param name="delta_y"></param>
        /// <returns></returns>
        public Coord offset(float delta_x, float delta_y)
        {
            return create(x + delta_x, y + delta_y);
        }
        /// <summary>
        /// 偏移(3D)
        /// </summary>
        /// <param name="delta_x"></param>
        /// <param name="delta_y"></param>
        /// <param name="delta_z"></param>
        /// <returns></returns>
        public Coord offset(float delta_x, float delta_y, float delta_z)
        {
            return create(x + delta_x, y + delta_y, z + delta_z);
        }

        #endregion

        /// <summary>
        /// 计算2个Coord的欧几里得距离
        /// </summary>
        /// <param name="c1"></param>
        /// <param name="c2"></param>
        /// <returns></returns>
        public static double get_distance(Coord c1, Coord c2)
        {
            if (c1.dim != c2.dim)
                return -1;
            if (c1.dim == c2.dim && c1.dim == Dimension.D2)
                return Math.Sqrt((c1.x - c2.x) * (c1.x - c2.x) + (c1.y - c2.y) * (c1.y - c2.y));
            if (c1.dim == c2.dim && c1.dim == Dimension.D3)
                return Math.Sqrt((c1.x - c2.x) * (c1.x - c2.x) + (c1.y - c2.y) * (c1.y - c2.y) + (c1.z - c2.z) * (c1.z - c2.z));
            return -1;
        }
        /// <summary>
        /// 计算Coord与原点的欧几里得距离
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public static double get_distance_to_origin(Coord c)
        {
            if (c.dim == Dimension.D2)
                return Math.Sqrt(c.x * c.x + c.y * c.y);
            if (c.dim == Dimension.D3)
                return Math.Sqrt(c.x * c.x + c.y * c.y + c.z * c.z);
            return -1;
        }

        /// <summary>
        /// 根据距离
        /// </summary>
        /// <param name="coord"></param>
        /// <param name="distance"></param>
        public List<(Coord coord, double distance)> order_by_distance(Coord[] coords)
        {
            List<(Coord coord, double distance)> result = new();
            for (int i = 0; i < coords.Length; i++)
            {
                result.Add((coords[i], get_distance(this, coords[i])));
            }
            result = result.OrderBy(a => a.distance).ToList();
            return result;
        }

        /// <summary>
        /// 深度复制
        /// </summary>
        /// <returns></returns>
        public Coord deep_clone()
        {
            return create(this);
        }
    }
}
