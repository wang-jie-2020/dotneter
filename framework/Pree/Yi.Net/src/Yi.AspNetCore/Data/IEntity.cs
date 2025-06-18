namespace Yi.AspNetCore.Data;

public interface IEntity<TKey>
{
    TKey Id { get; }
}
