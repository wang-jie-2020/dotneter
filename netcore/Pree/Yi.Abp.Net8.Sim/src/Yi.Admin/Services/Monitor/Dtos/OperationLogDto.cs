using Yi.AspNetCore.System.Logging;

namespace Yi.Admin.Services.Monitor.Dtos;

public class OperationLogDto 
{
    public Guid Id { get; set; }
    
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