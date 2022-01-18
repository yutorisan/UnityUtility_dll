using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UniRx.Operators;

namespace UnityUtility.Rx.Operators
{
    public static partial class ObservableEx
    {
        /// <summary>
        /// 値が流れてきたら一定時間待機し、その間に別の値が流れて来なかったら値を通します
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="waitTime">待機時間</param>
        /// <returns></returns>
        public static IObservable<T> Stabilize<T>(this IObservable<T> source, TimeSpan waitTime)
        {
            return new StabilizeObservable<T>(source, waitTime);
        }
        /// <summary>
        /// 特定の値が流れてきたら一定時間待機し、その間に別の値が流れて来なかったら値を通します
        /// 特定の値以外は即座に流します
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="waitTime">待機時間</param>
        /// <param name="targets">対象の値</param>
        /// <returns></returns>
        public static IObservable<T> Stabilize<T>(this IObservable<T> source, TimeSpan waitTime, params T[] targets)
        {
            return new StabilizeObservable<T>(source, waitTime, targets);
        }

        private class StabilizeObservable<T> : OperatorObservableBase<T>
        {
            private readonly IObservable<T> source;
            private readonly TimeSpan waitTime;
            private readonly IEnumerable<T> target;
            private readonly bool isAllTarget;
            private readonly IScheduler scheduler = Scheduler.DefaultSchedulers.TimeBasedOperations;

            public StabilizeObservable(IObservable<T> source, TimeSpan waitTime)
                : base(source.IsRequiredSubscribeOnCurrentThread())
            {
                this.source = source;
                this.waitTime = waitTime;
                this.isAllTarget = true;
            }
            public StabilizeObservable(IObservable<T> source, TimeSpan waitTime, IEnumerable<T> target)
                : this(source, waitTime)
            {
                this.target = target;
                this.isAllTarget = false;
            }

            protected override IDisposable SubscribeCore(IObserver<T> observer, IDisposable cancel)
            {
                return new Stabilize(this, observer, cancel).Run();
            }

            private abstract class StabilizeBase : OperatorObserverBase<T, T>
            {
                protected readonly StabilizeObservable<T> parent;
                protected SerialDisposable timerSubscription = new SerialDisposable();
                protected SingleAssignmentDisposable sourceSubscription = new SingleAssignmentDisposable();
                protected ulong objectId = 0ul;
                protected readonly object gate = new object();

                protected StabilizeBase(StabilizeObservable<T> parent, IObserver<T> observer, IDisposable cancel)
                    : base(observer, cancel)
                {
                    this.parent = parent;
                }

                public override void OnCompleted()
                {
                    //オブジェクトIDを変更して前のタイマーイベントが発生しないようにする
                    lock (gate) ++objectId;

                    //タイマーを止める
                    timerSubscription.Dispose();
                    try { observer.OnCompleted(); }
                    finally { Dispose(); }
                }

                public override void OnError(Exception error)
                {
                    lock (gate) ++objectId;

                    timerSubscription.Dispose();
                    try { observer.OnError(error); }
                    finally { Dispose(); }
                }

                /// <summary>
                /// 値がターゲットに存在するかをチェックする
                /// </summary>
                /// <param name="value"></param>
                /// <returns></returns>
                protected bool isTarget(T value) =>
                    parent.isAllTarget || parent.target.Any(x => x.Equals(value));
            }

            private class Stabilize : StabilizeBase
            {
                public Stabilize(StabilizeObservable<T> parent, IObserver<T> observer, IDisposable cancel)
                    : base(parent, observer, cancel)
                {
                }

                public IDisposable Run()
                {
                    sourceSubscription.Disposable = parent.source.Subscribe(this);
                    return StableCompositeDisposable.Create(timerSubscription, sourceSubscription);
                }

                private IDisposable RunTimer(ulong timerID, T value)
                {
                    //指定された時間の計測を開始する。第二引数は時間経過後に実行する処理
                    return parent.scheduler.Schedule(parent.waitTime, () =>
                    {
                        bool isStabilized = false;
                        lock (gate)
                        {
                            //タイマー開始時のIDと、タイマー満了時のIDが同一であるかをチェック
                            if (objectId == timerID)
                            {
                                //タイマー満了したので値を流すようにマーク
                                isStabilized = true;
                            }
                        }
                        //タイマーが満了していたら値を発行
                        if (isStabilized) observer.OnNext(value);
                    });
                }

                public override void OnNext(T value)
                {
                    ulong useObjectId;
                    lock (gate)
                    {
                        //IDを更新
                        useObjectId = ++objectId;
                    }

                    //スタビライズターゲットに存在したら
                    if (isTarget(value))
                    {
                        //前のタイマーを停止して、新しくタイマーを開始
                        timerSubscription.Disposable = RunTimer(useObjectId, value);
                    }
                    //ターゲット外の値が来たら
                    else
                    {
                        //前のタイマーを停止
                        timerSubscription.Disposable = Disposable.Empty;
                        //即座に値を流す
                        observer.OnNext(value);
                    }
                }
            }
        }
    }
}

