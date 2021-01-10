using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UnityUtility.Extensions
{
    public static class IEnumerableExtensions
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
            var l = label ?? $"[{label}]";
            foreach (var item in source)
            {
                UnityEngine.Debug.Log(l + item.ToString());
            }
            return source;
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
            source.Select(selector).Debug(label);
            return source;
        }
    }
}
