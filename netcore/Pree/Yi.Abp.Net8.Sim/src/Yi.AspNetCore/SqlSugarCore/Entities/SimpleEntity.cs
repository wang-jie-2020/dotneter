using SqlSugar;
using Volo.Abp.Domain.Entities;

namespace Yi.AspNetCore.SqlSugarCore.Entities;

public class SimpleEntity : IEntity<Guid>
{
    [SugarColumn(ColumnName = "Id", IsPrimaryKey = true)]
    public Guid Id { get; set; }
    
    public object?[] GetKeys()
    {
        return new object?[] { Id };
    }
}