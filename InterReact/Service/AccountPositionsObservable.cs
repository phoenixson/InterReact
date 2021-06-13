﻿using System;

namespace InterReact
{
    public sealed partial class Services
    {
        /// <summary>
        /// An observable which emits one or more account positions, then completes.
        /// </summary>
        public IObservable<AccountPosition> CreateAccountPositionsObservable() =>
            Response
                .ToObservableMultiple<AccountPosition, AccountPositionEnd>(
                    Request.RequestAccountPositions,
                    Request.CancelAccountPositions)
                .ToShareSource();
    }
}