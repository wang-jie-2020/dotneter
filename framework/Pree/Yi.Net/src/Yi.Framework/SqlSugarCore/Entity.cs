using SqlSugar;

namespace Yi.Framework.SqlSugarCore;

public abstract class Entity<T> : IEntity<T>
{
    [SugarColumn(ColumnName = "Id", IsPrimaryKey = true)]
    public T Id { get; set; }

    public object?[] GetKeys()
    {
        return new object?[] { Id };
    }

    public Entity()
    {

    }

    public Entity(T id)
    {
        Id = id;
    }
}