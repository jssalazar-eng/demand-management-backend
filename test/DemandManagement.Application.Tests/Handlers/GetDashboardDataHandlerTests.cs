using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Xunit;
using DemandManagement.Application.Handlers;
using DemandManagement.Application.Requests;
using DemandManagement.Domain.Entities;
using DemandManagement.Domain.Repositories;
using DemandManagement.Domain.ValueObjects;

namespace DemandManagement.Application.Tests.Handlers;

public class GetDashboardDataHandlerTests
{
    private readonly Mock<IUnitOfWork> _mockUow;
    private readonly Mock<IDemandRepository> _mockDemandRepo;
    private readonly Mock<IDemandTypeRepository> _mockDemandTypeRepo;
    private readonly Mock<IStatusRepository> _mockStatusRepo;
    private readonly Mock<IUserRepository> _mockUserRepo;
    private readonly GetDashboardDataHandler _handler;

    public GetDashboardDataHandlerTests()
    {
        _mockUow = new Mock<IUnitOfWork>();
        _mockDemandRepo = new Mock<IDemandRepository>();
        _mockDemandTypeRepo = new Mock<IDemandTypeRepository>();
        _mockStatusRepo = new Mock<IStatusRepository>();
        _mockUserRepo = new Mock<IUserRepository>();

        _mockUow.Setup(u => u.Demands).Returns(_mockDemandRepo.Object);
        _mockUow.Setup(u => u.DemandTypes).Returns(_mockDemandTypeRepo.Object);
        _mockUow.Setup(u => u.Statuses).Returns(_mockStatusRepo.Object);
        _mockUow.Setup(u => u.Users).Returns(_mockUserRepo.Object);

        _handler = new GetDashboardDataHandler(_mockUow.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnDashboardData_WithCorrectStats()
    {
        // Arrange
        _mockDemandRepo.Setup(r => r.GetTotalCountAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(100);
        
        _mockDemandRepo.Setup(r => r.GetCountByStatusNamesAsync(
                It.Is<IEnumerable<string>>(names => names.Contains("En Desarrollo")),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(30);
        
        _mockDemandRepo.Setup(r => r.GetCountByStatusNamesAsync(
                It.Is<IEnumerable<string>>(names => names.Contains("Cerrada")),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(50);
        
        _mockDemandRepo.Setup(r => r.GetCountByPriorityAsync(PriorityLevel.Critical, It.IsAny<CancellationToken>()))
            .ReturnsAsync(10);

        var recentDemands = new List<Demand>
        {
            Demand.Create("Demand 1", "Desc", Priority.From(PriorityLevel.High), DemandTypeId.New(), StatusId.New(), UserId.New())
        };

        _mockDemandRepo.Setup(r => r.GetRecentAsync(5, It.IsAny<CancellationToken>()))
            .ReturnsAsync(recentDemands);

        var demandType = DemandType.Create("Proyecto", "Test", "SLA");
        var status = Status.Create("En Desarrollo", 1);
        var user = User.Create(FullName.From("Test User"), CorporateEmail.From("test@test.com"), RoleId.New());

        _mockDemandTypeRepo.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<DemandType> { demandType });
        _mockStatusRepo.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Status> { status });
        _mockUserRepo.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<User> { user });

        var query = new GetDashboardDataQuery(5);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Stats.TotalDemands.Should().Be(100);
        result.Stats.InProgressDemands.Should().Be(30);
        result.Stats.CompletedDemands.Should().Be(50);
        result.Stats.CriticalDemands.Should().Be(10);
        result.RecentDemands.Should().HaveCount(1);
    }

    [Fact]
    public async Task Handle_ShouldReturnEmptyRecentDemands_WhenNoneExist()
    {
        // Arrange
        _mockDemandRepo.Setup(r => r.GetTotalCountAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(0);
        _mockDemandRepo.Setup(r => r.GetCountByStatusNamesAsync(It.IsAny<IEnumerable<string>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(0);
        _mockDemandRepo.Setup(r => r.GetCountByPriorityAsync(It.IsAny<PriorityLevel>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(0);
        _mockDemandRepo.Setup(r => r.GetRecentAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Demand>());

        _mockDemandTypeRepo.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<DemandType>());
        _mockStatusRepo.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Status>());
        _mockUserRepo.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<User>());

        var query = new GetDashboardDataQuery(5);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.RecentDemands.Should().BeEmpty();
        result.Stats.TotalDemands.Should().Be(0);
    }
}