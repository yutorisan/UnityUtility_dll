using System;
using System.Linq;
using System.Collections.Generic;

namespace UnityUtility.Linq
{
    public static partial class LinqExtensions
    {
        /// <summary>
        /// KeyValuePairのシーケンスからKeyを取り出したシーケンスを取得します。
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="kvps"></param>
        /// <returns></returns>
        public static IEnumerable<TKey> Keys<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue>> kvps)
        {
            if (kvps is null)
            {
                throw new ArgumentNullException(nameof(kvps));
            }

            return kvps.Select(kvp => kvp.Key);
        }
    }
}