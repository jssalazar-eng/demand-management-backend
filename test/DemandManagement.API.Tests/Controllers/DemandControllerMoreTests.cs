using System;
using System.Collections.Generic;
using System.Linq;
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

namespace DemandManagement.API.Tests.Controllers;

public class DemandControllerMoreTests
{
    private readonly Mock<IMediator> _mockMediator;
    private readonly DemandController _controller;

    public DemandControllerMoreTests()
    {
        _mockMediator = new Mock<IMediator>();
        _controller = new DemandController(_mockMediator.Object);
    }

    [Fact]
    public async Task GetAll_ShouldReturnAllDemands()
    {
        // Arrange
        var expectedDtos = new List<DemandDto>
        {
            CreateSampleDemandDto(),
            CreateSampleDemandDto()
        };

        _mockMediator.Setup(m => m.Send(It.IsAny<GetAllDemandsQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedDtos);

        // Act
        var result = await _controller.GetAll();

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(2);
    }

    [Fact]
    public async Task GetFiltered_ShouldReturnPagedResult()
    {
        // Arrange
        var pagedResult = new PagedResult<DemandDto>(
            new List<DemandDto> { CreateSampleDemandDto() },
            1,
            1,
            10
        );

        _mockMediator.Setup(m => m.Send(It.IsAny<GetFilteredDemandsQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(pagedResult);

        // Act
        var result = await _controller.GetFiltered();

        // Assert
        result.Result.Should().BeOfType<OkObjectResult>();
        var okResult = result.Result as OkObjectResult;
        var value = okResult!.Value as PagedResult<DemandDto>;
        value.Should().NotBeNull();
        value!.Items.Should().HaveCount(1);
    }

    private static DemandDto CreateSampleDemandDto() => new(
        Guid.NewGuid(),
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
}