﻿using Volo.Abp.DependencyInjection;
using Yi.AspNetCore.Data.Seeding;
using Yi.Framework.SqlSugarCore.Repositories;
using Yi.System.Domains.Entities;

namespace Yi.System.Domains.DataSeeds;

public class RoleDataSeed : IDataSeedContributor, ITransientDependency
{
    private readonly ISqlSugarRepository<RoleEntity> _repository;

    public RoleDataSeed(ISqlSugarRepository<RoleEntity> repository)
    {
        _repository = repository;
    }

    public async Task SeedAsync()
    {
        if (!await _repository.IsAnyAsync(x => true)) await _repository.InsertRangeAsync(GetSeedData());
    }

    public List<RoleEntity> GetSeedData()
    {
        var entities = new List<RoleEntity>();
        var role1 = new RoleEntity
        {
            RoleName = "管理员",
            RoleCode = "admin",
            DataScope = DataScopeEnum.ALL,
            OrderNum = 999,
            Remark = "管理员",
            IsDeleted = false
        };
        entities.Add(role1);

        var role2 = new RoleEntity
        {
            RoleName = "测试角色",
            RoleCode = "test",
            DataScope = DataScopeEnum.ALL,
            OrderNum = 1,
            Remark = "测试用的角色",
            IsDeleted = false
        };
        entities.Add(role2);

        var role3 = new RoleEntity
        {
            RoleName = "普通角色",
            RoleCode = "common",
            DataScope = DataScopeEnum.ALL,
            OrderNum = 1,
            Remark = "正常用户",
            IsDeleted = false
        };
        entities.Add(role3);

        var role4 = new RoleEntity
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