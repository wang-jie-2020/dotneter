using Yi.AspNetCore.System.Loggings;

namespace Yi.Admin.Services.Monitor.Dtos;

public class OperLogDto 
{
    public Guid Id { get; set; }
    
    public string? Title { get; set; }
    
    public OperLogEnum OperType { get; set; }
    
    public string? RequestMethod { get; set; }
    
    public string? OperUser { get; set; }
    
    public string? OperIp { get; set; }
    
    public string? OperLocation { get; set; }
    
    public string? Method { get; set; }
    
    public string? RequestParam { get; set; }
    
    public string? RequestResult { get; set; }
    
    public DateTime CreationTime { get; set; }
}