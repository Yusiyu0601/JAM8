namespace JAM8.Algorithms.Geometry
{
    /// <summary>
    /// 空间索引
    /// </summary>
    public class SpatialIndex
    {
        private SpatialIndex()
        {
        }

        /// <summary>
        /// 维度
        /// </summary>
        public Dimension dim { get; internal set; } = Dimension.D2;

        /// <summary>
        /// index of x，初始化为0
        /// </summary>
        public int ix { get; internal set; } = 0;

        /// <summary>
        /// index of y，初始化为0
        /// </summary>
        public int iy { get; internal set; } = 0;

        /// <summary>
        /// index of z，初始化为0
        /// </summary>
        public int iz { get; internal set; } = 0;

        /// <summary>
        /// 深度复制
        /// </summary>
        /// <returns></returns>
        public SpatialIndex clone()
        {
            SpatialIndex si_new = new()
            {
                ix = ix,
                iy = iy,
                iz = iz,
                dim = dim
            };
            return si_new;
        }

        /// <summary>
        /// 创建SpatialIndex
        /// </summary>
        /// <param name="ix"></param>
        /// <param name="iy"></param>
        /// <returns></returns>
        public static SpatialIndex create(int ix, int iy)
        {
            var si = new SpatialIndex
            {
                ix = ix,
                iy = iy,
                dim = Dimension.D2
            };
            return si;
        }

        /// <summary>
        /// 创建SpatialIndex
        /// </summary>
        /// <param name="ix"></param>
        /// <param name="iy"></param>
        /// <param name="iz"></param>
        /// <returns></returns>
        public static SpatialIndex create(int ix, int iy, int iz)
        {
            var si = new SpatialIndex
            {
                ix = ix,
                iy = iy,
                iz = iz,
                dim = Dimension.D3
            };
            return si;
        }

        /// <summary>
        /// 创建SpatialIndex，相当于复制si
        /// </summary>
        /// <param name="si"></param>
        /// <returns></returns>
        public static SpatialIndex create(SpatialIndex si)
        {
            return si.clone();
        }

        /// <summary>
        /// 创建SpatialIndex
        /// </summary>
        /// <param name="si_viewText"></param>
        /// <returns></returns>
        public static SpatialIndex create(string si_viewText)
        {
            var strs_1 = si_viewText.Split(" ");
            var strs_2 = strs_1[1].Split("_[");
            var str_dim = strs_2[0];
            var ix_iy_iz = strs_2[1].Trim(']').Split('_');
            int ix = int.Parse(ix_iy_iz[0]);
            int iy = int.Parse(ix_iy_iz[1]);
            int iz = int.Parse(ix_iy_iz[2]);
            if (str_dim == "D2")
                return create(ix, iy);
            if (str_dim == "D3")
                return create(ix, iy, iz);
            else
                return null;
        }

        /// <summary>
        /// 创建原点的空间索引
        /// </summary>
        /// <param name="dim"></param>
        /// <returns></returns>
        public static SpatialIndex create_in_origin(Dimension dim = Dimension.D2)
        {
            if (dim == Dimension.D2)
                return SpatialIndex.create(0, 0);
            if (dim == Dimension.D3)
                return SpatialIndex.create(0, 0, 0);
            return null;
        }

        /// <summary>
        /// 计算距离
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static float calc_dist(SpatialIndex left, SpatialIndex right)
        {
            if (left.dim != right.dim)
                return -1;

            if (left.dim == Dimension.D2)
                return (float)Math.Sqrt(Math.Pow(left.ix - right.ix, 2)
                                        + Math.Pow(left.iy - right.iy, 2));

            if (left.dim == Dimension.D3)
                return (float)Math.Sqrt(Math.Pow(left.ix - right.ix, 2)
                                        + Math.Pow(left.iy - right.iy, 2)
                                        + Math.Pow(left.iz - right.iz, 2));
            return -1;
        }

        /// <summary>
        /// 计算距离平方
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static float calc_dist_power2(SpatialIndex left, SpatialIndex right)
        {
            if (left.dim != right.dim)
                return -1;

            if (left.dim == Dimension.D2)
                return (float)(Math.Pow(left.ix - right.ix, 2)
                               + Math.Pow(left.iy - right.iy, 2));

            if (left.dim == Dimension.D3)
                return (float)(Math.Pow(left.ix - right.ix, 2)
                               + Math.Pow(left.iy - right.iy, 2)
                               + Math.Pow(left.iz - right.iz, 2));

            return -1;
        }

        /// <summary>
        /// 计算与原点的距离
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static float calc_dist_to_origin(SpatialIndex si)
        {
            if (si.dim == Dimension.D2)
                return (float)Math.Sqrt(si.ix * si.ix + si.iy * si.iy);

            if (si.dim == Dimension.D3)
                return (float)Math.Sqrt(si.ix * si.ix + si.iy * si.iy + si.iz * si.iz);

            return -1;
        }

        /// <summary>
        /// 计算距离平方
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static float calc_dist_power2_to_origin(SpatialIndex si)
        {
            if (si.dim == Dimension.D2)
                return si.ix * si.ix + si.iy * si.iy;

            if (si.dim == Dimension.D3)
                return si.ix * si.ix + si.iy * si.iy + si.iz * si.iz;

            return -1;
        }


        /// <summary>
        /// 偏移(2D & 3D)
        /// </summary>
        /// <param name="delta"></param>
        /// <returns></returns>
        public SpatialIndex offset(SpatialIndex delta)
        {
            if (dim != delta.dim)
                return null;
            if (delta.dim == Dimension.D2)
                return offset(delta.ix, delta.iy);
            if (delta.dim == Dimension.D3)
                return offset(delta.ix, delta.iy, delta.iz);
            return null;
        }

        /// <summary>
        /// 偏移(2D)
        /// </summary>
        /// <param name="delta_x"></param>
        /// <param name="delta_y"></param>
        /// <returns></returns>
        public SpatialIndex offset(int delta_x, int delta_y)
        {
            return create(ix + delta_x, iy + delta_y);
        }

        /// <summary>
        /// 偏移(3D)
        /// </summary>
        /// <param name="delta_x"></param>
        /// <param name="delta_y"></param>
        /// <param name="delta_z"></param>
        /// <returns></returns>
        public SpatialIndex offset(int delta_x, int delta_y, int delta_z)
        {
            return create(ix + delta_x, iy + delta_y, iz + delta_z);
        }

        /// <summary>
        /// 两个空间索引相减，返回表示从 b 到 a 的偏移向量（a - b）
        /// </summary>
        /// <param name="a">被减索引</param>
        /// <param name="b">减去的索引</param>
        /// <returns>差向量（SpatialIndex），表示 a 相对 b 的偏移</returns>
        /// <exception cref="InvalidOperationException">若维度不一致则抛出异常</exception>
        public static SpatialIndex operator -(SpatialIndex a, SpatialIndex b)
        {
            if (a.dim != b.dim)
                throw new InvalidOperationException("Cannot subtract SpatialIndex of different dimensions.");

            return a.dim == Dimension.D2
                ? create(a.ix - b.ix, a.iy - b.iy)
                : create(a.ix - b.ix, a.iy - b.iy, a.iz - b.iz);
        }

        /// <summary>
        /// 两个空间索引相加，表示坐标平移（a + b）
        /// </summary>
        /// <param name="a">起始索引</param>
        /// <param name="b">偏移索引</param>
        /// <returns>偏移后的新索引</returns>
        /// <exception cref="InvalidOperationException">若维度不一致则抛出异常</exception>
        public static SpatialIndex operator +(SpatialIndex a, SpatialIndex b)
        {
            if (a.dim != b.dim)
                throw new InvalidOperationException("Cannot add SpatialIndex of different dimensions.");

            return a.dim == Dimension.D2
                ? create(a.ix + b.ix, a.iy + b.iy)
                : create(a.ix + b.ix, a.iy + b.iy, a.iz + b.iz);
        }

        /// <summary>
        /// 将索引转换为三元组 (x, y, z) 表示
        /// </summary>
        public (float x, float y, float z) to_tuple()
        {
            return (ix, iy, iz);
        }

        /// <summary>
        /// 打印
        /// </summary>
        /// <returns></returns>
        public string view_text()
        {
            return $"SpatialIndex {dim}_[{ix}_{iy}_{iz}]";
        }

        /// <summary>
        /// 只用于调试展示数据用
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return view_text();
        }
    }
}