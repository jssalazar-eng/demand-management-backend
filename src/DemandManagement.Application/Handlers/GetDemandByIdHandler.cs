using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using DemandManagement.Domain.Repositories;
using DemandManagement.Domain.ValueObjects;
using DemandManagement.Application.DTOs;
using DemandManagement.Application.Requests;
using DemandManagement.Application.Mappers;

namespace DemandManagement.Application.Handlers;

public sealed class GetDemandByIdHandler : IRequestHandler<GetDemandByIdQuery, DemandDto?>
{
    private readonly IUnitOfWork _uow;

    public GetDemandByIdHandler(IUnitOfWork uow) => _uow = uow;

    public async Task<DemandDto?> Handle(GetDemandByIdQuery request, CancellationToken cancellationToken)
    {
        var demand = await _uow.Demands.GetByIdAsync(DemandId.From(request.Id), cancellationToken);
        if (demand is null) return null;

        return await DemandMapper.MapToDtoAsync(demand, _uow, cancellationToken);
    }
}