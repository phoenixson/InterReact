﻿using System.Reactive.Threading.Tasks;

namespace InterReact;

public partial class Service
{
    /// <summary>
    /// Returns a snapshot of market data ticks.
    /// Tick class may be selected by using the OfTickClass extension method.
    /// </summary>
    public async Task<IList<object>> GetTickSnapshotAsync(
        Contract contract, IEnumerable<GenericTickType>? genericTickTypes = null, bool isRegulatorySnapshot = false, Tag[]? options = null, CancellationToken ct = default)
    {
        int id = Request.GetNextId();
        options ??= Array.Empty<Tag>();

        Task<IList<object>> task = Response
            .WithRequestId(id)
            .TakeUntil(x => x is SnapshotEndTick or AlertMessage { IsFatal: true })
            .Where(m => m is not SnapshotEndTick)
            .ToList()
            .ToTask(ct);

        Request.RequestMarketData(id, contract, genericTickTypes, true, isRegulatorySnapshot, options);

        IList<object> list = await task.ConfigureAwait(false);

        return list;
    }
}
