using System;
using UniRx;
using UniRx.Operators;
using UnityUtility.Modules;

namespace UnityUtility.Rx.Operators
{
    public static partial class ObservableEx
    {
        /// <summary>
        /// 流れてきた値にカウントを付加して次に流します
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static IObservable<WithIndexL<T>> WithCount<T>(this IObservable<T> source)
        {
            return new WithCountObservable<T>(source);
        }
        private class WithCountObservable<T> : OperatorObservableBase<WithIndexL<T>>
        {
            private readonly IObservable<T> source;

            public WithCountObservable(IObservable<T> source) : base(source.IsRequiredSubscribeOnCurrentThread())
            {
                this.source = source;
            }

            protected override IDisposable SubscribeCore(IObserver<WithIndexL<T>> observer, IDisposable cancel)
            {
                return source.Subscribe(new WithCount(this, observer, cancel));
            }

            private class WithCount : OperatorObserverBase<T, WithIndexL<T>>
            {
                private readonly WithCountObservable<T> parent;
                private long count;

                public WithCount(WithCountObservable<T> parent, IObserver<WithIndexL<T>> observer, IDisposable cancel)
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
                        checked { ++count; }
                    }
                    catch (Exception ex)
                    {
                        try { observer.OnError(ex); } finally { Dispose(); }
                        return;
                    }
                    observer.OnNext(new WithIndexL<T>(value, count));
                }
            }
        }
    }
}

