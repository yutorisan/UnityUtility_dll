using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

namespace UnityUtility.Collections.Core
{
    /// <summary>
    /// 固定長Queueの基本クラス
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class FixedCapacityQueueBase<T> : IQueue<T>
    {
        protected readonly IQueue<T> innerQueue;
        private readonly int capacity;
        private readonly CapacityOverQueueBehaviour capacityOverBehaviour;

        public FixedCapacityQueueBase(IQueue<T> innerQueue, int capacity, CapacityOverQueueBehaviour behaviour)
        {
            this.innerQueue = innerQueue;
            this.capacity = capacity;
            this.capacityOverBehaviour = behaviour;
        }

        public int Count => innerQueue.Count;

        public T Dequeue() => innerQueue.Dequeue();

        public void Enqueue(T item)
        {
            //空きがあれば普通にEnqueue
            if (HasVacancy) innerQueue.Enqueue(item);
            //空きがなければ設定に応じた処理を実行
            else
            {
                switch (capacityOverBehaviour)
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

        /// <summary>
        /// 空きがあるかどうか
        /// </summary>
        public bool HasVacancy => Count < capacity;

        public IEnumerator<T> GetEnumerator() => innerQueue.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}