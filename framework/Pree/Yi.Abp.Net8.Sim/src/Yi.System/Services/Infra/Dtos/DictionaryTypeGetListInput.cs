using Yi.AspNetCore.System;

namespace Yi.Sys.Services.Infra.Dtos;

public class DictionaryTypeGetListInput : PagedInput
{
    public string? DictName { get; set; }
    
    public string? DictType { get; set; }
    
    public string? Remark { get; set; }

    public bool? State { get; set; }
}