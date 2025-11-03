using System;

namespace DemandManagement.Domain.Entities;

public abstract class BaseEntity<TId>
{
    public TId Id { get; protected init; }

    protected BaseEntity(TId id) => Id = id;

    public override bool Equals(object? obj)
    {
        if (obj is not BaseEntity<TId> other) return false;
        return EqualityComparer<TId>.Default.Equals(Id, other.Id);
    }

    public override int GetHashCode() => Id?.GetHashCode() ?? 0;
}