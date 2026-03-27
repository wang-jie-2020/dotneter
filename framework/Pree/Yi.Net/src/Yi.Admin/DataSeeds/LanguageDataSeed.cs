using Yi.AspNetCore.Data.Seeding;
using Yi.Framework.Core.Entities;
using Yi.Framework.Utils;

namespace Yi.Admin.DataSeeds;

public class LanguageDataSeed : IDataSeedContributor, ITransientDependency
{
    private readonly ISqlSugarRepository<LanguageEntity> _repository;

    public LanguageDataSeed(ISqlSugarRepository<LanguageEntity> repository)
    {
        _repository = repository;
    }

    public async Task SeedAsync()
    {
        if (!await _repository.IsAnyAsync(x => true)) await _repository.InsertRangeAsync(GetSeedData());
    }

    public List<LanguageEntity> GetSeedData()
    {
        var entities = new List<LanguageEntity>();

        entities.Add(new LanguageEntity()
        {
            Name = "InternalServerErrorMessage",
            Value = "对不起，在处理您的请求期间产生了一个服务器内部错误",
            Culture = "zh"
        });
                    
        entities.Add(new LanguageEntity()
        {
            Name = "InternalServerErrorMessage",
            Value = "Sorry, a server internal error occurred while processing your request.",
            Culture = "en"
        });
                    
        entities.Add(new LanguageEntity()
        {
            Name = "InternalServerErrorMessage",
            Value = "Désolé, une erreur interne du serveur s'est produite lors du traitement de votre demande.",
            Culture = "fr"
        });
        
        return entities;
    }
}