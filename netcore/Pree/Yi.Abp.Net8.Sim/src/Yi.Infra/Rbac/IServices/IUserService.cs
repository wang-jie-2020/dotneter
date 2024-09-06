using Yi.Abp.Infra.Rbac.Dtos.User;
using Yi.Framework.Ddd.Application.Contracts;

namespace Yi.Abp.Infra.Rbac.IServices
{
    /// <summary>
    /// User服务抽象
    /// </summary>
    public interface IUserService : IYiCrudAppService<UserGetOutputDto, UserGetListOutputDto, Guid, UserGetListInputVo, UserCreateInputVo, UserUpdateInputVo>
    {
    }
}
