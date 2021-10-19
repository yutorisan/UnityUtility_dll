using System;
using System.Collections.Generic;
using System.Linq;

namespace UnityUtility.Linq
{
    public static partial class LinqExtensions
    {
        /// <summary>
        /// IEnumerableシーケンスのすべての値をDebug.Logで出力します
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="label">指定した文字列が先頭に[]囲みで出力されます</param>
        /// <returns></returns>
        public static IEnumerable<T> Debug<T>(this IEnumerable<T> source, string label = null)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            var l = label != null ? $"[{label}]" : string.Empty;
            foreach (var item in source)
            {
                UnityEngine.Debug.Log(l + item.ToString());
                yield return item;
            }
        }
        /// <summary>
        /// IEnumerableシーケンスのすべての値をDebug.Logで出力します
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="selector">出力するオブジェクトを指定します</param>
        /// <param name="label">指定した文字列が先頭に[]囲みで出力されます</param>
        /// <returns></returns>
        public static IEnumerable<TSource> Debug<TSource, TSelector>(this IEnumerable<TSource> source, Func<TSource, TSelector> selector, string label = null)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (selector is null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            if (string.IsNullOrEmpty(label))
            {
                throw new ArgumentException($"'{nameof(label)}' を null または空にすることはできません", nameof(label));
            }

            var l = label != null ? $"[{label}]" : string.Empty;
            foreach (var item in source)
            {
                UnityEngine.Debug.Log(l + selector(item).ToString());
                yield return item;
            }
        }
    }
}
