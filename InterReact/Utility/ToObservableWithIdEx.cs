﻿using System;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;

namespace InterReact
{
    // For requests that use RequestId.
    public static partial class Extensions
    {
        // Single result: HistoricalData, FundamentalData, ScannerData
        internal static IObservable<T> ToObservableWithIdSingle<T>(this IObservable<object> source,
            Func<int> getNextId, Action<int> startRequest, Action<int>? stopRequest = null)
                where T : class, IHasRequestId
        {
            return Observable.Create<T>(observer =>
            {
                var requestId = getNextId();
                bool? cancelable = null;

                var subscription = source
                    .WithRequestId(requestId)
                    .Finally(() => cancelable = false)
                    .SubscribeSafe(Observer.Create<object>(
                        onNext: m =>
                        {
                            if (m is Alert alert)
                            {
                                cancelable = false;
                                observer.OnError(alert);
                                return;
                            }
                            if (m is not T t)
                                throw new InvalidCastException($"Invalid type: {m.GetType()}.");
                            observer.OnNext(t);
                            cancelable = false;
                            observer.OnCompleted();
                        },
                        onError: observer.OnError,
                        onCompleted: observer.OnCompleted));

                if (cancelable == null)
                    startRequest(requestId);
                if (cancelable == null)
                    cancelable = true;

                return Disposable.Create(() =>
                {
                    if (cancelable == true)
                        stopRequest?.Invoke(requestId);
                    subscription.Dispose();
                });
            });
        }


        // Multiple results: TickSnapshot, ContractData, AccountSummary, Executions
        internal static IObservable<T> ToObservableWithIdMultiple<T, TEnd>(this IObservable<object> source,
            Func<int> getNextId, Action<int> startRequest, Action<int>? stopRequest = null)
                where T : class, IHasRequestId
        {
            return Observable.Create<T>(observer =>
            {
                var requestId = getNextId();
                bool? cancelable = null;

                var subscription = source
                    .WithRequestId(requestId)
                    .Finally(() => cancelable = false)
                    .SubscribeSafe(Observer.Create<object>(
                        onNext: m =>
                        {
                            if (m is Alert alert)
                            {
                                cancelable = false;
                                observer.OnError(alert);
                                return;
                            }
                            if (m is T t)
                            {
                                observer.OnNext(t);
                                return;
                            }
                            if (m is not TEnd t2)
                                throw new InvalidCastException($"Invalid type: {m.GetType()}.");
                            cancelable = false;
                            observer.OnCompleted();
                        },
                        onError: observer.OnError,
                        onCompleted: observer.OnCompleted));

                if (cancelable == null)
                    startRequest(requestId);
                if (cancelable == null)
                    cancelable = true;

                return Disposable.Create(() =>
                {
                    if (cancelable == true)
                        stopRequest?.Invoke(requestId);
                    subscription.Dispose();
                });
            });
        }


        // Continuous results: Tick, MarketDepth, RealtimeBar
        internal static IObservable<T> ToObservableWithIdContinuous<T>(this IObservable<object> source,
            Func<int> getNextId, Action<int> startRequest, Action<int> stopRequest)
                where T : class, IHasRequestId
        {
            return Observable.Create<T>(observer =>
            {
                var requestId = getNextId();
                bool? cancelable = null;

                var subscription = source
                    .WithRequestId(requestId)
                    .Finally(() => cancelable = false)
                    .SubscribeSafe(Observer.Create<object>(
                        onNext: m =>
                        {
                            if (m is Alert alert)
                            {
                                cancelable = false;
                                observer.OnError(alert);
                                return;
                            }
                            if (m is not T t)
                                throw new InvalidCastException($"Invalid type: {m.GetType()}.");
                            observer.OnNext(t);
                        },
                        onError: observer.OnError,
                        onCompleted: observer.OnCompleted));

                if (cancelable == null)
                    startRequest(requestId);
                if (cancelable == null)
                    cancelable = true;

                return Disposable.Create(() =>
                {
                    if (cancelable == true)
                        stopRequest.Invoke(requestId);
                    subscription.Dispose();
                });
            });
        }
    }
}