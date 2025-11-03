using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DemandManagement.Domain.Entities;
using DemandManagement.Domain.ValueObjects;

namespace DemandManagement.Domain.Repositories;

public interface IDemandTypeRepository
{
    Task<DemandType?> GetByIdAsync(DemandTypeId id, CancellationToken cancellationToken = default);
    Task<IEnumerable<DemandType>> GetAllAsync(CancellationToken cancellationToken = default);
}