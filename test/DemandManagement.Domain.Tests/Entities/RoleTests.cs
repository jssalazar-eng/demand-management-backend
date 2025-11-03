using System;
using FluentAssertions;
using Xunit;
using DemandManagement.Domain.Entities;

namespace DemandManagement.Domain.Tests.Entities;

public class RoleTests
{
    [Fact]
    public void Create_ShouldCreateValidRole_WhenParametersAreValid()
    {
        // Arrange
        var name = "Administrator";
        var description = "Full system access";

        // Act
        var role = Role.Create(name, description);

        // Assert
        role.Should().NotBeNull();
        role.Name.Should().Be(name);
        role.Description.Should().Be(description);
        role.Id.Value.Should().NotBeEmpty();
    }

    [Theory]
    [InlineData("")]
    [InlineData("  ")]
    [InlineData(null)]
    public void Create_ShouldThrowArgumentException_WhenNameIsInvalid(string? name)
    {
        // Act & Assert
        var act = () => Role.Create(name!, "Description");
        act.Should().Throw<ArgumentException>().WithMessage("*Name required*");
    }

    [Fact]
    public void Update_ShouldUpdateProperties_WhenParametersAreValid()
    {
        // Arrange
        var role = Role.Create("Old Name", "Old Description");
        var newName = "New Name";
        var newDescription = "New Description";

        // Act
        role.Update(newName, newDescription);

        // Assert
        role.Name.Should().Be(newName);
        role.Description.Should().Be(newDescription);
    }
}