using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Uow;
using Yi.AspNetCore.Data.Filtering;
using Yi.AspNetCore.MultiTenancy;
using Yi.AspNetCore.Security;

namespace Yi.AspNetCore.Core;

public abstract class BaseService : ITransientDependency
{
    public IAbpLazyServiceProvider LazyServiceProvider { get; set; } = default!;

    protected IUnitOfWorkManager UnitOfWorkManager => LazyServiceProvider.LazyGetRequiredService<IUnitOfWorkManager>();

    protected ILoggerFactory LoggerFactory => LazyServiceProvider.LazyGetRequiredService<ILoggerFactory>();

    protected ICurrentTenant CurrentTenant => LazyServiceProvider.LazyGetRequiredService<ICurrentTenant>();

    protected IDataFilter DataFilter => LazyServiceProvider.LazyGetRequiredService<IDataFilter>();

    protected ICurrentUser CurrentUser => LazyServiceProvider.LazyGetRequiredService<ICurrentUser>();

    protected IAuthorizationService AuthorizationService => LazyServiceProvider.LazyGetRequiredService<IAuthorizationService>();

    protected IStringLocalizerFactory StringLocalizerFactory => LazyServiceProvider.LazyGetRequiredService<IStringLocalizerFactory>();

    protected IStringLocalizer L
    {
        get
        {
            if (_localizer == null)
            {
                //_localizer = CreateLocalizer();
            }

            return _localizer;
        }
    }
    private IStringLocalizer? _localizer;

    //protected Type? LocalizationResource {
    //    get => _localizationResource;
    //    set {
    //        _localizationResource = value;
    //        _localizer = null;
    //    }
    //}
    //private Type? _localizationResource = typeof(DefaultResource);

    protected IUnitOfWork? CurrentUnitOfWork => UnitOfWorkManager?.Current;

    protected ILogger Logger => LazyServiceProvider.LazyGetService<ILogger>(provider => LoggerFactory?.CreateLogger(GetType().FullName!) ?? NullLogger.Instance);

    //protected virtual IStringLocalizer CreateLocalizer()
    //{
    //    if (LocalizationResource != null)
    //    {
    //        return StringLocalizerFactory.Create(LocalizationResource);
    //    }

    //    var localizer = StringLocalizerFactory.CreateDefaultOrNull();
    //    if (localizer == null)
    //    {
    //        throw new Exception($"Set {nameof(LocalizationResource)} or define the default localization resource type (by configuring the {nameof(AbpLocalizationOptions)}.{nameof(AbpLocalizationOptions.DefaultResourceType)}) to be able to use the {nameof(L)} object!");
    //    }

    //    return localizer;
    //}
}
