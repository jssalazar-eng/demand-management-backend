using System;
using FluentAssertions;
using Xunit;
using DemandManagement.Domain.ValueObjects;

namespace DemandManagement.Domain.Tests.ValueObjects;

public class FullNameTests
{
    [Theory]
    [InlineData("John Doe")]
    [InlineData("María García")]
    [InlineData("  Trimmed Name  ")]
    public void From_ShouldCreateValidFullName_WhenNameIsValid(string name)
    {
        // Act
        var fullName = FullName.From(name);

        // Assert
        fullName.Value.Should().Be(name.Trim());
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void From_ShouldThrowArgumentException_WhenNameIsInvalid(string? name)
    {
        // Act & Assert
        var act = () => FullName.From(name!);
        act.Should().Throw<ArgumentException>().WithMessage("*FullName is required*");
    }
}