using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DemandManagement.Domain.Entities;
using DemandManagement.Domain.ValueObjects;

namespace DemandManagement.Domain.Repositories;

public interface IStatusRepository
{
    Task<Status?> GetByIdAsync(StatusId id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Status>> GetAllAsync(CancellationToken cancellationToken = default);
}