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

public sealed class GetRoleByIdHandler : IRequestHandler<GetRoleByIdQuery, RoleDto?>
{
    private readonly IUnitOfWork _uow;

    public GetRoleByIdHandler(IUnitOfWork uow) => _uow = uow;

    public async Task<RoleDto?> Handle(GetRoleByIdQuery request, CancellationToken cancellationToken)
    {
        var role = await _uow.Roles.GetByIdAsync(RoleId.From(request.Id), cancellationToken);
        
        if (role is null) return null;

        return new RoleDto(
            role.Id.Value,
            role.Name,
            role.Description
        );
    }
}

public sealed class GetAllRolesHandler : IRequestHandler<GetAllRolesQuery, IEnumerable<RoleDto>>
{
    private readonly IUnitOfWork _uow;

    public GetAllRolesHandler(IUnitOfWork uow) => _uow = uow;

    public async Task<IEnumerable<RoleDto>> Handle(GetAllRolesQuery request, CancellationToken cancellationToken)
    {
        var roles = await _uow.Roles.GetAllAsync(cancellationToken);

        return roles.Select(r => new RoleDto(
            r.Id.Value,
            r.Name,
            r.Description
        ));
    }
}