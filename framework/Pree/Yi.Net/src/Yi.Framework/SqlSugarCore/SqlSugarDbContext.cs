using System.Diagnostics;
using System.Reflection;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SqlSugar;
using Volo.Abp.DependencyInjection;
using Yi.AspNetCore;
using Yi.AspNetCore.Data;
using Yi.AspNetCore.Data.Filtering;
using Yi.AspNetCore.MultiTenancy;
using Yi.AspNetCore.Security;
using Yi.Framework.Core;
using Yi.Framework.SqlSugarCore.Profilers;
using Yi.Framework.Utils;
using Yitter.IdGenerator;
using Check = Volo.Abp.Check;

namespace Yi.Framework.SqlSugarCore;

public class SqlSugarDbContext : ISqlSugarDbContext
{
    protected static readonly DiagnosticListener s_diagnosticListener =
        new DiagnosticListener("SQLSugar");

    private ISqlSugarDbConnectionCreator _dbConnectionCreator;

    public SqlSugarDbContext(IAbpLazyServiceProvider lazyServiceProvider)
    {
        LazyServiceProvider = lazyServiceProvider;
        var connectionCreator = LazyServiceProvider.LazyGetRequiredService<ISqlSugarDbConnectionCreator>();
        _dbConnectionCreator = connectionCreator;
        connectionCreator.OnSqlSugarClientConfig = OnSqlSugarClientConfig;
        connectionCreator.EntityService = EntityService;
        connectionCreator.DataExecuting = DataExecuting;
        connectionCreator.DataExecuted = DataExecuted;
        connectionCreator.OnLogExecuting = OnLogExecuting;
        connectionCreator.OnLogExecuted = OnLogExecuted;
        SqlSugarClient = new SqlSugarClient(connectionCreator.Build(options =>
        {
            options.ConnectionString = GetCurrentConnectionString();
            options.DbType = GetCurrentDbType();
        }));
        connectionCreator.SetDbAop(SqlSugarClient);
    }

    public ICurrentUser CurrentUser => LazyServiceProvider.GetRequiredService<ICurrentUser>();

    private IAbpLazyServiceProvider LazyServiceProvider { get; }

    protected ILoggerFactory Logger => LazyServiceProvider.LazyGetRequiredService<ILoggerFactory>();

    private ICurrentTenant CurrentTenant => LazyServiceProvider.LazyGetRequiredService<ICurrentTenant>();

    public IDataFilter DataFilter => LazyServiceProvider.LazyGetRequiredService<IDataFilter>();

    protected virtual bool IsMultiTenantFilterEnabled => DataFilter?.IsEnabled<IMultiTenant>() ?? false;

    protected virtual bool IsSoftDeleteFilterEnabled => DataFilter?.IsEnabled<ISoftDelete>() ?? false;

    public DbConnectionOptions ConnectionOptions => LazyServiceProvider.LazyGetRequiredService<IOptions<DbConnectionOptions>>().Value;

    /// <summary>
    ///     SqlSugar 客户端
    /// </summary>
    public ISqlSugarClient SqlSugarClient { get; private set; }

    public DbConnOptions Options => LazyServiceProvider.LazyGetRequiredService<IOptions<DbConnOptions>>().Value;

    public void SetSqlSugarClient(ISqlSugarClient sqlSugarClient)
    {
        SqlSugarClient = sqlSugarClient;
    }

    /// <summary>
    ///     db切换多库支持
    /// </summary>
    /// <returns></returns>
    protected virtual string GetCurrentConnectionString()
    {
        var defaultUrl = Options.Url ?? ConnectionOptions.GetConnectionStringOrNull(ConnectionStrings.DefaultConnectionStringName);

        //如果未开启多租户，返回db url 或者 默认连接字符串
        if (!Options.EnabledSaasMultiTenancy) return defaultUrl;

        //开启了多租户
        var connectionStringResolver = LazyServiceProvider.LazyGetRequiredService<IConnectionStringResolver>();
        var connectionString = connectionStringResolver.ResolveAsync().GetAwaiter().GetResult();

        //没有检测到使用多租户功能，默认使用默认库即可
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            Check.NotNull(Options.Url, "租户默认库Default未找到");
            connectionString = defaultUrl;
        }

