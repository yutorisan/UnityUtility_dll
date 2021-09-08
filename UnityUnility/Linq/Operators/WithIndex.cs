using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityUtility.Linq.Modules;

namespace UnityUtility.Linq
{
    public static partial class LinqExtensions
    {
        /// <summary>
        /// シーケンスにインデックス情報を付加します
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static IWithIndexEnumerable<T> WithIndex<T>(this IEnumerable<T> source) =>
            new WithIndexEnumerable<T>(source);
    }
}
