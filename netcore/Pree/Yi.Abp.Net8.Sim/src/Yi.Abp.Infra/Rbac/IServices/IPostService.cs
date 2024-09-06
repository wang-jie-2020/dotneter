using Yi.Abp.Infra.Rbac.Dtos.Post;
using Yi.Framework.Ddd.Application.Contracts;

namespace Yi.Abp.Infra.Rbac.IServices
{
    /// <summary>
    /// Post服务抽象
    /// </summary>
    public interface IPostService : IYiCrudAppService<PostGetOutputDto, PostGetListOutputDto, Guid, PostGetListInputVo, PostCreateInputVo, PostUpdateInputVo>
    {

    }
}
