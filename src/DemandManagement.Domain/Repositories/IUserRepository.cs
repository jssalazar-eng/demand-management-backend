using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DemandManagement.Domain.Entities;
using DemandManagement.Domain.ValueObjects;

namespace DemandManagement.Domain.Repositories;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(UserId id, CancellationToken cancellationToken = default);
    Task<IEnumerable<User>> GetAllAsync(CancellationToken cancellationToken = default);
}