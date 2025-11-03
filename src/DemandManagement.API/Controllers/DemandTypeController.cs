using DemandManagement.Application.DTOs;
using DemandManagement.Application.Requests;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DemandManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DemandTypeController : ControllerBase
{
    private readonly IMediator _mediator;

    public DemandTypeController(IMediator mediator) => _mediator = mediator;

    /// <summary>
    /// Obtiene todos los tipos de demanda
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<DemandTypeDto>>> GetAll()
    {
        var result = await _mediator.Send(new GetAllDemandTypesQuery());
        return Ok(result);
    }

    /// <summary>
    /// Obtiene un tipo de demanda por ID
    /// </summary>
    /// <param name="id">ID del tipo de demanda</param>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<DemandTypeDto>> GetById(Guid id)
    {
        var dto = await _mediator.Send(new GetDemandTypeByIdQuery(id));
        if (dto is null) return NotFound();
        return Ok(dto);
    }
}