using Yi.AspNetCore.Data.Seeding;
using Yi.System.Domains.Entities;

namespace Yi.System.Domains.DataSeeds;

public class UserRoleDataSeed: IDataSeedContributor, ITransientDependency
{
    private readonly ISqlSugarRepository<UserRoleEntity> _repository;

    public UserRoleDataSeed(ISqlSugarRepository<UserRoleEntity> repository)
    {
        _repository = repository;
    }

    public async Task SeedAsync()
    {
        if (!await _repository.IsAnyAsync(x => true)) await _repository.InsertRangeAsync(GetSeedData());
    }

    public List<UserRoleEntity> GetSeedData()
    {
        var entities = new List<UserRoleEntity>();
        var role1 = new UserRoleEntity
        {
           UserId = Guid.Parse("661DBEB5-79F6-42C6-A295-A13ADA6D505D"),
           RoleId = Guid.Parse("BD4469C3-7EC7-4F72-ABBF-01B754532006")
        };
        entities.Add(role1);

        return entities;
    }
}