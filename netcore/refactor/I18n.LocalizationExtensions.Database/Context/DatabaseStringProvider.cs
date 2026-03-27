using I18n.LocalizationExtensions.Database.I18n;

namespace I18n.LocalizationExtensions.Database.Context;

public class DatabaseStringProvider: IDatabaseStringProvider
{
    private readonly ResourceDbContext _resourceDbContext;

    public DatabaseStringProvider(ResourceDbContext resourceDbContext)
    {
        _resourceDbContext = resourceDbContext;
    }
    
    public Dictionary<string, string> GetStrings(string cultureName)
    {
        var list = _resourceDbContext.Resources.Where(r => r.Culture == cultureName).ToList();
        return list.ToDictionary(r => r.Name, r => r.Value);
    }
}