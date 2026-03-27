namespace Yi.System.Services.Dtos;

public class LanguageQuery : PagedQuery
{
    public string? Name { get; set; }
    
    public string? Value { get; set; }
    
    public string? Culture { get; set; }
}
