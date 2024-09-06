using Yi.Framework.Ddd.Application.Contracts;
using Yi.Infra.TenantManagement.Dtos;

namespace Yi.Infra.TenantManagement
{
    public interface ITenantService:IYiCrudAppService< TenantGetOutputDto, TenantGetListOutputDto, Guid, TenantGetListInput, TenantCreateInput, TenantUpdateInput>
    {
    }
}
