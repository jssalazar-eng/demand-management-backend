using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using DemandManagement.Domain.Repositories;
using DemandManagement.Domain.ValueObjects;
using DemandManagement.Application.DTOs;
using DemandManagement.Application.Requests;

namespace DemandManagement.Application.Handlers;

public sealed class GetFilteredDemandsHandler : IRequestHandler<GetFilteredDemandsQuery, PagedResult<DemandDto>>
{
    private readonly IUnitOfWork _uow;

    public GetFilteredDemandsHandler(IUnitOfWork uow) => _uow = uow;

    public async Task<PagedResult<DemandDto>> Handle(GetFilteredDemandsQuery request, CancellationToken cancellationToken)
    {
        var pageNumber = request.PageNumber < 1 ? 1 : request.PageNumber;
        var pageSize = request.PageSize < 1 ? 10 : (request.PageSize > 100 ? 100 : request.PageSize);

        DemandTypeId? demandTypeId = request.DemandTypeId.HasValue ? DemandTypeId.From(request.DemandTypeId.Value) : null;
        StatusId? statusId = request.StatusId.HasValue ? StatusId.From(request.StatusId.Value) : null;

        var (items, totalCount) = await _uow.Demands.GetPagedAsync(
            demandTypeId,
            statusId,
            request.Priority,
            request.SearchTerm,
            pageNumber,
            pageSize,
            cancellationToken);

        var demandTypes = (await _uow.DemandTypes.GetAllAsync(cancellationToken)).ToDictionary(dt => dt.Id.Value);
        var statuses = (await _uow.Statuses.GetAllAsync(cancellationToken)).ToDictionary(s => s.Id.Value);
        var users = (await _uow.Users.GetAllAsync(cancellationToken)).ToDictionary(u => u.Id.Value);

        var dtos = items.Select(d => new DemandDto(
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

        return new PagedResult<DemandDto>(dtos, totalCount, pageNumber, pageSize);
    }
}