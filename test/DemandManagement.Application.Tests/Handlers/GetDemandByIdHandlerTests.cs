using System;
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

public class GetDemandByIdHandlerTests
{
    private readonly Mock<IUnitOfWork> _mockUow;
    private readonly Mock<IDemandRepository> _mockDemandRepo;
    private readonly Mock<IDemandTypeRepository> _mockDemandTypeRepo;
    private readonly Mock<IStatusRepository> _mockStatusRepo;
    private readonly Mock<IUserRepository> _mockUserRepo;
    private readonly GetDemandByIdHandler _handler;

    public GetDemandByIdHandlerTests()
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

        _handler = new GetDemandByIdHandler(_mockUow.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnDemandDto_WhenDemandExists()
    {
        // Arrange
        var demandId = DemandId.New();
        var demand = Demand.Create(
            "Test Demand",
            "Test Description",
            Priority.From(PriorityLevel.High),
            DemandTypeId.New(),
            StatusId.New(),
            UserId.New()
        );

        var demandType = DemandType.Create("Proyecto", "Test", "SLA");
        var status = Status.Create("Registrada", 1, isInitial: true);
        var user = User.Create(FullName.From("Test User"), CorporateEmail.From("test@test.com"), RoleId.New());

        _mockDemandRepo.Setup(r => r.GetByIdAsync(demandId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(demand);
        _mockDemandTypeRepo.Setup(r => r.GetByIdAsync(It.IsAny<DemandTypeId>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(demandType);
        _mockStatusRepo.Setup(r => r.GetByIdAsync(It.IsAny<StatusId>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(status);
        _mockUserRepo.Setup(r => r.GetByIdAsync(It.IsAny<UserId>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        var query = new GetDemandByIdQuery(demandId.Value);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Title.Should().Be("Test Demand");
        result.DemandTypeName.Should().Be("Proyecto");
        result.StatusName.Should().Be("Registrada");
        result.RequestingUserName.Should().Be("Test User");
    }

    [Fact]
    public async Task Handle_ShouldReturnNull_WhenDemandDoesNotExist()
    {
        // Arrange
        var demandId = DemandId.New();
        _mockDemandRepo.Setup(r => r.GetByIdAsync(demandId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Demand?)null);

        var query = new GetDemandByIdQuery(demandId.Value);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }
}