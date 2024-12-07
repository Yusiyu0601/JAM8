using MathNet.Numerics;
using MathNet.Numerics.Interpolation;

namespace JAM8.Algorithms.Numerics
{
    /// <summary>
    /// 累积概率分布 -> 分位数 变换
    /// z - p 为单调递增函数
    /// 喻思羽 2023-7-24
    /// </summary>
    public class Quantile
    {
        private Quantile() { }

        /// <summary>
        /// 累积概率(Y轴),cumulative probability function
        /// </summary>
        public List<double> cumulative_probabilities { get; internal set; }

        /// <summary>
        /// 分位数(X轴)
        /// </summary>
        public List<double> quantile_values { get; internal set; }

        IInterpolation interpolation_X2Y = null;
        IInterpolation interpolation_Y2X = null;

        public static Quantile create(List<double> data)
        {
            Quantile q = new()
            {
                cumulative_probabilities = new(),
                quantile_values = new()
            };

            Dictionary<double, int> dict = new();
            var keys = data.Distinct().ToList();
            foreach (var key in keys)
            {
                dict.Add(key, 0);
            }
            for (int i = 0; i < data.Count; i++)
            {
                double value = data[i];
                dict[value]++;
            }
            dict = dict.OrderBy(o => o.Key).ToDictionary(o => o.Key, p => p.Value);
            //值的频率(从小到大排序)
            var (values, frequencies) = (dict.Keys.ToArray(), dict.Values.ToArray());

            int cumulative_frequency = 0;//累积频数
            for (int i = 0; i < values.Length; i++)
            {
                cumulative_frequency += frequencies[i];
                q.quantile_values.Add(values[i]);
                q.cumulative_probabilities.Add(cumulative_frequency / (float)data.Count);
            }
            //将累积概率从(x,1)映射到(0,1)，其中x>0
            DataMapper mapper = new();
            mapper.Reset(q.cumulative_probabilities.Min(), q.cumulative_probabilities.Max(), 0, 1);
            q.cumulative_probabilities = mapper.MapAToB(q.cumulative_probabilities).ToList();

            q.interpolation_X2Y = Interpolate.Linear(q.quantile_values, q.cumulative_probabilities);
            q.interpolation_Y2X = Interpolate.Linear(q.cumulative_probabilities, q.quantile_values);

            return q;
        }

        public static Quantile create(List<float> data)
        {
            List<double> data1 = data.Select(a => (double)a).ToList();
            return create(data1);
        }

        /// <summary>
        /// 根据y轴的累积概率值p计算x轴的分位数quantileValue
        /// </summary>
        /// <param name="cumulative_probability">累积概率</param>
        /// <returns>分位数</returns>
        /// <exception cref="Exception"></exception>
        public double get_quantileValue(double cumulative_probability)
        {
            if (cumulative_probability < 0 || cumulative_probability > 1)
                throw new Exception("p取值范围为(0,1]");

            return interpolation_Y2X.Interpolate(cumulative_probability);
        }

        /// <summary>
        /// 获取x轴的分位数(quantile)对应的y轴的累积概率值(cpf)
        /// </summary>
        /// <param name="quantile_value">累积概率的分位数</param>
        /// <returns>累积概率值</returns>
        public double get_cumulativeProbabilities(double quantile_value)
        {
            if (quantile_value < quantile_values.Min() || quantile_value > quantile_values.Max())
                throw new Exception("quantile取值越界");

            return interpolation_X2Y.Interpolate(quantile_value);
        }
    }
}
