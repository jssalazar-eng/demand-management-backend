using System;
using MediatR;
using DemandManagement.Domain.ValueObjects;

namespace DemandManagement.Application.Requests;

public sealed record CreateDemandCommand(
    string Title,
    string? Description,
    PriorityLevel Priority,
    Guid DemandTypeId,
    Guid StatusId,
    Guid RequestingUserId,
    Guid? AssignedToId,
    DateTimeOffset? DueDate
) : IRequest<DemandManagement.Application.DTOs.DemandDto>;