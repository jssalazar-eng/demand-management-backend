using System;

namespace DemandManagement.Application.DTOs;

public sealed record RecentDemandDto(
    Guid Id,
    string Title,
    string Priority,
    string StatusName,
    string DemandTypeName,
    string RequestingUserName,
    DateTimeOffset CreatedDate
);