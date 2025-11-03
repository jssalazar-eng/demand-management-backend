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

public sealed class UserRepository : IUserRepository
{
    private readonly DemandManagementDbContext _context;

    public UserRepository(DemandManagementDbContext context) => _context = context;

    public async Task<User?> GetByIdAsync(UserId id, CancellationToken cancellationToken = default)
    {
        return await _context.Users
            .FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<User>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Users
            .OrderBy(u => u.FullName.Value)
            .ToListAsync(cancellationToken);
    }
}