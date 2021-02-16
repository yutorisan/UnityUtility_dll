using System;
using System.Collections.Generic;
using System.Linq;

namespace UnityUtility.Linq
{
    public static class DictionaryExtensions
    {
        /// <summary>
        /// KeyValuePairのシーケンスからKeyを取り出したシーケンスを取得します。
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="kvps"></param>
        /// <returns></returns>
        public static IEnumerable<TKey> SelectKey<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue>> kvps) => kvps.Select(kvp => kvp.Key);
        /// <summary>
        /// KeyValuePairのシーケンスからValueを取り出したシーケンスを取得します。
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="kvps"></param>
        /// <returns></returns>
        public static IEnumerable<TValue> SelectValue<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue>> kvps) => kvps.Select(kvp => kvp.Value);
        /// <summary>
        /// 辞書型のValueに対してWhereを実行します。
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="dic"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public static IEnumerable<KeyValuePair<TKey, TValue>> DictionaryWhere<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue>> dic, Predicate<TValue> predicate) =>
            dic.Where(kvp => predicate(kvp.Value));
        /// <summary>
        /// 辞書型のValueに対してSelectを実行します。
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValueSource"></typeparam>
        /// <typeparam name="TValueResult"></typeparam>
        /// <param name="dic"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public static IEnumerable<KeyValuePair<TKey, TValueResult>> DictionarySelect<TKey, TValueSource, TValueResult>(this IEnumerable<KeyValuePair<TKey, TValueSource>> dic, Func<TValueSource, TValueResult> func) =>
            dic.Select(kvp => new KeyValuePair<TKey, TValueResult>(kvp.Key, func(kvp.Value)));
        /// <summary>
        /// 辞書型のIEnumerableインターフェイスを直接Dictionary具象クラスに変換します。
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="dic"></param>
        /// <returns></returns>
        public static Dictionary<TKey, TValue> ToDictionary<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue>> dic) =>
            dic.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        /// <summary>
        /// 2つの辞書型のキー同士に対してSequenceEqualsを実行した結果を返します。
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue1"></typeparam>
        /// <typeparam name="TValue2"></typeparam>
        /// <param name="sourceDic"></param>
        /// <param name="otherDic"></param>
        /// <returns></returns>
        public static bool KeyMapEquals<TKey, TValue1, TValue2>(this IEnumerable<KeyValuePair<TKey, TValue1>> sourceDic, IEnumerable<KeyValuePair<TKey, TValue2>> otherDic) =>
            sourceDic.SelectKey().SequenceEqual(otherDic.SelectKey());
        /// <summary>
        /// キーマップが同一の2つの辞書型に対して、それぞれの同一Keyに対するValue同士を合成してひとつの辞書型を生成します。
        /// </summary>
        /// <typeparam name="TKey">合成する辞書型のキー</typeparam>
        /// <typeparam name="TSourceValue1">合成する辞書型のValue</typeparam>
        /// <typeparam name="TSourceValue2">合成する辞書型のValue</typeparam>
        /// <typeparam name="TResultValue">合成結果の辞書型のValue</typeparam>
        /// <param name="source"></param>
        /// <param name="second"></param>
        /// <param name="resultSelector"></param>
        /// <exception cref="InvalidOperationException">キーマップが同一ではない場合にスローされます。</exception>
        /// <returns></returns>
        public static IEnumerable<KeyValuePair<TKey, TResultValue>> DictionaryZip<TKey, TSourceValue1, TSourceValue2, TResultValue>(this IEnumerable<KeyValuePair<TKey, TSourceValue1>> source,
                                                                                                                                    IEnumerable<KeyValuePair<TKey, TSourceValue2>> second,
                                                                                                                                    Func<TSourceValue1, TSourceValue2, TResultValue> resultSelector)
        {
            if (!source.KeyMapEquals(second)) throw new InvalidOperationException("キーマップが一致していないため合成できません。");
            foreach (var sourceKvp in source)
            {
                yield return new KeyValuePair<TKey, TResultValue>(sourceKvp.Key, resultSelector(sourceKvp.Value, second.First(kvp2 => kvp2.Key.Equals(sourceKvp.Key)).Value));
            }
        }
        /// <summary>
        /// 辞書型のValueがnullのものを排除します。
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static IEnumerable<KeyValuePair<TKey, TValue>> ExcludeNullValue<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue>> source) =>
            source.Where(kvp => kvp.Value != null);
    }
}
