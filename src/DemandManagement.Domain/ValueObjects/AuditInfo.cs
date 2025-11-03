using System;

namespace DemandManagement.Domain.ValueObjects;

public sealed class AuditInfo
{
    public DateTimeOffset CreatedDate { get; private set; }
    public DateTimeOffset UpdatedDate { get; private set; }

    private AuditInfo()
    {
    }

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