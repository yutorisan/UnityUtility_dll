using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

namespace UnityUtility.Linq
{
    public static class LinqExtensions
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
            var l = label != null ? $"[{label}]" : string.Empty;
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
        /// <summary>
        /// 条件に一致する場合、シーケンスの要素に型が不変の任意の処理を加えて後続に流します。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="process">型が不変の任意の処理</param>
        /// <param name="condition">条件</param>
        /// <returns></returns>
        public static IEnumerable<T> ProcessIf<T>(this IEnumerable<T> source, Func<T, T> process, bool condition)
        {
            return condition ? source.Select(process) : source;
        }
        /// <summary>
        /// シーケンスの各要素が条件に一致する場合、型が不変の任意の処理を加えて後続に流します。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="process">型が不変の任意の処理</param>
        /// <param name="predicate">シーケンスの要素に適用する条件</param>
        /// <returns></returns>
        public static IEnumerable<T> ProcessIf<T>(this IEnumerable<T> source, Func<T, T> process, Predicate<T> predicate)
        {
            foreach (var element in source)
            {
                yield return predicate(element) ? process(element) : element;
            }
        }
    }
}
