using Yi.AspNetCore.Core;
using Yi.Framework.Core;

namespace Yi.System.Services.Dtos;

public class PostGetListInput : PagedInput
{
    public bool? State { get; set; }

    public string? PostCode { get; set; } = string.Empty;

    public string? PostName { get; set; } = string.Empty;
}