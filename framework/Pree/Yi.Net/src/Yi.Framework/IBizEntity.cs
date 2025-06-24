namespace Yi.Framework;

public interface IBizEntity
{
    Guid? LastModifierId { get; }

    DateTime? LastModificationTime { get; }

    DateTime CreationTime { get; }

    Guid? CreatorId { get; }
}
