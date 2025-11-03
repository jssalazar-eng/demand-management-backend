using System;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Xunit;
using DemandManagement.Application.Handlers;
using DemandManagement.Application.Requests;
using DemandManagement.Domain.Repositories;
using DemandManagement.Domain.ValueObjects;

namespace DemandManagement.Application.Tests.Handlers;

public class DeleteDemandHandlerTests
{
    private readonly Mock<IUnitOfWork> _mockUow;
    private readonly Mock<IDemandRepository> _mockDemandRepo;
    private readonly DeleteDemandHandler _handler;

    public DeleteDemandHandlerTests()
    {
        _mockUow = new Mock<IUnitOfWork>();
        _mockDemandRepo = new Mock<IDemandRepository>();

        _mockUow.Setup(u => u.Demands).Returns(_mockDemandRepo.Object);

        _handler = new DeleteDemandHandler(_mockUow.Object);
    }

    [Fact]
    public async Task Handle_ShouldDeleteDemand_WhenIdIsValid()
    {
        // Arrange
        var demandId = Guid.NewGuid();
        var command = new DeleteDemandCommand(demandId);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _mockDemandRepo.Verify(r => r.DeleteAsync(It.Is<DemandId>(id => id.Value == demandId), It.IsAny<CancellationToken>()), Times.Once);
        _mockUow.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}