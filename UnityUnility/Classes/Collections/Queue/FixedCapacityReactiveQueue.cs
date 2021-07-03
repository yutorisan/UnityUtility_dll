using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityUtility.Collections.Core;

namespace UnityUtility.Collections
{
    /// <summary>
    /// 固定長の<see cref="ReactiveQueue{T}"/>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class FixedCapacityReactiveQueue<T> : FixedCapacityQueueBase<T>, IReactiveQueue<T>
    {
        /// <summary>
        /// キャパシティを指定してインスタンスを新規作成します
        /// </summary>
        /// <param name="capacity">要素数</param>
        /// <param name="mode">キャパを超えて要素を追加しようとしたときの挙動</param>
        public FixedCapacityReactiveQueue(int capacity, CapacityOverQueueBehaviour behaviour = CapacityOverQueueBehaviour.ThrowException)
            : base(new ReactiveQueue<T>(capacity), capacity, behaviour) { }
        /// <summary>
        /// ソースコレクションを指定してインスタンスを新規作成します
        /// ソースコレクションの要素数がキャパシティとなります
        /// </summary>
        /// <param name="source">ソースコレクション</param>
        /// <param name="mode">キャパを超えて要素を追加しようとしたときの挙動</param>
        public FixedCapacityReactiveQueue(IReadOnlyCollection<T> source, CapacityOverQueueBehaviour behaviour = CapacityOverQueueBehaviour.ThrowException)
            : base(new ReactiveQueue<T>(source), source.Count, behaviour) { }

        public T this[int index] => (innerQueue as IReactiveQueue<T>)[index];

        public IObservable<T> ObserveDequeue() => (innerQueue as IReactiveQueue<T>).ObserveDequeue();
        public IObservable<T> ObserveEnqueue() => (innerQueue as IReactiveQueue<T>).ObserveEnqueue();
        public IObservable<CollectionAddEvent<T>> ObserveAdd() => (innerQueue as IReactiveQueue<T>).ObserveAdd();
        public IObservable<int> ObserveCountChanged(bool notifyCurrentCount = false) => (innerQueue as IReactiveQueue<T>).ObserveCountChanged(notifyCurrentCount);
        public IObservable<CollectionMoveEvent<T>> ObserveMove() => (innerQueue as IReactiveQueue<T>).ObserveMove();
        public IObservable<CollectionRemoveEvent<T>> ObserveRemove() => (innerQueue as IReactiveQueue<T>).ObserveRemove();
        public IObservable<CollectionReplaceEvent<T>> ObserveReplace() => (innerQueue as IReactiveQueue<T>).ObserveReplace();
        public IObservable<Unit> ObserveReset() => (innerQueue as IReactiveQueue<T>).ObserveReset();
    }
}
