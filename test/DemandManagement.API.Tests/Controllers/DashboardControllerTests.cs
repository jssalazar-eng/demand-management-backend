using System;
using System.Collections.Generic;
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

public class DashboardControllerTests
{
    private readonly Mock<IMediator> _mockMediator;
    private readonly DashboardController _controller;

    public DashboardControllerTests()
    {
        _mockMediator = new Mock<IMediator>();
        _controller = new DashboardController(_mockMediator.Object);
    }

    [Fact]
    public async Task GetDashboardData_ShouldReturnDashboardDto()
    {
        // Arrange
        var expectedDto = new DashboardDto(
            new DashboardStatsDto(100, 30, 50, 10),
            new List<RecentDemandDto>
            {
                new(Guid.NewGuid(), "Test Demand", "High", "En Desarrollo", "Proyecto", "Test User", DateTimeOffset.UtcNow)
            }
        );

        _mockMediator.Setup(m => m.Send(It.IsAny<GetDashboardDataQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedDto);

        // Act
        var result = await _controller.GetDashboardData(5);

        // Assert
        result.Result.Should().BeOfType<OkObjectResult>();
        var okResult = result.Result as OkObjectResult;
        okResult!.Value.Should().Be(expectedDto);
    }

    [Theory]
    [InlineData(5)]
    [InlineData(10)]
    [InlineData(20)]
    public async Task GetDashboardData_ShouldPassCorrectRecentCount(int recentCount)
    {
        // Arrange
        var expectedDto = new DashboardDto(
            new DashboardStatsDto(0, 0, 0, 0),
            new List<RecentDemandDto>()
        );

        _mockMediator.Setup(m => m.Send(
                It.Is<GetDashboardDataQuery>(q => q.RecentDemandsCount == recentCount),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedDto);

        // Act
        await _controller.GetDashboardData(recentCount);

        // Assert
        _mockMediator.Verify(m => m.Send(
            It.Is<GetDashboardDataQuery>(q => q.RecentDemandsCount == recentCount),
            It.IsAny<CancellationToken>()), Times.Once);
    }
}