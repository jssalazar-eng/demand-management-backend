using System;

namespace DemandManagement.Application.DTOs;

public sealed record StatusDto(
    Guid Id,
    string Name,
    int SequenceOrder,
    bool IsFinal,
    bool IsInitial
);