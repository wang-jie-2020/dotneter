using Yi.Infra.Rbac.Dtos.Post;

namespace Yi.Infra.Rbac.IServices;

/// <summary>
///     Post服务抽象
/// </summary>
public interface IPostService : IYiCrudAppService<PostGetOutputDto, PostGetListOutputDto, Guid, PostGetListInputVo,
    PostCreateInputVo, PostUpdateInputVo>
{
}