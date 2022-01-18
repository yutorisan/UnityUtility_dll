using System;
using System.Linq;
using System.Collections.Generic;

namespace UnityUtility.Linq
{
    public static partial class LinqExtensions
    {
        /// <summary>
        /// 2つのシーケンスを合成して新しいシーケンスを生成します。
        /// →合成後のシーケンスの要素数は合成元のシーケンスの要素数のうち大きい方となります
        /// </summary>
        /// <typeparam name="TSource1">1つ目の合成元シーケンスの型</typeparam>
        /// <typeparam name="TSource2">2つ目の合成元シーケンスの型</typeparam>
        /// <typeparam name="TResult">合成後のシーケンスの型</typeparam>
        /// <param name="source1">1つ目の合成元シーケンス</param>
        /// <param name="source2">2つ目の合成元シーケンス</param>
        /// <param name="combiner">合成方法</param>
        /// <param name="defaultValue1Selector">1つ目のシーケンスの要素が不足している場合の値の補完方法</param>
        /// <param name="defaultValue2Selector">2つ目のシーケンスの要素が不足している場合の値の補完方法</param>
        /// <returns></returns>
        public static IEnumerable<TResult> Combine<TSource1, TSource2, TResult>(
            this IEnumerable<TSource1> source1,
            IEnumerable<TSource2> source2,
            Func<TSource1, TSource2, TResult> combiner,
            Func<TSource2, TSource1> defaultValue1Selector,
            Func<TSource1, TSource2> defaultValue2Selector)
        {
            if (source1 is null) throw new ArgumentNullException(nameof(source1));
            if (source2 is null) throw new ArgumentNullException(nameof(source2));
            if (combiner is null) throw new ArgumentNullException(nameof(combiner));
            if (defaultValue1Selector is null) throw new ArgumentNullException(nameof(defaultValue1Selector));
            if (defaultValue2Selector is null) throw new ArgumentNullException(nameof(defaultValue2Selector));

            var enumerator1 = source1.GetEnumerator();
            var enumerator2 = source2.GetEnumerator();
            while (true)
            {
                TSource1 element1;
                TSource2 element2;
                bool still1 = enumerator1.MoveNext();
                bool still2 = enumerator2.MoveNext();
                if (!still1 && !still2) yield break;
                element1 = still1 ? enumerator1.Current : defaultValue1Selector(enumerator2.Current);
                element2 = still2 ? enumerator2.Current : defaultValue2Selector(enumerator1.Current);

                yield return combiner(element1, element2);
            }
        }
        /// <summary>
        /// 2つのシーケンスを合成して新しいシーケンスを生成します。
        /// 要素数が異なる場合、先に終端に達したシーケンスは以後default値が使用されて合成されます。
        /// →合成後のシーケンスの要素数は合成元のシーケンスの要素数のうち大きい方となります
        /// </summary>
        /// <typeparam name="TSource1">1つ目の合成元シーケンスの型</typeparam>
        /// <typeparam name="TSource2">2つ目の合成元シーケンスの型</typeparam>
        /// <typeparam name="TResult">合成後のシーケンスの型</typeparam>
        /// <param name="source1">1つ目の合成元シーケンス</param>
        /// <param name="source2">2つ目の合成元シーケンス</param>
        /// <param name="combiner">合成方法</param>
        public static IEnumerable<TResult> Combine<TSource1, TSource2, TResult>(
            this IEnumerable<TSource1> source1,
            IEnumerable<TSource2> source2,
            Func<TSource1, TSource2, TResult> combiner) =>
            Combine(source1, source2, combiner, _ => default, _ => default);

    }
}