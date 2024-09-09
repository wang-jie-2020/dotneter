using Yi.Infra.Rbac.Operlog;

namespace Yi.Infra.Rbac.Dtos.OperLog;

public class OperationLogGetListInputVo : PagedInfraInput
{
    public OperEnum? OperType { get; set; }
    public string? OperUser { get; set; }
}