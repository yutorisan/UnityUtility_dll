using System;
using System.Collections.Generic;
using System.Linq;

namespace UnityUtility.Linq
{
    public static partial class LinqExtensions
    {
        /// <summary>
        /// selectorに指定した関数の戻り値が最大となる要素をすべて取得します。
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TCompare"><see cref="IEquatable{T}"/>と<see cref="IComparable{T}"/>を実装したオブジェクト</typeparam>
        /// <param name="source"></param>
        /// <param name="selector"></param>
        /// <returns></returns>
        public static IEnumerable<TSource> MaxBy<TSource, TCompare>(this IEnumerable<TSource> source, Func<TSource, TCompare> selector)
            where TCompare : IComparable<TCompare>, IEquatable<TCompare>
        {
            var ordered = source.OrderByDescending(selector);
            var max = selector(ordered.First());
            return ordered.Where(src => selector(src).Equals(max));
        }

        /// <summary>
        /// selectorに指定した関数の戻り値が最小となる要素をすべて取得します。
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TCompare"><see cref="IEquatable{T}"/>と<see cref="IComparable{T}"/>を実装したオブジェクト</typeparam>
        /// <param name="source"></param>
        /// <param name="selector"></param>
        /// <returns></returns>
        public static IEnumerable<TSource> MinBy<TSource, TCompare>(this IEnumerable<TSource> source, Func<TSource, TCompare> selector)
            where TCompare : IComparable<TCompare>, IEquatable<TCompare>
        {
            var ordered = source.OrderBy(selector);
            var max = selector(ordered.First());
            return ordered.Where(src => selector(src).Equals(max));
        }

        /// <summary>
        /// MaxBy().First()
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TComapre"></typeparam>
        /// <param name="source"></param>
        /// <param name="selector"></param>
        /// <returns></returns>
        public static TSource MaxByFirst<TSource, TComapre>(this IEnumerable<TSource> source, Func<TSource, TComapre> selector)
            where TComapre : IComparable<TComapre>, IEquatable<TComapre> =>
                source.MaxBy(selector).First();

        /// <summary>
        /// MinBy().First()
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TComapre"></typeparam>
        /// <param name="source"></param>
        /// <param name="selector"></param>
        /// <returns></returns>
        public static TSource MinByFirst<TSource, TComapre>(this IEnumerable<TSource> source, Func<TSource, TComapre> selector)
            where TComapre : IComparable<TComapre>, IEquatable<TComapre> =>
                source.MinBy(selector).First();
    }
}
