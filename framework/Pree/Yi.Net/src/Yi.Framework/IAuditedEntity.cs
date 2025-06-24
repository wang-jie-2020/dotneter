namespace Yi.Framework;

public interface IAuditedEntity
{
    Guid? LastModifierId { get; }

    DateTime? LastModificationTime { get; }

    DateTime CreationTime { get; }

    Guid? CreatorId { get; }
}
