﻿using System;
namespace UnityUtility.Collections
{
    /// <summary>
    /// キャパオーバー時の挙動
    /// </summary>
    public enum CapacityOverQueueBehaviour
    {
        /// <summary>
        /// 何もしない、無視する
        /// </summary>
        Ignore,
        /// <summary>
        /// DequeueしてからEnqueueする
        /// </summary>
        Dequeue,
        /// <summary>
        /// 例外をスローする
        /// </summary>
        ThrowException
    }
}
