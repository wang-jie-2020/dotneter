using Volo.Abp.DependencyInjection;
using Yi.AspNetCore.I18n;
using Yi.Framework.Core.Entities;
using Yi.Framework.SqlSugarCore.Repositories;

namespace Yi.Framework.Core;

public class DatabaseStringProvider: IDatabaseStringProvider, ITransientDependency
{
    private readonly ISqlSugarRepository<LanguageEntity> _languageRepository;
    
    public DatabaseStringProvider(ISqlSugarRepository<LanguageEntity> languageRepository)
    {
        _languageRepository = languageRepository;
    }
    
    public Dictionary<string, string> GetStrings(string cultureName)
    {
        var list = _languageRepository.AsQueryable().Where(r => r.Culture == cultureName).ToList();
        return list.ToDictionary(r => r.Name, r => r.Value);
    }
}