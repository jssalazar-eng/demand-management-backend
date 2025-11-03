using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using DemandManagement.Domain.Repositories;
using DemandManagement.Domain.ValueObjects;
using DemandManagement.Application.DTOs;
using DemandManagement.Application.Requests;

namespace DemandManagement.Application.Handlers;

public sealed class GetDashboardDataHandler : IRequestHandler<GetDashboardDataQuery, DashboardDto>
{
    private readonly IUnitOfWork _uow;

    public GetDashboardDataHandler(IUnitOfWork uow) => _uow = uow;

    public async Task<DashboardDto> Handle(GetDashboardDataQuery request, CancellationToken cancellationToken)
    {
        // Execute queries sequentially to avoid DbContext concurrency issues
        var totalCount = await _uow.Demands.GetTotalCountAsync(cancellationToken);
        
        var inProgressCount = await _uow.Demands.GetCountByStatusNamesAsync(
            new[] { "En Análisis", "En Desarrollo", "En Pruebas" }, 
            cancellationToken);
        
        var completedCount = await _uow.Demands.GetCountByStatusNamesAsync(
            new[] { "Cerrada" }, 
            cancellationToken);
        
        var criticalCount = await _uow.Demands.GetCountByPriorityAsync(
            PriorityLevel.Critical, 
            cancellationToken);
        
        var recentDemands = await _uow.Demands.GetRecentAsync(
            request.RecentDemandsCount, 
            cancellationToken);

        // Load related entities for recent demands
        var demandTypes = (await _uow.DemandTypes.GetAllAsync(cancellationToken))
            .ToDictionary(dt => dt.Id.Value);
        var statuses = (await _uow.Statuses.GetAllAsync(cancellationToken))
            .ToDictionary(s => s.Id.Value);
        var users = (await _uow.Users.GetAllAsync(cancellationToken))
            .ToDictionary(u => u.Id.Value);

        var stats = new DashboardStatsDto(
            totalCount,
            inProgressCount,
            completedCount,
            criticalCount
        );

        var recentDemandDtos = recentDemands.Select(d => new RecentDemandDto(
            d.Id.Value,
            d.Title,
            d.Priority.Level.ToString(),
            statuses.TryGetValue(d.StatusId.Value, out var status) ? status.Name : "Unknown",
            demandTypes.TryGetValue(d.DemandTypeId.Value, out var type) ? type.Name : "Unknown",
            users.TryGetValue(d.RequestingUserId.Value, out var user) ? user.FullName.Value : "Unknown",
            d.Audit.CreatedDate.UtcDateTime
        ));

        return new DashboardDto(stats, recentDemandDtos);
    }
}