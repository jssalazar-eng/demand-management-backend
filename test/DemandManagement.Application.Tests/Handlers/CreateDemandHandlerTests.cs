using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Xunit;
using DemandManagement.Application.Handlers;
using DemandManagement.Application.Requests;
using DemandManagement.Application.Mappers;
using DemandManagement.Domain.Entities;
using DemandManagement.Domain.Repositories;
using DemandManagement.Domain.ValueObjects;

namespace DemandManagement.Application.Tests.Handlers;

public class CreateDemandHandlerTests
{
    private readonly Mock<IUnitOfWork> _mockUow;
    private readonly Mock<IDemandRepository> _mockDemandRepo;
    private readonly Mock<IDemandTypeRepository> _mockDemandTypeRepo;
    private readonly Mock<IStatusRepository> _mockStatusRepo;
    private readonly Mock<IUserRepository> _mockUserRepo;
    private readonly CreateDemandHandler _handler;

    public CreateDemandHandlerTests()
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

        _handler = new CreateDemandHandler(_mockUow.Object);
    }

    [Fact]
    public async Task Handle_ShouldCreateDemand_WhenCommandIsValid()
    {
        // Arrange
        var command = new CreateDemandCommand(
            "Test Demand",
            "Test Description",
            PriorityLevel.High,
            Guid.NewGuid(),
            Guid.NewGuid(),
            Guid.NewGuid(),
            null,
            null
        );

        var demandType = DemandType.Create("Proyecto", "Test", "SLA");
        var status = Status.Create("Registrada", 1, isInitial: true);
        var user = User.Create(FullName.From("Test User"), CorporateEmail.From("test@test.com"), RoleId.New());

        _mockDemandTypeRepo.Setup(r => r.GetByIdAsync(It.IsAny<DemandTypeId>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(demandType);
        _mockStatusRepo.Setup(r => r.GetByIdAsync(It.IsAny<StatusId>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(status);
        _mockUserRepo.Setup(r => r.GetByIdAsync(It.IsAny<UserId>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Title.Should().Be(command.Title);
        result.Description.Should().Be(command.Description);
        result.Priority.Should().Be(PriorityLevel.High.ToString());

        _mockDemandRepo.Verify(r => r.AddAsync(It.IsAny<Demand>(), It.IsAny<CancellationToken>()), Times.Once);
        _mockUow.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}