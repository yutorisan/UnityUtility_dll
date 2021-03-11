using System;
using System.Linq;
using System.Collections.Generic;

namespace UnityUtility.Linq
{
    public static partial class LinqExtensions
    {
        /// <summary>
        /// シーケンスを最初から順番に調べ、指定した条件に最初に合致した要素を返します。
        /// 見つからなかった場合、指定した任意の値を返します。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="predicate">検索条件</param>
        /// <param name="notFoundValue">見つからなかった場合に返す値</param>
        /// <returns></returns>
        public static T FirstOrAny<T>(this IEnumerable<T> source, Predicate<T> predicate, T notFoundValue)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (predicate is null)
            {
                throw new ArgumentNullException(nameof(predicate));
            }

            foreach (var item in source)
            {
                if (predicate(item)) return item; 
            }
            return notFoundValue;
        }

        /// <summary>
        /// シーケンスを最後から順番に調べ、指定した条件に最初に合致した要素を返します。
        /// 見つからなかった場合、指定した任意の値を返します。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="predicate">検索条件</param>
        /// <param name="notFoundValue">見つからなかった場合に返す値</param>
        /// <returns></returns>
        public static T LastOrAny<T>(this IEnumerable<T> source, Predicate<T> predicate, T notFoundValue) =>
            source.Reverse().FirstOrAny(predicate, notFoundValue);
    }
}