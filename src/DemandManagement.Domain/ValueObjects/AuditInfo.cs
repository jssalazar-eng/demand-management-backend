using System;

namespace DemandManagement.Domain.ValueObjects;

public sealed record AuditInfo
{
    public DateTimeOffset CreatedDate { get; init; }
    public DateTimeOffset UpdatedDate { get; init; }

    private AuditInfo(DateTimeOffset created, DateTimeOffset updated)
    {
        CreatedDate = created;
        UpdatedDate = updated;
    }

    public static AuditInfo CreateNow()
    {
        var now = DateTimeOffset.UtcNow;
        return new AuditInfo(now, now);
    }

    public AuditInfo WithUpdated(DateTimeOffset updated) => new(CreatedDate, updated);
}