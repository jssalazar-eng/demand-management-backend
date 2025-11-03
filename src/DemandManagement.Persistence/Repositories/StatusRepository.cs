using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DemandManagement.Domain.Entities;
using DemandManagement.Domain.Repositories;
using DemandManagement.Domain.ValueObjects;
using DemandManagement.Persistence.Data;

namespace DemandManagement.Persistence.Repositories;

public sealed class StatusRepository : IStatusRepository
{
    private readonly DemandManagementDbContext _context;

    public StatusRepository(DemandManagementDbContext context) => _context = context;

    public async Task<Status?> GetByIdAsync(StatusId id, CancellationToken cancellationToken = default)
    {
        return await _context.Statuses
            .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<Status>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Statuses
            .OrderBy(s => s.SequenceOrder)
            .ToListAsync(cancellationToken);
    }
}