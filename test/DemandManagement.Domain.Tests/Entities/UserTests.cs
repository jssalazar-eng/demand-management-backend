using System;
using FluentAssertions;
using Xunit;
using DemandManagement.Domain.Entities;
using DemandManagement.Domain.ValueObjects;

namespace DemandManagement.Domain.Tests.Entities;

public class UserTests
{
    [Fact]
    public void Create_ShouldCreateValidUser_WhenParametersAreValid()
    {
        // Arrange
        var fullName = FullName.From("John Doe");
        var email = CorporateEmail.From("john@company.com");
        var roleId = RoleId.New();
        var department = "IT";

        // Act
        var user = User.Create(fullName, email, roleId, department);

        // Assert
        user.Should().NotBeNull();
        user.FullName.Should().Be(fullName);
        user.CorporateEmail.Should().Be(email);
        user.RoleId.Should().Be(roleId);
        user.Department.Should().Be(department);
        user.Id.Value.Should().NotBeEmpty();
    }

    [Fact]
    public void Update_ShouldUpdateProperties_WhenParametersAreValid()
    {
        // Arrange
        var user = User.Create(
            FullName.From("Old Name"),
            CorporateEmail.From("old@company.com"),
            RoleId.New(),
            "Old Dept"
        );

        var newFullName = FullName.From("New Name");
        var newEmail = CorporateEmail.From("new@company.com");
        var newRoleId = RoleId.New();
        var newDepartment = "New Dept";

        // Act
        user.Update(newFullName, newEmail, newRoleId, newDepartment);

        // Assert
        user.FullName.Should().Be(newFullName);
        user.CorporateEmail.Should().Be(newEmail);
        user.RoleId.Should().Be(newRoleId);
        user.Department.Should().Be(newDepartment);
    }
}