using Volo.Abp.Uow;

namespace Yi.Framework.SqlSugarCore.Uow;

public class SqlSugarTransactionApi : ITransactionApi, ISupportsRollback
{
    private readonly ISqlSugarDbContext _sqlsugarDbContext;

    public SqlSugarTransactionApi(ISqlSugarDbContext sqlsugarDbContext)
    {
        _sqlsugarDbContext = sqlsugarDbContext;
    }

    public async Task RollbackAsync(CancellationToken cancellationToken = default)
    {
        await _sqlsugarDbContext.SqlSugarClient.Ado.RollbackTranAsync();
    }

    public async Task CommitAsync(CancellationToken cancellationToken = default)
    {
        await _sqlsugarDbContext.SqlSugarClient.Ado.CommitTranAsync();
    }

    public void Dispose()
    {
        _sqlsugarDbContext.SqlSugarClient.Ado.Dispose();
    }

    public ISqlSugarDbContext GetDbContext()
    {
        return _sqlsugarDbContext;
    }
}