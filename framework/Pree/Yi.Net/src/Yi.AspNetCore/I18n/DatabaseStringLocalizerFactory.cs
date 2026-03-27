using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

namespace Yi.AspNetCore.I18n;

public class DatabaseStringLocalizerFactory : IStringLocalizerFactory
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly ILoggerFactory _loggerFactory;
    private DatabaseStringLocalizer _databaseStringLocalizer;

    public DatabaseStringLocalizerFactory(IServiceScopeFactory serviceScopeFactory, ILoggerFactory loggerFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
        _loggerFactory = loggerFactory;
        
        _databaseStringLocalizer = new DatabaseStringLocalizer(_serviceScopeFactory, _loggerFactory.CreateLogger<DatabaseStringLocalizer>());
    }
    
    public IStringLocalizer Create(Type resourceSource)
    {
        return _databaseStringLocalizer;
    }
    
    /// <param name="baseName"></param>
    /// <param name="location"></param>
    /// <returns></returns>
    public IStringLocalizer Create(string baseName, string location)
    {
        return _databaseStringLocalizer;
    }
    
    public void Reset()
    {
        _databaseStringLocalizer = new DatabaseStringLocalizer(_serviceScopeFactory, _loggerFactory.CreateLogger<DatabaseStringLocalizer>());
    }
}