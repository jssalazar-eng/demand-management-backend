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
        // Validar parámetros de paginación
        var pageNumber = request.PageNumber < 1 ? 1 : request.PageNumber;
        var pageSize = request.PageSize < 1 ? 10 : (request.PageSize > 100 ? 100 : request.PageSize);

        // Convertir IDs a value objects
        DemandTypeId? demandTypeId = request.DemandTypeId.HasValue ? DemandTypeId.From(request.DemandTypeId.Value) : null;
        StatusId? statusId = request.StatusId.HasValue ? StatusId.From(request.StatusId.Value) : null;

        // Obtener datos paginados
        var (items, totalCount) = await _uow.Demands.GetPagedAsync(
            demandTypeId,
            statusId,
            request.Priority,
            request.SearchTerm,
            pageNumber,
            pageSize,
            cancellationToken);

        // Mapear a DTOs
        var dtos = items.Select(d => new DemandDto(
            d.Id.Value,
            d.Title,
            d.Description,
            d.Priority.Level.ToString(),
            d.DemandTypeId.Value,
            d.StatusId.Value,
            d.RequestingUserId.Value,
            d.AssignedToId?.Value,
            d.DueDate,
            d.CloseDate,
            d.Audit.CreatedDate.UtcDateTime,
            d.Audit.UpdatedDate.UtcDateTime
        ));

        return new PagedResult<DemandDto>(dtos, totalCount, pageNumber, pageSize);
    }
}