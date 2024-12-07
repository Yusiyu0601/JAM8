using JAM8.Algorithms.Geometry;

namespace DataStructures.ViliWonka.KDTree
{
    public struct Vector3 : IEquatable<Vector3>, IFormattable
    {
        public const float kEpsilon = 1E-05F;
        public const float kEpsilonNormalSqrt = 1E-15F;

        //
        // 摘要:
        //     X component of the vector.
        public float x;

        //
        // 摘要:
        //     Y component of the vector.
        public float y;

        //
        // 摘要:
        //     Z component of the vector.
        public float z;

        public Vector3(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public float this[int index]
        {
            get
            {
                if (index == 0)
                {
                    return x;
                }
                else if (index == 1)
                {
                    return y;
                }
                else
                {
                    return z;
                }
            }
            set
            {
                if (index == 0)
                {
                    x = value;
                }
                else if (index == 1)
                {
                    y = value;
                }
                else
                {
                    z = value;
                }
            }
        }

        //
        // 摘要:
        //     Shorthand for writing Vector3(1, 0, 0).
        public static Vector3 right { get { return new Vector3(1, 0, 0); } }

        //
        // 摘要:
        //     Shorthand for writing Vector3(-1, 0, 0).
        public static Vector3 left { get { return new Vector3(-1, 0, 0); } }

        //
        // 摘要:
        //     Shorthand for writing Vector3(0, 1, 0).
        public static Vector3 up { get { return new Vector3(0, 1, 0); } }

        //
        // 摘要:
        //     Shorthand for writing Vector3(0, 0, -1).
        public static Vector3 back { get { return new Vector3(0, 0, -1); } }

        //
        // 摘要:
        //     Shorthand for writing Vector3(0, 0, 1).
        public static Vector3 forward { get { return new Vector3(0, 0, 1); } }

        //
        // 摘要:
        //     Shorthand for writing Vector3(1, 1, 1).
        public static Vector3 one { get { return new Vector3(1, 1, 1); } }

        //
        // 摘要:
        //     Shorthand for writing Vector3(0, 0, 0).
        public static Vector3 zero { get { return new Vector3(0, 0, 0); } }

        //
        // 摘要:
        //     Shorthand for writing Vector3(float.NegativeInfinity, float.NegativeInfinity,
        //     float.NegativeInfinity).
        public static Vector3 negativeInfinity { get { return new Vector3(float.NegativeInfinity, float.NegativeInfinity, float.NegativeInfinity); } }

        //
        // 摘要:
        //     Shorthand for writing Vector3(float.PositiveInfinity, float.PositiveInfinity,
        //     float.PositiveInfinity).
        public static Vector3 positiveInfinity { get { return new Vector3(float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity); } }

        //
        // 摘要:
        //     Shorthand for writing Vector3(0, -1, 0).
        public static Vector3 down { get { return new Vector3(0, -1, 0); } }

        [Obsolete("Use Vector3.forward instead.")]
        public static Vector3 fwd { get { return new Vector3(0, 0, 1); } }

        /// <summary>
        /// Returns this vector with a magnitude of 1 (Read Only).
        /// </summary>
        public Vector3 normalized
        {
            get
            {
                if (magnitude > 0)
                {
                    return new Vector3(x / magnitude, y / magnitude, z / magnitude);
                }
                else
                {
                    return zero;
                }
            }
        }

        /// <summary>
        /// Returns the length of this vector (Read Only).
        /// </summary>
        public float magnitude { get { return (float)Math.Sqrt(x * x + y * y + z * z); } }

        /// <summary>
        /// Returns the squared length of this vector (Read Only).
        /// </summary>
        /// <param name="vector"></param>
        /// <returns></returns>
        public static float SqrMagnitude(Vector3 vector)
        { return vector.x * vector.x + vector.y * vector.y + vector.z * vector.z; }

        /// <summary>
        /// Returns the squared length of this vector (RO).
        /// </summary>
        public float sqrMagnitude
        {
            get { return x * x + y * y + z * z; }
        }

        //
        // 摘要:
        //     Returns the angle in degrees between from and to.
        //
        // 参数:
        //   from:
        //     The vector from which the angular difference is measured.
        //
        //   to:
        //     The vector to which the angular difference is measured.
        //
        // 返回结果:
        //     The angle in degrees between the two vectors.
        public static float Angle(Vector3 from, Vector3 to)
        {
            double radian_angle = Math.Atan2(Cross(from, to).magnitude, Dot(from, to));
            //如果需要角度在0到360之间就取下下面注释。
            //if (Cross(from, to).z < 0)
            //{
            //    radian_angle = 2 * 3.1415926535f - radian_angle;
            //}
            //0到180度
            return (float)(radian_angle * 180 / 3.1415926535f);
        }

        //
        // 摘要:
        //     Returns the distance between a and b.
        //
        // 参数:
        //   a:
        //
        //   b:
        public static float Distance(Vector3 a, Vector3 b)
        {
            return (float)Math.Sqrt(Math.Pow(a.x - b.x, 2.0) + Math.Pow(a.y - b.y, 2.0) + Math.Pow(a.z - b.z, 2.0));
        }

        //
        // 摘要:
        //     Cross Product of two vectors.
        //
        // 参数:
        //   lhs:
        //
        //   rhs:
        public static Vector3 Cross(Vector3 lhs, Vector3 rhs)
        {
            return new Vector3(lhs.y * rhs.z - lhs.z * rhs.y, lhs.z * rhs.x - lhs.x * rhs.z, lhs.x * rhs.y - lhs.y * rhs.x);
        }

        //
        // 摘要:
        //     Projects a vector onto another vector.
        //
        // 参数:
        //   vector:
        //
        //   onNormal:
        public static Vector3 Project(Vector3 vector, Vector3 onNormal)
        {
            if (onNormal.magnitude > 0)
            {
                return (Vector3.Dot(vector, onNormal) / onNormal.magnitude) * onNormal.normalized;
            }
            else
            {
                return zero;
            }
        }

        //
        // 摘要:
        //     Dot Product of two vectors.
        //
        // 参数:
        //   lhs:
        //
        //   rhs:
        public static float Dot(Vector3 lhs, Vector3 rhs)
        {
            return lhs.x * rhs.x + lhs.y * rhs.y + lhs.z * rhs.z;
        }

        //
        // 摘要:
        //     Linearly interpolates between two points.
        //
        // 参数:
        //   a:
        //     Start value, returned when t = 0.
        //
        //   b:
        //     End value, returned when t = 1.
        //
        //   t:
        //     Value used to interpolate between a and b.
        //
        // 返回结果:
        //     Interpolated value, equals to a + (b - a) * t.
        public static Vector3 Lerp(Vector3 a, Vector3 b, float t)
        {
            if (t > 1)
            {
                return new Vector3(b.x, b.y, b.z);
            }
            return new Vector3(a.x + (b.x - a.x) * t, a.y + (b.y - a.y) * t, a.z + (b.z - a.z) * t);
        }

        //
        // 摘要:
        //     Calculate a position between the points specified by current and target, moving
        //     no farther than the distance specified by maxDistanceDelta.
        //
        // 参数:
        //   current:
        //     The position to move from.
        //
        //   target:
        //     The position to move towards.
        //
        //   maxDistanceDelta:
        //     Distance to move current per call.
        //
        // 返回结果:
        //     The new position.
        public static Vector3 MoveTowards(Vector3 current, Vector3 target, float maxDistanceDelta)
        {
            if (maxDistanceDelta >= Distance(current, target))
                return target;
            return current + (target - current).normalized * maxDistanceDelta;
        }

        public void Set(float newX, float newY, float newZ)
        {
            x = newX;
            y = newY;
            z = newZ;
        }

        /// <summary>
        /// 等于
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(Vector3 other)
        {
            if ((this.x == other.x) && (this.y == other.y) && (this.y == other.y))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 求字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("({0},{1},{2})", x, y, z);
        }

        public override bool Equals(object obj)
        {
            return obj is Vector3 && Equals((Vector3)obj);
        }

        public override int GetHashCode()
        {
            return x.GetHashCode() ^ y.GetHashCode() ^ z.GetHashCode();
        }

        public string ToString(string format, IFormatProvider formatProvider)
        {
            throw new NotImplementedException();
        }
        public static Vector3 operator +(Vector3 a, Vector3 b)
        { return new Vector3(a.x + b.x, a.y + b.y, a.z + b.z); }
        public static Vector3 operator -(Vector3 a)
        { return new Vector3(-a.x, -a.y, -a.z); }
        public static Vector3 operator -(Vector3 a, Vector3 b)
        { return new Vector3(a.x - b.x, a.y - b.y, a.z - b.z); }
        public static Vector3 operator *(float d, Vector3 a)
        { return new Vector3(a.x * d, a.y * d, a.z * d); }
        public static Vector3 operator *(Vector3 a, float d)
        { return new Vector3(a.x * d, a.y * d, a.z * d); }
        public static Vector3 operator /(Vector3 a, float d)
        { return new Vector3(a.x / d, a.y / d, a.z / d); }
        public static bool operator ==(Vector3 lhs, Vector3 rhs)
        { return (lhs.x == rhs.x) && (lhs.y == rhs.y) && (lhs.z == rhs.z) ? true : false; }
        public static bool operator !=(Vector3 lhs, Vector3 rhs)
        { return (lhs.x == rhs.x) && (lhs.y == rhs.y) && (lhs.z == rhs.z) ? false : true; }
    }
}
