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

public sealed class GetDemandTypeByIdHandler : IRequestHandler<GetDemandTypeByIdQuery, DemandTypeDto?>
{
    private readonly IUnitOfWork _uow;

    public GetDemandTypeByIdHandler(IUnitOfWork uow) => _uow = uow;

    public async Task<DemandTypeDto?> Handle(GetDemandTypeByIdQuery request, CancellationToken cancellationToken)
    {
        var demandType = await _uow.DemandTypes.GetByIdAsync(DemandTypeId.From(request.Id), cancellationToken);

        if (demandType is null) return null;

        return new DemandTypeDto(
            demandType.Id.Value,
            demandType.Name,
            demandType.Description,
            demandType.ServiceLevel
        );
    }
}

public sealed class GetAllDemandTypesHandler : IRequestHandler<GetAllDemandTypesQuery, IEnumerable<DemandTypeDto>>
{
    private readonly IUnitOfWork _uow;

    public GetAllDemandTypesHandler(IUnitOfWork uow) => _uow = uow;

    public async Task<IEnumerable<DemandTypeDto>> Handle(GetAllDemandTypesQuery request, CancellationToken cancellationToken)
    {
        var demandTypes = await _uow.DemandTypes.GetAllAsync(cancellationToken);

        return demandTypes.Select(dt => new DemandTypeDto(
            dt.Id.Value,
            dt.Name,
            dt.Description,
            dt.ServiceLevel
        ));
    }
}