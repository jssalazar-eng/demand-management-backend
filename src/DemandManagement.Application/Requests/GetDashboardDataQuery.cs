using MediatR;
using DemandManagement.Application.DTOs;

namespace DemandManagement.Application.Requests;

public sealed record GetDashboardDataQuery(int RecentDemandsCount = 5) : IRequest<DashboardDto>;