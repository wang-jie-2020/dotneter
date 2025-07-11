﻿namespace Yi.System.Services.Dtos;

public class LoginInput
{
    public string UserName { get; set; } = string.Empty;
    
    public string Password { get; set; } = string.Empty;

    public string? Uuid { get; set; }

    public string? Code { get; set; }
}