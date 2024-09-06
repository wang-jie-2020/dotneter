using Yi.Abp.Infra.TenantManagement.Dtos;
using Yi.Framework.Ddd.Application.Contracts;

namespace Yi.Abp.Infra.TenantManagement
{
    public interface ITenantService:IYiCrudAppService< TenantGetOutputDto, TenantGetListOutputDto, Guid, TenantGetListInput, TenantCreateInput, TenantUpdateInput>
    {
    }
}
