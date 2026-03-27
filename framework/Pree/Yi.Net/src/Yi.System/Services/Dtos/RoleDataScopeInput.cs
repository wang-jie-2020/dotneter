namespace Yi.System.Services.Dtos;

public class UpdateDataScopeInput
{
    public long RoleId { get; set; }

    public List<long>? DeptIds { get; set; }

    public DataScopeEnum DataScope { get; set; }
}