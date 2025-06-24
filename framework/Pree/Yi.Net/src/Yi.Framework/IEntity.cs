namespace Yi.Framework;

public interface IEntity<out TKey>
{
    TKey Id { get; }
}
