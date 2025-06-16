namespace Yi.AspNetCore.Data;

public class DbConnectionOptions
{
    public ConnectionStrings ConnectionStrings { get; set; }

    public DbConnectionOptions()
    {
        ConnectionStrings = new ConnectionStrings();
    }

    public string? GetConnectionStringOrNull(string connectionStringName)
    {
        var connectionString = ConnectionStrings.GetOrDefault(connectionStringName);
        if (!connectionString.IsNullOrEmpty())
        {
            return connectionString;
        }

        return null;
    }
}
