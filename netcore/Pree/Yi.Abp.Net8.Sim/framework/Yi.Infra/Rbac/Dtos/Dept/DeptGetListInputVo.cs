using Yi.Framework.Ddd.Application;

namespace Yi.Infra.Rbac.Dtos.Dept;

public class DeptGetListInputVo : PagedRequestInput
{
    public Guid Id { get; set; }
    public bool? State { get; set; }
    public string? DeptName { get; set; }
    public string? DeptCode { get; set; }
    public string? Leader { get; set; }
}