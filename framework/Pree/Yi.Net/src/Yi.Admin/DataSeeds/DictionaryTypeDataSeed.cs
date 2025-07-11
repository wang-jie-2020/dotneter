using Yi.AspNetCore.Data.Seeding;
using Yi.Framework.Core.Entities;

namespace Yi.Admin.DataSeeds;

public class DictionaryTypeDataSeed : IDataSeedContributor, ITransientDependency
{
    private readonly ISqlSugarRepository<DictionaryTypeEntity> _repository;

    public DictionaryTypeDataSeed(ISqlSugarRepository<DictionaryTypeEntity> repository)
    {
        _repository = repository;
    }

    public async Task SeedAsync()
    {
        if (!await _repository.IsAnyAsync(x => true)) await _repository.InsertRangeAsync(GetSeedData());
    }

    public List<DictionaryTypeEntity> GetSeedData()
    {
        var entities = new List<DictionaryTypeEntity>();
        var dict1 = new DictionaryTypeEntity
        {
            DictName = "用户性别",
            DictType = "sys_user_sex",
            OrderNum = 100,
            Remark = "用户性别列表",
            IsDeleted = false,
            State = true
        };
        entities.Add(dict1);

        var dict2 = new DictionaryTypeEntity
        {
            DictName = "菜单状态",
            DictType = "sys_show_hide",
            OrderNum = 100,
            Remark = "菜单状态列表",
            IsDeleted = false,
            State = true
        };
        entities.Add(dict2);

        var dict3 = new DictionaryTypeEntity
        {
            DictName = "系统开关",
            DictType = "sys_normal_disable",
            OrderNum = 100,
            Remark = "系统开关列表",
            IsDeleted = false,
            State = true
        };
        entities.Add(dict3);

        var dict4 = new DictionaryTypeEntity
        {
            DictName = "任务状态",
            DictType = "sys_job_status",
            OrderNum = 100,
            Remark = "任务状态列表",
            IsDeleted = false,
            State = true
        };
        entities.Add(dict4);

        var dict5 = new DictionaryTypeEntity
        {
            DictName = "任务分组",
            DictType = "sys_job_group",
            OrderNum = 100,
            Remark = "任务分组列表",
            IsDeleted = false,
            State = true
        };
        entities.Add(dict5);

        var dict6 = new DictionaryTypeEntity
        {
            DictName = "系统是否",
            DictType = "sys_yes_no",
            OrderNum = 100,
            Remark = "系统是否列表",
            IsDeleted = false,
            State = true
        };
        entities.Add(dict6);

        var dict7 = new DictionaryTypeEntity
        {
            DictName = "通知类型",
            DictType = "sys_notice_type",
            OrderNum = 100,
            Remark = "通知类型列表",
            IsDeleted = false,
            State = true
        };
        entities.Add(dict7);
        
        var dict8 = new DictionaryTypeEntity
        {
            DictName = "通知状态",
            DictType = "sys_notice_status",
            OrderNum = 100,
            Remark = "通知状态列表",
            IsDeleted = false,
            State = true
        };
        entities.Add(dict8);

        var dict9 = new DictionaryTypeEntity
        {
            DictName = "操作类型",
            DictType = "sys_oper_type",
            OrderNum = 100,
            Remark = "操作类型列表",
            IsDeleted = false,
            State = true
        };
        entities.Add(dict9);
        
        var dict10 = new DictionaryTypeEntity
        {
            DictName = "系统状态",
            DictType = "sys_common_status",
            OrderNum = 100,
            Remark = "登录状态列表",
            IsDeleted = false,
            State = true
        };
        entities.Add(dict10);
        
        return entities;
    }
}