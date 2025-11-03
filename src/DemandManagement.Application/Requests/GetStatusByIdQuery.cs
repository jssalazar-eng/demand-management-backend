using System;
using MediatR;
using DemandManagement.Application.DTOs;

namespace DemandManagement.Application.Requests;

public sealed record GetStatusByIdQuery(Guid Id) : IRequest<StatusDto?>;