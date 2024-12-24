using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JAM8.Algorithms.Numerics
{
    // 枚举类型，用于表示条件判断的逻辑
    public enum ConditionLogic
    {
        AND, // 所有条件都必须满足
        OR   // 满足任意一个条件即可
    }

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
