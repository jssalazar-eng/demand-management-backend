using System;
using FluentAssertions;
using Xunit;
using DemandManagement.Domain.Entities;
using DemandManagement.Domain.ValueObjects;
using System.Threading.Tasks; // <-- No change needed here, just for context

namespace DemandManagement.Domain.Tests.Entities;

public class DemandTests
{
    [Fact]
    public void Create_ShouldCreateValidDemand_WhenAllParametersAreValid()
    {
        // Arrange
        var title = "Test Demand";
        var description = "Test Description";
        var priority = Priority.From(PriorityLevel.High);
        var demandTypeId = DemandTypeId.New();
        var statusId = StatusId.New();
        var requestingUserId = UserId.New();

        // Act
        var demand = Demand.Create(title, description, priority, demandTypeId, statusId, requestingUserId);

        // Assert
        demand.Should().NotBeNull();
        demand.Title.Should().Be(title);
        demand.Description.Should().Be(description);
        demand.Priority.Should().Be(priority);
        demand.DemandTypeId.Should().Be(demandTypeId);
        demand.StatusId.Should().Be(statusId);
        demand.RequestingUserId.Should().Be(requestingUserId);
        demand.Id.Value.Should().NotBeEmpty();
        demand.Audit.CreatedDate.Should().BeCloseTo(DateTimeOffset.UtcNow, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public void Create_ShouldThrowArgumentException_WhenTitleIsEmpty()
    {
        // Arrange
        var title = "";
        var priority = Priority.From(PriorityLevel.High);
        var demandTypeId = DemandTypeId.New();
        var statusId = StatusId.New();
        var requestingUserId = UserId.New();

        // Act & Assert
        var act = () => Demand.Create(title, null, priority, demandTypeId, statusId, requestingUserId);
        act.Should().Throw<ArgumentException>().WithMessage("*Title is required*");
    }

    [Fact]
    public void UpdateDetails_ShouldUpdateDemandProperties()
    {
        // Arrange
        var demand = CreateTestDemand();
        var newTitle = "Updated Title";
        var newDescription = "Updated Description";
        var newPriority = Priority.From(PriorityLevel.Critical);
        var newDemandTypeId = DemandTypeId.New();

        // Act
        demand.UpdateDetails(newTitle, newDescription, newPriority, newDemandTypeId);

        // Assert
        demand.Title.Should().Be(newTitle);
        demand.Description.Should().Be(newDescription);
        demand.Priority.Should().Be(newPriority);
        demand.DemandTypeId.Should().Be(newDemandTypeId);
        demand.Audit.UpdatedDate.Should().BeAfter(demand.Audit.CreatedDate);
    }

    [Fact]
    public void AssignTo_ShouldAssignUserToDemand()
    {
        // Arrange
        var demand = CreateTestDemand();
        var assigneeId = UserId.New();

        // Act
        demand.AssignTo(assigneeId);

        // Assert
        demand.AssignedToId.Should().Be(assigneeId);
        demand.Audit.UpdatedDate.Should().BeAfter(demand.Audit.CreatedDate);
    }

    [Fact]
    public void Close_ShouldSetCloseDate()
    {
        // Arrange
        var demand = CreateTestDemand();
        var closeDate = DateTimeOffset.UtcNow;

        // Act
        demand.Close(closeDate);

        // Assert
        demand.CloseDate.Should().Be(closeDate);
    }

    [Fact]
    public void Close_ShouldThrowException_WhenAlreadyClosed()
    {
        // Arrange
        var demand = CreateTestDemand();
        demand.Close(DateTimeOffset.UtcNow);

        // Act & Assert
        Action act = () => demand.Close(DateTimeOffset.UtcNow);
        act.Should().Throw<InvalidOperationException>().WithMessage("*already closed*");
    }

    private static Demand CreateTestDemand()
    {
        return Demand.Create(
            "Test Demand",
            "Test Description",
            Priority.From(PriorityLevel.Medium),
            DemandTypeId.New(),
            StatusId.New(),
            UserId.New()
        );
    }
}