using System.Collections.Concurrent;
using System.Globalization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

namespace Yi.AspNetCore.I18n;

public class DatabaseStringLocalizer : IStringLocalizer
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly ILogger _logger;
    
    private readonly ConcurrentDictionary<string, ConcurrentDictionary<string, string>> _resourcesCache = new();
    private readonly ConcurrentDictionary<string, object> _missingManifestCache = new();
    
    public DatabaseStringLocalizer(IServiceScopeFactory serviceScopeFactory, ILogger logger)
    {
        _serviceScopeFactory = serviceScopeFactory;
        _logger = logger;
    }

    public LocalizedString this[string name]
    {
        get
        {
            var value = GetString(name);
            return new LocalizedString(name, value ?? name, resourceNotFound: value == null);
        }
    }

    public LocalizedString this[string name, params object[] arguments]
    {
        get
        {
            var format = GetString(name);
            var value = string.Format(format ?? name, arguments);

            return new LocalizedString(name, value, resourceNotFound: format == null);
        }
    }

    public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures) => GetStrings();
    
    private string GetString(string name)
    {
        var culture = CultureInfo.CurrentUICulture;
        LoadResourceSet(culture.Name);
        
        var missingKey = $"name={name}&culture={culture.Name}";
        if (_missingManifestCache.ContainsKey(missingKey))
        {
            return null;
        }
        
        if (_resourcesCache.TryGetValue(culture.Name, out var resources))
        {
            if (resources.TryGetValue(name, out var value))
            {
                return value.ToString();
            }
        }

        _missingManifestCache.TryAdd(missingKey, null);
        return null;
    }
    
    protected virtual IEnumerable<LocalizedString> GetStrings()
    {
        var culture = CultureInfo.CurrentUICulture;
        LoadResourceSet(culture.Name);

        if (_resourcesCache.TryGetValue(culture.Name, out var resources))
        {
            foreach (var resource in resources)
            {
                yield return new LocalizedString(resource.Key, resource.Value, true);
            }
        }
    }
    
    private void LoadResourceSet(string cultureName)
    {
        if (!_resourcesCache.ContainsKey(cultureName))
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var databaseStringProvider = scope.ServiceProvider.GetRequiredService<IDatabaseStringProvider>();
                
                var resources = databaseStringProvider.GetStrings(cultureName);
                _resourcesCache.TryAdd(cultureName, new ConcurrentDictionary<string, string>(resources));
            }
        }
    }
}