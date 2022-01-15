using System.Collections.Generic;
using UnityUtility.Modules;

namespace UnityUtility.Collections
{
    public interface IReadOnlyMap<out T> : IEnumerable<T>
    {
        T this[int column, int row] { get; }
        T this[Cell cell] { get; }
        /// <summary>
        /// マップの行数を取得します
        /// </summary>
        int RowCount { get; }
        /// <summary>
        /// マップの列数を取得します
        /// </summary>
        int ColumnCount { get; }
        /// <summary>
        /// マップのプレーン値を取得します
        /// </summary>
        T PlainValue { get; }
        /// <summary>
        /// マップにプレーン値が含まれているかどうかを取得します
        /// </summary>
        bool HasPlain { get; }
        /// <summary>
        /// 特定の行に含まれる要素を取得します
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        IEnumerable<T> GetRowEnumerable(int row);
        /// <summary>
        /// 特定の列に含まれる要素を取得します
        /// </summary>
        /// <param name="column"></param>
        /// <returns></returns>
        IEnumerable<T> GetColumnEnumerable(int column);
    }
}

