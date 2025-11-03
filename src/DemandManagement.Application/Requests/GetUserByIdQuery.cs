using System;
using MediatR;
using DemandManagement.Application.DTOs;

namespace DemandManagement.Application.Requests;

public sealed record GetUserByIdQuery(Guid Id) : IRequest<UserDto?>;