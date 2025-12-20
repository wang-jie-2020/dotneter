using System;
using I18n.LocalizationExtensions.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Localization;

namespace I18n.LocalizationExtensions;

public static class JsonLocalizationServiceCollectionExtensions
{
    public static IServiceCollection AddJsonLocalization(this IServiceCollection services)
    {
        services.AddOptions();

        AddJsonLocalizationServices(services);

        return services;
    }

    public static IServiceCollection AddJsonLocalization(this IServiceCollection services, Action<JsonLocalizationOptions> setupAction)
    {
        AddJsonLocalizationServices(services, setupAction);

        return services;
    }

    internal static void AddJsonLocalizationServices(IServiceCollection services)
    {
        services.TryAddSingleton<IStringLocalizerFactory, JsonStringLocalizerFactory>();
        services.TryAddTransient(typeof(IStringLocalizer<>), typeof(StringLocalizer<>));
        services.TryAddTransient(typeof(IStringLocalizer), typeof(StringLocalizer));
    }

    internal static void AddJsonLocalizationServices(IServiceCollection services, Action<JsonLocalizationOptions> setupAction)
    {
        AddJsonLocalizationServices(services);
        services.Configure(setupAction);
    }
}