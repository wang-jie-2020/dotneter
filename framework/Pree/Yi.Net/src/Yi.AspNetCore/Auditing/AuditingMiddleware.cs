using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Uow;
using Yi.AspNetCore.Security;

namespace Yi.AspNetCore.Auditing;

public class AuditingMiddleware : IMiddleware, ITransientDependency
{
    private readonly IAuditingManager _auditingManager;
    protected AuditingOptions AuditingOptions { get; }
    protected ICurrentUser CurrentUser { get; }
    protected IUnitOfWorkManager UnitOfWorkManager { get; }

    public AuditingMiddleware(
        IAuditingManager auditingManager,
        ICurrentUser currentUser,
        IOptions<AuditingOptions> auditingOptions,
        IUnitOfWorkManager unitOfWorkManager)
    {
        _auditingManager = auditingManager;

        CurrentUser = currentUser;
        UnitOfWorkManager = unitOfWorkManager;
        AuditingOptions = auditingOptions.Value;
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

        return false;
    }
}
