namespace Demo.EntityFrameworkCore
{
    public interface IConnectionStringResolver
    {
        string Resolve(string connectionStringName);
    }
}
