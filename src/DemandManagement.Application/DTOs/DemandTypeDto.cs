using System;

namespace DemandManagement.Application.DTOs;

public sealed record DemandTypeDto(
    Guid Id,
    string Name,
    string? Description,
    string? ServiceLevel
);