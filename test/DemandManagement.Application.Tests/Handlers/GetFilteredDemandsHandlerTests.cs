using System;
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

public class GetFilteredDemandsHandlerTests
{
    private readonly Mock<IUnitOfWork> _mockUow;
    private readonly Mock<IDemandRepository> _mockDemandRepo;
    private readonly Mock<IDemandTypeRepository> _mockDemandTypeRepo;
    private readonly Mock<IStatusRepository> _mockStatusRepo;
    private readonly Mock<IUserRepository> _mockUserRepo;
    private readonly GetFilteredDemandsHandler _handler;

    public GetFilteredDemandsHandlerTests()
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

        _handler = new GetFilteredDemandsHandler(_mockUow.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnPagedResult_WhenDemandsExist()
    {
        // Arrange
        var demands = new List<Demand>
        {
            Demand.Create("Demand 1", "Desc 1", Priority.From(PriorityLevel.High), DemandTypeId.New(), StatusId.New(), UserId.New()),
            Demand.Create("Demand 2", "Desc 2", Priority.From(PriorityLevel.Medium), DemandTypeId.New(), StatusId.New(), UserId.New())
        };

        var demandType = DemandType.Create("Proyecto", "Test", "SLA");
        var status = Status.Create("Registrada", 1, isInitial: true);
        var user = User.Create(FullName.From("Test User"), CorporateEmail.From("test@test.com"), RoleId.New());

        _mockDemandRepo.Setup(r => r.GetPagedAsync(
                It.IsAny<DemandTypeId?>(),
                It.IsAny<StatusId?>(),
                It.IsAny<PriorityLevel?>(),
                It.IsAny<string?>(),
                It.IsAny<int>(),
                It.IsAny<int>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((demands, 2));

        _mockDemandTypeRepo.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<DemandType> { demandType });
        _mockStatusRepo.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Status> { status });
        _mockUserRepo.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<User> { user });

        var query = new GetFilteredDemandsQuery(null, null, null, null, 1, 10);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Items.Should().HaveCount(2);
        result.TotalCount.Should().Be(2);
        result.PageNumber.Should().Be(1);
        result.PageSize.Should().Be(10);
        result.TotalPages.Should().Be(1);
    }

    [Fact]
    public async Task Handle_ShouldApplyFilters_WhenFiltersAreProvided()
    {
        // Arrange
        var demandTypeId = Guid.NewGuid();
        var statusId = Guid.NewGuid();
        var priority = PriorityLevel.High;

        _mockDemandRepo.Setup(r => r.GetPagedAsync(
                It.IsAny<DemandTypeId?>(),
                It.IsAny<StatusId?>(),
                It.Is<PriorityLevel?>(p => p == priority),
                It.IsAny<string?>(),
                It.IsAny<int>(),
                It.IsAny<int>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((new List<Demand>(), 0));

        _mockDemandTypeRepo.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<DemandType>());
        _mockStatusRepo.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Status>());
        _mockUserRepo.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<User>());

        var query = new GetFilteredDemandsQuery(demandTypeId, statusId, priority, "search", 1, 10);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        _mockDemandRepo.Verify(r => r.GetPagedAsync(
            It.IsAny<DemandTypeId?>(),
            It.IsAny<StatusId?>(),
            It.Is<PriorityLevel?>(p => p == priority),
            It.Is<string?>(s => s == "search"),
            1,
            10,
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Theory]
    [InlineData(0, 10, 1, 10)]
    [InlineData(-1, 10, 1, 10)]
    [InlineData(1, 0, 1, 10)]
    [InlineData(1, 200, 1, 100)]
    public async Task Handle_ShouldNormalizePageParameters_WhenInvalid(int pageNumber, int pageSize, int expectedPageNumber, int expectedPageSize)
    {
        // Arrange
        _mockDemandRepo.Setup(r => r.GetPagedAsync(
                It.IsAny<DemandTypeId?>(),
                It.IsAny<StatusId?>(),
                It.IsAny<PriorityLevel?>(),
                It.IsAny<string?>(),
                expectedPageNumber,
                expectedPageSize,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((new List<Demand>(), 0));

        _mockDemandTypeRepo.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<DemandType>());
        _mockStatusRepo.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Status>());
        _mockUserRepo.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<User>());

        var query = new GetFilteredDemandsQuery(null, null, null, null, pageNumber, pageSize);

        // Act
        await _handler.Handle(query, CancellationToken.None);

        // Assert
        _mockDemandRepo.Verify(r => r.GetPagedAsync(
            It.IsAny<DemandTypeId?>(),
            It.IsAny<StatusId?>(),
            It.IsAny<PriorityLevel?>(),
            It.IsAny<string?>(),
            expectedPageNumber,
            expectedPageSize,
            It.IsAny<CancellationToken>()), Times.Once);
    }
}