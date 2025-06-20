using Yi.Framework.Loggings;

namespace Yi.System.Monitor.Dtos;

public class OperLogDto
{
    public long Id { get; set; }

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