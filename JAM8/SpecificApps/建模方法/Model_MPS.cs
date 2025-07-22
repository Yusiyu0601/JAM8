using System.Diagnostics;
using JAM8.Algorithms.Forms;
using JAM8.Algorithms.Geometry;
using JAM8.Algorithms.Numerics;
using JAM8.SpecificApps.建模方法.Forms;
using JAM8.Utilities;

namespace JAM8.SpecificApps.建模方法
{
    public class Model_MPS
    {
        public static void DS_Run()
        {
            Form_DS frm = new();
            frm.ShowDialog();
        }

        public static void MPS_Run()
        {
            Form_MPS frm = new();
            frm.ShowDialog();
        }

        public static void Snesim_Run()
        {
            Form_GridCatalog frm = new();
            if (frm.ShowDialog() != DialogResult.OK)
                return;
            var g = frm.selected_grids.First();
            //var g = Grid.create_from_gslibwin().grid;
            var (cd, _) = CData.read_from_gslib_win();
            GridStructure gs_model = g.gridStructure;

            var snesim = Snesim.create();
            var (model, _) = snesim.run(1, 1, 60, (7, 7, 1), g.first_gridProperty(), cd, gs_model);
            model.showGrid_win();
            //MyConsoleHelper.write_string_to_console("计算时间", (time / 1000.0).ToString());
        }

        //三重网格建模，前2重用simpat，第1重用snesim
        public static void 混合建模()
        {
            var gp_ti = Grid.create_from_gslibwin().grid.
                select_gridProperty_win().grid_property;
            var gs = gp_ti.grid_structure;
            var random_seed = 123456;

            var dim = gp_ti.grid_structure.dim;
            Mould mould;

            Dictionary<int, (Mould mould, Patterns patterns)> pats_mg = new();

            Stopwatch sw = new();
            sw.Start();

            pats_mg = new();
            for (int m = 1; m <= 3; m++)//多重网格模拟
            {
                mould = Mould.create_by_ellipse(20, 20, m);
                Patterns pats = Patterns.create(mould, gp_ti);//提取模式
                if (pats.Count > 0)
                {
                    pats_mg.Add(m, (mould, pats));
                    Console.WriteLine(pats.Count);
                }
            }

            GridProperty gp = GridProperty.create(gs);
            for (int m = pats_mg.Count; m >= 1; m--)//多重网格模拟
            {
                //如果不是最细网格，用simpat
                if (m > 1)
                {
                    var mould_m = pats_mg[m].mould;
                    var patterns_m = pats_mg[m].patterns;

                    SimulationPath path_m = SimulationPath.create(gs, m, new Random(random_seed));

                    while (true != path_m.is_visit_over())
                    {
                        MyConsoleProgress.Print(path_m.progress, $"{m}");
                        var si_m = path_m.visit_next();
                        MouldInstance dataEvent_m = MouldInstance.create_from_gridProperty(mould_m, si_m, gp);
                        MouldInstance bestPat_m;
                        if (dataEvent_m.neighbor_nulls_ids.Count == 0)
                            bestPat_m = patterns_m[patterns_m.random_select(new Random(random_seed))];
                        else
                        {
                            float min_dist = float.MaxValue;
                            int min_index = -1;
                            Parallel.For(0, patterns_m.Count, i =>
                            {
                                float distance = 0f;
                                for (int n = 0; n < mould_m.neighbors_number; n++)
                                {
                                    if (dataEvent_m[n] != null && patterns_m.get_by_index(i)[n] != null)
                                    {
                                        distance += Math.Abs(dataEvent_m[n].Value - patterns_m.get_by_index(i)[n].Value);
                                    }
                                }
                                if (distance < min_dist)
                                {
                                    min_dist = distance;
                                    min_index = i;
                                }
                                if (min_dist == 0)
                                    return;
                            });
                            bestPat_m = patterns_m.get_by_index(min_index);
                        }
                        var neighbors = bestPat_m.paste_to_gridProperty(mould_m, si_m, gp);
                        path_m.freeze(neighbors);
                    }
                }
            }

            //采用snesim模拟剩下网格点数据

            var g_cd = gp.convert_to_grid();
            var cd = CData.create_from_gridProperty(g_cd[0], g_cd.propertyNames[0], CompareType.NotEqual, null);

            var mould_snesim = gs.dim == Dimension.D2 ? Mould.create_by_ellipse(10, 10, 1) : Mould.create_by_ellipse(15, 15, 2, 1);
            mould = Mould.create_by_mould(mould_snesim, 50);

            var snesim = Snesim.create();
            var (model, time) = snesim.run(gp_ti, cd, gs, 1, mould);
            model.showGrid_win();
            sw.Stop();
            Console.Write(sw.ElapsedMilliseconds);

            gp.show_win();
        }
    }
}
