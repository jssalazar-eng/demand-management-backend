using System;
using MediatR;
using DemandManagement.Application.DTOs;

namespace DemandManagement.Application.Requests;

public sealed record GetDemandByIdQuery(Guid Id) : IRequest<DemandDto?>;