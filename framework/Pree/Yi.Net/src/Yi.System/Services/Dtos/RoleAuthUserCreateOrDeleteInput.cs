﻿using System.ComponentModel.DataAnnotations;

namespace Yi.System.Services.Dtos;

public class RoleAuthUserCreateOrDeleteInput
{
    [Required] public Guid RoleId { get; set; }

    [Required] public List<Guid> UserIds { get; set; }
}