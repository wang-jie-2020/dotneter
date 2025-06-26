using Volo.Abp.Uow;

namespace Yi.Framework.SqlSugarCore.Uow;

public class SqlSugarTransactionApi : ITransactionApi, ISupportsRollback
{
    public ISqlSugarDbContext DbContext { get; }

    public SqlSugarTransactionApi(ISqlSugarDbContext dbContext)
    {
        DbContext = dbContext;
    }

    public async Task RollbackAsync(CancellationToken cancellationToken = default)
    {
        await DbContext.SqlSugarClient.Ado.RollbackTranAsync();
    }

    public async Task CommitAsync(CancellationToken cancellationToken = default)
    {
        await DbContext.SqlSugarClient.Ado.CommitTranAsync();
    }

    public void Dispose()
    {
        DbContext.SqlSugarClient.Ado.Dispose();
    }
}