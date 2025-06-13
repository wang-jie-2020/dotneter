using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Volo.Abp.Data;
using Volo.Abp.Threading;
using Volo.Abp.Uow;
using Yi.AspNetCore.MultiTenancy;

namespace Yi.AspNetCore.SqlSugarCore.Uow;

public class UnitOfWorkSqlSugarDbContextProvider<TDbContext> : ISugarDbContextProvider<TDbContext> where TDbContext : ISqlSugarDbContext
{
    private readonly ISqlSugarDbConnectionCreator _dbConnectionCreator;
    protected readonly ICancellationTokenProvider CancellationTokenProvider;
    protected readonly IConnectionStringResolver ConnectionStringResolver;
    protected readonly ICurrentTenant CurrentTenant;
    protected readonly IUnitOfWorkManager UnitOfWorkManager;

    public UnitOfWorkSqlSugarDbContextProvider(
        IUnitOfWorkManager unitOfWorkManager,
        IConnectionStringResolver connectionStringResolver,
        ICancellationTokenProvider cancellationTokenProvider,
        ICurrentTenant currentTenant,
        ISqlSugarDbConnectionCreator dbConnectionCreator
    )
    {
        UnitOfWorkManager = unitOfWorkManager;
        ConnectionStringResolver = connectionStringResolver;
        CancellationTokenProvider = cancellationTokenProvider;
        CurrentTenant = currentTenant;
        Logger = NullLogger<UnitOfWorkSqlSugarDbContextProvider<TDbContext>>.Instance;
        _dbConnectionCreator = dbConnectionCreator;
    }

    public ILogger<UnitOfWorkSqlSugarDbContextProvider<TDbContext>> Logger { get; set; }
    
    public IServiceProvider ServiceProvider { get; set; }

    private static AsyncLocalDbContextAccessor ContextInstance => AsyncLocalDbContextAccessor.Instance;
    
    public virtual async Task<TDbContext> GetDbContextAsync()
    {
        var connectionStringName = ConnectionStrings.DefaultConnectionStringName;

        //获取当前连接字符串，未多租户时，默认为空
        var connectionString = await ResolveConnectionStringAsync(connectionStringName);
        var dbContextKey = $"{GetType().FullName}_{connectionString}";
        
        var unitOfWork = UnitOfWorkManager.Current;
        if (unitOfWork == null /*|| unitOfWork.Options.IsTransactional == false*/)
        {
            var dbContext = (TDbContext)ServiceProvider.GetRequiredService<ISqlSugarDbContext>();
            //提高体验，取消工作单元强制性
            //throw new AbpException("A DbContext can only be created inside a unit of work!");
            //如果不启用工作单元，创建一个新的db，不开启事务即可
            return dbContext;
        }
        
        //尝试当前工作单元获取db
        var databaseApi = unitOfWork.FindDatabaseApi(dbContextKey);

        //当前没有db创建一个新的db
        if (databaseApi == null)
        {
            //db根据连接字符串来创建
            databaseApi = new SqlSugarDatabaseApi(
                await CreateDbContextAsync(unitOfWork, connectionStringName, connectionString)
            );
            
            //创建的db加入到当前工作单元中
            unitOfWork.AddDatabaseApi(dbContextKey, databaseApi);
        }

        return (TDbContext)((SqlSugarDatabaseApi)databaseApi).DbContext;
    }
    
    protected virtual async Task<TDbContext> CreateDbContextAsync(IUnitOfWork unitOfWork, string connectionStringName,
        string connectionString)
    {
        var creationContext = new SqlSugarDbContextCreationContext(connectionStringName, connectionString);

        using (SqlSugarDbContextCreationContext.Use(creationContext))
        {
            var dbContext = await CreateDbContextAsync(unitOfWork);
            return dbContext;
        }
    }

    protected virtual async Task<TDbContext> CreateDbContextAsync(IUnitOfWork unitOfWork)
    {
        return unitOfWork.Options.IsTransactional
            ? await CreateDbContextWithTransactionAsync(unitOfWork)
            : unitOfWork.ServiceProvider.GetRequiredService<TDbContext>();
    }

    protected virtual async Task<TDbContext> CreateDbContextWithTransactionAsync(IUnitOfWork unitOfWork)
    {
        var transactionApiKey = $"SqlsugarCore_{SqlSugarDbContextCreationContext.Current.ConnectionString}";
        
        var activeTransaction = unitOfWork.FindTransactionApi(transactionApiKey) as SqlSugarTransactionApi;
        
        if (activeTransaction == null)
        {
            var dbContext = unitOfWork.ServiceProvider.GetRequiredService<TDbContext>();
            var transaction = new SqlSugarTransactionApi(
                dbContext
            );
            unitOfWork.AddTransactionApi(transactionApiKey, transaction);

            await dbContext.SqlSugarClient.Ado.BeginTranAsync();
            return dbContext;
        }

        return (TDbContext)activeTransaction.GetDbContext();
    }
    
    protected virtual async Task<string> ResolveConnectionStringAsync(string connectionStringName)
    {
        return await ConnectionStringResolver.ResolveAsync(connectionStringName);
    }
}