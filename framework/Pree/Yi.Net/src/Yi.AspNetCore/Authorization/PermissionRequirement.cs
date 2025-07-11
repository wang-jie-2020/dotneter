﻿using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Authorization;

namespace Yi.AspNetCore.Authorization;

public class PermissionRequirement: IAuthorizationRequirement
{
    public string PermissionName { get; }

    public PermissionRequirement([NotNull] string permissionName)
    {
        Check.NotNull(permissionName, nameof(permissionName));

        PermissionName = permissionName;
    }

    public override string ToString()
    {
        return $"PermissionRequirement: {PermissionName}";
    }
}