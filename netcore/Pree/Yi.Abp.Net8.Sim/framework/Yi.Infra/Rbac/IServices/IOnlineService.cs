using Volo.Abp.Application.Dtos;
using Yi.Infra.Rbac.Model;

namespace Yi.Infra.Rbac.IServices;

public interface IOnlineService
{
    Task<PagedResultDto<OnlineUserModel>> GetListAsync(OnlineUserModel online);
}