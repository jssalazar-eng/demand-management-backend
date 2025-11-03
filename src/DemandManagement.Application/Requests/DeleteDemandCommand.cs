using System;
using MediatR;

namespace DemandManagement.Application.Requests;

public sealed record DeleteDemandCommand(Guid Id) : IRequest;