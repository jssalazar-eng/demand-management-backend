using System;
using System.Threading;
using System.Threading.Tasks;
using DemandManagement.Application.DTOs;
using DemandManagement.Application.Requests;
using DemandManagement.Application.Mappers;
using DemandManagement.Domain.Entities;
using DemandManagement.Domain.Exceptions;
using DemandManagement.Domain.Repositories;
using DemandManagement.Domain.ValueObjects;
using MediatR;

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

        return await DemandMapper.MapToDtoAsync(demand, _uow, cancellationToken);
    }
}

public sealed class UpdateDemandHandler : IRequestHandler<UpdateDemandCommand, DemandDto>
{
    private readonly IUnitOfWork _uow;
    public UpdateDemandHandler(IUnitOfWork uow) => _uow = uow;

    public async Task<DemandDto> Handle(UpdateDemandCommand request, CancellationToken cancellationToken)
    {
        var id = DemandId.From(request.Id);
        var existing = await _uow.Demands.GetByIdAsync(id, cancellationToken)
            ?? throw new NotFoundException(nameof(Demand), request.Id);

        existing.UpdateDetails(
            request.Title, 
            request.Description, 
            Priority.From(request.Priority), 
            DemandTypeId.From(request.DemandTypeId)
        );

        // ✅ Actualizar asignación solo si es diferente al actual
        if (request.AssignedToId.HasValue)
        {
            var newAssignee = UserId.From(request.AssignedToId.Value);
            
            // ✅ VALIDACIÓN: Solo actualizar si realmente cambió
            if (existing.AssignedToId?.Value != newAssignee.Value)
            {
                existing.AssignTo(newAssignee);
            }
            // Si es el mismo usuario, no hace nada (evita actualización innecesaria)
        }
        else
        {
            // Si se envía null, desasignar solo si había alguien asignado
            if (existing.AssignedToId != null)
            {
                existing.Unassign();
            }
        }

        await _uow.Demands.UpdateAsync(existing, cancellationToken);
        await _uow.SaveChangesAsync(cancellationToken);

        return await DemandMapper.MapToDtoAsync(existing, _uow, cancellationToken);
    }
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