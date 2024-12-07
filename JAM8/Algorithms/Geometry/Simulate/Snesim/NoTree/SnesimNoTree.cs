using JAM8.Algorithms.Numerics;

namespace JAM8.Algorithms.Geometry
{
    public class SnesimNoTree
    {
        private SnesimNoTree() { }

        public GridProperty TI { get; internal set; }
        public Mould mould { get; internal set; }
        public Patterns pats { get; internal set; }
        public List<(float? value, float freq)> pdf { get; internal set; }

        public GridProperty Run()
        {
            GridProperty model = GridProperty.create(TI.gridStructure);
            SimulationPath path = SimulationPath.create(TI.gridStructure, 1, new Random(1));
            Random rnd = new(1);
            while (path.is_visit_over() == false)
            {
                var si = path.visit_next();
                if (model.get_value(si) == null)
                {
                    var dataEvent = MouldInstance.create_from_gridProperty(mould, si, model);
                    if (dataEvent.neighbor_not_nulls_ids.Count == 0)
                    {
                        var v = cdf_sampler.sample(pdf, (float)rnd.NextDouble());
                        model.set_value(si, v);
                    }
                    else
                    {
                        foreach (var pat in pats)
                        {

                        }
                    }

                }

            }
            return model;
        }

        public static SnesimNoTree create(Mould mould, GridProperty TI)
        {
            SnesimNoTree snesim = new()
            {
                mould = mould,
                TI = TI,
                pats = Patterns.create(mould, TI),
                pdf = TI.discrete_category_freq(false)
            };
            return snesim;
        }
    }
}
