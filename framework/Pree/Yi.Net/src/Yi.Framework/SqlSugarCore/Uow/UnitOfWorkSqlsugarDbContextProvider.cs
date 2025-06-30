using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Uow;

namespace Yi.Framework.SqlSugarCore.Uow;

public class UnitOfWorkSqlSugarDbContextProvider<TDbContext> : ISugarDbContextProvider<TDbContext>
    where TDbContext : ISqlSugarDbContext
{
    private readonly IUnitOfWorkManager _unitOfWorkManager;

    public UnitOfWorkSqlSugarDbContextProvider(IUnitOfWorkManager unitOfWorkManager)
    {
        _unitOfWorkManager = unitOfWorkManager;
    }

    public virtual async Task<TDbContext> GetDbContextAsync()
    {
        var unitOfWork = _unitOfWorkManager.Current;
        if (unitOfWork == null)
        {
            throw new Exception("A DbContext can only be created inside a unit of work!");
            //return ServiceProvider.GetRequiredService<TDbContext>();
        }

        var dbContextKey = $"{typeof(TDbContext).Name}";
        var databaseApi = unitOfWork.FindDatabaseApi(dbContextKey);
        if (databaseApi == null)
        {
            databaseApi = new SqlSugarDatabaseApi(await CreateDbContextAsync(unitOfWork));
            unitOfWork.AddDatabaseApi(dbContextKey, databaseApi);
        }

        return (TDbContext)((SqlSugarDatabaseApi)databaseApi).DbContext;
    }

    protected virtual async Task<TDbContext> CreateDbContextAsync(IUnitOfWork unitOfWork)
    {
        return unitOfWork.Options.IsTransactional
            ? await CreateDbContextWithTransactionAsync(unitOfWork)
            : unitOfWork.ServiceProvider.GetRequiredService<TDbContext>();
    }

    protected virtual async Task<TDbContext> CreateDbContextWithTransactionAsync(IUnitOfWork unitOfWork)
    {
        var transactionApiKey = $"SqlSugarCore_{typeof(TDbContext).Name}";

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
}