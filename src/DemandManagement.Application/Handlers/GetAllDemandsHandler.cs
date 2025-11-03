using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using DemandManagement.Domain.Repositories;
using DemandManagement.Application.DTOs;
using DemandManagement.Application.Requests;
using DemandManagement.Application.Mappers;

namespace DemandManagement.Application.Handlers;

public sealed class GetAllDemandsHandler : IRequestHandler<GetAllDemandsQuery, IEnumerable<DemandDto>>
{
    private readonly IUnitOfWork _uow;

    public GetAllDemandsHandler(IUnitOfWork uow) => _uow = uow;

    public async Task<IEnumerable<DemandDto>> Handle(GetAllDemandsQuery request, CancellationToken cancellationToken)
    {
        var demands = await _uow.Demands.GetAllAsync(cancellationToken);
        return await DemandMapper.MapToDtosAsync(demands, _uow, cancellationToken);
    }
}