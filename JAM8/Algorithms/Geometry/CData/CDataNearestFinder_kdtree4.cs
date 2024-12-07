using Accord.Collections;

namespace JAM7.Algorithms.Geometry
{
    public class CDataNearestFinder_kdtree4
    {
        public CData cd { get; internal set; }
        public GridStructure gs { get; internal set; }

        KDTree<string> tree;

        private CDataNearestFinder_kdtree4() { }

        public List<(CDataItem cdi, SpatialIndex si, float distance)> find(SpatialIndex si, int k)
        {
            if (si.dim != gs.dim)
                return null;

            List<(CDataItem, SpatialIndex, float)> founds = new();
            double[] point = gs.dim == Dimension.D2 ? new double[] { si.ix, si.iy } : new double[] { si.ix, si.iy, si.iz };
            var neighbors = tree.Nearest(point, k);
            foreach (var item in neighbors)
            {
                SpatialIndex si_found = gs.dim == Dimension.D2 ?
                    SpatialIndex.create((int)item.Node.Position[0], (int)item.Node.Position[1]) :
                    SpatialIndex.create((int)item.Node.Position[0], (int)item.Node.Position[1], (int)item.Node.Position[2]);
                CDataItem cdi_found = cd[int.Parse(item.Node.Value)];
                float distance = SpatialIndex.calc_dist(si, si_found);
                founds.Add((cdi_found, si_found, distance));
            }
            return founds;
        }

        /// <summary>
        /// 创建CDataNearestFinder，根据gridStructure计算CDataItem的网格点索引。查询过程基于gridStructure实施。
        /// </summary>
        /// <param name="cd"></param>
        /// <param name="gs"></param>
        /// <returns></returns>
        public static CDataNearestFinder_kdtree4 create(GridStructure gs, CData cd)
        {
            if (cd.dim != gs.dim)
                return null;

            CDataNearestFinder_kdtree4 cd_finder = new()
            {
                cd = cd,
                gs = gs,
            };

            List<double[]> points = new();
            List<string> nodes = new();
            for (int i = 0; i < cd.N; i++)
            {
                SpatialIndex si = gs.coord_to_spatialIndex(cd[i].coord);
                if (si == null)
                    continue;
                points.Add(gs.dim == Dimension.D2 ? 
                    new double[] { si.ix, si.iy } : 
                    new double[] { si.ix, si.iy, si.iz });
                nodes.Add(i.ToString());
            }
            cd_finder.tree = KDTree.FromData(points.ToArray(), nodes.ToArray());
            return cd_finder;
        }


    }
}
