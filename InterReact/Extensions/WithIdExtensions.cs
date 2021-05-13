﻿using System;
using System.Linq;
using System.Reactive.Linq;

namespace InterReact.Extensions
{
    public static class WithIdExtensions
    {
        public static IObservable<IHasRequestId> WithRequestId(this IObservable<object> source, int requestId) =>
            source.OfType<IHasRequestId>()
            .Where(m => m.RequestId == requestId);

        public static IObservable<IHasOrderId> WithOrderId(this IObservable<object> source, int orderId) =>
            source.OfType<IHasOrderId>()
            .Where(m => m.OrderId == orderId);
    }
}