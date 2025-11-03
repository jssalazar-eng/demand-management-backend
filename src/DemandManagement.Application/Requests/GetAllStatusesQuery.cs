using System.Collections.Generic;
using MediatR;
using DemandManagement.Application.DTOs;

namespace DemandManagement.Application.Requests;

public sealed record GetAllStatusesQuery() : IRequest<IEnumerable<StatusDto>>;
