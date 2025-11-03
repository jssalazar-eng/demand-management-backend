using System;
using System.Collections.Generic;
using System.Linq;
using DemandManagement.Domain.ValueObjects;

namespace DemandManagement.Domain.Entities;

public sealed class Demand : BaseEntity<DemandId>
{
    public string Title { get; private set; }
    public string? Description { get; private set; }
    public AuditInfo Audit { get; private set; }
    public Priority Priority { get; private set; }
    public DemandTypeId DemandTypeId { get; private set; }
    public StatusId StatusId { get; private set; }
    public UserId RequestingUserId { get; private set; }
    public UserId? AssignedToId { get; private set; }
    public DateTimeOffset? DueDate { get; private set; }
    public DateTimeOffset? CloseDate { get; private set; }

    private readonly List<AssociatedDocument> _documents = new();
    public IReadOnlyCollection<AssociatedDocument> Documents => _documents.AsReadOnly();

    private Demand() : base(default!)
    {
        Title = string.Empty;
        Audit = default!;
        Priority = default!;
        DemandTypeId = default!;
        StatusId = default!;
        RequestingUserId = default!;
    }

    private Demand(DemandId id,
                   string title,
                   string? description,
                   AuditInfo audit,
                   Priority priority,
                   DemandTypeId demandTypeId,
                   StatusId statusId,
                   UserId requestingUserId,
                   UserId? assignedToId,
                   DateTimeOffset? dueDate,
                   DateTimeOffset? closeDate) : base(id)
    {
        Title = title;
        Description = description;
        Audit = audit;
        Priority = priority;
        DemandTypeId = demandTypeId;
        StatusId = statusId;
        RequestingUserId = requestingUserId;
        AssignedToId = assignedToId;
        DueDate = dueDate;
        CloseDate = closeDate;
    }

    public static Demand Create(string title,
                                string? description,
                                Priority priority,
                                DemandTypeId demandTypeId,
                                StatusId statusId,
                                UserId requestingUserId,
                                UserId? assignedToId = null,
                                DateTimeOffset? dueDate = null)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Title is required", nameof(title));

        var audit = AuditInfo.CreateNow();
        var id = DemandId.New();

        if (dueDate.HasValue && dueDate.Value < audit.CreatedDate)
            throw new ArgumentException("DueDate cannot be before CreatedDate", nameof(dueDate));

        return new Demand(id, title.Trim(), description?.Trim(), audit, priority, demandTypeId, statusId, requestingUserId, assignedToId, dueDate, null);
    }

    public void UpdateDetails(string title, string? description, Priority priority, DemandTypeId demandTypeId)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Title is required", nameof(title));

        Title = title.Trim();
        Description = description?.Trim();
        Priority = priority;
        DemandTypeId = demandTypeId;
        Audit = Audit.WithUpdated(DateTimeOffset.UtcNow);
    }

    public void AssignTo(UserId assignee)
    {
        AssignedToId = assignee;
        Audit = Audit.WithUpdated(DateTimeOffset.UtcNow);
    }

    public void ChangeStatus(StatusId newStatus)
    {
        StatusId = newStatus;
        Audit = Audit.WithUpdated(DateTimeOffset.UtcNow);
    }

    public void AddDocument(AssociatedDocument doc)
    {
        if (doc.DemandId.Value != Id.Value)
            throw new InvalidOperationException("Document DemandId mismatch.");

        if (_documents.Any(d => d.Id.Value == doc.Id.Value))
            return;

        _documents.Add(doc);
        Audit = Audit.WithUpdated(DateTimeOffset.UtcNow);
    }

    public void Close(DateTimeOffset closedAt)
    {
        if (CloseDate.HasValue)
            throw new InvalidOperationException("Demand is already closed.");

        CloseDate = closedAt;
        Audit = Audit.WithUpdated(DateTimeOffset.UtcNow);
    }
}