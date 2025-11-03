using System;

namespace DemandManagement.Application.DTOs;

public sealed record DemandDto(
    Guid Id,
    string Title,
    string? Description,
    string Priority,
    Guid DemandTypeId,
    string DemandTypeName,
    Guid StatusId,
    string StatusName,
    Guid RequestingUserId,
    string RequestingUserName,
    Guid? AssignedToId,
    string? AssignedToName,
    DateTimeOffset? DueDate,
    DateTimeOffset? CloseDate,
    DateTimeOffset CreatedDate,
    DateTimeOffset UpdatedDate
);