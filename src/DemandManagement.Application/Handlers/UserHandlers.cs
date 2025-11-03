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

public sealed class GetUserByIdHandler : IRequestHandler<GetUserByIdQuery, UserDto?>
{
    private readonly IUnitOfWork _uow;

    public GetUserByIdHandler(IUnitOfWork uow) => _uow = uow;

    public async Task<UserDto?> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var user = await _uow.Users.GetByIdAsync(UserId.From(request.Id), cancellationToken);
        
        if (user is null) return null;

        return new UserDto(
            user.Id.Value,
            user.FullName.Value,
            user.CorporateEmail.Value,
            user.RoleId.Value,
            user.Department
        );
    }
}

public sealed class GetAllUsersHandler : IRequestHandler<GetAllUsersQuery, IEnumerable<UserDto>>
{
    private readonly IUnitOfWork _uow;

    public GetAllUsersHandler(IUnitOfWork uow) => _uow = uow;

    public async Task<IEnumerable<UserDto>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
    {
        var users = await _uow.Users.GetAllAsync(cancellationToken);

        return users.Select(u => new UserDto(
            u.Id.Value,
            u.FullName.Value,
            u.CorporateEmail.Value,
            u.RoleId.Value,
            u.Department
        ));
    }
}