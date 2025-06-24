namespace Yi.Framework;

public interface IEntity<TKey>
{
    TKey Id { get; }
}
