using System;
using System.Collections.Generic;
using UnityUtility.Enums;
using UnityUtility.Modules;

namespace UnityUtility.Collections
{
    // recordは共変にできないので仕方なくoutつけてない
    // readonly-interface定義してもいいけど、共変/反変は値型には使えないためそもそも使われることはあまりないかも、という判断
    public interface IReadOnlyReactiveMap<T> : IReadOnlyMap<T>
    {
        /// <summary>
        /// マップの単一セル値の変更通知を提供します
        /// </summary>
        /// <returns></returns>
        IObservable<MapCellReplaceEvent<T>> ObservableCellReplace();
        /// <summary>
        /// マップの拡張通知を提供します
        /// </summary>
        /// <returns></returns>
        IObservable<MapExpandEvent> ObservableExpand();
    }
    public interface IReactiveMap<T> : IMap<T>, IReadOnlyReactiveMap<T>
    {
    }

    public record MapCellReplaceEvent<T>(int row, int column, T oldValue, T newValue);
    public record MapRowReplaceEvent<T>(int row, IEnumerable<T> oldRowValues, IEnumerable<T> newRowValues);
    public record MapColumnReplaceEvent<T>(int column, IEnumerable<T> oldColumnValues, IEnumerable<T> newRowValues);
    public record MapReplaceEvent<T>(IEnumerable<T> oldValues, IEnumerable<T> newValues);
    public record MapExpandEvent(Direction Direction, int expandCount);

}

