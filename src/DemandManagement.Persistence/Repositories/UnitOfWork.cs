using System;
using System.Threading;
using System.Threading.Tasks;
using DemandManagement.Domain.Repositories;
using DemandManagement.Persistence.Data;

namespace DemandManagement.Persistence.Repositories;

public sealed class UnitOfWork : IUnitOfWork
{
    private readonly DemandManagementDbContext _context;
    private IDemandRepository? _demands;
    private IDemandTypeRepository? _demandTypes;
    private IStatusRepository? _statuses;
    private IRoleRepository? _roles;
    private IUserRepository? _users;

    public UnitOfWork(DemandManagementDbContext context) => _context = context;

    public IDemandRepository Demands => _demands ??= new DemandRepository(_context);
    public IDemandTypeRepository DemandTypes => _demandTypes ??= new DemandTypeRepository(_context);
    public IStatusRepository Statuses => _statuses ??= new StatusRepository(_context);
    public IRoleRepository Roles => _roles ??= new RoleRepository(_context);
    public IUserRepository Users => _users ??= new UserRepository(_context);

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }

    public void Dispose() => _context.Dispose();
}