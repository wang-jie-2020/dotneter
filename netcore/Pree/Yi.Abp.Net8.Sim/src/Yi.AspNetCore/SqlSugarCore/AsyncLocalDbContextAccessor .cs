namespace Yi.AspNetCore.SqlSugarCore;

public class AsyncLocalDbContextAccessor
{
    private readonly AsyncLocal<ISqlSugarDbContext> _currentScope;

    public AsyncLocalDbContextAccessor()
    {
        _currentScope = new AsyncLocal<ISqlSugarDbContext?>();
    }

    public static AsyncLocalDbContextAccessor Instance { get; } = new();

    public ISqlSugarDbContext? Current
    {
        get => _currentScope.Value;
        set => _currentScope.Value = value;
    }
}