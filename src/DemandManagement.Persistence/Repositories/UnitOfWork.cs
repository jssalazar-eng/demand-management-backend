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

    public UnitOfWork(DemandManagementDbContext context) => _context = context;

    public IDemandRepository Demands => _demands ??= new DemandRepository(_context);

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }

    public void Dispose() => _context.Dispose();
}