using System.Collections.Concurrent;
using System.Diagnostics;
using JAM8.Utilities;

namespace JAM8.Algorithms.Geometry
{
    public class Simpat
    {
        private Simpat()
        {
        }

        private int random_seed = 1;
        private int multi_grid = 1;
        private GridProperty train_image;
        private GridStructure gs_re;
        private CData cd;
        private Mould mould;
        private (int rx, int ry, int rz) template_rx_ry_rz;
        private int N = 1; //建模个数

        private Dimension dim
        {
            get { return train_image.grid_structure.dim; }
        }

        //样式库
        public Dictionary<int, (Mould mould, Patterns patterns)> pats_mg { get; internal set; }

        /// <summary>
        /// 初始化，创建模式库
        /// </summary>
        private void init()
        {
            pats_mg = [];
            for (int m = 1; m <= multi_grid; m++) //多重网格模拟
            {
                if (dim == Dimension.D2)
                    mould = Mould.create_by_rectangle(template_rx_ry_rz.rx, template_rx_ry_rz.ry, m);
                if (dim == Dimension.D3)
                    mould = Mould.create_by_rectangle(template_rx_ry_rz.rx, template_rx_ry_rz.ry, template_rx_ry_rz.rz,
                        m);
                Patterns pats = Patterns.create(mould, train_image); //提取模式
                if (pats.Count > 0)
                {
                    pats_mg.Add(m, (mould, pats));
                    Console.WriteLine(pats.Count);
                }
            }
        }

        /// <summary>
        /// 创建simpat对象
        /// </summary>
        /// <param name="random_seed"></param>
        /// <param name="multi_grid"></param>
        /// <param name="template_rx_ry_rz"></param>
        /// <param name="train_image"></param>
        /// <param name="gs_re"></param>
        /// <param name="N"></param>
        /// <returns></returns>
        public static Simpat create(int random_seed, int multi_grid, (int, int, int) template_rx_ry_rz,
            GridProperty train_image, CData cd, GridStructure gs_re, int N)
        {
            Simpat simpat = new()
            {
                random_seed = random_seed,
                multi_grid = multi_grid,
                train_image = train_image,
                gs_re = gs_re,
                N = N,
                template_rx_ry_rz = template_rx_ry_rz,
                cd = cd
            };
            simpat.init();
            return simpat;
        }

        public Grid run(int index)
        {
            return null;
        }
    }
}