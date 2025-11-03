using System;
using MediatR;
using DemandManagement.Application.DTOs;

namespace DemandManagement.Application.Requests;

public sealed record GetRoleByIdQuery(Guid Id) : IRequest<RoleDto?>;