using Volo.Abp.Application.Dtos;

namespace Yi.Abp.Infra.TenantManagement.Dtos
{
    public class TenantSelectOutputDto : EntityDto<Guid>
    {
        public string Name { get; set; }
    }
}
