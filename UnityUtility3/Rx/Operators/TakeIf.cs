using System;
using UniRx;
using UniRx.Operators;

namespace UnityUtility.Rx.Operators
{
    public static partial class ObservableEx
    {
        /// <summary>
        /// 最初に流れてきた値を評価し、適合すればストリームを後続に流し続けます。
        /// 適合しない場合はその時点で発行元の購読を解除し、OnCompletedを発行します。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public static IObservable<T> TakeIf<T>(this IObservable<T> source, Predicate<T> predicate)
        {
            return new TakeIfObservable<T>(source, predicate);
        }
        private class TakeIfObservable<T> : OperatorObservableBase<T>
        {
            private readonly IObservable<T> source;
            private readonly Predicate<T> predicate;

            public TakeIfObservable(IObservable<T> source, Predicate<T> predicate) : base(source.IsRequiredSubscribeOnCurrentThread())
            {
                this.source = source;
                this.predicate = predicate;
            }

            protected override IDisposable SubscribeCore(IObserver<T> observer, IDisposable cancel)
            {
                return source.Subscribe(new TakeIf(this, observer, cancel));
            }

            private class TakeIf : OperatorObserverBase<T, T>
            {
                private readonly TakeIfObservable<T> parent;
                private object lockObj = new object();
                private bool isTaking;
                private bool isEvaluated;

                public TakeIf(TakeIfObservable<T> parent, IObserver<T> observer, IDisposable cancel) : base(observer, cancel)
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
                    if (isEvaluated && isTaking)
                    {
                        observer.OnNext(value);
                    }
                    else
                    {
                        lock (lockObj)
                        {
                            //同時にlockに入ってきた場合に再評価されるのを防ぐ
                            if(isEvaluated)
                            {
                                if (isTaking)
                                {
                                    observer.OnNext(value);
                                }
                                return;
                            }
                            //最初の値を評価する
                            try
                            {
                                //最初の値が通ったら今後永続的に通す
                                if (parent.predicate(value))
                                {
                                    isTaking = true;
                                    observer.OnNext(value);
                                }
                                //通らなかったらOnCompletedを発行して購読を切る
                                else
                                {
                                    try { observer.OnCompleted(); } finally { Dispose(); }
                                }
                            }
                            catch (Exception ex)
                            {
                                try { observer.OnError(ex); } finally { Dispose(); }
                            }
                            finally
                            {
                                isEvaluated = true;
                            }
                        }
                    }
                }
            }
        }
    }
}

