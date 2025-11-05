using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DemandManagement.Application.DTOs;
using DemandManagement.Application.Requests;
using DemandManagement.Domain.ValueObjects;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DemandManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DemandController : ControllerBase
{
    private readonly IMediator _mediator;
    public DemandController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<IEnumerable<DemandDto>> GetAll() => await _mediator.Send(new GetAllDemandsQuery());

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<DemandDto>> GetById(Guid id)
    {
        var dto = await _mediator.Send(new GetDemandByIdQuery(id));
        if (dto is null) return NotFound();
        return Ok(dto);
    }

    /// <summary>
    /// Obtiene demandas con filtros y paginación
    /// </summary>
    /// <param name="demandTypeId">Filtro por tipo de demanda (opcional)</param>
    /// <param name="statusId">Filtro por estado (opcional)</param>
    /// <param name="priority">Filtro por prioridad: 0=Low, 1=Medium, 2=High, 3=Critical (opcional)</param>
    /// <param name="searchTerm">Búsqueda en título o descripción (opcional)</param>
    /// <param name="pageNumber">Número de página (por defecto 1)</param>
    /// <param name="pageSize">Tamaño de página (por defecto 10, máximo 100)</param>
    [HttpGet("filtered")]
    public async Task<ActionResult<PagedResult<DemandDto>>> GetFiltered(
        [FromQuery] Guid? demandTypeId = null,
        [FromQuery] Guid? statusId = null,
        [FromQuery] PriorityLevel? priority = null,
        [FromQuery] string? searchTerm = null,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        var query = new GetFilteredDemandsQuery(demandTypeId, statusId, priority, searchTerm, pageNumber, pageSize);
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<DemandDto>> Create(CreateDemandCommand cmd)
    {
        var created = await _mediator.Send(cmd);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    /// <summary>
    /// Actualizar demanda
    /// Permite cambiar: título, descripción, prioridad (recalcula DueDate), tipo, y asignación
    /// </summary>
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<DemandDto>> Update(Guid id, UpdateDemandCommand cmd)
    {
        if (id != cmd.Id) return BadRequest("El ID de la ruta no coincide con el ID del comando");
        var updated = await _mediator.Send(cmd);
        return Ok(updated);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _mediator.Send(new DeleteDemandCommand(id));
        return NoContent();
    }
}