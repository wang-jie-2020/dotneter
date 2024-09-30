using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Yi.AspNetCore.Helpers;
using Yi.AspNetCore.System;
using Yi.System.Domains.System.Entities;
using Yi.System.Services.System.Dtos;

namespace Yi.System.Services.System.Impl;

public class PostService : ApplicationService, IPostService
{
    private readonly ISqlSugarRepository<PostEntity, long> _repository;

    public PostService(ISqlSugarRepository<PostEntity, long> repository)
    {
        _repository = repository;
    }

    public async Task<PostDto> GetAsync(long id)
    {
        var entity = await _repository.GetAsync(id);
        return entity.Adapt<PostDto>();
    }

    public async Task<PagedResultDto<PostDto>> GetListAsync(PostGetListInput input)
    {
        RefAsync<int> total = 0;

        var entities = await _repository.DbQueryable
            .WhereIF(!string.IsNullOrEmpty(input.PostName), x => x.PostName.Contains(input.PostName!))
            .WhereIF(!string.IsNullOrEmpty(input.PostCode), x => x.PostCode.Contains(input.PostCode!))
            .WhereIF(input.State is not null, x => x.State == input.State)
            .ToPageListAsync(input.PageNum, input.PageSize, total);
        return new PagedResultDto<PostDto>(total, entities.Adapt<List<PostDto>>());
    }

    public async Task<PostDto> CreateAsync(PostCreateInput input)
    {
        var entity = input.Adapt<PostEntity>();
        await _repository.InsertAsync(entity, autoSave: true);

        return entity.Adapt<PostDto>();
    }

    public async Task<PostDto> UpdateAsync(long id, PostUpdateInput input)
    {
        var entity = await _repository.GetAsync(id);
        input.Adapt(entity);
        await _repository.UpdateAsync(entity, autoSave: true);

        return entity.Adapt<PostDto>();
    }

    public async Task DeleteAsync(IEnumerable<long> id)
    {
        await _repository.DeleteManyAsync(id);
    }
}