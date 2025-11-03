using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using DemandManagement.Application.DTOs;
using DemandManagement.Application.Requests;

namespace DemandManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StatusController : ControllerBase
{
    private readonly IMediator _mediator;

    public StatusController(IMediator mediator) => _mediator = mediator;

    /// <summary>
    /// Obtiene todos los estados ordenados por secuencia
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<StatusDto>>> GetAll()
    {
        var result = await _mediator.Send(new GetAllStatusesQuery());
        return Ok(result);
    }

    /// <summary>
    /// Obtiene un estado por ID
    /// </summary>
    /// <param name="id">ID del estado</param>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<StatusDto>> GetById(Guid id)
    {
        var dto = await _mediator.Send(new GetStatusByIdQuery(id));
        if (dto is null) return NotFound();
        return Ok(dto);
    }
}