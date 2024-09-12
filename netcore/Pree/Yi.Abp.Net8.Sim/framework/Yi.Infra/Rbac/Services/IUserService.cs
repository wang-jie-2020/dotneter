using Yi.Infra.Rbac.Dtos.User;

namespace Yi.Infra.Rbac.Services;

/// <summary>
///     User服务抽象
/// </summary>
public interface IUserService : IYiCrudAppService<UserGetOutputDto, UserGetListOutputDto, Guid, UserGetListInputVo,
    UserCreateInputVo, UserUpdateInputVo>
{
}