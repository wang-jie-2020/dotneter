using Volo.Abp.Application.Dtos;
using Yi.Abp.Infra.Rbac.Model;

namespace Yi.Abp.Infra.Rbac.IServices
{
    public interface IOnlineService
    {
      Task< PagedResultDto<OnlineUserModel>> GetListAsync(OnlineUserModel online);
    }
}
