using System;
using MediatR;
using DemandManagement.Domain.ValueObjects;

namespace DemandManagement.Application.Requests;

public sealed record UpdateDemandCommand(
    Guid Id,
    string Title,
    string? Description,
    PriorityLevel Priority,
    Guid DemandTypeId,
    Guid? AssignedToId  // ✅ NUEVO: Permite cambiar asignación
) : IRequest<DemandManagement.Application.DTOs.DemandDto>;