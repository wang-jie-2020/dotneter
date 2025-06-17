namespace Yi.AspNetCore.Auditing;

public abstract class AuditLogContributor
{
    public virtual void PreContribute(AuditLogContributionContext context)
    {

    }

    public virtual void PostContribute(AuditLogContributionContext context)
    {

    }
}
