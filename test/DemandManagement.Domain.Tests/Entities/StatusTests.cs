using System;
using FluentAssertions;
using Xunit;
using DemandManagement.Domain.Entities;

namespace DemandManagement.Domain.Tests.Entities;

public class StatusTests
{
    [Fact]
    public void Create_ShouldCreateValidStatus_WhenParametersAreValid()
    {
        // Arrange
        var name = "Registrada";
        var order = 1;
        var isFinal = false;
        var isInitial = true;

        // Act
        var status = Status.Create(name, order, isFinal, isInitial);

        // Assert
        status.Should().NotBeNull();
        status.Name.Should().Be(name);
        status.SequenceOrder.Should().Be(order);
        status.IsFinal.Should().Be(isFinal);
        status.IsInitial.Should().Be(isInitial);
        status.Id.Value.Should().NotBeEmpty();
    }

    [Theory]
    [InlineData("")]
    [InlineData("  ")]
    [InlineData(null)]
    public void Create_ShouldThrowArgumentException_WhenNameIsInvalid(string? name)
    {
        // Act & Assert
        var act = () => Status.Create(name!, 1);
        act.Should().Throw<ArgumentException>().WithMessage("*Name required*");
    }

    [Fact]
    public void Update_ShouldUpdateProperties_WhenParametersAreValid()
    {
        // Arrange
        var status = Status.Create("Old Name", 1, false, true);
        var newName = "New Name";
        var newOrder = 2;

        // Act
        status.Update(newName, newOrder, true, false);

        // Assert
        status.Name.Should().Be(newName);
        status.SequenceOrder.Should().Be(newOrder);
        status.IsFinal.Should().BeTrue();
        status.IsInitial.Should().BeFalse();
    }
}