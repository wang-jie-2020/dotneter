using Yi.Framework.Abstractions;
using Yi.Framework.Core.Entities;
using Yi.System.Services.Dtos;

namespace Yi.System.Services.Impl;

public class PostService : BaseService, IPostService
{
    private readonly ISqlSugarRepository<PostEntity> _repository;

    public PostService(ISqlSugarRepository<PostEntity> repository)
    {
        _repository = repository;
    }

    public async Task<PostDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetByIdAsync(id);
        return entity.Adapt<PostDto>();
    }

    public async Task<PagedResult<PostDto>> GetListAsync(PostQuery query)
    {
        RefAsync<int> total = 0;

        var entities = await _repository.AsQueryable()
            .WhereIF(!string.IsNullOrEmpty(query.PostName), x => x.PostName.Contains(query.PostName!))
            .WhereIF(!string.IsNullOrEmpty(query.PostCode), x => x.PostCode.Contains(query.PostCode!))
            .WhereIF(query.State is not null, x => x.State == query.State)
            .ToPageListAsync(query.PageNum, query.PageSize, total);
        return new PagedResult<PostDto>(total, entities.Adapt<List<PostDto>>());
    }

    public async Task<PostDto> CreateAsync(PostInput input)
    {
        var entity = input.Adapt<PostEntity>();
        await _repository.InsertAsync(entity);

        return entity.Adapt<PostDto>();
    }

    public async Task<PostDto> UpdateAsync(Guid id, PostInput input)
    {
        var entity = await _repository.GetByIdAsync(id);
        input.Adapt(entity);
        await _repository.UpdateAsync(entity);

        return entity.Adapt<PostDto>();
    }

    public async Task DeleteAsync(IEnumerable<Guid> id)
    {
        await _repository.DeleteByIdsAsync(id.Select(x => (object)x).ToArray());
    }
}