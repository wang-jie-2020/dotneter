namespace Yi.AspNetCore.Core.Entities;

public interface IAuditedEntity
{
    Guid? LastModifierId { get; }

    DateTime? LastModificationTime { get; }

    DateTime CreationTime { get; }

    Guid? CreatorId { get; }
}
