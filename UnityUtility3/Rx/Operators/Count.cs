using System;
using UniRx;
using UniRx.Operators;

namespace UnityUtility.Rx.Operators
{
    public static partial class ObservableEx
    {
        /// <summary>
        /// 流れてきた値の個数をカウントして次に流します
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static IObservable<long> Count<T>(this IObservable<T> source)
        {
            return new CountObservable<T>(source);
        }
        /// <summary>
        /// 流れてきた値のうち条件に合致するものの個数をカウントして次に流します
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public static IObservable<long> Count<T>(this IObservable<T> source, Predicate<T> predicate)
        {
            return new CountObservable<T>(source, predicate);
        }

        private class CountObservable<T> : OperatorObservableBase<long>
        {
            private readonly IObservable<T> source;
            private readonly Predicate<T> predicate;

            public CountObservable(IObservable<T> source) : base(source.IsRequiredSubscribeOnCurrentThread())
            {
                this.source = source;
                this.predicate = _ => true;
            }
            public CountObservable(IObservable<T> source, Predicate<T> predicate) : base(source.IsRequiredSubscribeOnCurrentThread())
            {
                this.source = source;
                this.predicate = predicate;
            }

            protected override IDisposable SubscribeCore(IObserver<long> observer, IDisposable cancel)
            {
                return source.Subscribe(new Count(this, observer, cancel));
            }

            private class Count : OperatorObserverBase<T, long>
            {
                private readonly CountObservable<T> parent;
                private long count;

                public Count(CountObservable<T> parent, IObserver<long> observer, IDisposable cancel)
                    : base(observer, cancel)
                {
                    this.parent = parent;
                }

                public override void OnCompleted()
                {
                    try { observer.OnCompleted(); } finally { Dispose(); }
                }

                public override void OnError(Exception error)
                {
                    try { observer.OnError(error); } finally { Dispose(); }
                }

                public override void OnNext(T value)
                {
                    try
                    {
                        checked
                        {
                            if (parent.predicate(value)) ++count;
                        }
                    }
                    catch (Exception ex)
                    {
                        try { observer.OnError(ex); } finally { Dispose(); }
                        return;
                    }
                    observer.OnNext(count);
                }
            }
        }
    }
}
