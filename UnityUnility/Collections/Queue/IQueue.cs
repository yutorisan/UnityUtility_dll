using System;
using System.Collections.Generic;

namespace UnityUtility.Collections
{
    public interface IQueue<T> : IReadOnlyCollection<T>, IEnumerable<T>
    {
        /// <summary>
        /// キューに値を投入する
        /// </summary>
        /// <param name="item"></param>
        void Enqueue(T item);
        /// <summary>
        /// キューから値を取り出す
        /// </summary>
        /// <returns></returns>
        T Dequeue();
        /// <summary>
        /// キューから値を削除せずに値を取得する
        /// </summary>
        /// <returns></returns>
        T Peek();
        /// <summary>
        /// 要素をすべて削除します。
        /// </summary>
        void Clear();
    }
}
