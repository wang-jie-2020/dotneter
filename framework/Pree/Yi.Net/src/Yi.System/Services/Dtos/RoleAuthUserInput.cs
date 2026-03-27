using System.ComponentModel.DataAnnotations;

namespace Yi.System.Services.Dtos;

public class RoleAuthUserInput
{
    [Required] public long RoleId { get; set; }

    [Required] public List<long> UserIds { get; set; }
}