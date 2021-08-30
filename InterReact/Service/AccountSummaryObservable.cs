﻿using System;
using System.Linq;
using System.Reactive.Linq;

namespace InterReact
{
    public partial class Services
    {
        /// <summary>
        /// Creates an observable which continually emits account summary items.
        /// All items are sent initially, and then only any changes.
        /// AccountSummaryEnd is emitted after the initial values for each account have been emitted.
        /// Use CreateAccountSummaryObservable().Publish()[.RefCount() | .AutoConnect()] to support multiple observers.
        /// Use CreateAccountSummaryObservable().CacheSource(Services.GetAccountSummaryCacheKey)
        /// to cache the latest values for replay to new subscribers.
        /// </summary>
        public IObservable<IHasRequestId> CreateAccountSummaryObservable()
        {
            return Response
                .ToObservableContinuousWithId(
                    Request.GetNextId, id => Request.RequestAccountSummary(id), Request.CancelAccountSummary);
        }

        public static string GetAccountSummaryCacheKey(Union<AccountSummary, AccountSummaryEnd, Alert> union)
        {
            return union.Source switch
            {
                AccountSummary a => $"{a.Account}+{a.Currency}+{a.Tag}",
                AccountSummaryEnd => "AccountSummaryEnd",
                Alert alert => $"{alert.Code}+{alert.Message}",
                _ => throw new ArgumentException($"Unhandled type: {union.Source.GetType()}.")
            };
        }

    }
}