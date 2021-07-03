using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityUtility.Collections.Core;

namespace UnityUtility.Collections
{
    /// <summary>
    /// 要素数が固定の<see cref="Queue{T}"/>
    /// </summary>
    public class FixedCapacityQueue<T> : FixedCapacityQueueBase<T>, IQueue<T>
    {
        /// <summary>
        /// キャパシティを指定してインスタンスを新規作成します
        /// </summary>
        /// <param name="capacity">要素数</param>
        /// <param name="mode">キャパを超えて要素を追加しようとしたときの挙動</param>
        public FixedCapacityQueue(int capacity, CapacityOverQueueBehaviour behaviour = CapacityOverQueueBehaviour.ThrowException)
            : base(new Queue<T>(capacity), capacity, behaviour) { }
        /// <summary>
        /// ソースコレクションを指定してインスタンスを新規作成します
        /// ソースコレクションの要素数がキャパシティとなります
        /// </summary>
        /// <param name="source">ソースコレクション</param>
        /// <param name="mode">キャパを超えて要素を追加しようとしたときの挙動</param>
        public FixedCapacityQueue(IReadOnlyCollection<T> source, CapacityOverQueueBehaviour behaviour = CapacityOverQueueBehaviour.ThrowException)
            : base(new Queue<T>(source), source.Count, behaviour) { }
    }
}
