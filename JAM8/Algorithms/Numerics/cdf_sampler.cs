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
    public class cdf_sampler
    {
        private cdf_sampler()
        {
        }

        /// <summary>
        /// 从"value-freq"中进行随机抽样，要求所有value的freq之和等于1
        /// </summary>
        /// <param name="value_freq_discrete">所有value的freq之和等于1</param>
        /// <returns></returns>
        public static float? sample(List<(float? value, float freq)> value_freq_discrete, float p)
        {
            float max_p = 0;
            float? value = null;
            for (int i = 0; i < value_freq_discrete.Count; i++)
            {
                float min_p;
                if (i == 0)
                {
                    min_p = 0;
                    max_p = value_freq_discrete[0].freq;
                }
                else
                {
                    min_p = max_p;
                    max_p = min_p + value_freq_discrete[i].freq;
                }

                if (p >= min_p && p < max_p)
                {
                    value = value_freq_discrete[i].value;
                    break;
                }
            }

            return value;
        }

        /// <summary>
        /// 从"value-freq"中进行随机抽样，要求所有value的freq之和等于1
        /// </summary>
        /// <param name="value_freq_discrete">所有value的freq之和等于1</param>
        /// <returns></returns>
        public static T sample<T>(List<(T value, float freq)> value_freq_discrete, float p)
        {
            float max_p = 0;
            T value = default;
            for (int i = 0; i < value_freq_discrete.Count; i++)
            {
                float min_p;
                if (i == 0)
                {
                    min_p = 0;
                    max_p = value_freq_discrete[0].freq;
                }
                else
                {
                    min_p = max_p;
                    max_p = min_p + value_freq_discrete[i].freq;
                }

                if (p >= min_p && p < max_p)
                {
                    value = value_freq_discrete[i].value;
                    break;
                }
            }

            return value;
        }

        /// <summary>
        /// 从"value-freq"中进行随机抽样，要求所有value的freq之和等于1
        /// </summary>
        /// <param name="value_freq_discrete">所有value的freq之和等于1</param>
        /// <returns></returns>
        public static float? sample(Dictionary<float?, float> value_freq_discrete, float p)
        {
            float max_p = 0;
            float? result = null;
            int i = 0;
            foreach (var (value, freq) in value_freq_discrete)
            {
                float min_p;
                if (i == 0)
                {
                    min_p = 0;
                    max_p = freq;
                }
                else
                {
                    min_p = max_p;
                    max_p = min_p + freq;
                }

                if (p >= min_p && p < max_p)
                {
                    result = value;
                    break;
                }

                i++;
            }

            return result;
        }

        /// <summary>
        /// 从 "value-freq" 字典中进行随机抽样，要求所有 freq 之和等于 1
        /// </summary>
        /// <typeparam name="T">值的类型</typeparam>
        /// <param name="value_freq_discrete">所有值与其频率组成的字典，频率总和应为 1</param>
        /// <param name="p">0 到 1 之间的随机数</param>
        /// <returns>根据频率抽样得到的值</returns>
        public static T sample<T>(Dictionary<T, float> value_freq_discrete, float p)
        {
            float max_p = 0;
            T result = default;
            int i = 0;

            foreach (var (value, freq) in value_freq_discrete)
            {
                float min_p;
                if (i == 0)
                {
                    min_p = 0;
                    max_p = freq;
                }
                else
                {
                    min_p = max_p;
                    max_p = min_p + freq;
                }

                if (p >= min_p && p < max_p)
                {
                    result = value;
                    break;
                }

                i++;
            }

            return result;
        }
    }
}