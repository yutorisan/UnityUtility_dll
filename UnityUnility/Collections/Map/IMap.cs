using System;
using System.Collections.Generic;
using UnityUtility.Enums;
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
    public interface IMap<T> : IReadOnlyMap<T>
    {
        new T this[int column, int row] { get; set; }
        new T this[Cell cell] { get; set; }
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
    public interface IMapSlim<T> : IMap<T> where T : unmanaged
    {
        Span<T> GetRowSpan(int row);
        Span<T> GetAllSpan();
    }
    public interface IExpandableMap<T> : IMap<T>
    {
        /// <summary>
        /// 列を追加します
        /// </summary>
        /// <param name="lr">左右どちらから増やすか</param>
        /// <param name="amount">追加する列数(負の値で縮小)</param>
        /// <exception cref="ArgumentOutOfRangeException">追加または削除する列数が不正の場合</exception>
        void ExpandColumn(LeftRight lr, int amount);
        /// <summary>
        /// 行を追加します
        /// </summary>
        /// <param name="ud">上下どちらから増やすか</param>
        /// <param name="amount">追加する行数(負の値で縮小)</param>
        /// <exception cref="ArgumentOutOfRangeException">追加または削除する行数が不正の場合</exception>
        void ExpandRow(UpDown ud, int amount);
    }
}

