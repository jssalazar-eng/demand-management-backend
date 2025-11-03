using System;
using DemandManagement.Domain.ValueObjects;

namespace DemandManagement.Domain.Entities;

public sealed class User : BaseEntity<UserId>
{
    public FullName FullName { get; private set; }
    public CorporateEmail CorporateEmail { get; private set; }
    public RoleId RoleId { get; private set; }
    public string? Department { get; private set; }

    // Constructor privado sin parámetros para EF Core
    private User() : base(default!)
    {
        FullName = default!;
        CorporateEmail = default!;
        RoleId = default!;
    }

    public User(UserId id, FullName fullName, CorporateEmail email, RoleId roleId, string? department) : base(id)
    {
        FullName = fullName;
        CorporateEmail = email;
        RoleId = roleId;
        Department = department?.Trim();
    }

    public static User Create(FullName fullName, CorporateEmail email, RoleId roleId, string? department = null)
        => new(UserId.New(), fullName, email, roleId, department);

    public void Update(FullName fullName, CorporateEmail email, RoleId roleId, string? department)
    {
        FullName = fullName;
        CorporateEmail = email;
        RoleId = roleId;
        Department = department?.Trim();
    }
}