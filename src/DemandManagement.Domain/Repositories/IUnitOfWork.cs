using System;
using System.Threading;
using System.Threading.Tasks;

namespace DemandManagement.Domain.Repositories;

public interface IUnitOfWork : IDisposable
{
    IDemandRepository Demands { get; }
    IDemandTypeRepository DemandTypes { get; }
    IStatusRepository Statuses { get; }
    IRoleRepository Roles { get; }
    IUserRepository Users { get; }
    
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}