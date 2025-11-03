namespace DemandManagement.Domain.ValueObjects;

public readonly record struct DemandId(Guid Value)
{
    public static DemandId New() => new(Guid.NewGuid());
    public static DemandId From(Guid id) => new(id);
}

public readonly record struct DemandTypeId(Guid Value)
{
    public static DemandTypeId New() => new(Guid.NewGuid());
    public static DemandTypeId From(Guid id) => new(id);
}

public readonly record struct StatusId(Guid Value)
{
    public static StatusId New() => new(Guid.NewGuid());
    public static StatusId From(Guid id) => new(id);
}

public readonly record struct UserId(Guid Value)
{
    public static UserId New() => new(Guid.NewGuid());
    public static UserId From(Guid id) => new(id);
}

public readonly record struct RoleId(Guid Value)
{
    public static RoleId New() => new(Guid.NewGuid());
    public static RoleId From(Guid id) => new(id);
}

public readonly record struct DocumentId(Guid Value)
{
    public static DocumentId New() => new(Guid.NewGuid());
    public static DocumentId From(Guid id) => new(id);
}