using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Yi.Framework.SqlSugarCore;
using Yi.Infra.Rbac.Entities;
using Yi.Infra.Rbac.Enums;

namespace Yi.Infra.Rbac.DataSeeds;

public class RoleDataSeed : IDataSeedContributor, ITransientDependency
{
    private readonly ISqlSugarRepository<RoleAggregateRoot> _repository;

    public RoleDataSeed(ISqlSugarRepository<RoleAggregateRoot> repository)
    {
        _repository = repository;
    }

    public async Task SeedAsync(DataSeedContext context)
    {
        if (!await _repository.IsAnyAsync(x => true)) await _repository.InsertManyAsync(GetSeedData());
    }

    public List<RoleAggregateRoot> GetSeedData()
    {
        var entities = new List<RoleAggregateRoot>();
        var role1 = new RoleAggregateRoot
        {
            RoleName = "管理员",
            RoleCode = "admin",
            DataScope = DataScopeEnum.ALL,
            OrderNum = 999,
            Remark = "管理员",
            IsDeleted = false
        };
        entities.Add(role1);

        var role2 = new RoleAggregateRoot
        {
            RoleName = "测试角色",
            RoleCode = "test",
            DataScope = DataScopeEnum.ALL,
            OrderNum = 1,
            Remark = "测试用的角色",
            IsDeleted = false
        };
        entities.Add(role2);

        var role3 = new RoleAggregateRoot
        {
            RoleName = "普通角色",
            RoleCode = "common",
            DataScope = DataScopeEnum.ALL,
            OrderNum = 1,
            Remark = "正常用户",
            IsDeleted = false
        };
        entities.Add(role3);

        var role4 = new RoleAggregateRoot
        {
            RoleName = "默认角色",
            RoleCode = "default",
            DataScope = DataScopeEnum.ALL,
            OrderNum = 1,
            Remark = "可简单浏览",
            IsDeleted = false
        };
        entities.Add(role4);


        return entities;
    }
}