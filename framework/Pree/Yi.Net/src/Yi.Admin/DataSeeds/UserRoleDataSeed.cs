using Yi.AspNetCore.Data.Seeding;
using Yi.Framework.Core.Entities;

namespace Yi.Admin.DataSeeds;

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
           UserId = 1,
           RoleId = 1
        };
        entities.Add(role1);

        return entities;
    }
}