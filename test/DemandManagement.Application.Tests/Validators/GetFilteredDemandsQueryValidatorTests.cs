using FluentAssertions;
using Xunit;
using DemandManagement.Application.Requests;
using DemandManagement.Application.Validators;
using DemandManagement.Domain.ValueObjects;

namespace DemandManagement.Application.Tests.Validators;

public class GetFilteredDemandsQueryValidatorTests
{
    private readonly GetFilteredDemandsQueryValidator _validator;

    public GetFilteredDemandsQueryValidatorTests()
    {
        _validator = new GetFilteredDemandsQueryValidator();
    }

    [Fact]
    public void Validate_ShouldPass_WhenQueryIsValid()
    {
        // Arrange
        var query = new GetFilteredDemandsQuery(null, null, null, null, 1, 10);

        // Act
        var result = _validator.Validate(query);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Validate_ShouldFail_WhenPageNumberIsInvalid(int pageNumber)
    {
        // Arrange
        var query = new GetFilteredDemandsQuery(null, null, null, null, pageNumber, 10);

        // Act
        var result = _validator.Validate(query);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "PageNumber");
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(101)]
    public void Validate_ShouldFail_WhenPageSizeIsInvalid(int pageSize)
    {
        // Arrange
        var query = new GetFilteredDemandsQuery(null, null, null, null, 1, pageSize);

        // Act
        var result = _validator.Validate(query);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "PageSize");
    }

    [Fact]
    public void Validate_ShouldFail_WhenSearchTermExceedsMaxLength()
    {
        // Arrange
        var longSearchTerm = new string('a', 201);
        var query = new GetFilteredDemandsQuery(null, null, null, longSearchTerm, 1, 10);

        // Act
        var result = _validator.Validate(query);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "SearchTerm");
    }
}