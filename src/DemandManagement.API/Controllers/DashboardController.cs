using DemandManagement.Application.DTOs;
using DemandManagement.Application.Requests;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DemandManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DashboardController : ControllerBase
{
    private readonly IMediator _mediator;

    public DashboardController(IMediator mediator) => _mediator = mediator;

    /// <summary>
    /// Get dashboard data including statistics and recent demands
    /// </summary>
    /// <param name="recentCount">Number of recent demands to retrieve (default: 5)</param>
    [HttpGet]
    public async Task<ActionResult<DashboardDto>> GetDashboardData([FromQuery] int recentCount = 5)
    {
        var query = new GetDashboardDataQuery(recentCount);
        var result = await _mediator.Send(query);
        return Ok(result);
    }
}