        return connectionString!;
    }

    protected virtual DbType GetCurrentDbType()
    {
        if (CurrentTenant.Name is not null)
        {
            var dbTypeFromTenantName = GetDbTypeFromTenantName(CurrentTenant.Name);
            if (dbTypeFromTenantName is not null) return dbTypeFromTenantName.Value;
        }

        Check.NotNull(Options.DbType, "默认DbType未配置！");
        return Options.DbType!.Value;
    }

    //根据租户name进行匹配db类型:  Test_Sqlite，[来自AI]
    private DbType? GetDbTypeFromTenantName(string name)
    {
        if (string.IsNullOrWhiteSpace(name)) return null;

        // 查找下划线的位置
        var underscoreIndex = name.LastIndexOf('_');

        if (underscoreIndex == -1 || underscoreIndex == name.Length - 1) return null;

        // 提取 枚举 部分
        var enumString = name.Substring(underscoreIndex + 1);

        // 尝试将 尾缀 转换为枚举
        if (Enum.TryParse(enumString, out DbType result)) return result;

        // 条件不满足时返回 null
        return null;
    }


    /// <summary>
    ///     上下文对象扩展
    /// </summary>
    /// <param name="sqlSugarClient"></param>
    protected virtual void OnSqlSugarClientConfig(ISqlSugarClient sqlSugarClient)
    {
        //需自定义扩展
        if (IsSoftDeleteFilterEnabled)
        {
            sqlSugarClient.QueryFilter.AddTableFilter<ISoftDelete>(u => u.IsDeleted == false);
        }

        if (IsMultiTenantFilterEnabled)
        {
            //表达式不能放方法
            var tenantId = CurrentTenant?.Id;
            sqlSugarClient.QueryFilter.AddTableFilter<IMultiTenant>(u => u.TenantId == tenantId);
        }

        CustomDataFilter(sqlSugarClient);
    }

    protected virtual void CustomDataFilter(ISqlSugarClient sqlSugarClient)
    {
    }

    protected virtual void DataExecuted(object oldValue, DataAfterModel entityInfo)
    {
    }

    /// <summary>
    ///     数据
    /// </summary>
    /// <param name="oldValue"></param>
    /// <param name="entityInfo"></param>
    protected virtual void DataExecuting(object oldValue, DataFilterModel entityInfo)
    {
        //审计日志
        switch (entityInfo.OperationType)
        {
            case DataFilterType.UpdateByObject:

                if (entityInfo.PropertyName.Equals(nameof(IAuditedEntity.LastModificationTime)))
                {
                    if (!DateTime.MinValue.Equals(oldValue))
                    {
                        entityInfo.SetValue(DateTime.Now);
                    }
                }

                if (entityInfo.PropertyName.Equals(nameof(IAuditedEntity.LastModifierId)))
                {
                    if (CurrentUser.Id != null)
                    {
                        entityInfo.SetValue(CurrentUser.Id);
                    }
                }

                break;
            case DataFilterType.InsertByObject:
                if (entityInfo.PropertyName.Equals(nameof(IEntity<Guid>.Id)))
                {
                    if (entityInfo.EntityColumnInfo.PropertyInfo.PropertyType == typeof(long))
                    {
                        if (0l.Equals(oldValue))
                        {
                            entityInfo.SetValue(YitIdHelper.NextId());
                        }
                    }
                    else
                    {
                        //主键为空或者为默认最小值
                        if (Guid.Empty.Equals(oldValue))
                        {
                            entityInfo.SetValue(SequentialGuidGenerator.Create());
                        }
                    }
                }

                if (entityInfo.PropertyName.Equals(nameof(IAuditedEntity.CreationTime)))
                {
                    //为空或者为默认最小值
                    if (oldValue is null || DateTime.MinValue.Equals(oldValue))
                    {
                        entityInfo.SetValue(DateTime.Now);
                    }
                }

                if (entityInfo.PropertyName.Equals(nameof(IAuditedEntity.CreatorId)))
                {
                    if (CurrentUser.Id != null)
                    {
                        entityInfo.SetValue(CurrentUser.Id);
                    }
                }

                //插入时，需要租户id,先预留
                if (entityInfo.PropertyName.Equals(nameof(IMultiTenant.TenantId)))
                {
                    if (CurrentTenant is not null)
                    {
                        entityInfo.SetValue(CurrentTenant.Id);
                    }
                }

                break;
        }
    }

    /// <summary>
    ///     日志
    /// </summary>
    /// <param name="sql"></param>
    /// <param name="pars"></param>
    protected virtual void OnLogExecuting(string sql, SugarParameter[] pars)
    {
        if (Options.EnabledSqlLog)
        {
            var sb = new StringBuilder();
            sb.AppendLine();
            sb.AppendLine("==========Yi-SQL执行:==========");
            sb.AppendLine(UtilMethods.GetSqlString(_dbConnectionCreator.Options.DbType ?? DbType.Sqlite, sql, pars));
            sb.AppendLine("===============================");
            Logger.CreateLogger<SqlSugarDbContext>().LogDebug(sb.ToString());
        }

        //CustomTiming customTiming = MiniProfiler.Current?.CustomTiming("SqlSugar", UtilMethods.GetSqlString(_dbConnectionCreator.Options.DbType ?? DbType.Sqlite, sql, pars));

        if (s_diagnosticListener.IsEnabled(LogExecutingEvent.EventName))
        {
            s_diagnosticListener.Write(LogExecutingEvent.EventName, new LogExecutingEvent(
                Guid.NewGuid(),
                Options.Url,
                UtilMethods.GetSqlString(_dbConnectionCreator.Options.DbType ?? DbType.Sqlite, sql, pars)
            ));
        }
    }

    /// <summary>
    ///     日志
    /// </summary>
    /// <param name="sql"></param>
    /// <param name="pars"></param>
    protected virtual void OnLogExecuted(string sql, SugarParameter[] pars)
    {
        if (Options.EnabledSqlLog)
        {
            var sqllog = $"=========Yi-SQL耗时{SqlSugarClient.Ado.SqlExecutionTime.TotalMilliseconds}毫秒=====";
            Logger.CreateLogger<SqlSugarDbContext>().LogDebug(sqllog);
        }
    }

    /// <summary>
    ///     实体配置
    /// </summary>
    /// <param name="property"></param>
    /// <param name="column"></param>
    protected virtual void EntityService(PropertyInfo property, EntityColumnInfo column)
    {
        if (property.Name == "ConcurrencyStamp")
        {
            column.IsIgnore = true;
        }

        if (property.Name == nameof(IEntity<object>.Id))
        {
            column.IsPrimarykey = true;
        }
    }
}