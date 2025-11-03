using System;
using MediatR;
using DemandManagement.Application.DTOs;
using DemandManagement.Domain.ValueObjects;

namespace DemandManagement.Application.Requests;

public sealed record GetFilteredDemandsQuery(
    Guid? DemandTypeId = null,
    Guid? StatusId = null,
    PriorityLevel? Priority = null,
    string? SearchTerm = null,
    int PageNumber = 1,
    int PageSize = 10
) : IRequest<PagedResult<DemandDto>>;