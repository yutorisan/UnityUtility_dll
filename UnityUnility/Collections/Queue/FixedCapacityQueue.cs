using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityUtility.Collections.Core;

namespace UnityUtility.Collections
{
    /// <summary>
    /// 要素数が固定の<see cref="Queue{T}"/>
    /// </summary>
    public class FixedCapacityQueue<T> : IQueue<T>
    {
        protected readonly IQueue<T> innerQueue;

        /// <summary>
        /// 内部に使用する<see cref="IQueue{T}"/>を指定して、インスタンスを新規作成します
        /// </summary>
        /// <param name="innerQueue">内部に使用する<see cref="IQueue{T}"/></param>
        /// <param name="capacity">固定長</param>
        /// <param name="behaviour">空きがない状態で<see cref="Enqueue(T)"/>したときの挙動</param>
        public FixedCapacityQueue(IQueue<T> innerQueue, int capacity, CapacityOverQueueBehaviour behaviour = CapacityOverQueueBehaviour.ThrowException)
        {
            this.innerQueue = innerQueue;
            this.Capacity = capacity;
            this.CapacityOverBehaviour = behaviour;
        }
        /// <summary>
        /// キャパシティを指定してインスタンスを新規作成します
        /// </summary>
        /// <param name="capacity">要素数</param>
        /// <param name="mode">キャパを超えて要素を追加しようとしたときの挙動</param>
        public FixedCapacityQueue(int capacity, CapacityOverQueueBehaviour behaviour = CapacityOverQueueBehaviour.ThrowException)
            : this(new Queue<T>(capacity), capacity, behaviour) { }
        /// <summary>
        /// ソースコレクションを指定してインスタンスを新規作成します
        /// ソースコレクションの要素数がキャパシティとなります
        /// </summary>
        /// <param name="source">ソースコレクション</param>
        /// <param name="mode">キャパを超えて要素を追加しようとしたときの挙動</param>
        public FixedCapacityQueue(IReadOnlyCollection<T> source, CapacityOverQueueBehaviour behaviour = CapacityOverQueueBehaviour.ThrowException)
            : this(new Queue<T>(source), source.Count, behaviour) { }

        public int Count => innerQueue.Count;
        /// <summary>
        /// 固定長
        /// </summary>
        public int Capacity { get; }
        /// <summary>
        /// 空きがあるかどうか
        /// </summary>
        public bool HasVacancy => Count < Capacity;
        /// <summary>
        /// 空きがない状態で<see cref="Enqueue(T)"/>したときの挙動を取得します。
        /// </summary>
        public CapacityOverQueueBehaviour CapacityOverBehaviour { get; }

        public T Dequeue() => innerQueue.Dequeue();

        public void Enqueue(T item)
        {
            //空きがあれば普通にEnqueue
            if (HasVacancy) innerQueue.Enqueue(item);
            //空きがなければ設定に応じた処理を実行
            else
            {
                switch (CapacityOverBehaviour)
                {
                    case CapacityOverQueueBehaviour.Ignore:
                        break;
                    case CapacityOverQueueBehaviour.Dequeue:
                        innerQueue.Dequeue();
                        innerQueue.Enqueue(item);
                        break;
                    case CapacityOverQueueBehaviour.ThrowException:
                        throw new CapacityOverException();
                    default:
                        throw new InvalidEnumArgumentException();
                }
            }
        }

        public T Peek() => innerQueue.Peek();

        public void Clear() => innerQueue.Clear();

        public IEnumerator<T> GetEnumerator() => innerQueue.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
