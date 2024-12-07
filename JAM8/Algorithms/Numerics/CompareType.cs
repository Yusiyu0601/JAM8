using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JAM8.Algorithms.Numerics
{
    /// <summary>
    /// 数值比较类型
    /// </summary>
    public enum CompareType
    {
        /// <summary>
        /// 不进行比较，任何数值都选取（满足比较条件）
        /// </summary>
        NoCompared,
        /// <summary>
        /// 等于
        /// </summary>
        Equals,
        /// <summary>
        /// 不等于
        /// </summary>
        NotEqual,
        /// <summary>
        /// 大于
        /// </summary>
        GreaterThan,
        /// <summary>
        /// 大于等于
        /// </summary>
        GreaterEqualsThan,
        /// <summary>
        /// 小于
        /// </summary>
        LessThan,
        /// <summary>
        /// 小于等于
        /// </summary>
        LessEqualsThan,
    }
}
