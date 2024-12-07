using JAM8.Algorithms.Forms;
using MathNet.Numerics.Optimization;
using System.Collections.Concurrent;
using Vector_double = MathNet.Numerics.LinearAlgebra.Vector<double>;

namespace JAM8.Algorithms.Geometry
{
    /// <summary>
    /// 理论变差函数类型
    /// </summary>
    public enum VariogramType
    {
        /// <summary>
        /// 球状理论模型
        /// </summary>
        Spherical,
        /// <summary>
        /// 高斯理论模型
        /// </summary>
        Guassian,
        /// <summary>
        /// 指数模型
        /// </summary>
        Exponential
    }

    /// <summary>
    /// 变差函数
    /// </summary>
    public class Variogram
    {
        private Variogram() { }
        /// <summary>
        /// 变差函数类型
        /// </summary>
        public VariogramType vt { get; internal set; }
        /// <summary>
        /// 块金值
        /// </summary>
        public float nugget { get; internal set; }
        /// <summary>
        /// 基台值
        /// </summary>
        public float sill { get; internal set; }
        /// <summary>
        /// 变程
        /// </summary>
        public float range { get; internal set; }

        public override string ToString()
        {
            return $"variogram type:{vt} \n\tnugget={nugget} \n\tsill={sill} \n\trange={range}";
        }

        /// <summary>
        /// 计算变差函数
        /// </summary>
        /// <param name="h"></param>
        /// <returns></returns>
        public float calc_variogram(float h)
        {
            double result = 0;
            float a;
            switch (vt)
            {
                //球状模型
                case VariogramType.Spherical:
                    if (h == 0)
                        result = nugget;
                    if (h > 0 && h <= range)
                        result = nugget + (sill - nugget) * (1.5 * (h / range) - 0.5 * Math.Pow(h / range, 3));
                    if (h > range)
                        result = sill;
                    break;
                //高斯模型
                case VariogramType.Guassian:
                    a = (float)(range / Math.Sqrt(3));
                    if (h == 0)
                        result = nugget;
                    if (h > 0)
                        result = nugget + (sill - nugget) * (1 - Math.Exp(-(h / a) * (h / a)));
                    break;
                //指数模型
                case VariogramType.Exponential:
                    a = range / 3.0f;
                    if (h == 0)
                        result = nugget;
                    if (h > 0)
                        result = nugget + (sill - nugget) * (1 - Math.Exp(-(h / a)));
                    break;
            }
            return (float)result;
        }

        /// <summary>
        /// 计算协方差函数
        /// </summary>
        /// <param name="h"></param>
        /// <returns></returns>
        public float calc_covariance(float h)
        {
            return sill - calc_variogram(h);
        }

        /// <summary>
        /// 创建Variogram
        /// </summary>
        /// <param name="vt"></param>
        /// <param name="nugget"></param>
        /// <param name="sill"></param>
        /// <param name="range"></param>
        /// <returns></returns>
        public static Variogram create(VariogramType vt, float nugget, float sill, float range)
        {
            Variogram v = new()
            {
                vt = vt,
                nugget = nugget,
                sill = sill,
                range = range
            };
            return v;
        }

