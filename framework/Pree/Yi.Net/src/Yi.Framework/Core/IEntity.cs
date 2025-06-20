namespace Yi.Framework.Core;

public interface IEntity<TKey>
{
    TKey Id { get; }
}
