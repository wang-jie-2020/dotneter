namespace Yi.System.Services.Dtos;

public class LanguageDto
{
    public long Id { get; set; }
    
    public DateTime CreationTime { get; set; } = DateTime.Now;
    
    public Guid? CreatorId { get; set; }
    
    public string Name { get; set; } = string.Empty;
    
    public string Value { get; set; } = string.Empty;
    
    public string Culture { get; set; } = string.Empty;
}
