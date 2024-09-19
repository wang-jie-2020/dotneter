using Volo.Abp.Application.Dtos;
using Yi.AspNetCore.Common;

namespace Yi.Admin.Services.OperationLogging.Dtos;

public class OperationLogDto : EntityDto<Guid>
{
    public string? Title { get; set; }
    
    public OperationLogEnum OperType { get; set; }
    
    public string? RequestMethod { get; set; }
    
    public string? OperUser { get; set; }
    
    public string? OperIp { get; set; }
    
    public string? OperLocation { get; set; }
    
    public string? Method { get; set; }
    
    public string? RequestParam { get; set; }
    
    public string? RequestResult { get; set; }
    
    public DateTime CreationTime { get; set; }
}