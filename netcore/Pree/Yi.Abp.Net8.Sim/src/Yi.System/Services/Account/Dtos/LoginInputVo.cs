﻿namespace Yi.System.Services.Account.Dtos;

public class LoginInputVo
{
    public string UserName { get; set; } = string.Empty;
    
    public string Password { get; set; } = string.Empty;

    public string? Uuid { get; set; }

    public string? Code { get; set; }
}