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

    // New methods for dashboard
    Task<int> GetTotalCountAsync(CancellationToken cancellationToken = default);
    Task<int> GetCountByStatusNamesAsync(IEnumerable<string> statusNames, CancellationToken cancellationToken = default);
    Task<int> GetCountByPriorityAsync(PriorityLevel priority, CancellationToken cancellationToken = default);
    Task<IEnumerable<Demand>> GetRecentAsync(int count, CancellationToken cancellationToken = default);

    Task AddAsync(Demand demand, CancellationToken cancellationToken = default);
    Task UpdateAsync(Demand demand, CancellationToken cancellationToken = default);
    Task DeleteAsync(DemandId id, CancellationToken cancellationToken = default);
}