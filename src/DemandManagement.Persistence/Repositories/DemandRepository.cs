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

public sealed class DemandRepository : IDemandRepository
{
    private readonly DemandManagementDbContext _context;

    public DemandRepository(DemandManagementDbContext context) => _context = context;

    public async Task<Demand?> GetByIdAsync(DemandId id, CancellationToken cancellationToken = default)
    {
        return await _context.Demands
            .Include(d => d.Documents)
            .FirstOrDefaultAsync(d => d.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<Demand>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Demands
            .Include(d => d.Documents)
            .ToListAsync(cancellationToken);
    }

    public async Task<(IEnumerable<Demand> Items, int TotalCount)> GetPagedAsync(
        DemandTypeId? demandTypeId,
        StatusId? statusId,
        PriorityLevel? priority,
        string? searchTerm,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        var query = _context.Demands
            .Include(d => d.Documents)
            .AsQueryable();

        // Aplicar filtros
        if (demandTypeId is not null)
        {
            query = query.Where(d => d.DemandTypeId == demandTypeId);
        }

        if (statusId is not null)
        {
            query = query.Where(d => d.StatusId == statusId);
        }

        if (priority is not null)
        {
            query = query.Where(d => d.Priority.Level == priority.Value);
        }

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            var term = searchTerm.Trim().ToLower();
            query = query.Where(d =>
                d.Title.ToLower().Contains(term) ||
                (d.Description != null && d.Description.ToLower().Contains(term)));
        }

        // Contar total antes de paginar
        var totalCount = await query.CountAsync(cancellationToken);

        // Aplicar paginación y ordenar por fecha de creación descendente
        var items = await query
            .OrderByDescending(d => d.Audit.CreatedDate)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }

    public async Task AddAsync(Demand demand, CancellationToken cancellationToken = default)
    {
        await _context.Demands.AddAsync(demand, cancellationToken);
    }

    public Task UpdateAsync(Demand demand, CancellationToken cancellationToken = default)
    {
        _context.Demands.Update(demand);
        return Task.CompletedTask;
    }

    public async Task DeleteAsync(DemandId id, CancellationToken cancellationToken = default)
    {
        var demand = await _context.Demands.FindAsync(new object[] { id }, cancellationToken);
        if (demand is not null)
        {
            _context.Demands.Remove(demand);
        }
    }
}