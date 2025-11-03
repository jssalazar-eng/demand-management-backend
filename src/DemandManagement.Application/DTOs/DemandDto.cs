using System;

namespace DemandManagement.Application.DTOs;

public sealed record DemandDto(
    Guid Id,
    string Title,
    string? Description,
    string Priority,
    Guid DemandTypeId,
    Guid StatusId,
    Guid RequestingUserId,
    Guid? AssignedToId,
    DateTimeOffset? DueDate,
    DateTimeOffset? CloseDate,
    DateTimeOffset CreatedDate,
    DateTimeOffset UpdatedDate
);