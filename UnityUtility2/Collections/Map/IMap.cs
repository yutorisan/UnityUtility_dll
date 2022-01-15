using System;
using UnityUtility.Enums;

namespace UnityUtility.Collections
{
    public interface IMap<T> : IFixedMap<T>
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

