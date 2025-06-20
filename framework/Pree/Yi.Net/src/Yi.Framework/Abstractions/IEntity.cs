namespace Yi.Framework.Abstractions;

public interface IEntity<TKey>
{
    TKey Id { get; }
}
