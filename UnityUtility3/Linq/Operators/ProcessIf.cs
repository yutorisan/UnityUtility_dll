using System;
using System.Collections.Generic;

namespace UnityUtility.Linq
{
    public static partial class LinqExtensions
    {
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
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (process is null)
            {
                throw new ArgumentNullException(nameof(process));
            }

            if (predicate is null)
            {
                throw new ArgumentNullException(nameof(predicate));
            }

            foreach (var element in source)
            {
                yield return predicate(element) ? process(element) : element;
            }
        }
    }
}