using System;
using FluentAssertions;
using Xunit;
using DemandManagement.Domain.ValueObjects;

namespace DemandManagement.Domain.Tests.ValueObjects;

public class AuditInfoTests
{
    [Fact]
    public void CreateNow_ShouldCreateAuditInfoWithCurrentTime()
    {
        // Act
        var auditInfo = AuditInfo.CreateNow();

        // Assert
        auditInfo.CreatedDate.Should().BeCloseTo(DateTimeOffset.UtcNow, TimeSpan.FromSeconds(1));
        auditInfo.UpdatedDate.Should().BeCloseTo(DateTimeOffset.UtcNow, TimeSpan.FromSeconds(1));
        auditInfo.CreatedDate.Should().Be(auditInfo.UpdatedDate);
    }

    [Fact]
    public void WithUpdated_ShouldUpdateOnlyUpdatedDate()
    {
        // Arrange
        var auditInfo = AuditInfo.CreateNow();
        var originalCreatedDate = auditInfo.CreatedDate;
        var newUpdatedDate = DateTimeOffset.UtcNow.AddMinutes(5);

        // Act
        var updated = auditInfo.WithUpdated(newUpdatedDate);

        // Assert
        updated.CreatedDate.Should().Be(originalCreatedDate);
        updated.UpdatedDate.Should().Be(newUpdatedDate);
    }
}