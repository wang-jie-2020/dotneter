﻿namespace Yi.System.TenantManagement.Dtos;

public class TenantUpdateInput
{
    public string? Name { get; set; }
    
    public int? EntityVersion { get; set; }

    public string? TenantConnectionString { get; set; }

    public DbType? DbType { get; set; }
}