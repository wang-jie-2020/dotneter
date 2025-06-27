using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Volo.Abp.Uow;
using Yi.AspNetCore.Data;

namespace Yi.Framework.SqlSugarCore.Uow;

public class UnitOfWorkSqlSugarDbContextProvider<TDbContext> : ISugarDbContextProvider<TDbContext> 
    where TDbContext : ISqlSugarDbContext
{
    private readonly IUnitOfWorkManager _unitOfWorkManager;
    private readonly IConnectionStringResolver _connectionStringResolver;
    
    public UnitOfWorkSqlSugarDbContextProvider(
        IUnitOfWorkManager unitOfWorkManager,
        IConnectionStringResolver connectionStringResolver)
    {
        _unitOfWorkManager = unitOfWorkManager;
        _connectionStringResolver = connectionStringResolver;

        Logger = NullLogger<UnitOfWorkSqlSugarDbContextProvider<TDbContext>>.Instance;
    }

    public ILogger<UnitOfWorkSqlSugarDbContextProvider<TDbContext>> Logger { get; set; }
    
    public IServiceProvider ServiceProvider { get; set; }
    
    public virtual async Task<TDbContext> GetDbContextAsync()
    {
        var unitOfWork = _unitOfWorkManager.Current;
        if (unitOfWork == null)
        {
            //throw new Exception("A DbContext can only be created inside a unit of work!");
            return ServiceProvider.GetRequiredService<TDbContext>();
        }
        
        var connectionStringName = ConnectionStrings.DefaultConnectionStringName;
        var connectionString = await ResolveConnectionStringAsync(connectionStringName);
       
        var dbContextKey = $"{GetType().FullName}_{connectionString}";
        var databaseApi = unitOfWork.FindDatabaseApi(dbContextKey);
        if (databaseApi == null)
        {
            databaseApi = new SqlSugarDatabaseApi(
                await CreateDbContextAsync(unitOfWork, connectionStringName, connectionString)
            );
            
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

            if (unitOfWork.Options.IsolationLevel.HasValue)
            {
               await dbContext.SqlSugarClient.Ado.BeginTranAsync(unitOfWork.Options.IsolationLevel.Value);
            }
            else
            {
                await dbContext.SqlSugarClient.Ado.BeginTranAsync();
            }
            
            var transaction = new SqlSugarTransactionApi(
                dbContext
            );
            unitOfWork.AddTransactionApi(transactionApiKey, transaction);
            
            return dbContext;
        }

        return (TDbContext)activeTransaction.DbContext;
    }
    
    protected virtual async Task<string> ResolveConnectionStringAsync(string connectionStringName)
    {
        return await _connectionStringResolver.ResolveAsync(connectionStringName);
    }
}