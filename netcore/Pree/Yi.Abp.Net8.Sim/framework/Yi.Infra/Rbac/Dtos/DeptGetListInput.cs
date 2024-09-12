namespace Yi.Infra.Rbac.Dtos;

public class DeptGetListInput : PagedInfraInput
{
    public Guid Id { get; set; }
    public bool? State { get; set; }
    public string? DeptName { get; set; }
    public string? DeptCode { get; set; }
    public string? Leader { get; set; }
}