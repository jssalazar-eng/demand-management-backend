using System;
using DemandManagement.Domain.ValueObjects;

namespace DemandManagement.Domain.Entities;

public sealed class Role : BaseEntity<RoleId>
{
    public string Name { get; private set; }
    public string? Description { get; private set; }

    public Role(RoleId id, string name, string? description) : base(id)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Name required", nameof(name));
        Name = name.Trim();
        Description = description?.Trim();
    }

    public static Role Create(string name, string? description = null) => new(RoleId.New(), name, description);

    public void Update(string name, string? description)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Name required", nameof(name));
        Name = name.Trim();
        Description = description?.Trim();
    }
}