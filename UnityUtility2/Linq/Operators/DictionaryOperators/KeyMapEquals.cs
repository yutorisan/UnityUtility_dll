using System;
using System.Linq;
using System.Collections.Generic;

namespace UnityUtility.Linq
{
    public static partial class LinqExtensions
    {
        /// <summary>
        /// 2つの辞書型のキー同士に対してSequenceEqualsを実行した結果を返します。
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue1"></typeparam>
        /// <typeparam name="TValue2"></typeparam>
        /// <param name="sourceDic"></param>
        /// <param name="otherDic"></param>
        /// <returns></returns>
        public static bool KeyMapEquals<TKey, TValue1, TValue2>(this IEnumerable<KeyValuePair<TKey, TValue1>> sourceDic,
                                                                IEnumerable<KeyValuePair<TKey, TValue2>> otherDic)
        {
            if (sourceDic is null)
            {
                throw new ArgumentNullException(nameof(sourceDic));
            }

            if (otherDic is null)
            {
                throw new ArgumentNullException(nameof(otherDic));
            }

            return sourceDic.Keys().SequenceEqual(otherDic.Keys());
        }
    }
}