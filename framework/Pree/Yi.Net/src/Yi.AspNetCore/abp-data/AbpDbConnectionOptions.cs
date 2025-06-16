using System;
using System.Collections.Generic;

namespace Volo.Abp.Data;

public class AbpDbConnectionOptions
{
    public ConnectionStrings ConnectionStrings { get; set; }

    public AbpDbConnectionOptions()
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
