﻿namespace Yi.AspNetCore.Auditing;

public class AuditingOptions
{
    /// <summary>
    /// Default: true.
    /// </summary>
    public bool IsEnabled { get; set; }

    /// <summary>
    /// The name of the application or service writing audit logs.
    /// Default: null.
    /// </summary>
    public string? ApplicationName { get; set; }

    public List<AuditLogContributor> Contributors { get; }

    public AuditingOptions()
    {
        IsEnabled = true;
        Contributors = new List<AuditLogContributor>()
        {
            new AspNetCoreAuditLogContributor()
        };
    }
}
