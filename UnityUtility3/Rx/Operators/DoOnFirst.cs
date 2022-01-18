using System;
using System.Runtime.CompilerServices;
using UniRx;
using UniRx.Operators;

namespace UnityUtility.Rx.Operators
{
    public static partial class ObservableEx
    {
        /// <summary>
        /// 最初の1回のOnNext時のみ任意の処理を実行します。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="action">最初の1回のみ実行する処理</param>
        /// <returns></returns>
        public static IObservable<T> DoOnFirst<T>(this IObservable<T> source, Action<T> action) =>
            new DoOnFirstObservable<T>(source, action);

        private class DoOnFirstObservable<T> : OperatorObservableBase<T>
        {
            private readonly IObservable<T> source;
            private readonly Action<T> onFirstAction;

            public DoOnFirstObservable(IObservable<T> source, Action<T> action) : base(source.IsRequiredSubscribeOnCurrentThread())
            {
                this.source = source;
                this.onFirstAction = action;
            }

            protected override IDisposable SubscribeCore(IObserver<T> observer, IDisposable cancel)
            {
                return source.Subscribe(new DoOnFirst(this, observer, cancel));
            }

            private class DoOnFirst : OperatorObserverBase<T, T>
            {
                private readonly DoOnFirstObservable<T> parent;
                private bool already = false;

                public DoOnFirst(DoOnFirstObservable<T> parent, IObserver<T> observer, IDisposable cancel)
                    : base(observer, cancel)
                {
                    this.parent = parent;
                }

                public override void OnCompleted()
                {
                    try { observer.OnCompleted(); }
                    finally { Dispose(); }
                }

                public override void OnError(Exception error)
                {
                    try { observer.OnError(error); }
                    finally { Dispose(); }
                }

                public override void OnNext(T value)
                {
                    try
                    {
                        if (!already)
                        {
                            parent.onFirstAction(value);
                            already = true;
                        }
                        observer.OnNext(value);

                    }
                    catch (Exception ex)
                    {
                        try { observer.OnError(ex); }
                        finally { Dispose(); }
                    }
                }
            }
        }
    }

}

