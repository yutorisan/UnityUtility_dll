using System;
using UniRx;

namespace UnityUtility.Rx.Operators
{
    public static partial class ObservableEx
    {
        /// <summary>
        /// <code>Where(x => x != null)</code>の簡略記法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static IObservable<T> ExcludeNull<T>(this IObservable<T> source) =>
            source.Where(n => n != null);
        /// <summary>
        /// <code>Where(x => f(x) != null)</code>の簡略記法
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TSelector"></typeparam>
        /// <param name="source"></param>
        /// <param name="selector"></param>
        /// <returns></returns>
        public static IObservable<TSource> ExcludeNull<TSource, TSelector>(this IObservable<TSource> source, Func<TSource, TSelector> selector) =>
            source.Where(n => selector(n) != null);
    }
}

