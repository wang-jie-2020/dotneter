using Yi.Framework;
using Yi.Framework.Abstractions;
using Yi.System.Domains.Entities;
using Yi.System.Domains.Repositories;
using Yi.System.Services.Dtos;

namespace Yi.System.Services.Impl;

public class DeptService : BaseService, IDeptService
{
    private readonly IDeptRepository _repository;

    public DeptService(IDeptRepository repository)
    {
        _repository = repository;
    }

    public async Task<DeptGetOutputDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetByIdAsync(id);
        return entity.Adapt<DeptGetOutputDto>();
    }

    public async Task<PagedResult<DeptGetListOutputDto>> GetListAsync(DeptGetListInput input)
    {
        RefAsync<int> total = 0;
        var entities = await _repository.AsQueryable()
            .WhereIF(!string.IsNullOrEmpty(input.DeptName), u => u.DeptName.Contains(input.DeptName!))
            .WhereIF(input.State is not null, u => u.State == input.State)
            .OrderBy(u => u.OrderNum)
            .ToPageListAsync(input.PageNum, input.PageSize, total);

        return new PagedResult<DeptGetListOutputDto>
        {
            TotalCount = total,
            Items = entities.Adapt<List<DeptGetListOutputDto>>()
        };
    }

    public async Task<DeptGetOutputDto> CreateAsync(DeptCreateInput input)
    {
        var entity = input.Adapt<DeptEntity>();
        await _repository.InsertAsync(entity);

        return entity.Adapt<DeptGetOutputDto>();
    }

    public async Task<DeptGetOutputDto> UpdateAsync(Guid id, DeptUpdateInput input)
    {
        var entity = await _repository.GetByIdAsync(id);
        input.Adapt(entity);
        await _repository.UpdateAsync(entity);

        return entity.Adapt<DeptGetOutputDto>();
    }

    public async Task DeleteAsync(IEnumerable<Guid> id)
    {
        await _repository.DeleteByIdsAsync(id.Select(x => (object)x).ToArray());
    }
    
    public async Task<List<Guid>> GetChildListAsync(Guid deptId)
    {
        return await _repository.GetChildListAsync(deptId);
    }
    
    public async Task<List<DeptGetListOutputDto>> GetRoleIdAsync(Guid roleId)
    {
        var entities = await _repository.GetListRoleIdAsync(roleId);
        return entities.Adapt<List<DeptGetListOutputDto>>();
    }
}