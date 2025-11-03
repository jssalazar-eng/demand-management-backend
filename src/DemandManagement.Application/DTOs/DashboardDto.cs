namespace DemandManagement.Application.DTOs;

public sealed record DashboardDto(
    DashboardStatsDto Stats,
    IEnumerable<RecentDemandDto> RecentDemands
);