using FluentAssertions;
using Xunit;
using DemandManagement.Domain.ValueObjects;
using System;

namespace DemandManagement.Domain.Tests.ValueObjects;

public class PriorityTests
{
    [Theory]
    [InlineData("crítico", PriorityLevel.Critical)]
    [InlineData("Critical", PriorityLevel.Critical)]
    [InlineData("alto", PriorityLevel.High)]
    [InlineData("High", PriorityLevel.High)]
    [InlineData("medio", PriorityLevel.Medium)]
    [InlineData("bajo", PriorityLevel.Low)]
    public void FromName_ShouldReturnCorrectPriority(string name, PriorityLevel expected)
    {
        // Act
        var priority = Priority.FromName(name);

        // Assert
        priority.Level.Should().Be(expected);
    }

    [Fact]
    public void FromName_ShouldThrowArgumentException_WhenNameIsInvalid()
    {
        // Act & Assert
        var act = () => Priority.FromName("invalid");
        act.Should().Throw<ArgumentException>();
    }
}