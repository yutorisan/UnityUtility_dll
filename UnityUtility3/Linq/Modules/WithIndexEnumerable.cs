using System.Collections;
using System.Collections.Generic;
using UnityUtility.Modules;

namespace UnityUtility.Linq.Modules
{
    /// <summary>
    /// 要素のインデックス情報が取得できる<see cref="IEnumerable{T}"/>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IWithIndexEnumerable<T> : IEnumerable<WithIndex<T>>
    {
        //なし
    }

    internal class WithIndexEnumerable<T> : IWithIndexEnumerable<T>
    {
        private readonly IEnumerable<T> source;

        public WithIndexEnumerable(IEnumerable<T> source) => this.source = source;

        public IEnumerator<WithIndex<T>> GetEnumerator()
        {
            int index = 0;
            foreach (var item in source)
            {
                yield return new WithIndex<T>(item, index++);
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    public static class Extensions
    {
        /// <summary>
        /// オブジェクトを<see cref="WithIndex{T}(T, int)"/>にラップする拡張メソッドです
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static WithIndex<T> WithIndex<T>(this T value, int index) => new WithIndex<T>(value, index);
    }
}
