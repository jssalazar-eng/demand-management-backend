namespace DemandManagement.Application.DTOs;

public sealed record DashboardStatsDto(
    int TotalDemands,
    int InProgressDemands,
    int CompletedDemands,
    int CriticalDemands
);