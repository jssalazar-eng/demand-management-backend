using System;
using FluentAssertions;
using Xunit;
using DemandManagement.Domain.Entities;

namespace DemandManagement.Domain.Tests.Entities;

public class DemandTypeTests
{
    [Fact]
    public void Create_ShouldCreateValidDemandType_WhenParametersAreValid()
    {
        // Arrange
        var name = "Proyecto";
        var description = "Test Description";
        var serviceLevel = "SLA: 30 días";

        // Act
        var demandType = DemandType.Create(name, description, serviceLevel);

        // Assert
        demandType.Should().NotBeNull();
        demandType.Name.Should().Be(name);
        demandType.Description.Should().Be(description);
        demandType.ServiceLevel.Should().Be(serviceLevel);
        demandType.Id.Value.Should().NotBeEmpty();
    }

    [Theory]
    [InlineData("")]
    [InlineData("  ")]
    [InlineData(null)]
    public void Create_ShouldThrowArgumentException_WhenNameIsInvalid(string name)
    {
        // Act & Assert
        var act = () => DemandType.Create(name, "Description", "SLA");
        act.Should().Throw<ArgumentException>().WithMessage("*Name required*");
    }

    [Fact]
    public void Update_ShouldUpdateProperties_WhenParametersAreValid()
    {
        // Arrange
        var demandType = DemandType.Create("Old Name", "Old Description", "Old SLA");
        var newName = "New Name";
        var newDescription = "New Description";
        var newServiceLevel = "New SLA";

        // Act
        demandType.Update(newName, newDescription, newServiceLevel);

        // Assert
        demandType.Name.Should().Be(newName);
        demandType.Description.Should().Be(newDescription);
        demandType.ServiceLevel.Should().Be(newServiceLevel);
    }
}