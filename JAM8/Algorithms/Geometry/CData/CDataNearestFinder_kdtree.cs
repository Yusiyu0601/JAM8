using Accord.Collections;

namespace JAM8.Algorithms.Geometry
{
    /// <summary>
    /// Finds nearest neighbors of conditional data based on original coordinates using KDTree.
    /// </summary>
    public class CDataNearestFinder_kdtree
    {
        public CData cd { get; internal set; }

        private KDTree<string> tree;

        private CDataNearestFinder_kdtree()
        {
        }

        /// <summary>
        /// Find the k nearest neighbors to a given coordinate.
        /// </summary>
        /// <param name="coord">The coordinate (x, y[, z]) to search from.</param>
        /// <param name="k">Number of nearest neighbors to retrieve.</param>
        /// <returns>List of nearest neighbors as (index, coordinate, attributes, distance)</returns>
        public List<(int idx, Coord coord, Dictionary<string, float?> attrs, float distance)> find(Coord coord, int k)
        {
            double[] query_point = cd.dim == Dimension.D2
                ? [coord.x, coord.y]
                : [coord.x, coord.y, coord.z];

            var neighbors = tree.Nearest(query_point, k);

            List<(int, Coord, Dictionary<string, float?>, float)> results = [];
            foreach (var item in neighbors)
            {
                int idx = int.Parse(item.Node.Value);
                Coord found_coord = cd.get_coord(idx);

                var (names, values) = cd.get_cdata_item(idx);
                var attrs = names.Zip(values).ToDictionary(x => x.First, x => x.Second);

                float distance = (float)Math.Sqrt(item.Distance); // Accord returns squared distance

                results.Add((idx, found_coord, attrs, distance));
            }

            return results;
        }

        /// <summary>
        /// Creates the nearest neighbor finder using raw coordinates from CData2.
        /// </summary>
        /// <param name="cd">The conditional data object.</param>
        /// <returns>A KDTree-based nearest neighbor finder.</returns>
        public static CDataNearestFinder_kdtree create(CData cd)
        {
            CDataNearestFinder_kdtree finder = new() { cd = cd };

            List<double[]> points = [];
            List<string> values = [];

            for (int i = 0; i < cd.N_cdata_items; i++)
            {
                Coord coord = cd.get_coord(i);
                points.Add(cd.dim == Dimension.D2 ? [coord.x, coord.y] : [coord.x, coord.y, coord.z]);
                values.Add(i.ToString());
            }

            finder.tree = KDTree.FromData(points.ToArray(), values.ToArray());

            return finder;
        }
    }
}