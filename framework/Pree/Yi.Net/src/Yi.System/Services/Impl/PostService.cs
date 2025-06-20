using Yi.AspNetCore.Core;
using Yi.Framework.Core;
using Yi.Framework.Core.Abstractions;
using Yi.Framework.SqlSugarCore.Repositories;
using Yi.System.Domains.Entities;
using Yi.System.Services.Dtos;

namespace Yi.System.Services.Impl;

public class PostService : BaseService, IPostService
{
    private readonly ISqlSugarRepository<PostEntity, Guid> _repository;

    public PostService(ISqlSugarRepository<PostEntity, Guid> repository)
    {
        _repository = repository;
    }

    public async Task<PostDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);
        return entity.Adapt<PostDto>();
    }

    public async Task<PagedResult<PostDto>> GetListAsync(PostGetListInput input)
    {
        RefAsync<int> total = 0;

        var entities = await _repository.DbQueryable
            .WhereIF(!string.IsNullOrEmpty(input.PostName), x => x.PostName.Contains(input.PostName!))
            .WhereIF(!string.IsNullOrEmpty(input.PostCode), x => x.PostCode.Contains(input.PostCode!))
            .WhereIF(input.State is not null, x => x.State == input.State)
            .ToPageListAsync(input.PageNum, input.PageSize, total);
        return new PagedResult<PostDto>(total, entities.Adapt<List<PostDto>>());
    }

    public async Task<PostDto> CreateAsync(PostCreateInput input)
    {
        var entity = input.Adapt<PostEntity>();
        await _repository.InsertAsync(entity);

        return entity.Adapt<PostDto>();
    }

    public async Task<PostDto> UpdateAsync(Guid id, PostUpdateInput input)
    {
        var entity = await _repository.GetAsync(id);
        input.Adapt(entity);
        await _repository.UpdateAsync(entity);

        return entity.Adapt<PostDto>();
    }

    public async Task DeleteAsync(IEnumerable<Guid> id)
    {
        await _repository.DeleteManyAsync(id);
    }
}