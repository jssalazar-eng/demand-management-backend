using System;
using FluentAssertions;
using Xunit;
using DemandManagement.Domain.ValueObjects;

namespace DemandManagement.Domain.Tests.ValueObjects;

public class CorporateEmailTests
{
    [Theory]
    [InlineData("test@company.com")]
    [InlineData("user.name@domain.co")]
    [InlineData("test123@test.com")]
    public void From_ShouldCreateValidEmail_WhenFormatIsCorrect(string email)
    {
        // Act
        var corporateEmail = CorporateEmail.From(email);

        // Assert
        corporateEmail.Value.Should().Be(email);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void From_ShouldThrowArgumentException_WhenEmailIsEmpty(string? email)
    {
        // Act & Assert
        var act = () => CorporateEmail.From(email!);
        act.Should().Throw<ArgumentException>();
    }

    [Theory]
    [InlineData("invalid")]
    [InlineData("test@")]
    [InlineData("@domain.com")]
    [InlineData("test @domain.com")]
    public void From_ShouldThrowArgumentException_WhenFormatIsInvalid(string email)
    {
        // Act & Assert
        var act = () => CorporateEmail.From(email);
        act.Should().Throw<ArgumentException>().WithMessage("*Invalid email format*");
    }
}