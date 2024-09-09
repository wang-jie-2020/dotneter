using Yi.Infra.TenantManagement.Dtos;

namespace Yi.Infra.TenantManagement.Services;

public interface ITenantService : IYiCrudAppService<TenantGetOutputDto, TenantGetListOutputDto, Guid, TenantGetListInput
    , TenantCreateInput, TenantUpdateInput>
{
}