using System.Drawing;
using MathNet.Numerics;
using ScottPlot.Palettes;

namespace JAM8.Algorithms.Geometry
{
    /// <summary>
    /// Coord class
    /// </summary>
    public class Coord
    {
        public Dimension dim { get; internal set; }
        public float x { get; internal set; }
        public float y { get; internal set; }
        public float z { get; internal set; }

        private Coord()
        {
        }

        /// <summary>
        /// Create a 2D Coord
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
        /// Create a 3D Coord
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
        /// Clone a Coord
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

        /// <summary>
        /// Returns a string representation of the coordinate in the format "Coord {dim}_[{x}_{y}_{z}]".
        /// </summary>
        /// <remarks>The returned string includes the dimension and the x, y, and z values of the coordinate. This method
        /// is useful for debugging or logging purposes to display the coordinate in a readable format.</remarks>
        /// <returns>A string that represents the coordinate, including its dimension and x, y, and z values.</returns>
        public override string ToString()
        {
            return $"Coord {dim}_[{x}_{y}_{z}]";
        }

        /// <summary>
        /// Offset by another Coord (2D or 3D)
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
        /// Offset in 2D
        /// </summary>
        /// <param name="delta_x"></param>
        /// <param name="delta_y"></param>
        /// <returns></returns>
        public Coord offset(float delta_x, float delta_y)
        {
            return create(x + delta_x, y + delta_y);
        }

        /// <summary>
        /// Offset in 3D
        /// </summary>
        /// <param name="delta_x"></param>
        /// <param name="delta_y"></param>
        /// <param name="delta_z"></param>
        /// <returns></returns>
        public Coord offset(float delta_x, float delta_y, float delta_z)
        {
            return create(x + delta_x, y + delta_y, z + delta_z);
        }

        /// <summary>
        /// Compute Euclidean distance between two Coord instances
        /// </summary>
        /// <param name="c1"></param>
        /// <param name="c2"></param>
        /// <returns></returns>
        public static double get_distance(Coord c1, Coord c2)
        {
            if (c1.dim != c2.dim)
                return -1;
            if (c1.dim == Dimension.D2)
                return Math.Sqrt((c1.x - c2.x) * (c1.x - c2.x) + (c1.y - c2.y) * (c1.y - c2.y));
            if (c1.dim == Dimension.D3)
                return Math.Sqrt((c1.x - c2.x) * (c1.x - c2.x) + (c1.y - c2.y) * (c1.y - c2.y) +
                                 (c1.z - c2.z) * (c1.z - c2.z));
            return -1;
        }

        /// <summary>
        /// Compute Euclidean distance from Coord to origin
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
        /// Sort an array of Coords by distance to this Coord
        /// </summary>
        /// <param name="coords"></param>
        /// <returns></returns>
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
        /// Deep clone the Coord
        /// </summary>
        /// <returns></returns>
        public Coord deep_clone()
        {
            return create(this);
        }

        #region Equality

        // Equality ==
        public static bool operator ==(Coord left, Coord right)
        {
            if (ReferenceEquals(left, right)) return true; // 同一个引用，或者都是null
            if (left is null || right is null) return false; // 一个是null，一个不是null

            if (left.dim != right.dim) return false;
            if (left.x != right.x) return false;
            if (left.y != right.y) return false;
            if (left.z != right.z) return false;
            return true;
        }

        // Inequality !=
        public static bool operator !=(Coord left, Coord right)
        {
            return !(left == right);
        }

        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof(Coord)) return false;

            var other = (Coord)obj;
            return this == other;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 31 + x.GetHashCode();
                hash = hash * 31 + y.GetHashCode();
                hash = hash * 31 + z.GetHashCode();
                hash = hash * 31 + dim.GetHashCode();
                return hash;
            }
        }

        #endregion
    }
}