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

    /// <summary>
    /// Calcula la fecha límite basada en la prioridad
    /// Critical: +1 día, High: +2 días, Medium: +3 días, Low: +4 días
    /// </summary>
    public DateTimeOffset CalculateDueDate(DateTimeOffset fromDate)
    {
        var daysToAdd = Level switch
        {
            PriorityLevel.Critical => 1,
            PriorityLevel.High => 2,
            PriorityLevel.Medium => 3,
            PriorityLevel.Low => 4,
            _ => 3 // Default a Medium si es un valor desconocido
        };

        return fromDate.AddDays(daysToAdd);
    }

    /// <summary>
    /// Calcula la fecha límite desde ahora
    /// </summary>
    public DateTimeOffset CalculateDueDateFromNow()
        => CalculateDueDate(DateTimeOffset.UtcNow);

    public override string ToString() => Level.ToString();
}