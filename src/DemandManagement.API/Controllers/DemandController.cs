using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using DemandManagement.Application.DTOs;
using DemandManagement.Application.Requests;

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

    [HttpPost]
    public async Task<ActionResult<DemandDto>> Create(CreateDemandCommand cmd)
    {
        var created = await _mediator.Send(cmd);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<DemandDto>> Update(Guid id, UpdateDemandCommand cmd)
    {
        if (id != cmd.Id) return BadRequest();
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