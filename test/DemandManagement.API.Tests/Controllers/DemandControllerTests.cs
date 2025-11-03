using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using DemandManagement.API.Controllers;
using DemandManagement.Application.DTOs;
using DemandManagement.Application.Requests;
using DemandManagement.Domain.ValueObjects;

namespace DemandManagement.API.Tests.Controllers;

public class DemandControllerTests
{
    private readonly Mock<IMediator> _mockMediator;
    private readonly DemandController _controller;

    public DemandControllerTests()
    {
        _mockMediator = new Mock<IMediator>();
        _controller = new DemandController(_mockMediator.Object);
    }

    [Fact]
    public async Task GetById_ShouldReturnOk_WhenDemandExists()
    {
        // Arrange
        var demandId = Guid.NewGuid();
        var expectedDto = new DemandDto(
            demandId,
            "Test Demand",
            "Description",
            "High",
            Guid.NewGuid(),
            "Proyecto",
            Guid.NewGuid(),
            "Registrada",
            Guid.NewGuid(),
            "Test User",
            null,
            null,
            null,
            null,
            DateTimeOffset.UtcNow,
            DateTimeOffset.UtcNow
        );

        _mockMediator.Setup(m => m.Send(It.IsAny<GetDemandByIdQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedDto);

        // Act
        var result = await _controller.GetById(demandId);

        // Assert
        result.Result.Should().BeOfType<OkObjectResult>();
        var okResult = result.Result as OkObjectResult;
        okResult!.Value.Should().Be(expectedDto);
    }

    [Fact]
    public async Task GetById_ShouldReturnNotFound_WhenDemandDoesNotExist()
    {
        // Arrange
        var demandId = Guid.NewGuid();
        _mockMediator.Setup(m => m.Send(It.IsAny<GetDemandByIdQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((DemandDto?)null);

        // Act
        var result = await _controller.GetById(demandId);

        // Assert
        result.Result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task Create_ShouldReturnCreatedAtAction_WhenCommandIsValid()
    {
        // Arrange
        var command = new CreateDemandCommand(
            "Test Demand",
            "Description",
            PriorityLevel.High,
            Guid.NewGuid(),
            Guid.NewGuid(),
            Guid.NewGuid(),
            null,
            null
        );

        var createdDto = new DemandDto(
            Guid.NewGuid(),
            command.Title,
            command.Description,
            "High",
            command.DemandTypeId,
            "Proyecto",
            command.StatusId,
            "Registrada",
            command.RequestingUserId,
            "Test User",
            null,
            null,
            null,
            null,
            DateTimeOffset.UtcNow,
            DateTimeOffset.UtcNow
        );

        _mockMediator.Setup(m => m.Send(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(createdDto);

        // Act
        var result = await _controller.Create(command);

        // Assert
        result.Result.Should().BeOfType<CreatedAtActionResult>();
        var createdResult = result.Result as CreatedAtActionResult;
        createdResult!.Value.Should().Be(createdDto);
        createdResult.ActionName.Should().Be(nameof(DemandController.GetById));
    }

    [Fact]
    public async Task Delete_ShouldReturnNoContent_WhenDemandIsDeleted()
    {
        // Arrange
        var demandId = Guid.NewGuid();
        _mockMediator.Setup(m => m.Send(It.IsAny<DeleteDemandCommand>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.Delete(demandId);

        // Assert
        result.Should().BeOfType<NoContentResult>();
    }

    [Fact]
    public async Task Update_ShouldReturnBadRequest_WhenIdMismatch()
    {
        // Arrange
        var demandId = Guid.NewGuid();
        var command = new UpdateDemandCommand(
            Guid.NewGuid(), // Different ID
            "Updated Title",
            "Updated Description",
            PriorityLevel.Critical,
            Guid.NewGuid()
        );

        // Act
        var result = await _controller.Update(demandId, command);

        // Assert
        result.Result.Should().BeOfType<BadRequestResult>();
    }
}