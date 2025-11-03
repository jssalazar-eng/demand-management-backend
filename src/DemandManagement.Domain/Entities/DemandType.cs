using System;
using DemandManagement.Domain.ValueObjects;

namespace DemandManagement.Domain.Entities;

public sealed class DemandType : BaseEntity<DemandTypeId>
{
    public string Name { get; private set; }
    public string? Description { get; private set; }
    public string? ServiceLevel { get; private set; }

    public DemandType(DemandTypeId id, string name, string? description, string? serviceLevel) : base(id)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Name required", nameof(name));
        Name = name.Trim();
        Description = description?.Trim();
        ServiceLevel = serviceLevel?.Trim();
    }

    public static DemandType Create(string name, string? description = null, string? serviceLevel = null)
    {
        return new DemandType(DemandTypeId.New(), name, description, serviceLevel);
    }

    public void Update(string name, string? description, string? serviceLevel)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Name required", nameof(name));
        Name = name.Trim();
        Description = description?.Trim();
        ServiceLevel = serviceLevel?.Trim();
    }
}