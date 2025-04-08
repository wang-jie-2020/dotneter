using Yi.AspNetCore.System;

namespace Yi.Sys.Services.Infra.Dtos;

public class PostGetListInput : PagedInput
{
    public bool? State { get; set; }

    public string? PostCode { get; set; } = string.Empty;

    public string? PostName { get; set; } = string.Empty;
}