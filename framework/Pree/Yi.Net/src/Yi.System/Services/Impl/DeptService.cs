using Yi.Framework.Abstractions;
using Yi.Framework.Core.Entities;
using Yi.System.Services.Dtos;

namespace Yi.System.Services.Impl;

public class DeptService : BaseService, IDeptService
{
    private readonly ISqlSugarRepository<DeptEntity> _repository;

    public DeptService(ISqlSugarRepository<DeptEntity> repository)
    {
        _repository = repository;
    }

    public async Task<DeptDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetByIdAsync(id);
        return entity.Adapt<DeptDto>();
    }

    public async Task<PagedResult<DeptDto>> GetListAsync(DeptQuery query)
    {
        RefAsync<int> total = 0;
        var entities = await _repository.AsQueryable()
            .WhereIF(!string.IsNullOrEmpty(query.DeptName), u => u.DeptName.Contains(query.DeptName!))
            .WhereIF(query.State is not null, u => u.State == query.State)
            .OrderBy(u => u.OrderNum)
            .ToPageListAsync(query.PageNum, query.PageSize, total);

        return new PagedResult<DeptDto>
        {
            TotalCount = total,
            Items = entities.Adapt<List<DeptDto>>()
        };
    }

    public async Task<DeptDto> CreateAsync(DeptInput input)
    {
        var entity = input.Adapt<DeptEntity>();
        await _repository.InsertAsync(entity);

        return entity.Adapt<DeptDto>();
    }

    public async Task<DeptDto> UpdateAsync(Guid id, DeptInput input)
    {
        var entity = await _repository.GetByIdAsync(id);
        input.Adapt(entity);
        await _repository.UpdateAsync(entity);

        return entity.Adapt<DeptDto>();
    }

    public async Task DeleteAsync(IEnumerable<Guid> id)
    {
        await _repository.DeleteByIdsAsync(id.Select(x => (object)x).ToArray());
    }
    
    public async Task<List<Guid>> GetChildListAsync(Guid deptId)
    {
        var entities = await _repository.AsQueryable().ToChildListAsync(x => x.ParentId, deptId);
        return entities.Select(x => x.Id).ToList();
    }
    
    public async Task<List<DeptDto>> GetRoleIdAsync(Guid roleId)
    {
        var entities = await _repository.AsQueryable().
            Where(d => SqlFunc.Subqueryable<RoleDeptEntity>().Where(rd => rd.RoleId == roleId && d.Id == rd.DeptId).Any())
            .ToListAsync();
        return entities.Adapt<List<DeptDto>>();
    }
}