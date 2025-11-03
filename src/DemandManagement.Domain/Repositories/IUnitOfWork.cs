using System;
using System.Threading;
using System.Threading.Tasks;

namespace DemandManagement.Domain.Repositories;

public interface IUnitOfWork : IDisposable
{
    IDemandRepository Demands { get; }
    // Agrega otros repositorios si los necesitas:
    // IDemandTypeRepository DemandTypes { get; }
    // IStatusRepository Statuses { get; }
    // IUserRepository Users { get; }
    // IRoleRepository Roles { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}