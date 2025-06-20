using Yi.AspNetCore.Core;
using Yi.Framework.Core;

namespace Yi.System.Services.Dtos;

public class DictionaryGetListInput : PagedInput
{
    public string? DictType { get; set; }
    
    public string? DictLabel { get; set; }
    
    public bool? State { get; set; }
}