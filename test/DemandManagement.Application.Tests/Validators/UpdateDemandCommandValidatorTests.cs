using System;
using FluentAssertions;
using Xunit;
using DemandManagement.Application.Requests;
using DemandManagement.Application.Validators;
using DemandManagement.Domain.ValueObjects;

namespace DemandManagement.Application.Tests.Validators;

public class UpdateDemandCommandValidatorTests
{
    private readonly UpdateDemandCommandValidator _validator;

    public UpdateDemandCommandValidatorTests()
    {
        _validator = new UpdateDemandCommandValidator();
    }

    [Fact]
    public void Validate_ShouldPass_WhenCommandIsValid()
    {
        // Arrange
        var command = new UpdateDemandCommand(
            Guid.NewGuid(),
            "Valid Title",
            "Valid Description",
            PriorityLevel.High,
            Guid.NewGuid()
        );

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Validate_ShouldFail_WhenIdIsEmpty()
    {
        // Arrange
        var command = new UpdateDemandCommand(
            Guid.Empty,
            "Valid Title",
            "Description",
            PriorityLevel.High,
            Guid.NewGuid()
        );

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Id");
    }

    [Theory]
    [InlineData("")]
    [InlineData("  ")]
    [InlineData(null)]
    public void Validate_ShouldFail_WhenTitleIsEmpty(string title)
    {
        // Arrange
        var command = new UpdateDemandCommand(
            Guid.NewGuid(),
            title,
            "Description",
            PriorityLevel.High,
            Guid.NewGuid()
        );

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Title");
    }
}