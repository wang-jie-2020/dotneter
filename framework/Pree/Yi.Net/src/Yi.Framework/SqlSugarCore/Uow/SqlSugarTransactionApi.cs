using Volo.Abp.Uow;

namespace Yi.Framework.SqlSugarCore.Uow;

public class SqlSugarTransactionApi : ITransactionApi, ISupportsRollback
{
    private readonly ISqlSugarDbContext _sqlSugarDbContext;

    public SqlSugarTransactionApi(ISqlSugarDbContext sqlSugarDbContext)
    {
        _sqlSugarDbContext = sqlSugarDbContext;
    }

    public async Task RollbackAsync(CancellationToken cancellationToken = default)
    {
        await _sqlSugarDbContext.SqlSugarClient.Ado.RollbackTranAsync();
    }

    public async Task CommitAsync(CancellationToken cancellationToken = default)
    {
        await _sqlSugarDbContext.SqlSugarClient.Ado.CommitTranAsync();
    }

    public void Dispose()
    {
        _sqlSugarDbContext.SqlSugarClient.Ado.Dispose();
    }

    public ISqlSugarDbContext GetDbContext()
    {
        return _sqlSugarDbContext;
    }
}