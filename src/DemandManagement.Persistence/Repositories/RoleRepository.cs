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

public sealed class RoleRepository : IRoleRepository
{
    private readonly DemandManagementDbContext _context;

    public RoleRepository(DemandManagementDbContext context) => _context = context;

    public async Task<Role?> GetByIdAsync(RoleId id, CancellationToken cancellationToken = default)
    {
        return await _context.Roles
            .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<Role>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Roles
            .OrderBy(r => r.Name)
            .ToListAsync(cancellationToken);
    }
}