using Yi.AspNetCore.System;

namespace Yi.System.Services.System.Dtos;

public class PostGetListInput : PagedInput
{
    public bool? State { get; set; }

    public string? PostCode { get; set; } = string.Empty;

    public string? PostName { get; set; } = string.Empty;
}