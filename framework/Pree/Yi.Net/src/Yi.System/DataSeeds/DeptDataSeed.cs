using Yi.AspNetCore.Data.Seeding;
using Yi.Framework.Utils;
using Yi.System.Entities;

namespace Yi.System.DataSeeds;

public class DeptDataSeed : IDataSeedContributor, ITransientDependency
{
    private readonly ISqlSugarRepository<DeptEntity> _repository;

    public DeptDataSeed(ISqlSugarRepository<DeptEntity> repository)
    {
        _repository = repository;
    }

    public async Task SeedAsync()
    {
        if (!await _repository.IsAnyAsync(x => true)) await _repository.InsertRangeAsync(GetSeedData());
    }

    public List<DeptEntity> GetSeedData()
    {
        var entities = new List<DeptEntity>();

        var chengziDept = new DeptEntity(SequentialGuidGenerator.Create())
        {
            DeptName = "橙子科技",
            DeptCode = "Yi",
            OrderNum = 100,
            IsDeleted = false,
            Leader = "橙子",
            Remark = "如名所指"
        };
        entities.Add(chengziDept);
        
        var shenzhenDept = new DeptEntity(SequentialGuidGenerator.Create())
        {
            DeptName = "深圳总公司",
            OrderNum = 100,
            IsDeleted = false,
            ParentId = chengziDept.Id
        };
        entities.Add(shenzhenDept);
        
        var jiangxiDept = new DeptEntity(SequentialGuidGenerator.Create())
        {
            DeptName = "江西总公司",
            OrderNum = 100,
            IsDeleted = false,
            ParentId = chengziDept.Id
        };
        entities.Add(jiangxiDept);
        
        var szDept1 = new DeptEntity(SequentialGuidGenerator.Create())
        {
            DeptName = "研发部门",
            OrderNum = 100,
            IsDeleted = false,
            ParentId = shenzhenDept.Id
        };
        entities.Add(szDept1);

        var szDept2 = new DeptEntity(SequentialGuidGenerator.Create())
        {
            DeptName = "市场部门",
            OrderNum = 100,
            IsDeleted = false,
            ParentId = shenzhenDept.Id
        };
        entities.Add(szDept2);

        var szDept3 = new DeptEntity(SequentialGuidGenerator.Create())
        {
            DeptName = "测试部门",
            OrderNum = 100,
            IsDeleted = false,
            ParentId = shenzhenDept.Id
        };
        entities.Add(szDept3);

        var szDept4 = new DeptEntity(SequentialGuidGenerator.Create())
        {
            DeptName = "财务部门",
            OrderNum = 100,
            IsDeleted = false,
            ParentId = shenzhenDept.Id
        };
        entities.Add(szDept4);

        var szDept5 = new DeptEntity(SequentialGuidGenerator.Create())
        {
            DeptName = "运维部门",
            OrderNum = 100,
            IsDeleted = false,
            ParentId = shenzhenDept.Id
        };
        entities.Add(szDept5);


        var jxDept1 = new DeptEntity(SequentialGuidGenerator.Create())
        {
            DeptName = "市场部门",
            OrderNum = 100,
            IsDeleted = false,
            ParentId = jiangxiDept.Id
        };
        entities.Add(jxDept1);


        var jxDept2 = new DeptEntity(SequentialGuidGenerator.Create())
        {
            DeptName = "财务部门",
            OrderNum = 100,
            IsDeleted = false,
            ParentId = jiangxiDept.Id
        };
        entities.Add(jxDept2);
        
        return entities;
    }
}