using Accord.Collections;
using JAM8.Algorithms.Numerics;

namespace JAM8.Algorithms.Geometry
{
    /// <summary>
    /// 在kdtree4版本基础上，添加各向异性
    /// </summary>
    public class CDataNearestFinder_kdtree4_anisotropy
    {
        public CData cd { get; internal set; }
        public GridStructure gs { get; internal set; }

        KDTree<string> tree;

        DataMapper dm_x;
        DataMapper dm_y;
        DataMapper dm_z;

        Dictionary<int, SpatialIndex> locs = null;

        private CDataNearestFinder_kdtree4_anisotropy() { }

        public List<(CDataItem cdi, SpatialIndex si, float distance)> find(SpatialIndex si, int k)
        {
            if (si.dim != gs.dim)
                return null;

            List<(CDataItem, SpatialIndex, float)> founds = [];
            double[] point = gs.dim == Dimension.D2 ?
                [si.ix, si.iy] :
                [dm_x.MapAToB(si.ix), dm_y.MapAToB(si.iy), dm_z.MapAToB(si.iz)];
            var neighbors = tree.Nearest(point, k);
            foreach (var item in neighbors)
            {
                int idx = int.Parse(item.Node.Value);
                SpatialIndex si_found = locs[idx];
                CDataItem cdi_found = cd[idx];
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
        public static CDataNearestFinder_kdtree4_anisotropy create(GridStructure gs, CData cd)
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

            List<double[]> points_adjusted = [];//调整后的点坐标
            List<string> nodes = [];
            cd_finder.locs = [];//调整前的点坐标
            for (int i = 0; i < cd.N; i++)
            {
                SpatialIndex si = gs.coord_to_spatialIndex(cd[i].coord);

                if (si == null)
                    continue;

                cd_finder.locs.Add(i, si);
                points_adjusted.Add(gs.dim == Dimension.D2 ?
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
