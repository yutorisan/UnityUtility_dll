using System;
using UnityUtility.Modules;

namespace UnityUtility.Collections
{
    public interface IFixedMap<T> : IReadOnlyMap<T>
    {
        new T this[int column, int row] { get; set; }
        /// <summary>
        /// 特定の行の要素を順番に書き換えます
        /// </summary>
        /// <param name="row"></param>
        /// <param name="rewriter"></param>
        void ReWriteRow(int row, Func<T, T> rewriter);
        /// <summary>
        /// 特定の列の要素を順番に書き換えます
        /// </summary>
        /// <param name="column"></param>
        /// <param name="rewriter"></param>
        void ReWriteColumn(int column, Func<T, T> rewriter);
        /// <summary>
        /// 全ての要素を順番に書き換えます
        /// </summary>
        /// <param name="rewriter"></param>
        void ReWriteAll(Func<T, T> rewriter);
        /// <summary>
        /// マップのシャローコピーを作成します。
        /// </summary>
        Map<T> Clone();
    }
}

