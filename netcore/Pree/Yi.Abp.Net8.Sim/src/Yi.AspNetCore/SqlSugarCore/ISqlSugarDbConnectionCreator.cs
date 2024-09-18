using System.Reflection;
using SqlSugar;

namespace Yi.AspNetCore.SqlSugarCore;

public interface ISqlSugarDbConnectionCreator
{
    DbConnOptions Options { get; }
    Action<ISqlSugarClient> OnSqlSugarClientConfig { get; set; }
    Action<object, DataAfterModel> DataExecuted { get; set; }
    Action<object, DataFilterModel> DataExecuting { get; set; }
    Action<string, SugarParameter[]> OnLogExecuting { get; set; }
    Action<string, SugarParameter[]> OnLogExecuted { get; set; }
    Action<PropertyInfo, EntityColumnInfo> EntityService { get; set; }
    ConnectionConfig Build(Action<ConnectionConfig>? action = null);
    void SetDbAop(ISqlSugarClient currentDb);
}