﻿namespace JAM8.Algorithms.Numerics
{
    /// <summary>
    /// 正态(高斯)分布函数
    /// 作者:喻思羽
    /// </summary>
    public class Gaussian
    {
        /// <summary>
        /// 均值
        /// </summary>
        public double Mean { get; internal set; }

        /// <summary>
        /// 方差
        /// </summary>
        public double STD { get; internal set; }

        private Random rnd = null;

        //构造函数
        public Gaussian()
        {
            Mean = 0;
            STD = 1;
            rnd = new Random();
        }
        public Gaussian(double Mean, double STD)
        {
            this.Mean = Mean;
            this.STD = STD;
            rnd = new Random();
        }

        /// <summary>
        /// 随机抽取一个服从高斯分布的数值
        /// </summary>
        public double Sample()
        {
            return Sample(rnd);
        }
        /// <summary>
        /// 随机抽取一个服从高斯分布的数值
        /// </summary>
        /// <param name="randomSource">The random source.</param>
        /// <returns>System.Double.</returns>
        public double Sample(Random randomSource)
        {
            return Mean + STD * SampleBoxMuller(randomSource).Item1;
        }
        /// <summary>
        /// 随机抽取服从高斯分布的数组
        /// </summary>
        /// <param name="N"></param>
        /// <param name="rnd"></param>
        /// <returns></returns>
        public double[] sample(int N, Random rnd)
        {
            double[] data = new double[N];
            for (int i = 0; i < N; i++)
            {
                data[i] = Sample(rnd);
            }
            return data;
        }

        /// <summary>
        /// Samples a pair of standard normal distributed random variables using the <i>Box-Muller</i> algorithm.
        /// </summary>
        /// <param name="rnd">The random number generator to use.</param>
        /// <returns>a pair of random numbers from the standard normal distribution.</returns>
        internal static Tuple<double, double> SampleBoxMuller(Random rnd)
        {
            var v1 = 2.0 * rnd.NextDouble() - 1.0;
            var v2 = 2.0 * rnd.NextDouble() - 1.0;
            var r = v1 * v1 + v2 * v2;
            while (r >= 1.0 || r == 0.0)
            {
                v1 = 2.0 * rnd.NextDouble() - 1.0;
                v2 = 2.0 * rnd.NextDouble() - 1.0;
                r = v1 * v1 + v2 * v2;
            }

            var fac = Math.Sqrt(-2.0 * Math.Log(r) / r);
            return new Tuple<double, double>(v1 * fac, v2 * fac);
        }

        /// <summary>
        /// 返回正态分布函数值(该分布的平均值为 mean，标准偏差为 standard_dev)
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="mean">The mean.</param>
        /// <param name="standard_dev">The standard dev.</param>
        /// <param name="cumulative">if set to <c>true</c> [cumulative].</param>
        /// <returns>System.Double.</returns>
        public static double NormDist(double x, double mean, double standard_dev)
        {
            return 0.5 * (1.0 + Erf((x - mean) / (standard_dev * Constants.Sqrt2)));
        }
        /// <summary>
        /// 返回正态分布函数值(该分布的平均值为 mean，标准偏差为 standard_dev)
        /// </summary>
        /// <param name="probability">The probability.</param>
        /// <param name="mean">The mean.</param>
        /// <param name="standard_dev">The standard dev.</param>
        /// <returns>System.Double.</returns>
        public static double NormInv(double probability, double mean, double standard_dev)
        {
            return mean - standard_dev * Math.Sqrt(2.0) * ErfcInv(2.0 * probability);
        }

        /// <summary>
        /// 返回标准正态分布函数值(该分布的平均值为0，标准偏差为1)
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static double NormStandDist(double x)
        {
            return NormDist(x, 0, 1);
            #region 被替换方法（注释）

            //// constants
            //double a1 = 0.254829592;
            //double a2 = -0.284496736;
            //double a3 = 1.421413741;
            //double a4 = -1.453152027;
            //double a5 = 1.061405429;
            //double p = 0.3275911;

            //// Save the sign of x
            //int sign = 1;
            //if (x < 0)
            //    sign = -1;
            //x = Math.Abs(x) / Math.Sqrt(2.0);

            //// A&S formula 7.1.26
            //double t = 1.0 / (1.0 + p * x);
            //double y = 1.0 - (((((a5 * t + a4) * t) + a3) * t + a2) * t + a1) * t * Math.Exp(-x * x);

            //return 0.5 * (1.0 + sign * y);

            #endregion
        }
        /// <summary>
        /// 返回标准正态累积分布函数的反函数(该分布的平均值为0，标准偏差为1)
        /// </summary>
        /// <param name="probability ">正态分布的概率值，probability的范围是(0,1)</param>
        /// <returns></returns>
        public static double NormStandInv(double probability)
        {
            return NormInv(probability, 0, 1);
            #region 被替换方法（注释）
            ////在 Excel 2010 中使用 NORMSINV 函数时应该注意以下三点：
            ////1）如果参数为非数值型，则函数返回错误值遵「#VALUE!」； 　　
            ////2）如果 probability< 0 或 probability> 1 ，则函数返回错误值「#NUM!」； 　　
            ////3）当已经给定概率值时，NORMSINV 使用 NORMSDIST(z) = probability 求解数值 Z ，
            ////   因此 NORMSINV 的精度取决于 NORMSDIST 的精度；NORMSlNV 使用了迭代技术，
            ////   如果在搜索 100 次迭代之后没有收敛，则函数返回错误值「#N/A」。
            //if (probability <= 0.0 || probability >= 1.0)
            //{
            //    string msg = String.Format("无效的输入参数: {0}.", probability);
            //    throw new ArgumentOutOfRangeException(msg);
            //}

            //// See article above for explanation of this section.
            //if (probability < 0.5)
            //{
            //    // F^-1(p) = - G^-1(p)
            //    return -RationalApproximation(Math.Sqrt(-2.0 * Math.Log(probability)));
            //}
            //else
            //{
            //    // F^-1(p) = G^-1(1-p)
            //    return RationalApproximation(Math.Sqrt(-2.0 * Math.Log(1.0 - probability)));
            //}
            #endregion
        }

        #region 私有方法

        private static double RationalApproximation(double t)
        {
            // Abramowitz and Stegun formula 26.2.23.
            // The absolute value of the error should be less than 4.5 e-4.
            double[] c = { 2.515517, 0.802853, 0.010328 };
            double[] d = { 1.432788, 0.189269, 0.001308 };
            return t - ((c[2] * t + c[1]) * t + c[0]) /
                        (((d[2] * t + d[1]) * t + d[0]) * t + 1.0);
        }

        /// <summary>Calculates the error function.</summary>
        /// <param name="x">The value to evaluate.</param>
        /// <returns>the error function evaluated at given value.</returns>
        /// <remarks>
        ///     <list type="bullet">
        ///         <item>returns 1 if <c>x == Double.PositiveInfinity</c>.</item>
        ///         <item>returns -1 if <c>x == Double.NegativeInfinity</c>.</item>
        ///     </list>
        /// </remarks>
        private static double Erf(double x)
        {
            if (x == 0)
            {
                return 0;
            }

            if (double.IsPositiveInfinity(x))
            {
                return 1;
            }

            if (double.IsNegativeInfinity(x))
            {
                return -1;
            }

            if (double.IsNaN(x))
            {
                return double.NaN;
            }

            return ErfImp(x, false);
        }
        /// <summary>
        /// A helper function to evaluate polynomials fast.
        /// </summary>
        /// <param name="poly">The coefficients of the polynomial.</param>
        /// <param name="z">The location where to evaluate the polynomial at.</param>
        /// <returns>the evaluation of the polynomial.</returns>
        private static double EvaluatePolynomial(double[] poly, double z)
        {
            int count = poly.Length;
            double sum = poly[count - 1];
            for (int i = count - 2; i >= 0; --i)
            {
                sum *= z;
                sum += poly[i];
            }

            return sum;
        }
        /// <summary>
        /// Implementation of the error function.
        /// </summary>
        /// <param name="z">Where to evaluate the error function.</param>
        /// <param name="invert">Whether to compute 1 - the error function.</param>
        /// <returns>the error function.</returns>
        private static double ErfImp(double z, bool invert)
        {
            if (z < 0)
            {
                if (!invert)
                {
                    return -ErfImp(-z, invert);
                }

                if (z < -0.5)
                {
                    return 2 - ErfImp(-z, invert);
                }

                return 1 + ErfImp(-z, false);
            }

            double result;

            //
            // Big bunch of selection statements now to pick which
            // implementation to use, try to put most likely options
            // first:
            //
            if (z < 0.5)
            {
                //
                // We're going to calculate erf:
                //
                if (z < 1e-10)
                {
                    result = z * 1.125 + z * 0.003379167095512573896158903121545171688;
                }
                else
                {
                    // Worst case absolute error found: 6.688618532e-21
                    double[] n = new double[] { 0.00337916709551257388990745, -0.00073695653048167948530905, -0.374732337392919607868241, 0.0817442448733587196071743, -0.0421089319936548595203468, 0.0070165709512095756344528, -0.00495091255982435110337458, 0.000871646599037922480317225 };
                    double[] d = new double[] { 1, -0.218088218087924645390535, 0.412542972725442099083918, -0.0841891147873106755410271, 0.0655338856400241519690695, -0.0120019604454941768171266, 0.00408165558926174048329689, -0.000615900721557769691924509 };

                    result = z * 1.125 + z * EvaluatePolynomial(n, z) / EvaluatePolynomial(d, z);
                }
            }
            else if (z < 110 || z < 110 && invert)
            {
                //
                // We'll be calculating erfc:
                //
                invert = !invert;
                double r, b;
                if (z < 0.75)
                {
                    // Worst case absolute error found: 5.582813374e-21
                    double[] n = new double[] { -0.0361790390718262471360258, 0.292251883444882683221149, 0.281447041797604512774415, 0.125610208862766947294894, 0.0274135028268930549240776, 0.00250839672168065762786937 };
                    double[] d = new double[] { 1, 1.8545005897903486499845, 1.43575803037831418074962, 0.582827658753036572454135, 0.124810476932949746447682, 0.0113724176546353285778481 };
                    r = EvaluatePolynomial(n, z - 0.5) / EvaluatePolynomial(d, z - 0.5);
                    b = 0.3440242112F;
                }
                else if (z < 1.25)
                {
                    // Worst case absolute error found: 4.01854729e-21
                    double[] n = new double[] { -0.0397876892611136856954425, 0.153165212467878293257683, 0.191260295600936245503129, 0.10276327061989304213645, 0.029637090615738836726027, 0.0046093486780275489468812, 0.000307607820348680180548455 };
                    double[] d = new double[] { 1, 1.95520072987627704987886, 1.64762317199384860109595, 0.768238607022126250082483, 0.209793185936509782784315, 0.0319569316899913392596356, 0.00213363160895785378615014 };
                    r = EvaluatePolynomial(n, z - 0.75) / EvaluatePolynomial(d, z - 0.75);
                    b = 0.419990927F;
                }
                else if (z < 2.25)
                {
                    // Worst case absolute error found: 2.866005373e-21
                    double[] n = new double[] { -0.0300838560557949717328341, 0.0538578829844454508530552, 0.0726211541651914182692959, 0.0367628469888049348429018, 0.00964629015572527529605267, 0.00133453480075291076745275, 0.778087599782504251917881e-4 };
                    double[] d = new double[] { 1, 1.75967098147167528287343, 1.32883571437961120556307, 0.552528596508757581287907, 0.133793056941332861912279, 0.0179509645176280768640766, 0.00104712440019937356634038, -0.106640381820357337177643e-7 };
                    r = EvaluatePolynomial(n, z - 1.25) / EvaluatePolynomial(d, z - 1.25);
                    b = 0.4898625016F;
                }
                else if (z < 3.5)
                {
                    // Worst case absolute error found: 1.045355789e-21
                    double[] n = new double[] { -0.0117907570137227847827732, 0.014262132090538809896674, 0.0202234435902960820020765, 0.00930668299990432009042239, 0.00213357802422065994322516, 0.00025022987386460102395382, 0.120534912219588189822126e-4 };
                    double[] d = new double[] { 1, 1.50376225203620482047419, 0.965397786204462896346934, 0.339265230476796681555511, 0.0689740649541569716897427, 0.00771060262491768307365526, 0.000371421101531069302990367 };
                    r = EvaluatePolynomial(n, z - 2.25) / EvaluatePolynomial(d, z - 2.25);
                    b = 0.5317370892F;
                }
                else if (z < 5.25)
                {
                    // Worst case absolute error found: 8.300028706e-22
                    double[] n = new double[] { -0.00546954795538729307482955, 0.00404190278731707110245394, 0.0054963369553161170521356, 0.00212616472603945399437862, 0.000394984014495083900689956, 0.365565477064442377259271e-4, 0.135485897109932323253786e-5 };
                    double[] d = new double[] { 1, 1.21019697773630784832251, 0.620914668221143886601045, 0.173038430661142762569515, 0.0276550813773432047594539, 0.00240625974424309709745382, 0.891811817251336577241006e-4, -0.465528836283382684461025e-11 };
                    r = EvaluatePolynomial(n, z - 3.5) / EvaluatePolynomial(d, z - 3.5);
                    b = 0.5489973426F;
                }
                else if (z < 8)
                {
                    // Worst case absolute error found: 1.700157534e-21
                    double[] n = new double[] { -0.00270722535905778347999196, 0.0013187563425029400461378, 0.00119925933261002333923989, 0.00027849619811344664248235, 0.267822988218331849989363e-4, 0.923043672315028197865066e-6 };
                    double[] d = new double[] { 1, 0.814632808543141591118279, 0.268901665856299542168425, 0.0449877216103041118694989, 0.00381759663320248459168994, 0.000131571897888596914350697, 0.404815359675764138445257e-11 };
                    r = EvaluatePolynomial(n, z - 5.25) / EvaluatePolynomial(d, z - 5.25);
                    b = 0.5571740866F;
                }
                else if (z < 11.5)
                {
                    // Worst case absolute error found: 3.002278011e-22
                    double[] n = new double[] { -0.00109946720691742196814323, 0.000406425442750422675169153, 0.000274499489416900707787024, 0.465293770646659383436343e-4, 0.320955425395767463401993e-5, 0.778286018145020892261936e-7 };
                    double[] d = new double[] { 1, 0.588173710611846046373373, 0.139363331289409746077541, 0.0166329340417083678763028, 0.00100023921310234908642639, 0.24254837521587225125068e-4 };
                    r = EvaluatePolynomial(n, z - 8) / EvaluatePolynomial(d, z - 8);
                    b = 0.5609807968F;
                }
                else if (z < 17)
                {
                    // Worst case absolute error found: 6.741114695e-21
                    double[] n = new double[] { -0.00056907993601094962855594, 0.000169498540373762264416984, 0.518472354581100890120501e-4, 0.382819312231928859704678e-5, 0.824989931281894431781794e-7 };
                    double[] d = new double[] { 1, 0.339637250051139347430323, 0.043472647870310663055044, 0.00248549335224637114641629, 0.535633305337152900549536e-4, -0.117490944405459578783846e-12 };
                    r = EvaluatePolynomial(n, z - 11.5) / EvaluatePolynomial(d, z - 11.5);
                    b = 0.5626493692F;
                }
                else if (z < 24)
                {
                    // Worst case absolute error found: 7.802346984e-22
                    double[] n = new double[] { -0.000241313599483991337479091, 0.574224975202501512365975e-4, 0.115998962927383778460557e-4, 0.581762134402593739370875e-6, 0.853971555085673614607418e-8 };
                    double[] d = new double[] { 1, 0.233044138299687841018015, 0.0204186940546440312625597, 0.000797185647564398289151125, 0.117019281670172327758019e-4 };
                    r = EvaluatePolynomial(n, z - 17) / EvaluatePolynomial(d, z - 17);
                    b = 0.5634598136F;
                }
                else if (z < 38)
                {
                    // Worst case absolute error found: 2.414228989e-22
                    double[] n = new double[] { -0.000146674699277760365803642, 0.162666552112280519955647e-4, 0.269116248509165239294897e-5, 0.979584479468091935086972e-7, 0.101994647625723465722285e-8 };
                    double[] d = new double[] { 1, 0.165907812944847226546036, 0.0103361716191505884359634, 0.000286593026373868366935721, 0.298401570840900340874568e-5 };
                    r = EvaluatePolynomial(n, z - 24) / EvaluatePolynomial(d, z - 24);
                    b = 0.5638477802F;
                }
                else if (z < 60)
                {
                    // Worst case absolute error found: 5.896543869e-24
                    double[] n = new double[] { -0.583905797629771786720406e-4, 0.412510325105496173512992e-5, 0.431790922420250949096906e-6, 0.993365155590013193345569e-8, 0.653480510020104699270084e-10 };
                    double[] d = new double[] { 1, 0.105077086072039915406159, 0.00414278428675475620830226, 0.726338754644523769144108e-4, 0.477818471047398785369849e-6 };
                    r = EvaluatePolynomial(n, z - 38) / EvaluatePolynomial(d, z - 38);
                    b = 0.5640528202F;
                }
                else if (z < 85)
                {
                    // Worst case absolute error found: 3.080612264e-21
                    double[] n = new double[] { -0.196457797609229579459841e-4, 0.157243887666800692441195e-5, 0.543902511192700878690335e-7, 0.317472492369117710852685e-9 };
                    double[] d = new double[] { 1, 0.052803989240957632204885, 0.000926876069151753290378112, 0.541011723226630257077328e-5, 0.535093845803642394908747e-15 };
                    r = EvaluatePolynomial(n, z - 60) / EvaluatePolynomial(d, z - 60);
                    b = 0.5641309023F;
                }
                else
                {
                    // Worst case absolute error found: 8.094633491e-22
                    double[] n = new double[] { -0.789224703978722689089794e-5, 0.622088451660986955124162e-6, 0.145728445676882396797184e-7, 0.603715505542715364529243e-10 };
                    double[] d = new double[] { 1, 0.0375328846356293715248719, 0.000467919535974625308126054, 0.193847039275845656900547e-5 };
                    r = EvaluatePolynomial(n, z - 85) / EvaluatePolynomial(d, z - 85);
                    b = 0.5641584396F;
                }

                double g = Math.Exp(-z * z) / z;
                result = g * b + g * r;
            }
            else
            {
                //
                // Any value of z larger than 28 will underflow to zero:
                //
                result = 0;
                invert = !invert;
            }

            if (invert)
            {
                result = 1 - result;
            }

            return result;
        }
        /// <summary>Calculates the complementary inverse error function evaluated at z.</summary>
        /// <returns>The complementary inverse error function evaluated at given value.</returns>
        /// <remarks> We have tested this implementation against the arbitrary precision mpmath library
        /// and found cases where we can only guarantee 9 significant figures correct.
        ///     <list type="bullet">
        ///         <item>returns Double.PositiveInfinity if <c>z &lt;= 0.0</c>.</item>
        ///         <item>returns Double.NegativeInfinity if <c>z &gt;= 2.0</c>.</item>
        ///     </list>
        /// </remarks>
        /// <summary>calculates the complementary inverse error function evaluated at z.</summary>
        /// <param name="z">value to evaluate.</param>
        /// <returns>the complementary inverse error function evaluated at Z.</returns>
        private static double ErfcInv(double z)
        {
            if (z <= 0.0)
            {
                return double.PositiveInfinity;
            }

            if (z >= 2.0)
            {
                return double.NegativeInfinity;
            }

            double p, q, s;
            if (z > 1)
            {
                q = 2 - z;
                p = 1 - q;
                s = -1;
            }
            else
            {
                p = 1 - z;
                q = z;
                s = 1;
            }

            return ErfInvImpl(p, q, s);
        }
        /// <summary>
        /// The implementation of the inverse error function.
        /// </summary>
        /// <param name="p">First intermediate parameter.</param>
        /// <param name="q">Second intermediate parameter.</param>
        /// <param name="s">Third intermediate parameter.</param>
        /// <returns>the inverse error function.</returns>
        private static double ErfInvImpl(double p, double q, double s)
        {
            double result;

            if (p <= 0.5)
            {
                //
                // Evaluate inverse erf using the rational approximation:
                //
                // x = p(p+10)(Y+R(p))
                //
                // Where Y is a constant, and R(p) is optimized for a low
                // absolute error compared to |Y|.
                //
                // double: Max error found: 2.001849e-18
                // long double: Max error found: 1.017064e-20
                // Maximum Deviation Found (actual error term at infinite precision) 8.030e-21
                //
                float Y = 0.0891314744949340820313f;
                double[] P = new double[] { -0.000508781949658280665617, -0.00836874819741736770379, 0.0334806625409744615033, -0.0126926147662974029034, -0.0365637971411762664006, 0.0219878681111168899165, 0.00822687874676915743155, -0.00538772965071242932965 };
                double[] Q = new double[] { 1, -0.970005043303290640362, -1.56574558234175846809, 1.56221558398423026363, 0.662328840472002992063, -0.71228902341542847553, -0.0527396382340099713954, 0.0795283687341571680018, -0.00233393759374190016776, 0.000886216390456424707504 };
                double g = p * (p + 10);
                double r = EvaluatePolynomial(P, p) / EvaluatePolynomial(Q, p);
                result = g * Y + g * r;
            }
            else if (q >= 0.25)
            {
                //
                // Rational approximation for 0.5 > q >= 0.25
                //
                // x = sqrt(-2*log(q)) / (Y + R(q))
                //
                // Where Y is a constant, and R(q) is optimized for a low
                // absolute error compared to Y.
                //
                // double : Max error found: 7.403372e-17
                // long double : Max error found: 6.084616e-20
                // Maximum Deviation Found (error term) 4.811e-20
                //
                float Y = 2.249481201171875f;
                double[] P = new double[] { -0.202433508355938759655, 0.105264680699391713268, 8.37050328343119927838, 17.6447298408374015486, -18.8510648058714251895, -44.6382324441786960818, 17.445385985570866523, 21.1294655448340526258, -3.67192254707729348546 };
                double[] Q = new double[] { 1, 6.24264124854247537712, 3.9713437953343869095, -28.6608180499800029974, -20.1432634680485188801, 48.5609213108739935468, 10.8268667355460159008, -22.6436933413139721736, 1.72114765761200282724 };
                double g = Math.Sqrt(-2 * Math.Log(q));
                double xs = q - 0.25;
                double r = EvaluatePolynomial(P, xs) / EvaluatePolynomial(Q, xs);
                result = g / (Y + r);
            }
            else
            {
                //
                // For q < 0.25 we have a series of rational approximations all
                // of the general form:
                //
                // let: x = sqrt(-log(q))
                //
                // Then the result is given by:
                //
                // x(Y+R(x-B))
                //
                // where Y is a constant, B is the lowest value of x for which 
                // the approximation is valid, and R(x-B) is optimized for a low
                // absolute error compared to Y.
                //
                // Note that almost all code will really go through the first
                // or maybe second approximation.  After than we're dealing with very
                // small input values indeed: 80 and 128 bit long double's go all the
                // way down to ~ 1e-5000 so the "tail" is rather long...
                //
                double x = Math.Sqrt(-Math.Log(q));
                if (x < 3)
                {
                    // Max error found: 1.089051e-20
                    float Y = 0.807220458984375f;
                    double[] P = new double[] { -0.131102781679951906451, -0.163794047193317060787, 0.117030156341995252019, 0.387079738972604337464, 0.337785538912035898924, 0.142869534408157156766, 0.0290157910005329060432, 0.00214558995388805277169, -0.679465575181126350155e-6, 0.285225331782217055858e-7, -0.681149956853776992068e-9 };
                    double[] Q = new double[] { 1, 3.46625407242567245975, 5.38168345707006855425, 4.77846592945843778382, 2.59301921623620271374, 0.848854343457902036425, 0.152264338295331783612, 0.01105924229346489121 };
                    double xs = x - 1.125;
                    double R = EvaluatePolynomial(P, xs) / EvaluatePolynomial(Q, xs);
                    result = Y * x + R * x;
                }
                else if (x < 6)
                {
                    // Max error found: 8.389174e-21
                    float Y = 0.93995571136474609375f;
                    double[] P = new double[] { -0.0350353787183177984712, -0.00222426529213447927281, 0.0185573306514231072324, 0.00950804701325919603619, 0.00187123492819559223345, 0.000157544617424960554631, 0.460469890584317994083e-5, -0.230404776911882601748e-9, 0.266339227425782031962e-11 };
                    double[] Q = new double[] { 1, 1.3653349817554063097, 0.762059164553623404043, 0.220091105764131249824, 0.0341589143670947727934, 0.00263861676657015992959, 0.764675292302794483503e-4 };
                    double xs = x - 3;
                    double R = EvaluatePolynomial(P, xs) / EvaluatePolynomial(Q, xs);
                    result = Y * x + R * x;
                }
                else if (x < 18)
                {
                    // Max error found: 1.481312e-19
                    float Y = 0.98362827301025390625f;
                    double[] P = new double[] { -0.0167431005076633737133, -0.00112951438745580278863, 0.00105628862152492910091, 0.000209386317487588078668, 0.149624783758342370182e-4, 0.449696789927706453732e-6, 0.462596163522878599135e-8, -0.281128735628831791805e-13, 0.99055709973310326855e-16 };
                    double[] Q = new double[] { 1, 0.591429344886417493481, 0.138151865749083321638, 0.0160746087093676504695, 0.000964011807005165528527, 0.275335474764726041141e-4, 0.282243172016108031869e-6 };
                    double xs = x - 6;
                    double R = EvaluatePolynomial(P, xs) / EvaluatePolynomial(Q, xs);
                    result = Y * x + R * x;
                }
                else if (x < 44)
                {
                    // Max error found: 5.697761e-20
                    float Y = 0.99714565277099609375f;
                    double[] P = new double[] { -0.0024978212791898131227, -0.779190719229053954292e-5, 0.254723037413027451751e-4, 0.162397777342510920873e-5, 0.396341011304801168516e-7, 0.411632831190944208473e-9, 0.145596286718675035587e-11, -0.116765012397184275695e-17 };
                    double[] Q = new double[] { 1, 0.207123112214422517181, 0.0169410838120975906478, 0.000690538265622684595676, 0.145007359818232637924e-4, 0.144437756628144157666e-6, 0.509761276599778486139e-9 };
                    double xs = x - 18;
                    double R = EvaluatePolynomial(P, xs) / EvaluatePolynomial(Q, xs);
                    result = Y * x + R * x;
                }
                else
                {
                    // Max error found: 1.279746e-20
                    float Y = 0.99941349029541015625f;
                    double[] P = new double[] { -0.000539042911019078575891, -0.28398759004727721098e-6, 0.899465114892291446442e-6, 0.229345859265920864296e-7, 0.225561444863500149219e-9, 0.947846627503022684216e-12, 0.135880130108924861008e-14, -0.348890393399948882918e-21 };
                    double[] Q = new double[] { 1, 0.0845746234001899436914, 0.00282092984726264681981, 0.468292921940894236786e-4, 0.399968812193862100054e-6, 0.161809290887904476097e-8, 0.231558608310259605225e-11 };
                    double xs = x - 44;
                    double R = EvaluatePolynomial(P, xs) / EvaluatePolynomial(Q, xs);
                    result = Y * x + R * x;
                }
            }

            return s * result;
        }
        #endregion
    }
}
