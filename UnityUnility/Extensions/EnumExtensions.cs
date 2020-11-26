using System;
using System.Collections.Generic;
using System.Linq;

namespace UnityUtility
{
    public static class EnumExtensions
    {
        /// <summary>
        /// 列挙型のすべてにメンバを取得します。
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <returns></returns>
        public static IEnumerable<TEnum> GetEnumValues<TEnum>()
        {
            return Enum.GetValues(typeof(TEnum)).Cast<TEnum>();
        }
    }
}
