namespace Yi.System.Services.Dtos;

public class DictionaryQuery : PagedQuery
{
    public string? DictType { get; set; }
    
    public string? DictLabel { get; set; }
    
    public bool? State { get; set; }
}