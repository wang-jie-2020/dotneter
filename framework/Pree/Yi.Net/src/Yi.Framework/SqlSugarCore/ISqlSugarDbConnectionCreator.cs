using System.Reflection;
using SqlSugar;

namespace Yi.Framework.SqlSugarCore;

public interface ISqlSugarDbConnectionCreator
{
    List<ConnectionConfig> Build(Action<ConnectionConfig>? action = null);
}