﻿using SqlSugar;
using Yi.Framework.SqlSugarCore;

namespace Yi.Framework.Core.Entities;

/// <summary>
///     角色菜单关系表
/// </summary>
[SugarTable("Sys_RoleMenu")]
public class RoleMenuEntity : Entity<long>
{
    /// <summary>
    /// </summary>
    [SugarColumn(ColumnName = "RoleId")]
    public Guid RoleId { get; set; }

    /// <summary>
    /// </summary>
    [SugarColumn(ColumnName = "MenuId")]
    public Guid MenuId { get; set; }
}