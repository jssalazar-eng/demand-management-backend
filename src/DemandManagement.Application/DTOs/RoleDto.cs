using System;

namespace DemandManagement.Application.DTOs;

public sealed record RoleDto(
    Guid Id,
    string Name,
    string? Description
);