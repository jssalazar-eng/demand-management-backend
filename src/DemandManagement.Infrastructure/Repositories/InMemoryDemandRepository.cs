using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DemandManagement.Domain.Entities;
using DemandManagement.Domain.Repositories;
using DemandManagement.Domain.ValueObjects;

namespace DemandManagement.Infrastructure.Repositories;

public sealed class InMemoryDemandRepository : IDemandRepository
{
    private readonly ConcurrentDictionary<Guid, Demand> _store = new();

    public Task AddAsync(Demand demand, CancellationToken cancellationToken = default)
    {
        _store[demand.Id.Value] = demand;
        return Task.CompletedTask;
    }

    public Task DeleteAsync(DemandId id, CancellationToken cancellationToken = default)
    {
        _store.TryRemove(id.Value, out _);
        return Task.CompletedTask;
    }

    public Task<IEnumerable<Demand>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var all = _store.Values.AsEnumerable();
        return Task.FromResult(all);
    }

    public Task<Demand?> GetByIdAsync(DemandId id, CancellationToken cancellationToken = default)
    {
        _store.TryGetValue(id.Value, out var d);
        return Task.FromResult(d);
    }

    public Task UpdateAsync(Demand demand, CancellationToken cancellationToken = default)
    {
        _store[demand.Id.Value] = demand;
        return Task.CompletedTask;
    }
}