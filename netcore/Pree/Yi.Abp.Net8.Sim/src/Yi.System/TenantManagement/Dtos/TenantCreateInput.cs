﻿namespace Yi.System.TenantManagement.Dtos;

public class TenantCreateInput
{
    public string Name { get; set; }
    
    public string? TenantConnectionString { get; set; }

    public DbType DbType { get; set; }
}