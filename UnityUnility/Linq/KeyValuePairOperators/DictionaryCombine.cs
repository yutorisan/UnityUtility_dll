using System;
using System.Linq;
using System.Collections.Generic;

namespace UnityUtility.Linq
{
    public static partial class LinqExtensions
    {
        /// <summary>
        /// キーが同一な2つの辞書型を指定された方法に従って合成し、新しい辞書型を生成します。
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TSourceValue1"></typeparam>
        /// <typeparam name="TSourceValue2"></typeparam>
        /// <typeparam name="TResultValue"></typeparam>
        /// <param name="source1"></param>
        /// <param name="source2"></param>
        /// <param name="resultSelector"></param>
        /// <param name="defaultValue1Selector">source2のみに存在するキーがある場合に使用するsource1の値セレクター</param>
        /// <param name="defaultValue2Selector">source1のみに存在するキーがある場合に使用するsource2の値セレクター</param>
        /// <returns></returns>
        public static IEnumerable<KeyValuePair<TKey, TResultValue>> DictionaryCombine<TKey, TSourceValue1, TSourceValue2, TResultValue>
            (this IEnumerable<KeyValuePair<TKey, TSourceValue1>> source1,
             IEnumerable<KeyValuePair<TKey, TSourceValue2>> source2,
             Func<TSourceValue1, TSourceValue2, TResultValue> resultSelector,
             Func<TSourceValue1> defaultValue1Selector,
             Func<TSourceValue2> defaultValue2Selector)
        {
            if (source1 is null)
            {
                throw new ArgumentNullException(nameof(source1));
            }

            if (source2 is null)
            {
                throw new ArgumentNullException(nameof(source2));
            }

            if (resultSelector is null)
            {
                throw new ArgumentNullException(nameof(resultSelector));
            }

            if (defaultValue1Selector is null)
            {
                throw new ArgumentNullException(nameof(defaultValue1Selector));
            }

            if (defaultValue2Selector is null)
            {
                throw new ArgumentNullException(nameof(defaultValue2Selector));
            }
            //まずはsource1を精査
            foreach (var element1 in source1)
            {
                TKey key1 = element1.Key;
                TSourceValue1 value1 = element1.Value;
                TSourceValue2 value2 = source2.FirstOrAny(kvp => kvp.Key.Equals(key1), new KeyValuePair<TKey, TSourceValue2>(default, defaultValue2Selector())).Value;

                yield return new KeyValuePair<TKey, TResultValue>(
                    key1,
                    resultSelector(value1, value2));
            }

            //次にsource2だけに存在する要素の精査
            foreach (var element2 in source2)
            {
                if (!source1.ContainsKey(element2.Key))
                {
                    yield return new KeyValuePair<TKey, TResultValue>(
                        element2.Key,
                        resultSelector(defaultValue1Selector(), element2.Value));
                }
            }
        }

        /// <summary>
        /// キーが同一な2つの辞書型を指定された方法に従って合成し、新しい辞書型を生成します。
        /// 片方にしかないキーが存在した場合、もう片方はdefaultとして合成します。
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TSourceValue1"></typeparam>
        /// <typeparam name="TSourceValue2"></typeparam>
        /// <typeparam name="TResultValue"></typeparam>
        /// <param name="source1"></param>
        /// <param name="source2"></param>
        /// <param name="resultSelector"></param>
        /// <returns></returns>
        public static IEnumerable<KeyValuePair<TKey, TResultValue>> DictionaryCombine<TKey, TSourceValue1, TSourceValue2, TResultValue>
            (this IEnumerable<KeyValuePair<TKey, TSourceValue1>> source1,
             IEnumerable<KeyValuePair<TKey, TSourceValue2>> source2,
             Func<TSourceValue1, TSourceValue2, TResultValue> resultSelector) =>
                source1.DictionaryCombine(source2, resultSelector, () => default, () => default);

    }
}