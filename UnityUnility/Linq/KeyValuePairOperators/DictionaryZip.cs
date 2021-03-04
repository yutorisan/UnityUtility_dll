using System;
using System.Linq;
using System.Collections.Generic;

namespace UnityUtility.Linq
{
    public static partial class LinqExtensions
    {
        /// <summary>
        /// キーが同一な2つの辞書型を指定された方法に従って合成し、新しい辞書型を生成します。
        /// 片方にしかないキーが存在した場合、その項目は排除されます。
        /// </summary>
        /// <typeparam name="TKey">合成する辞書型のキー</typeparam>
        /// <typeparam name="TSourceValue1">合成する辞書型のValue</typeparam>
        /// <typeparam name="TSourceValue2">合成する辞書型のValue</typeparam>
        /// <typeparam name="TResultValue">合成結果の辞書型のValue</typeparam>
        /// <param name="source1"></param>
        /// <param name="source2"></param>
        /// <param name="resultSelector"></param>
        /// <exception cref="InvalidOperationException">キーマップが同一ではない場合にスローされます。</exception>
        /// <returns></returns>
        public static IEnumerable<KeyValuePair<TKey, TResultValue>> DictionaryZip<TKey, TSourceValue1, TSourceValue2, TResultValue>
            (this IEnumerable<KeyValuePair<TKey, TSourceValue1>> source1,
             IEnumerable<KeyValuePair<TKey, TSourceValue2>> source2,
             Func<TSourceValue1, TSourceValue2, TResultValue> resultSelector)
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

            foreach (var element1 in source1)
            {
                //両方にキーが存在するものだけyield return
                if (source2.Any(kvp => kvp.Key.Equals(element1.Key)))
                    yield return new KeyValuePair<TKey, TResultValue>(
                        element1.Key,
                        resultSelector(element1.Value, source2.First(kvp => kvp.Key.Equals(element1.Key)).Value));
            }
        }
    }
}