using Mapster;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Yi.Framework.Core.Helper;
using Yi.Framework.SqlSugarCore;
using Yi.Infra.Rbac.Dtos;
using Yi.Infra.Rbac.Entities;

namespace Yi.Infra.Rbac.Services;

[RemoteService(false)]
public class PostService : ApplicationService, IPostService
{
    private readonly ISqlSugarRepository<PostAggregateRoot, Guid> _repository;

    public PostService(ISqlSugarRepository<PostAggregateRoot, Guid> repository)
    {
        _repository = repository;
    }

    public async Task<PostDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);
        return entity.Adapt<PostDto>();
    }

    public async Task<PagedResultDto<PostDto>> GetListAsync(PostGetListInput input)
    {
        RefAsync<int> total = 0;

        var entities = await _repository._DbQueryable
            .WhereIF(!string.IsNullOrEmpty(input.PostName), x => x.PostName.Contains(input.PostName!))
            .WhereIF(!string.IsNullOrEmpty(input.PostCode), x => x.PostCode.Contains(input.PostCode!))
            .WhereIF(input.State is not null, x => x.State == input.State)
            .ToPageListAsync(input.SkipCount, input.MaxResultCount, total);
        return new PagedResultDto<PostDto>(total, entities.Adapt<List<PostDto>>());
    }

    public async Task<PostDto> CreateAsync(PostCreateInput input)
    {
        var entity = input.Adapt<PostAggregateRoot>();
        await _repository.InsertAsync(entity, autoSave: true);

        return entity.Adapt<PostDto>();
    }

    public async Task<PostDto> UpdateAsync(Guid id, PostUpdateInput input)
    {
        var entity = await _repository.GetAsync(id);
        input.Adapt(entity);
        await _repository.UpdateAsync(entity, autoSave: true);

        return entity.Adapt<PostDto>();
    }

    public async Task DeleteAsync(IEnumerable<Guid> id)
    {
        await _repository.DeleteManyAsync(id);
    }

    public async Task<IActionResult> GetExportExcelAsync(PostGetListInput input)
    {
        if (input is IPagedResultRequest paged)
        {
            paged.SkipCount = 0;
            paged.MaxResultCount = LimitedResultRequestDto.MaxMaxResultCount;
        }

        var output = await GetListAsync(input);
        return new PhysicalFileResult(ExporterHelper.ExportExcel(output.Items), "application/vnd.ms-excel");
    }
}