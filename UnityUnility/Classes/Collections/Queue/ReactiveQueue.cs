using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityUtility.Collections;

namespace UnityUtility.Collections
{
    public interface IReadOnlyReactiveQueue<out T>
    {
        /// <summary>
        /// Enqueueされた値の通知を取得します
        /// </summary>
        /// <returns></returns>
        IObservable<T> ObserveEnqueue();
        /// <summary>
        /// Dequeueされたときに取得された値の通知を取得します
        /// </summary>
        /// <returns></returns>
        IObservable<T> ObserveDequeue();
    }
    public interface IReactiveQueue<T> : IReadOnlyReactiveQueue<T>, IQueue<T>, IReadOnlyReactiveCollection<T>
    {

    }
    public class ReactiveQueue<T> : Queue<T>, IReactiveQueue<T>
    {
        public ReactiveQueue() : base() { }
        public ReactiveQueue(int capacity) : base(capacity) { }
        public ReactiveQueue(IEnumerable<T> source) : base(source) { }

        public override void Enqueue(T value)
        {
            int addIndex = Count; //追加するインデックス番号は最後

            queue.Enqueue(value);

            //Enqueueされたことを通知
            enqueueSubject?.OnNext(value);
            //コレクションに要素が追加されたことを通知
            addSubject?.OnNext(new CollectionAddEvent<T>(addIndex, value));
            //Moveが購読されている場合は、Enqueueによってずれた要素を１個１個通知
            if(moveSubject != null)
            {
                for (int i = 1; i < Count; i++)
                {
                    moveSubject.OnNext(new CollectionMoveEvent<T>(i - 1, i, queue.ElementAt(i)));
                }
            }
            //コレクションの要素数が変更されたことを通知
            countSubject?.OnNext(addIndex + 1);
        }

        public override T Dequeue()
        {
            T value = queue.Dequeue();
            int count = Count;

            //Dequeueされたことを通知
            dequeueSubject?.OnNext(value);
            //要素が削除されたことを通知
            removeSubject?.OnNext(new CollectionRemoveEvent<T>(0, value));
            //Moveが購読されている場合は、Dequeueによってずれた要素を1個1個通知
            if (moveSubject != null)
            {
                for (int i = 0; i < Count; i++)
                {
                    moveSubject.OnNext(new CollectionMoveEvent<T>(i + 1, i, queue.ElementAt(i)));
                }
            }
            //コレクションの要素数が変更されたことを通知
            countSubject?.OnNext(count);

            return value;
        }

        public T this[int index]
        {
            get
            {
                if (index < 0 || Count <= index) throw new IndexOutOfRangeException();
                return queue.ElementAt(index);
            }
        }

        private Subject<T> enqueueSubject;
        public IObservable<T> ObserveEnqueue() => (enqueueSubject ??= new Subject<T>()).AsObservable();

        private Subject<T> dequeueSubject;
        public IObservable<T> ObserveDequeue() => (dequeueSubject ??= new Subject<T>()).AsObservable();

        private Subject<CollectionAddEvent<T>> addSubject;
        public IObservable<CollectionAddEvent<T>> ObserveAdd() => (addSubject ??= new Subject<CollectionAddEvent<T>>()).AsObservable();

        private Subject<int> countSubject;
        public IObservable<int> ObserveCountChanged(bool notifyCurrentCount = false)
        {
            countSubject ??= new Subject<int>();
            if (notifyCurrentCount) return countSubject.StartWith(Count);
            else return countSubject.AsObservable();
        }

        private Subject<CollectionMoveEvent<T>> moveSubject;
        public IObservable<CollectionMoveEvent<T>> ObserveMove() => (moveSubject ??= new Subject<CollectionMoveEvent<T>>()).AsObservable();

        private Subject<CollectionRemoveEvent<T>> removeSubject;
        public IObservable<CollectionRemoveEvent<T>> ObserveRemove() => (removeSubject ??= new Subject<CollectionRemoveEvent<T>>()).AsObservable();

        //Queueでは特定の要素の単純な置き換えは発生しない
        private IObservable<CollectionReplaceEvent<T>> replaceSubject;
        public IObservable<CollectionReplaceEvent<T>> ObserveReplace() => replaceSubject ??= Observable.Never<CollectionReplaceEvent<T>>();

        private Subject<Unit> resetSubject;
        public IObservable<Unit> ObserveReset() => (resetSubject ??= new Subject<Unit>()).AsObservable();
    }
}
