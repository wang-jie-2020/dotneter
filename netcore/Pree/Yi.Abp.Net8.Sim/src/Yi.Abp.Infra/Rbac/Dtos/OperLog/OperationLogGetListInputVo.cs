using Yi.Abp.Infra.Rbac.Operlog;
using Yi.Framework.Ddd.Application.Contracts;

namespace Yi.Abp.Infra.Rbac.Dtos.OperLog
{
    public class OperationLogGetListInputVo : PagedAllResultRequestDto
    {
        public OperEnum? OperType { get; set; }
        public string? OperUser { get; set; }
    }
}
