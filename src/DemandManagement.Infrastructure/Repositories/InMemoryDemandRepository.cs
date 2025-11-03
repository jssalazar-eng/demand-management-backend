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
        ArgumentNullException.ThrowIfNull(demand);

        if (_store.ContainsKey(demand.Id.Value))
        {
            throw new InvalidOperationException($"Demand with ID '{demand.Id.Value}' already exists.");
        }

        _store[demand.Id.Value] = demand;
        return Task.CompletedTask;
    }

    public Task DeleteAsync(DemandId id, CancellationToken cancellationToken = default)
    {
        if (id.Value == Guid.Empty)
        {
            throw new ArgumentException("DemandId cannot be empty.", nameof(id));
        }

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
        if (id.Value == Guid.Empty)
        {
            throw new ArgumentException("DemandId cannot be empty.", nameof(id));
        }

        _store.TryGetValue(id.Value, out var d);
        return Task.FromResult(d);
    }

    public Task<(IEnumerable<Demand> Items, int TotalCount)> GetPagedAsync(
        DemandTypeId? demandTypeId,
        StatusId? statusId,
        PriorityLevel? priority,
        string? searchTerm,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        // Validaciones de parámetros
        if (pageNumber < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(pageNumber), "Page number must be greater than 0.");
        }

        if (pageSize < 1 || pageSize > 100)
        {
            throw new ArgumentOutOfRangeException(nameof(pageSize), "Page size must be between 1 and 100.");
        }

        if (!string.IsNullOrWhiteSpace(searchTerm) && searchTerm.Length > 200)
        {
            throw new ArgumentException("Search term cannot exceed 200 characters.", nameof(searchTerm));
        }

        var query = _store.Values.AsEnumerable();

        // Aplicar filtros
        if (demandTypeId is not null)
        {
            query = query.Where(d => d.DemandTypeId.Value == demandTypeId.Value.Value);
        }

        if (statusId is not null)
        {
            query = query.Where(d => d.StatusId.Value == statusId.Value.Value);
        }

        if (priority is not null)
        {
            query = query.Where(d => d.Priority.Level == priority.Value);
        }

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            var term = searchTerm.Trim().ToLower();
            query = query.Where(d =>
                d.Title.ToLower().Contains(term) ||
                (d.Description != null && d.Description.ToLower().Contains(term)));
        }

        // Contar total antes de paginar
        var totalCount = query.Count();

        // Aplicar paginación y ordenar por fecha de creación descendente
        var items = query
            .OrderByDescending(d => d.Audit.CreatedDate)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        return Task.FromResult<(IEnumerable<Demand> Items, int TotalCount)>((items, totalCount));
    }

    public Task UpdateAsync(Demand demand, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(demand);

        if (!_store.ContainsKey(demand.Id.Value))
        {
            throw new InvalidOperationException($"Demand with ID '{demand.Id.Value}' does not exist.");
        }

        _store[demand.Id.Value] = demand;
        return Task.CompletedTask;
    }
}