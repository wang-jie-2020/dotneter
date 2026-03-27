using Microsoft.Extensions.Localization;

namespace I18n.LocalizationExtensions.Database.I18n;

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

    /// <summary>
    ///  不考虑resourceSource
    /// </summary>
    /// <param name="resourceSource"></param>
    /// <returns></returns>
    public IStringLocalizer Create(Type resourceSource)
    {
        return _databaseStringLocalizer;
    }

    /// <summary>
    ///  这个方法更偏向于由静态文件提供资源
    /// </summary>
    /// <param name="baseName"></param>
    /// <param name="location"></param>
    /// <returns></returns>
    public IStringLocalizer Create(string baseName, string location)
    {
        return _databaseStringLocalizer;
    }

    /// <summary>
    ///  总要有个刷新的地方
    /// </summary>
    public void Reset()
    {
        _databaseStringLocalizer = new DatabaseStringLocalizer(_serviceScopeFactory, _loggerFactory.CreateLogger<DatabaseStringLocalizer>());
    }
}