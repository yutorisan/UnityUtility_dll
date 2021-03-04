using System;
using System.Linq;
using System.Collections.Generic;

namespace UnityUtility.Linq
{
    public static partial class LinqExtensions
    {
        /// <summary>
        /// 辞書型のIEnumerableインターフェイスを直接Dictionary具象クラスに変換します。
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="dic"></param>
        /// <returns></returns>
        public static Dictionary<TKey, TValue> ToDictionary<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue>> dic)
        {
            if (dic is null)
            {
                throw new ArgumentNullException(nameof(dic));
            }

            return dic.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        }
    }
}