using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using UniRx;
using UnityUtility.Enums;
using UnityUtility.Linq.Modules;
using UnityUtility.Modules;

namespace UnityUtility.Collections
{
    public class Map<T> : IMap<T>, IReactiveMap<T>
    {
        #region constant
        protected static readonly string CannotConstructZeroSizeMapErrMsg = "行または列が0以下のマップは作成できません";
        protected static readonly string CannotConstructInvalidSizeMapErrMsg = "行列数とソース配列のサイズが合致しないためインスタンスを作成できません";
        protected static readonly string OutOfRangeErrMsg = "マップの範囲を超えています";
        protected static readonly string ExpandOutOfRangeErrMsg = "マップを拡張または縮小しようとしましたが、マップの有効範囲を超えました";
        #endregion

        #region field
        protected T[] m_map;

        private ISubject<MapCellReplaceEvent<T>> m_cellReplaceSubject;
        private ISubject<MapColumnReplaceEvent<T>> m_columnReplaceSubject;
        private ISubject<MapRowReplaceEvent<T>> m_rowReplaceSubject;
        private ISubject<MapReplaceEvent<T>> m_replaceSubject;
        private ISubject<MapExpandEvent> m_expandSubject;
        #endregion

        #region constructor
        /// <summary>
        /// 空のマップを新規作成します
        /// </summary>
        /// <param name="column">列数</param>
        /// <param name="row">行数</param>
        /// <param name="plainValue">マップのデフォルト値</param>
        /// <exception cref="ArgumentOutOfRangeException">行列数が0以下の場合</exception>
        public Map(int column, int row, T plainValue = default)
        {
            if (column <= 0 || row <= 0) throw new ArgumentOutOfRangeException(CannotConstructZeroSizeMapErrMsg);
            this.ColumnCount = column;
            this.RowCount = row;
            this.PlainValue = plainValue;
            this.m_map = plainValue.IsDefault() ?
                            new T[column * row] :
                            Enumerable.Repeat(plainValue, column * row).ToArray();
        }
        /// <summary>
        /// 一次元配列からマップを作成します
        /// </summary>
        /// <param name="column">列数</param>
        /// <param name="row">行数</param>
        /// <param name="source1dArray">ソースとなる一次元配列</param>
        /// <param name="plainValue">マップのデフォルト値</param>
        /// <exception cref="ArgumentOutOfRangeException">行列数が0以下の場合</exception>
        /// <exception cref="ArgumentNullException">ソース配列がnullの場合</exception>
        /// <exception cref="ArgumentException">行列数の乗数とソース配列の要素数が合致しない場合</exception>
        public Map(int column, int row, T[] source1dArray, T plainValue = default)
        {
            if (column <= 0 || row <= 0) throw new ArgumentOutOfRangeException(CannotConstructZeroSizeMapErrMsg);
            if (source1dArray == null) throw new ArgumentNullException(nameof(source1dArray));
            if (source1dArray.Length != column * row) throw new ArgumentException(CannotConstructInvalidSizeMapErrMsg);
            this.ColumnCount = column;
            this.RowCount = row;
            this.PlainValue = plainValue;
            this.m_map = source1dArray.Clone() as T[];
        }
        /// <summary>
        /// 二次元配列からマップを作成します
        /// </summary>
        /// <param name="source2dArray">ソースとなる二次元配列</param>
        /// <param name="plainValue">マップのデフォルト値</param>
        /// <exception cref="ArgumentNullException">ソース配列がnullの場合</exception>
        public Map(T[,] source2dArray, T plainValue = default)
        {
            if (source2dArray == null) throw new ArgumentNullException(nameof(source2dArray));
            this.ColumnCount = source2dArray.GetLength(1);
            this.RowCount = source2dArray.GetLength(0);
            this.PlainValue = plainValue;
            this.m_map = source2dArray.Cast<T>().ToArray();
        }
        /// <summary>
        /// 元となる別のMapをクローン元として新しいMapを作成します（シャローコピー）
        /// </summary>
        /// <param name="sourceMap"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public Map(IReadOnlyMap<T> sourceMap)
        {
            if (sourceMap is null) throw new ArgumentNullException(nameof(sourceMap));
            this.ColumnCount = sourceMap.ColumnCount;
            this.RowCount = sourceMap.RowCount;
            this.PlainValue = sourceMap.PlainValue;
            this.m_map = sourceMap.ToArray();
        }
        #endregion

