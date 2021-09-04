using System;
using System.Linq;
using System.Collections.Generic;

namespace UnityUtility.Linq
{
    public static partial class LinqExtensions
    {
        /// <summary>
        /// 2つのシーケンスを合成して新しいシーケンスを生成します。
        /// 要素数が異なる場合、先に終端に達したシーケンスは以後default値が使用されて合成されます。
        /// →合成後のシーケンスの要素数は合成元のシーケンスの要素数のうち大きい方となります
        /// </summary>
        /// <typeparam name="TSource1"></typeparam>
        /// <typeparam name="TSource2"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="source1"></param>
        /// <param name="source2"></param>
        /// <param name="combiner"></param>
        /// <returns></returns>
        public static IEnumerable<TResult> Combine<TSource1, TSource2, TResult>(this IEnumerable<TSource1> source1,
                                                                                IEnumerable<TSource2> source2,
                                                                                Func<TSource1, TSource2, TResult> combiner)
        {
            if (source1 is null) throw new ArgumentNullException(nameof(source1));
            if (source2 is null) throw new ArgumentNullException(nameof(source2));
            if (combiner is null) throw new ArgumentNullException(nameof(combiner));

            var enumerator1 = source1.GetEnumerator();
            var enumerator2 = source2.GetEnumerator();
            while (true)
            {
                TSource1 element1;
                TSource2 element2;
                bool still1 = enumerator1.MoveNext();
                bool still2 = enumerator2.MoveNext();
                if (!still1 && !still2) yield break;
                element1 = still1 ? enumerator1.Current : default;
                element2 = still2 ? enumerator2.Current : default;

                yield return combiner(element1, element2);
            }
        }
    }
}