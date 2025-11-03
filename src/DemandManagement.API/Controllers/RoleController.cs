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
public class RoleController : ControllerBase
{
    private readonly IMediator _mediator;

    public RoleController(IMediator mediator) => _mediator = mediator;

    /// <summary>
    /// Obtiene todos los roles ordenados por nombre
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<RoleDto>>> GetAll()
    {
        var result = await _mediator.Send(new GetAllRolesQuery());
        return Ok(result);
    }

    /// <summary>
    /// Obtiene un rol por ID
    /// </summary>
    /// <param name="id">ID del rol</param>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<RoleDto>> GetById(Guid id)
    {
        var dto = await _mediator.Send(new GetRoleByIdQuery(id));
        if (dto is null) return NotFound();
        return Ok(dto);
    }
}