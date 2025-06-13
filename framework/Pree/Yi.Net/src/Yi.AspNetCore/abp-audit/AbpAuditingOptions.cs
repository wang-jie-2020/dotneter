using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Volo.Abp.Auditing;

public class AbpAuditingOptions
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

    public AbpAuditingOptions()
    {
        IsEnabled = true;
        Contributors = new List<AuditLogContributor>();
    }
}
