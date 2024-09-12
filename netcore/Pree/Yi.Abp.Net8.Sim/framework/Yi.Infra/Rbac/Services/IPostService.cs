using Yi.Infra.Rbac.Dtos;

namespace Yi.Infra.Rbac.Services;

/// <summary>
///     Post服务抽象
/// </summary>
public interface IPostService : IYiCrudAppService<PostGetOutputDto, PostGetListOutputDto, Guid, PostGetListInput,
    PostCreateInput, PostUpdateInput>
{
}