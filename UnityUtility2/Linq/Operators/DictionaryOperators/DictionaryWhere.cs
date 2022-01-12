using System;
using System.Linq;
using System.Collections.Generic;

namespace UnityUtility.Linq
{
    public static partial class LinqExtensions
    {
        /// <summary>
        /// 辞書型のValueに対してWhereを実行します。
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="dic"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public static IEnumerable<KeyValuePair<TKey, TValue>> DictionaryWhere<TKey, TValue>
            (this IEnumerable<KeyValuePair<TKey, TValue>> dic, Predicate<TValue> predicate)
        {
            if (dic is null)
            {
                throw new ArgumentNullException(nameof(dic));
            }

            if (predicate is null)
            {
                throw new ArgumentNullException(nameof(predicate));
            }

            return dic.Where(kvp => predicate(kvp.Value));
        }
    }
}