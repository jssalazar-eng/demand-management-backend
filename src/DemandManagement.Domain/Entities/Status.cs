using System;
using DemandManagement.Domain.ValueObjects;

namespace DemandManagement.Domain.Entities;

public sealed class Status : BaseEntity<StatusId>
{
    public string Name { get; private set; }
    public int SequenceOrder { get; private set; }
    public bool IsFinal { get; private set; }
    public bool IsInitial { get; private set; }

    private Status() : base(default!)
    {
        Name = string.Empty;
    }

    public Status(StatusId id, string name, int sequenceOrder, bool isFinal, bool isInitial) : base(id)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Name required", nameof(name));
        Name = name.Trim();
        SequenceOrder = sequenceOrder;
        IsFinal = isFinal;
        IsInitial = isInitial;
    }

    public static Status Create(string name, int order, bool isFinal = false, bool isInitial = false)
        => new(StatusId.New(), name, order, isFinal, isInitial);

    public void Update(string name, int order, bool isFinal, bool isInitial)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Name required", nameof(name));
        Name = name.Trim();
        SequenceOrder = order;
        IsFinal = isFinal;
        IsInitial = isInitial;
    }
}