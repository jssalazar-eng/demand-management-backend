using MediatR;
using DemandManagement.Application.DTOs;
using DemandManagement.Domain.Constants;

namespace DemandManagement.Application.Requests;

public sealed record GetDashboardDataQuery(
    int RecentDemandsCount = ValidationConstants.Pagination.DefaultRecentItemsCount 
) : IRequest<DashboardDto>;