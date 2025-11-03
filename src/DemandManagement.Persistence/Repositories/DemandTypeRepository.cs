using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DemandManagement.Domain.Entities;
using DemandManagement.Domain.Repositories;
using DemandManagement.Domain.ValueObjects;
using DemandManagement.Persistence.Data;

namespace DemandManagement.Persistence.Repositories;

public sealed class DemandTypeRepository : IDemandTypeRepository
{
    private readonly DemandManagementDbContext _context;

    public DemandTypeRepository(DemandManagementDbContext context) => _context = context;

    public async Task<DemandType?> GetByIdAsync(DemandTypeId id, CancellationToken cancellationToken = default)
    {
        return await _context.DemandTypes
            .FirstOrDefaultAsync(dt => dt.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<DemandType>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.DemandTypes
            .OrderBy(dt => dt.Name)
            .ToListAsync(cancellationToken);
    }
}