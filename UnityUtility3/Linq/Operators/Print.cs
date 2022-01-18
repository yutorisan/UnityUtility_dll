using System;
using System.Collections.Generic;

namespace UnityUtility.Linq
{
    public static partial class LinqExtensions
    {
        public static void Print<T>(this IEnumerable<T> source, string label = null)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            var l = label != null ? $"[{label}]" : string.Empty;
            foreach (var item in source)
            {
                UnityEngine.Debug.Log(l + item.ToString());
            }
        }

        public static void Print<TSource, TSelector>(this IEnumerable<TSource> source, Func<TSource, TSelector> selector, string label = null)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (selector is null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            var l = label != null ? $"[{label}]" : string.Empty;
            foreach (var item in source)
            {
                UnityEngine.Debug.Log(l + selector(item).ToString());
            }
        }
    }
}

