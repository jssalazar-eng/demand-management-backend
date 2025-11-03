using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DemandManagement.Domain.Entities;
using DemandManagement.Domain.ValueObjects;

namespace DemandManagement.Domain.Repositories;

public interface IRoleRepository
{
    Task<Role?> GetByIdAsync(RoleId id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Role>> GetAllAsync(CancellationToken cancellationToken = default);
}