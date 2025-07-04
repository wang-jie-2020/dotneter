using System.Diagnostics;
using System.Reflection;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SqlSugar;
using Volo.Abp.DependencyInjection;
using Yi.AspNetCore.Data;
using Yi.AspNetCore.Data.Filtering;
using Yi.AspNetCore.MultiTenancy;
using Yi.AspNetCore.Security;
using Yi.Framework.SqlSugarCore.Profilers;
using Yi.Framework.Utils;
using Yitter.IdGenerator;

namespace Yi.Framework.SqlSugarCore;

public abstract class SqlSugarDbContext : ISqlSugarDbContext
{
    public ISqlSugarClient SqlSugarClient { get; private set; }
    
    public SqlSugarDbContext(IAbpLazyServiceProvider lazyServiceProvider)
    {
        LazyServiceProvider = lazyServiceProvider;

        var connectionCreator = LazyServiceProvider.LazyGetRequiredService<ISqlSugarDbConnectionCreator>();
        SqlSugarClient = new SqlSugarClient(connectionCreator.Build(options =>
        {
            options.ConnectionString = GetCurrentConnectionString();
            options.DbType = GetCurrentDbType();
            options.ConfigureExternalServices = new ConfigureExternalServices
            {
                EntityService = EntityService
            };
        }),
        db =>
        {
            db.Aop.OnLogExecuting = OnLogExecuting;
            db.Aop.OnLogExecuted = OnLogExecuted;
            db.Aop.DataExecuting = DataExecuting;
            db.Aop.DataExecuted = DataExecuted;
            CustomDataFilter(db);
        });
    }

    public ICurrentUser CurrentUser => LazyServiceProvider.GetRequiredService<ICurrentUser>();

    private IAbpLazyServiceProvider LazyServiceProvider { get; }

    protected ILoggerFactory Logger => LazyServiceProvider.LazyGetRequiredService<ILoggerFactory>();

    private ICurrentTenant CurrentTenant => LazyServiceProvider.LazyGetRequiredService<ICurrentTenant>();

    public IDataFilter DataFilter => LazyServiceProvider.LazyGetRequiredService<IDataFilter>();

    protected virtual bool IsMultiTenantFilterEnabled => DataFilter?.IsEnabled<IMultiTenant>() ?? false;

    protected virtual bool IsSoftDeleteFilterEnabled => DataFilter?.IsEnabled<ISoftDelete>() ?? false;
    
    protected readonly DiagnosticListener s_diagnosticListener = new DiagnosticListener("SQLSugar");
    
    /// <summary>
    ///     db切换多库支持
    /// </summary>
    /// <returns></returns>
    protected virtual string GetCurrentConnectionString()
    {
        var connectionStringResolver = LazyServiceProvider.LazyGetRequiredService<IConnectionStringResolver>();
        var connectionString = connectionStringResolver.ResolveAsync().Result;

        if (connectionString.IsNullOrEmpty())
        {
            throw new ArgumentNullException(nameof(connectionString));
        }

        return connectionString!;
    }

    protected virtual DbType GetCurrentDbType()
    {
        return DbType.Sqlite;
    }

    protected virtual void CustomDataFilter(ISqlSugarClient sqlSugarClient)
    {
        if (IsSoftDeleteFilterEnabled)
        {
            sqlSugarClient.QueryFilter.AddTableFilter<ISoftDelete>(u => u.IsDeleted == false);
        }

        if (IsMultiTenantFilterEnabled)
        {
            var tenantId = CurrentTenant?.Id;
            sqlSugarClient.QueryFilter.AddTableFilter<IMultiTenant>(u => u.TenantId == tenantId);
        }
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

                if (entityInfo.PropertyName.Equals(nameof(IBizEntity.LastModificationTime)))
                {
                    if (!DateTime.MinValue.Equals(oldValue))
                    {
                        entityInfo.SetValue(DateTime.Now);
                    }
                }

                if (entityInfo.PropertyName.Equals(nameof(IBizEntity.LastModifierId)))
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

                if (entityInfo.PropertyName.Equals(nameof(IBizEntity.CreationTime)))
                {
                    //为空或者为默认最小值
                    if (oldValue is null || DateTime.MinValue.Equals(oldValue))
                    {
                        entityInfo.SetValue(DateTime.Now);
                    }
                }

                if (entityInfo.PropertyName.Equals(nameof(IBizEntity.CreatorId)))
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
        var sb = new StringBuilder();
        sb.AppendLine();
        sb.AppendLine("==========Yi-SQL执行:==========");
        sb.AppendLine(UtilMethods.GetSqlString(SqlSugarClient.CurrentConnectionConfig.DbType, sql, pars));
        sb.AppendLine("===============================");
        Logger.CreateLogger<SqlSugarDbContext>().LogTrace(sb.ToString());

        if (s_diagnosticListener.IsEnabled(LogExecutingEvent.EventName))
        {
            s_diagnosticListener.Write(LogExecutingEvent.EventName, new LogExecutingEvent(
                Guid.NewGuid(),
                SqlSugarClient.CurrentConnectionConfig.ConnectionString,
                UtilMethods.GetSqlString(SqlSugarClient.CurrentConnectionConfig.DbType, sql, pars)
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
        var sqllog = $"=========Yi-SQL耗时{SqlSugarClient.Ado.SqlExecutionTime.TotalMilliseconds}毫秒=====";
        Logger.CreateLogger<SqlSugarDbContext>().LogTrace(sqllog);
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