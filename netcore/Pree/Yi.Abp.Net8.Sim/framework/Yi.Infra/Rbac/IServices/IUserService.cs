using Yi.Framework.Ddd.Application.Contracts;
using Yi.Infra.Rbac.Dtos.User;

namespace Yi.Infra.Rbac.IServices;

/// <summary>
///     User服务抽象
/// </summary>
public interface IUserService : IYiCrudAppService<UserGetOutputDto, UserGetListOutputDto, Guid, UserGetListInputVo,
    UserCreateInputVo, UserUpdateInputVo>
{
}