using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DemandManagement.Domain.Entities;
using DemandManagement.Domain.Repositories;
using DemandManagement.Application.DTOs;
using DemandManagement.Application.Requests;
using DemandManagement.Domain.ValueObjects;

namespace DemandManagement.Application.Mappers;

public static class DemandMapper
{
    public static async Task<DemandDto> MapToDtoAsync(
        Demand demand,
        IUnitOfWork uow,
        CancellationToken cancellationToken = default)
    {
        var demandType = await uow.DemandTypes.GetByIdAsync(demand.DemandTypeId, cancellationToken);
        var status = await uow.Statuses.GetByIdAsync(demand.StatusId, cancellationToken);
        var requestingUser = await uow.Users.GetByIdAsync(demand.RequestingUserId, cancellationToken);

        User? assignedUser = null;
        if (demand.AssignedToId.HasValue)
        {
            assignedUser = await uow.Users.GetByIdAsync(demand.AssignedToId.Value, cancellationToken);
        }

        return new DemandDto(
            demand.Id.Value,
            demand.Title,
            demand.Description,
            demand.Priority.Level.ToString(),
            demand.DemandTypeId.Value,
            demandType?.Name ?? "Unknown",
            demand.StatusId.Value,
            status?.Name ?? "Unknown",
            demand.RequestingUserId.Value,
            requestingUser?.FullName.Value ?? "Unknown",
            demand.AssignedToId?.Value,
            assignedUser?.FullName.Value,
            demand.DueDate,
            demand.CloseDate,
            demand.Audit.CreatedDate.UtcDateTime,
            demand.Audit.UpdatedDate.UtcDateTime
        );
    }

    public static async Task<IEnumerable<DemandDto>> MapToDtosAsync(
        IEnumerable<Demand> demands,
        IUnitOfWork uow,
        CancellationToken cancellationToken = default)
    {
        var demandTypes = (await uow.DemandTypes.GetAllAsync(cancellationToken)).ToDictionary(dt => dt.Id.Value);
        var statuses = (await uow.Statuses.GetAllAsync(cancellationToken)).ToDictionary(s => s.Id.Value);
        var users = (await uow.Users.GetAllAsync(cancellationToken)).ToDictionary(u => u.Id.Value);

        return demands.Select(d => new DemandDto(
            d.Id.Value,
            d.Title,
            d.Description,
            d.Priority.Level.ToString(),
            d.DemandTypeId.Value,
            demandTypes.TryGetValue(d.DemandTypeId.Value, out var dt) ? dt.Name : "Unknown",
            d.StatusId.Value,
            statuses.TryGetValue(d.StatusId.Value, out var st) ? st.Name : "Unknown",
            d.RequestingUserId.Value,
            users.TryGetValue(d.RequestingUserId.Value, out var reqUser) ? reqUser.FullName.Value : "Unknown",
            d.AssignedToId?.Value,
            d.AssignedToId.HasValue && users.TryGetValue(d.AssignedToId.Value.Value, out var assignedUser) ? assignedUser.FullName.Value : null,
            d.DueDate,
            d.CloseDate,
            d.Audit.CreatedDate.UtcDateTime,
            d.Audit.UpdatedDate.UtcDateTime
        ));
    }
}