        #region indexer
        public T this[int column, int row]
        {
            get
            {
                if (!IsWithInRange(column, row)) throw new IndexOutOfRangeException();
                return m_map[to1DIndex(column, row)];
            }
            set
            {
                if (!IsWithInRange(column, row)) throw new IndexOutOfRangeException();
                int index = to1DIndex(column, row);
                T old = m_map[index];
                m_map[index] = value;
                m_cellReplaceSubject?.OnNext(new MapCellReplaceEvent<T>(new Cell(column, row), old, value));
            }
        }
        public T this[Cell cell]
        {
            get => this[cell.Column, cell.Row];
            set => this[cell.Column, cell.Row] = value;
        }
        T IFixedMap<T>.this[int column, int row]
        {
            get => this[column, row];
            set => this[column, row] = value;
        }
        T IFixedMap<T>.this[Cell cell]
        {
            get => this[cell.Column, cell.Row];
            set => this[cell.Column, cell.Row] = value;
        }
        #endregion

        #region property
        public int ColumnCount { get; private set; }
        public int RowCount { get; private set; }
        public T PlainValue { get; }
        public bool HasPlain => this.Any(x => x?.Equals(PlainValue) ?? PlainValue == null);
        #endregion

        #region method public

        public IEnumerable<T> GetRowEnumerable(int row)
        {
            if (!IsWithInRangeRow(row)) throw new ArgumentOutOfRangeException(OutOfRangeErrMsg);
            for (int column = 0; column < ColumnCount; column++)
            {
                yield return m_map[to1DIndex(column, row)];
            }
        }

        public IEnumerable<T> GetColumnEnumerable(int column)
        {
            if (!IsWithInRangeColumn(column)) throw new ArgumentOutOfRangeException(OutOfRangeErrMsg);
            for (int row = 0; row < RowCount; row++)
            {
                yield return m_map[to1DIndex(column, row)];
            }
        }

        public void DoForEachCell(Action<Cell, T> cellAction)
        {
            for (int i = 0; i < m_map.Length; i++)
            {
                int row = i / ColumnCount;
                int column = i - row * ColumnCount;
                cellAction(new Cell(column, row), m_map[to1DIndex(column, row)]);
            }
        }

        public IEnumerator<T> GetEnumerator() => m_map.Cast<T>().GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => m_map.GetEnumerator();

        public virtual void ReWriteRow(int row, Func<T, T> rewriter)
        {
            if (!IsWithInRangeRow(row)) throw new ArgumentOutOfRangeException(OutOfRangeErrMsg);
            for (int column = 0; column < ColumnCount; column++)
            {
                int index1d = to1DIndex(column, row);
                T old = m_map[index1d];
                m_map[index1d] = rewriter(m_map[index1d]);
                m_cellReplaceSubject?.OnNext(new MapCellReplaceEvent<T>(new Cell(column, row), old, m_map[index1d]));
            }
        }
        public virtual void ReWriteColumn(int column, Func<T, T> rewriter)
        {
            if (!IsWithInRangeColumn(column)) throw new ArgumentOutOfRangeException(OutOfRangeErrMsg);
            for (int row = 0; row < RowCount; row++)
            {
                int index1d = to1DIndex(column, row);
                T old = m_map[index1d];
                m_map[index1d] = rewriter(m_map[index1d]);
                m_cellReplaceSubject?.OnNext(new MapCellReplaceEvent<T>(new Cell(column, row), old, m_map[index1d]));
            }
        }
        public virtual void ReWriteAll(Func<T, T> rewriter)
        {
            for (int column = 0; column < ColumnCount; column++)
            {
                for (int row = 0; row < RowCount; row++)
                {
                    int index1d = to1DIndex(column, row);
                    T old = m_map[index1d];
                    m_map[index1d] = rewriter(m_map[index1d]);
                    m_cellReplaceSubject?.OnNext(new MapCellReplaceEvent<T>(new Cell(column, row), old, m_map[index1d]));
                }
            }
        }

        public virtual void ExpandColumn(LeftRight lr, int amount)
        {
            //オーバーフローチェック
            int newColumns = throwArgumentOutOfRangeExceptionIfOverflow(() => ColumnCount + amount);
            //列数が0以下にならないかチェック
            if (newColumns <= 0) throw new ArgumentOutOfRangeException(ExpandOutOfRangeErrMsg);

            //追加削除列数が0なら何もしない
            if (amount == 0) return;
            Action action = amount > 0 ?
                //列の追加
                lr switch
                {
                    LeftRight.Left => () =>
                    {
                        replaceMap(newColumns, RowCount, (column, row) => column >= amount ? m_map[to1DIndex(column - amount, row)] : PlainValue);
                    }
                    ,
                    LeftRight.Right => () =>
                    {
                        replaceMap(newColumns, RowCount, (column, row) => column < ColumnCount ? m_map[to1DIndex(column, row)] : PlainValue);
                    }
                    ,
                    _ => throw new InvalidEnumArgumentException()
                } :
                //列の削除
                lr switch
                {
                    LeftRight.Left => () =>
                    {
                        replaceMap(newColumns, RowCount, (column, row) => m_map[to1DIndex(column - amount, row)]);
                    }
                    ,
                    LeftRight.Right => () =>
                    {
                        replaceMap(newColumns, RowCount, (column, row) => m_map[to1DIndex(column, row)]);
                    }
                    ,
                    _ => throw new InvalidEnumArgumentException()
                };
            action();
            m_expandSubject?.OnNext(new MapExpandEvent(lr.ToDirection(), amount));
        }

