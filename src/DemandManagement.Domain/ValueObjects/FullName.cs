using System;

namespace DemandManagement.Domain.ValueObjects;

public sealed record FullName
{
    public string Value { get; init; }

    private FullName(string value) => Value = value;

    public static FullName From(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("FullName is required", nameof(name));

        return new FullName(name.Trim());
    }

    public override string ToString() => Value;
}