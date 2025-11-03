using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DemandManagement.Domain.Entities;
using DemandManagement.Domain.ValueObjects;

namespace DemandManagement.Domain.Repositories;

public interface IDemandRepository
{
    Task<Demand?> GetByIdAsync(DemandId id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Demand>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<(IEnumerable<Demand> Items, int TotalCount)> GetPagedAsync(
        DemandTypeId? demandTypeId,
        StatusId? statusId,
        PriorityLevel? priority,
        string? searchTerm,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default);
    Task AddAsync(Demand demand, CancellationToken cancellationToken = default);
    Task UpdateAsync(Demand demand, CancellationToken cancellationToken = default);
    Task DeleteAsync(DemandId id, CancellationToken cancellationToken = default);
}