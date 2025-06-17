using SqlSugar;

namespace Yi.AspNetCore.Core.Entities;

public class SimpleEntity<T> : IEntity<T>
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