        /// <summary>
        /// 采用最小二乘法对实验变差函数进行拟合
        /// </summary>
        /// <param name="h"></param>
        /// <param name="gamma"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static (Variogram fitted_variogram, double loss) variogramFit(VariogramType vt, double[] h, double[] gamma, int[] N_pair)
        {
            if (h.Length != gamma.Length)
                throw new Exception("h.Length != gamma.Length");
            double[] weight_inverse_distance = new double[h.Length];//反距离作为另外一种权重
            double sum_inverse_distance = h.Sum(a => 1 / a);
            for (int i = 0; i < h.Length; i++)
            {
                weight_inverse_distance[i] = (1 / h[i]) / sum_inverse_distance;
            }
            //input:nugget sill range
            double func(Vector_double input)
            {
                double loss = 0.0;
                for (int i = 0; i < h.Length; i++)
                {
                    var nugget = input[0];
                    var sill = input[1];
                    var range = input[2];
                    var gamma_1 = 0.0;
                    if (vt == VariogramType.Spherical)
                    {
                        if (h[i] == 0)
                            gamma_1 = nugget;
                        if (h[i] > 0 && h[i] <= range)
                            gamma_1 = nugget + (sill - nugget) * (1.5 * (h[i] / range) - 0.5 * Math.Pow(h[i] / range, 3));
                        if (h[i] > range)
                            gamma_1 = sill;
                    }
                    if (vt == VariogramType.Exponential)
                    {
                        var a = range / 3.0f;
                        if (h[i] == 0)
                            gamma_1 = nugget;
                        if (h[i] > 0)
                            gamma_1 = nugget + (sill - nugget) * (1 - Math.Exp(-(h[i] / a)));
                    }
                    if (vt == VariogramType.Guassian)
                    {
                        var a = (float)(range / Math.Sqrt(3));
                        if (h[i] == 0)
                            gamma_1 = nugget;
                        if (h[i] > 0)
                            gamma_1 = nugget + (sill - nugget) * (1 - Math.Exp(-(h[i] / a) * (h[i] / a)));
                    }
                    var diff_i = gamma_1 - gamma[i];
                    //loss += N_pair[i] * diff_i * diff_i;//使用“点对权重”
                    loss += weight_inverse_distance[i] * N_pair[i] * diff_i * diff_i;//同时使用“点对权重”与“反距离权重”
                }

                return loss;
            }
            var obj = ObjectiveFunction.Value(func);
            var solver = new NelderMeadSimplex(convergenceTolerance: 0.00001, maximumIterations: 10000);
            var initialGuess = new MathNet.Numerics.LinearAlgebra.Double.DenseVector(new double[] { gamma.Max() / 2.0, gamma.Max(), h.Max() * 2 / 3 });
            var result = solver.FindMinimum(obj, initialGuess);
            var nugget = (float)result.MinimizingPoint[0];//块金值
            var sill = (float)result.MinimizingPoint[1];//基台值
            var range = (float)result.MinimizingPoint[2];//变程
            var loss = result.FunctionInfoAtMinimum.Value;
            //Console.WriteLine($"loss={loss}");
            Variogram variogram = create(vt, nugget, sill, range);
            return (variogram, result.FunctionInfoAtMinimum.Value);
        }

        /// <summary>
        /// 变差函数拟合，win模式显示
        /// </summary>
        /// <param name="h"></param>
        /// <param name="gamma"></param>
        public static void variogramFit_win(double[] h, double[] gamma, int[] N_pair)
        {
            Form_VariogramFit frm = new(h, gamma, N_pair);
            frm.Show();
        }
        /// <summary>
        /// 变差函数拟合，win模式显示
        /// </summary>
        public static void variogramFit_win()
        {
            Form_VariogramFit frm = new();
            frm.Show();
        }
        /// <summary>
        /// 2d模型的变差函数拟合,win模式显示
        /// </summary>
        public static void variogramFit4Grid_win()
        {
            Form_VariogramFit4Grid frm = new();
            frm.Show();
        }

