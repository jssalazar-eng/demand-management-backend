using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using MediatR;
using DemandManagement.Domain.Repositories;
using DemandManagement.Domain.ValueObjects;
using DemandManagement.Application.DTOs;
using DemandManagement.Application.Requests;
using DemandManagement.Domain.Entities;

namespace DemandManagement.Application.Handlers;

public sealed class CreateDemandHandler : IRequestHandler<CreateDemandCommand, DemandDto>
{
    private readonly IUnitOfWork _uow;
    public CreateDemandHandler(IUnitOfWork uow) => _uow = uow;

    public async Task<DemandDto> Handle(CreateDemandCommand request, CancellationToken cancellationToken)
    {
        var priority = Priority.From(request.Priority);
        var demand = Demand.Create(
            request.Title,
            request.Description,
            priority,
            DemandTypeId.From(request.DemandTypeId),
            StatusId.From(request.StatusId),
            UserId.From(request.RequestingUserId),
            request.AssignedToId is null ? null : UserId.From(request.AssignedToId.Value),
            request.DueDate
        );

        await _uow.Demands.AddAsync(demand, cancellationToken);
        await _uow.SaveChangesAsync(cancellationToken);

        return Map(demand);
    }

    private static DemandDto Map(Demand d) => new(
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
    );
}

public sealed class UpdateDemandHandler : IRequestHandler<UpdateDemandCommand, DemandDto>
{
    private readonly IUnitOfWork _uow;
    public UpdateDemandHandler(IUnitOfWork uow) => _uow = uow;

    public async Task<DemandDto> Handle(UpdateDemandCommand request, CancellationToken cancellationToken)
    {
        var id = DemandId.From(request.Id);
        var existing = await _uow.Demands.GetByIdAsync(id, cancellationToken)
            ?? throw new InvalidOperationException("Demand not found.");

        existing.UpdateDetails(request.Title, request.Description, Priority.From(request.Priority), DemandTypeId.From(request.DemandTypeId));

        await _uow.Demands.UpdateAsync(existing, cancellationToken);
        await _uow.SaveChangesAsync(cancellationToken);

        return Map(existing);
    }

    private static DemandDto Map(Demand d) => new(
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
    );
}

public sealed class DeleteDemandHandler : IRequestHandler<DeleteDemandCommand>
{
    private readonly IUnitOfWork _uow;
    public DeleteDemandHandler(IUnitOfWork uow) => _uow = uow;

    public async Task Handle(DeleteDemandCommand request, CancellationToken cancellationToken)
    {
        await _uow.Demands.DeleteAsync(DemandId.From(request.Id), cancellationToken);
        await _uow.SaveChangesAsync(cancellationToken);
    }
}

public sealed class GetDemandByIdHandler : IRequestHandler<GetDemandByIdQuery, DemandDto?>
{
    private readonly IUnitOfWork _uow;
    public GetDemandByIdHandler(IUnitOfWork uow) => _uow = uow;

    public async Task<DemandDto?> Handle(GetDemandByIdQuery request, CancellationToken cancellationToken)
    {
        var d = await _uow.Demands.GetByIdAsync(DemandId.From(request.Id), cancellationToken);
        if (d is null) return null;

        return new DemandDto(
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
        );
    }
}

public sealed class GetAllDemandsHandler : IRequestHandler<GetAllDemandsQuery, IEnumerable<DemandDto>>
{
    private readonly IUnitOfWork _uow;
    public GetAllDemandsHandler(IUnitOfWork uow) => _uow = uow;

    public async Task<IEnumerable<DemandDto>> Handle(GetAllDemandsQuery request, CancellationToken cancellationToken)
    {
        var list = await _uow.Demands.GetAllAsync(cancellationToken);
        return list.Select(d => new DemandDto(
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
    }
}