        public virtual void ExpandRow(UpDown ud, int amount)
        {
            int newRows = throwArgumentOutOfRangeExceptionIfOverflow(() => RowCount + amount);
            if (newRows <= 0) throw new ArgumentOutOfRangeException(ExpandOutOfRangeErrMsg);

            if (amount == 0) return;
            Action action = amount > 0 ?
                //行の追加
                ud switch
                {
                    UpDown.Up => () =>
                    {
                        replaceMap(ColumnCount, newRows, (column, row) => row >= amount ? m_map[to1DIndex(column, row - amount)] : PlainValue);
                    }
                    ,
                    UpDown.Down => () =>
                    {
                        replaceMap(ColumnCount, newRows, (column, row) => row < RowCount ? m_map[to1DIndex(column, row)] : PlainValue);
                    }
                    ,
                    _ => throw new InvalidEnumArgumentException()
                } :
                //行の削除
                ud switch
                {
                    UpDown.Up => () =>
                    {
                        replaceMap(ColumnCount, newRows, (column, row) => m_map[to1DIndex(column, row - amount)]);
                    }
                    ,
                    UpDown.Down => () =>
                    {
                        replaceMap(ColumnCount, newRows, (column, row) => m_map[to1DIndex(column, row)]);
                    }
                    ,
                    _ => throw new InvalidEnumArgumentException()
                };
            action();
            m_expandSubject?.OnNext(new MapExpandEvent(ud.ToDirection(), amount));
        }

        public Map<T> Clone() => new Map<T>(ColumnCount, RowCount, m_map, PlainValue);

        public IObservable<MapCellReplaceEvent<T>> ObservableCellReplace() =>
            m_cellReplaceSubject?.AsObservable() ?? (m_cellReplaceSubject = new Subject<MapCellReplaceEvent<T>>()).AsObservable();

        public IObservable<MapExpandEvent> ObservableExpand() =>
            m_expandSubject?.AsObservable() ?? (m_expandSubject = new Subject<MapExpandEvent>()).AsObservable();

        public bool IsWithInRangeRow(int row) => row >= 0 && row < RowCount;
        public bool IsWithInRangeColumn(int column) => column >= 0 && column < ColumnCount;
        public bool IsWithInRange(int column, int row) => IsWithInRangeColumn(column) && IsWithInRangeRow(row);
        public bool IsWithInRange(Cell cell) => IsWithInRange(cell.Column, cell.Row);

        #endregion

        #region method private
        /// <summary>
        /// 新しい行列数と、各行列番号に対する新しいマップ値のセレクターを指定すると、新しいマップを作成してm_mapに再格納する
        /// </summary>
        /// <param name="newColumns">新しい列数</param>
        /// <param name="newRows">新しい行数</param>
        /// <param name="arrayToValue">行列番号に対する値のセレクター</param>
        private void replaceMap(int newColumns, int newRows, Func<int, int, T> arrayToValue)
        {
            T[] newMap = new T[newColumns * newRows];
            for (int row = 0; row < newRows; row++)
            {
                for (int column = 0; column < newColumns; column++)
                {
                    newMap[newColumns * row + column] = arrayToValue(column, row);
                }
            }
            this.m_map = newMap;
            this.ColumnCount = newColumns;
            this.RowCount = newRows;
        }

        private int throwArgumentOutOfRangeExceptionIfOverflow(Func<int> formula)
        {
            try
            {
                checked
                {
                    return formula();
                }
            }
            catch (OverflowException ex)
            {
                throw new ArgumentOutOfRangeException(ExpandOutOfRangeErrMsg, ex);
            }
        }

        /// <summary>
        /// 行列番号から対応する一次元配列のインデックス番号を取得する
        /// </summary>
        /// <param name="column"></param>
        /// <param name="row"></param>
        /// <returns></returns>
        private int to1DIndex(int column, int row) => ColumnCount * row + column;
        #endregion

    }
}

