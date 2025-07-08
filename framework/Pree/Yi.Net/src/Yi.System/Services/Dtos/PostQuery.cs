namespace Yi.System.Services.Dtos;

public class PostQuery : PagedQuery
{
    public bool? State { get; set; }

    public string? PostCode { get; set; } = string.Empty;

    public string? PostName { get; set; } = string.Empty;
}