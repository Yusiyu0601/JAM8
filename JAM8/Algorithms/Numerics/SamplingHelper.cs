using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JAM8.Algorithms.Numerics
{
    /// <summary>
    /// 基于cdf的抽样器
    /// </summary>
    public class SamplingHelper
    {
        private SamplingHelper()
        {
        }

        /// <summary>
        /// 从给定的离散分布中进行采样。
        /// 要求所有频率值 freq 总和为 1（或非常接近 1），否则可能导致采样异常。
        /// 
        /// <code>【算法原理】</code>
        /// 将每个值对应的频率区间看作 [min_p, max_p)，根据随机数 p ∈ [0, 1) 选择落入某个区间的值作为结果。
        /// 
        /// <code>【使用示例】</code>
        /// <code>
        /// var list = new List&lt;(string, float)&gt; { ("A", 0.3f), ("B", 0.5f), ("C", 0.2f) };
        /// float random_p = 0.65f;
        /// string sampled = cdf_sampler.sample(list, random_p);
        /// Console.WriteLine(sampled);  // 输出可能是 "B"
        /// </code>
        /// 
        /// 使用 Dictionary 时可以这样：
        /// <code>
        /// var dict = new Dictionary&lt;string, float&gt; { ["Low"] = 0.1f, ["Medium"] = 0.6f, ["High"] = 0.3f };
        /// string sampled = cdf_sampler.sample(dict.Select(kv => (kv.Key, kv.Value)), 0.75f);
        /// </code>
        /// </summary>
        /// <typeparam name="T">抽样值的类型，例如 string、int、float 等</typeparam>
        /// <param name="value_freq_discrete">
        /// 一个由 (value, freq) 组成的序列，表示每个值及其出现的概率。
        /// 要求 freq 总和为 1，否则可能导致无法覆盖整个 [0,1) 区间。
        /// </param>
        /// <param name="p">
        /// 范围在 [0, 1) 的随机浮点数，用于定位某个值的概率区间。
        /// 例如：new Random().NextDouble() 或你自定义的随机数生成器。
        /// </param>
        /// <returns>根据输入概率分布采样得到的值；若分布异常，返回 default(T)</returns>
        public static T sample<T>(IEnumerable<(T value, double freq)> value_freq_discrete, double p)
        {
            var list = value_freq_discrete.ToList();

            if (list.Count == 0)
                throw new ArgumentException("输入频率列表不能为空");

            double total = list.Sum(item => item.freq);
            if (total <= 0)
                throw new ArgumentException("频率总和必须为正数");

            double cumulative = 0;
            foreach (var (value, freq) in list)
            {
                double next = cumulative + freq / total;
                if (p >= cumulative && p < next)
                    return value;
                cumulative = next;
            }

            // 若 p = 1 或浮点误差导致未命中，则返回最后一个
            return list.Last().value;
        }
    }
}