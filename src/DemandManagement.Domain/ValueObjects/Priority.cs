using System;

namespace DemandManagement.Domain.ValueObjects;

public enum PriorityLevel
{
    Low,
    Medium,
    High,
    Critical
}

public sealed record Priority
{
    public PriorityLevel Level { get; init; }

    private Priority(PriorityLevel level) => Level = level;

    public static Priority From(PriorityLevel level) => new(level);

    public static Priority FromName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Priority name required", nameof(name));

        return name.Trim().ToLowerInvariant() switch
        {
            "crítico" or "critico" or "critical" => new Priority(PriorityLevel.Critical),
            "alto" or "high" => new Priority(PriorityLevel.High),
            "medio" or "medium" => new Priority(PriorityLevel.Medium),
            "bajo" or "low" => new Priority(PriorityLevel.Low),
            _ => throw new ArgumentException($"Unknown priority '{name}'", nameof(name))
        };
    }

    public override string ToString() => Level.ToString();
}