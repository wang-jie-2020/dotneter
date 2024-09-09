using Yi.Framework.Ddd.Application;
using Yi.Infra.Rbac.Operlog;

namespace Yi.Infra.Rbac.Dtos.OperLog;

public class OperationLogGetListInputVo : PagedAllResultRequestDto
{
    public OperEnum? OperType { get; set; }
    public string? OperUser { get; set; }
}