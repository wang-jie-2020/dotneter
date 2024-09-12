using SqlSugar;
using Volo.Abp.Application.Dtos;
using Yi.Framework.SqlSugarCore;
using Yi.Infra.Rbac.Dtos.Post;
using Yi.Infra.Rbac.Entities;

namespace Yi.Infra.Rbac.Services;

/// <summary>
///     Post服务实现
/// </summary>
public class PostService : YiCrudAppService<PostAggregateRoot, PostGetOutputDto, PostGetListOutputDto, Guid,
        PostGetListInputVo, PostCreateInputVo, PostUpdateInputVo>,
    IPostService
{
    private readonly ISqlSugarRepository<PostAggregateRoot, Guid> _repository;

    public PostService(ISqlSugarRepository<PostAggregateRoot, Guid> repository) : base(repository)
    {
        _repository = repository;
    }

    public override async Task<PagedResultDto<PostGetListOutputDto>> GetListAsync(PostGetListInputVo input)
    {
        RefAsync<int> total = 0;

        var entities = await _repository._DbQueryable.WhereIF(!string.IsNullOrEmpty(input.PostName),
                x => x.PostName.Contains(input.PostName!))
            .WhereIF(input.State is not null, x => x.State == input.State)
            .ToPageListAsync(input.SkipCount, input.MaxResultCount, total);
        return new PagedResultDto<PostGetListOutputDto>(total, await MapToGetListOutputDtosAsync(entities));
    }
}