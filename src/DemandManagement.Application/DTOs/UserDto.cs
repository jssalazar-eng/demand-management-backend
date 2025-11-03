using System;

namespace DemandManagement.Application.DTOs;

public sealed record UserDto(
    Guid Id,
    string FullName,
    string CorporateEmail,
    Guid RoleId,
    string? Department
);