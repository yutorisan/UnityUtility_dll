using System;
using System.Linq;
using System.Collections.Generic;

namespace UnityUtility.Linq
{
    public static partial class LinqExtensions
    {
        /// <summary>
        /// KeyValuePairのシーケンスからValueを取り出したシーケンスを取得します。
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="kvps"></param>
        /// <returns></returns>
        public static IEnumerable<TValue> Values<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue>> kvps)
        {
            if (kvps is null)
            {
                throw new ArgumentNullException(nameof(kvps));
            }

            return kvps.Select(kvp => kvp.Value);
        }
    }
}