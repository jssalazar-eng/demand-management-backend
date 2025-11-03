using FluentAssertions;
using Xunit;
using DemandManagement.Application.Requests;
using DemandManagement.Application.Validators;
using DemandManagement.Domain.ValueObjects;
using System;
using System.Linq;

namespace DemandManagement.Application.Tests.Validators;

public class CreateDemandCommandValidatorTests
{
    private readonly CreateDemandCommandValidator _validator;

    public CreateDemandCommandValidatorTests()
    {
        _validator = new CreateDemandCommandValidator();
    }

    [Fact]
    public void Validate_ShouldPass_WhenCommandIsValid()
    {
        // Arrange
        var command = new CreateDemandCommand(
            "Valid Title",
            "Valid Description",
            PriorityLevel.High,
            Guid.NewGuid(),
            Guid.NewGuid(),
            Guid.NewGuid(),
            null,
            DateTimeOffset.UtcNow.AddDays(7)
        );

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("")]
    [InlineData("  ")]
    [InlineData(null)]
    public void Validate_ShouldFail_WhenTitleIsEmpty(string? title)
    {
        // Arrange
        var command = new CreateDemandCommand(
            title!,
            "Description",
            PriorityLevel.High,
            Guid.NewGuid(),
            Guid.NewGuid(),
            Guid.NewGuid(),
            null,
            null
        );

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Title");
        result.Errors.Count(e => e.PropertyName == "Title").Should().BeGreaterOrEqualTo(1);
    }

    [Fact]
    public void Validate_ShouldFail_WhenTitleExceedsMaxLength()
    {
        // Arrange
        var longTitle = new string('a', 201);
        var command = new CreateDemandCommand(
            longTitle,
            "Description",
            PriorityLevel.High,
            Guid.NewGuid(),
            Guid.NewGuid(),
            Guid.NewGuid(),
            null,
            null
        );

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Title" && e.ErrorMessage.Contains("200"));
    }

    [Fact]
    public void Validate_ShouldFail_WhenDueDateIsInThePast()
    {
        // Arrange
        var command = new CreateDemandCommand(
            "Valid Title",
            "Description",
            PriorityLevel.High,
            Guid.NewGuid(),
            Guid.NewGuid(),
            Guid.NewGuid(),
            null,
            DateTimeOffset.UtcNow.AddDays(-1)
        );

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle(e => e.PropertyName == "DueDate");
    }
}