        /// <summary>
        /// 根据2D模型计算的实验变差函数
        /// </summary>
        /// <param name="gp"></param>
        /// <param name="azimuth"></param>
        /// <param name="N_lag"></param>
        /// <param name="lag_unit"></param>
        /// <returns></returns>
        public static (double[] h, double[] gamma, int[] N_pair) calc_experiment_variogram(GridProperty gp, double azimuth, int N_lag, int lag_unit = 1)
        {
            double[] h = new double[N_lag];
            double[] gamma = new double[N_lag];
            int[] N_pair = new int[N_lag];

            GridStructure gs = gp.gridStructure;
            double dx_lag_unit = lag_unit * Math.Cos(Math.PI * azimuth / 180.0);//单位滞后距旋转后的x分量
            double dy_lag_unit = lag_unit * Math.Sin(Math.PI * azimuth / 180.0);//单位滞后距旋转后的y分量

            for (int lag_idx = 1; lag_idx <= N_lag; lag_idx++)//计算所有滞后距的变差值
            {
                h[lag_idx - 1] = (lag_idx) * lag_unit;
                //List<double> diffs = [];//累计变差函数值的临时变量
                ConcurrentBag<double> diffs = new();//累计变差函数值的临时变量
                //计算某个滞后距首尾点对的相对位移
                //注意：这里可能存在问题(2019-6-1)
                int dx = (int)Math.Ceiling(lag_idx * dx_lag_unit);
                int dy = (int)Math.Ceiling(lag_idx * dy_lag_unit);

                Parallel.For(1, gs.ny + 1, iy =>
                {
                    for (int ix = 1; ix <= gs.nx; ix++)
                    {
                        double? head = gp.get_value(ix, iy);
                        double? tail = gp.get_value(ix + dx, iy + dy);
                        if (tail == null || head == null)//如果头或尾跳出网格范围，则循环到下一个节点
                            continue;
                        diffs.Add(Math.Pow(head.Value - tail.Value, 2));
                    }
                });
                if (diffs.Count == 0 || diffs.Count == 1)
                    continue;

                gamma[lag_idx - 1] = diffs.Average() / 2.0;//（半）变差函数值
                N_pair[lag_idx - 1] = diffs.Count;//配对的数目
            }
            return (h, gamma, N_pair);
        }
        /// <summary>
        /// 根据3D模型计算水平方向实验变差函数
        /// </summary>
        /// <param name="gp"></param>
        /// <param name="azimuth"></param>
        /// <param name="N_lag"></param>
        /// <param name="lag_unit"></param>
        /// <returns></returns>
        public static (double[] h, double[] gamma, int[] N_pair) calc_3d_horizontal_experiment_variogram(GridProperty gp, double azimuth, int N_lag, int lag_unit = 1)
        {
            double[] h = new double[N_lag];
            double[] gamma = new double[N_lag];
            int[] N_pair = new int[N_lag];

            GridStructure gs = gp.gridStructure;
            double dx_lag_unit = lag_unit * Math.Cos(Math.PI * azimuth / 180.0);//单位滞后距旋转后的x分量
            double dy_lag_unit = lag_unit * Math.Sin(Math.PI * azimuth / 180.0);//单位滞后距旋转后的y分量

            for (int lag_idx = 1; lag_idx <= N_lag; lag_idx++)//计算所有滞后距的变差值
            {
                h[lag_idx - 1] = (lag_idx) * lag_unit;
                //List<double> diffs = [];//累计变差函数值的临时变量
                ConcurrentBag<double> diffs = new();//累计变差函数值的临时变量
                //计算某个滞后距首尾点对的相对位移
                //注意：这里可能存在问题(2019-6-1)
                int dx = (int)Math.Ceiling(lag_idx * dx_lag_unit);
                int dy = (int)Math.Ceiling(lag_idx * dy_lag_unit);

                Parallel.For(0, gs.nz, iz =>
                {
                    for (int iy = 0; iy < gs.ny; iy++)
                    {
                        for (int ix = 0; ix < gs.nx; ix++)
                        {
                            double? head = gp.get_value(ix, iy, iz);
                            double? tail = gp.get_value(ix + dx, iy + dy, iz);
                            if (tail == null || head == null)//如果头或尾跳出网格范围，则循环到下一个节点
                                continue;
                            diffs.Add(Math.Pow(head.Value - tail.Value, 2));
                        }
                    }
                });
                if (diffs.Count == 0 || diffs.Count == 1)
                    continue;

                gamma[lag_idx - 1] = diffs.Average() / 2.0;//（半）变差函数值
                N_pair[lag_idx - 1] = diffs.Count;//配对的数目
            }
            return (h, gamma, N_pair);
        }
        /// <summary>
        /// 根据3D模型计算垂直方向实验变差函数
        /// </summary>
        /// <param name="gp"></param>
        /// <param name="azimuth"></param>
        /// <param name="N_lag"></param>
        /// <param name="lag_unit"></param>
        /// <returns></returns>
        public static (double[] h, double[] gamma, int[] N_pair) calc_3d_vertical_experiment_variogram(GridProperty gp, int N_lag, int lag_unit = 1)
        {
            double[] h = new double[N_lag - 1];
            double[] gamma = new double[N_lag - 1];
            int[] N_pair = new int[N_lag - 1];

            GridStructure gs = gp.gridStructure;

            for (int lag_idx = 1; lag_idx < N_lag; lag_idx++)//计算所有滞后距的变差值
            {
                h[lag_idx - 1] = (lag_idx) * lag_unit;
                int dz = (lag_idx) * lag_unit;
                //List<double> diffs = [];//累计变差函数值的临时变量
                ConcurrentBag<double> diffs = new();//累计变差函数值的临时变量

                Parallel.For(0, gs.nz, iz =>
                {
                    for (int iy = 0; iy < gs.ny; iy++)
                    {
                        for (int ix = 0; ix < gs.nx; ix++)
                        {
                            double? head = gp.get_value(ix, iy, iz);
                            double? tail = gp.get_value(ix, iy, iz + dz);
                            if (tail == null || head == null)//如果头或尾跳出网格范围，则循环到下一个节点
                                continue;
                            diffs.Add(Math.Pow(head.Value - tail.Value, 2));
                        }
                    }
                });
                if (diffs.Count == 0 || diffs.Count == 1)
                    continue;

                gamma[lag_idx - 1] = diffs.Average() / 2.0;//（半）变差函数值
                N_pair[lag_idx - 1] = diffs.Count;//配对的数目
            }
            return (h, gamma, N_pair);
        }

    }
}
