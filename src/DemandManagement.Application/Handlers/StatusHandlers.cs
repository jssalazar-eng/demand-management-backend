using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using DemandManagement.Domain.Repositories;
using DemandManagement.Domain.ValueObjects;
using DemandManagement.Application.DTOs;
using DemandManagement.Application.Requests;

namespace DemandManagement.Application.Handlers;

public sealed class GetStatusByIdHandler : IRequestHandler<GetStatusByIdQuery, StatusDto?>
{
    private readonly IUnitOfWork _uow;

    public GetStatusByIdHandler(IUnitOfWork uow) => _uow = uow;

    public async Task<StatusDto?> Handle(GetStatusByIdQuery request, CancellationToken cancellationToken)
    {
        var status = await _uow.Statuses.GetByIdAsync(StatusId.From(request.Id), cancellationToken);
        
        if (status is null) return null;

        return new StatusDto(
            status.Id.Value,
            status.Name,
            status.SequenceOrder,
            status.IsFinal,
            status.IsInitial
        );
    }
}

public sealed class GetAllStatusesHandler : IRequestHandler<GetAllStatusesQuery, IEnumerable<StatusDto>>
{
    private readonly IUnitOfWork _uow;

    public GetAllStatusesHandler(IUnitOfWork uow) => _uow = uow;

    public async Task<IEnumerable<StatusDto>> Handle(GetAllStatusesQuery request, CancellationToken cancellationToken)
    {
        var statuses = await _uow.Statuses.GetAllAsync(cancellationToken);

        return statuses.Select(s => new StatusDto(
            s.Id.Value,
            s.Name,
            s.SequenceOrder,
            s.IsFinal,
            s.IsInitial
        ));
    }
}