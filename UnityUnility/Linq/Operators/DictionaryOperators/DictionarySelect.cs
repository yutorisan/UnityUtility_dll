using System;
using System.Linq;
using System.Collections.Generic;

namespace UnityUtility.Linq
{
    public static partial class LinqExtensions
    {
        /// <summary>
        /// 辞書型のValueに対してSelectを実行します。
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValueSource"></typeparam>
        /// <typeparam name="TValueResult"></typeparam>
        /// <param name="dic"></param>
        /// <param name="selector"></param>
        /// <returns></returns>
        public static IEnumerable<KeyValuePair<TKey, TValueResult>> DictionarySelect<TKey, TValueSource, TValueResult>
            (this IEnumerable<KeyValuePair<TKey, TValueSource>> dic, Func<TValueSource, TValueResult> selector)
        {
            if (dic is null)
            {
                throw new ArgumentNullException(nameof(dic));
            }

            if (selector is null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            return dic.Select(kvp => new KeyValuePair<TKey, TValueResult>(kvp.Key, selector(kvp.Value)));
        }

        /// <summary>
        /// 辞書型のValueに対してSelectを実行します。インデックスとしてKeyが取得できます。
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValueSource"></typeparam>
        /// <typeparam name="TValueResult"></typeparam>
        /// <param name="dic"></param>
        /// <param name="selector"></param>
        /// <returns></returns>
        public static IEnumerable<KeyValuePair<TKey, TValueResult>> DictionarySelect<TKey, TValueSource, TValueResult>
            (this IEnumerable<KeyValuePair<TKey, TValueSource>> dic, Func<TValueSource, TKey, TValueResult> selector)
        {
            if (dic is null)
            {
                throw new ArgumentNullException(nameof(dic));
            }

            if (selector is null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            return dic.Select(kvp => new KeyValuePair<TKey, TValueResult>(kvp.Key, selector(kvp.Value, kvp.Key)));
        }
    }
}