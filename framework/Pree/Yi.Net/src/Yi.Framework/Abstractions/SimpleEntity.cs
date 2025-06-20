using SqlSugar;

namespace Yi.Framework.Abstractions;

public abstract class SimpleEntity<T> : IEntity<T>
{
    [SugarColumn(ColumnName = "Id", IsPrimaryKey = true)]
    public T Id { get; set; }

    public object?[] GetKeys()
    {
        return new object?[] { Id };
    }

    public SimpleEntity()
    {

    }

    public SimpleEntity(T id)
    {
        Id = id;
    }
}