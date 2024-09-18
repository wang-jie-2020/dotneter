namespace Yi.System.OperationLogging.Dtos;

public class OperationLogGetListInput : PagedInfraInput
{
    public OperationEnum? OperType { get; set; }
    
    public string? OperUser { get; set; }
}