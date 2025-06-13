using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Volo.Abp.Auditing;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Uow;
using Volo.Abp.Users;

namespace Volo.Abp.AspNetCore.Auditing;

public class AbpAuditingMiddleware : IMiddleware, ITransientDependency
{
    private readonly IAuditingManager _auditingManager;
    protected AbpAuditingOptions AuditingOptions { get; }
    protected AbpAspNetCoreAuditingOptions AspNetCoreAuditingOptions { get; }
    protected ICurrentUser CurrentUser { get; }
    protected IUnitOfWorkManager UnitOfWorkManager { get; }

    public AbpAuditingMiddleware(
        IAuditingManager auditingManager,
        ICurrentUser currentUser,
        IOptions<AbpAuditingOptions> auditingOptions,
        IOptions<AbpAspNetCoreAuditingOptions> aspNetCoreAuditingOptions,
        IUnitOfWorkManager unitOfWorkManager)
    {
        _auditingManager = auditingManager;

        CurrentUser = currentUser;
        UnitOfWorkManager = unitOfWorkManager;
        AuditingOptions = auditingOptions.Value;
        AspNetCoreAuditingOptions = aspNetCoreAuditingOptions.Value;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        if (!AuditingOptions.IsEnabled || IsIgnoredUrl(context))
        {
            await next(context);
            return;
        }

        var hasError = false;
        using (var saveHandle = _auditingManager.BeginScope())
        {
            Debug.Assert(_auditingManager.Current != null);

            try
            {
                await next(context);

                if (_auditingManager.Current.Log.Exceptions.Any())
                {
                    hasError = true;
                }
            }
            catch (Exception ex)
            {
                hasError = true;

                if (!_auditingManager.Current.Log.Exceptions.Contains(ex))
                {
                    _auditingManager.Current.Log.Exceptions.Add(ex);
                }

                throw;
            }
            finally
            {
                if (UnitOfWorkManager.Current != null)
                {
                    try
                    {
                        await UnitOfWorkManager.Current.SaveChangesAsync();
                    }
                    catch (Exception ex)
                    {
                        if (!_auditingManager.Current.Log.Exceptions.Contains(ex))
                        {
                            _auditingManager.Current.Log.Exceptions.Add(ex);
                        }
                    }
                }

                await saveHandle.SaveAsync();

            }
        }
    }

    private bool IsIgnoredUrl(HttpContext context)
    {
        if (context.Request.Path.Value == null)
        {
            return false;
        }

        if (AspNetCoreAuditingOptions.IgnoredUrls.Any(x => context.Request.Path.Value.StartsWith(x, StringComparison.OrdinalIgnoreCase)))
        {
            return true;
        }

        return false;
    }
}
