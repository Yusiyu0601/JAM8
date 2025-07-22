using Accord.Collections;
using JAM8.Algorithms.Numerics;

namespace JAM8.Algorithms.Geometry
{
    /// <summary>
    /// 在kdtree4版本基础上，添加各向异性
    /// </summary>
    public class CDataNearestFinder_kdtree4_anisotropy
    {
        public CData2 cd { get; internal set; }
        public GridStructure gs { get; internal set; }

        private KDTree<string> tree;

        private DataMapper dm_x;
        private DataMapper dm_y;
        private DataMapper dm_z;

        private Dictionary<int, SpatialIndex> locs = null;

        private CDataNearestFinder_kdtree4_anisotropy()
        {
        }

        /// <summary>
        /// Finds the nearest neighbors to a given spatial index within a specified number of results.
        /// </summary>
        /// <remarks>The method uses a spatial tree to efficiently find the nearest neighbors. The
        /// distance is calculated using the spatial index's distance calculation method.</remarks>
        /// <param name="si">The spatial index to find neighbors for. Must have the same dimensionality as the global spatial index.</param>
        /// <param name="k">The number of nearest neighbors to retrieve.</param>
        /// <returns>A list of tuples, each containing a data item, its spatial index, and the distance to the given spatial
        /// index. Returns <see langword="null"/> if the dimensionality of <paramref name="si"/> does not match the
        /// global spatial index.</returns>
        public List<(CDataItem cdi, SpatialIndex si, float distance)> find(SpatialIndex si, int k)
        {
            if (si.dim != gs.dim)
                return null;

            List<(CDataItem, SpatialIndex, float)> founds = [];
            double[] point = gs.dim == Dimension.D2
                ? [si.ix, si.iy]
                : [dm_x.MapAToB(si.ix), dm_y.MapAToB(si.iy), dm_z.MapAToB(si.iz)];
            var neighbors = tree.Nearest(point, k);
            foreach (var item in neighbors)
            {
                int idx = int.Parse(item.Node.Value);
                SpatialIndex si_found = locs[idx];
                CDataItem cdi_found = cd.get_cdata_item(idx);
                float distance = SpatialIndex.calc_dist(si, si_found);
                founds.Add((cdi_found, si_found, distance));
            }

            return founds;
        }

        /// <summary>
        /// 创建CDataNearestFinder，根据gridStructure计算CDataItem的网格点索引。
        /// 查询过程基于gridStructure实施
        /// </summary>
        /// <param name="cd"></param>
        /// <param name="gs"></param>
        /// <returns></returns>
        public static CDataNearestFinder_kdtree4_anisotropy create(GridStructure gs, CData2 cd)
        {
            if (cd.dim != gs.dim)
                return null;

            CDataNearestFinder_kdtree4_anisotropy cd_finder = new()
            {
                cd = cd,
                gs = gs,
            };

            if (gs.dim == Dimension.D2)
            {
                var (min_x, max_x, min_y, max_y, _, _) = cd.get_boundary();
                cd_finder.dm_x = new();
                cd_finder.dm_x.Reset(min_x, max_x, 0, max_x - min_x);
                cd_finder.dm_y = new();
                cd_finder.dm_y.Reset(min_y, max_y, 0, max_y - min_y);
            }

            if (gs.dim == Dimension.D3)
            {
                var (min_x, max_x, min_y, max_y, min_z, max_z) = cd.get_boundary();
                cd_finder.dm_x = new();
                cd_finder.dm_x.Reset(min_x, max_x, 0, max_x - min_x);
                cd_finder.dm_y = new();
                cd_finder.dm_y.Reset(min_y, max_y, 0, max_y - min_y);
                cd_finder.dm_z = new();
                cd_finder.dm_z.Reset(min_z.Value, max_z.Value, 0, (max_z.Value - min_z.Value) * 10);
            }

            List<double[]> points_adjusted = []; //调整后的点坐标
            List<string> nodes = [];
            cd_finder.locs = []; //调整前的点坐标
            for (int i = 0; i < cd.N_cdata_items; i++)
            {
                SpatialIndex si = gs.coord_to_spatial_index(cd.get_coord(i));

                if (si == null)
                    continue;

                cd_finder.locs.Add(i, si);
                points_adjusted.Add(gs.dim == Dimension.D2
                    ?
                    [
                        cd_finder.dm_x.MapAToB(si.ix),
                        cd_finder.dm_y.MapAToB(si.iy)
                    ]
                    :
                    [
                        cd_finder.dm_x.MapAToB(si.ix),
                        cd_finder.dm_y.MapAToB(si.iy),
                        cd_finder.dm_z.MapAToB(si.iz)
                    ]);
                nodes.Add(i.ToString());
            }

            cd_finder.tree = KDTree.FromData([.. points_adjusted], nodes.ToArray());
            return cd_finder;
        }
    }
}