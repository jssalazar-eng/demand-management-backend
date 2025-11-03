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
    private readonly IDemandRepository _repo;
    public CreateDemandHandler(IDemandRepository repo) => _repo = repo;

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

        await _repo.AddAsync(demand, cancellationToken);

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
    private readonly IDemandRepository _repo;
    public UpdateDemandHandler(IDemandRepository repo) => _repo = repo;

    public async Task<DemandDto> Handle(UpdateDemandCommand request, CancellationToken cancellationToken)
    {
        var id = DemandId.From(request.Id);
        var existing = await _repo.GetByIdAsync(id, cancellationToken)
            ?? throw new InvalidOperationException("Demand not found.");

        existing.UpdateDetails(request.Title, request.Description, Priority.From(request.Priority), DemandTypeId.From(request.DemandTypeId));

        await _repo.UpdateAsync(existing, cancellationToken);

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
    private readonly IDemandRepository _repo;
    public DeleteDemandHandler(IDemandRepository repo) => _repo = repo;

    public async Task Handle(DeleteDemandCommand request, CancellationToken cancellationToken)
    {
        await _repo.DeleteAsync(DemandId.From(request.Id), cancellationToken);
    }
}

public sealed class GetDemandByIdHandler : IRequestHandler<GetDemandByIdQuery, DemandDto?>
{
    private readonly IDemandRepository _repo;
    public GetDemandByIdHandler(IDemandRepository repo) => _repo = repo;

    public async Task<DemandDto?> Handle(GetDemandByIdQuery request, CancellationToken cancellationToken)
    {
        var d = await _repo.GetByIdAsync(DemandId.From(request.Id), cancellationToken);
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
    private readonly IDemandRepository _repo;
    public GetAllDemandsHandler(IDemandRepository repo) => _repo = repo;

    public async Task<IEnumerable<DemandDto>> Handle(GetAllDemandsQuery request, CancellationToken cancellationToken)
    {
        var list = await _repo.GetAllAsync(cancellationToken);
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