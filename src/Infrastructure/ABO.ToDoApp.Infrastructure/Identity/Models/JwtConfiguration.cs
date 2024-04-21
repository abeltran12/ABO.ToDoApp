﻿namespace ABO.ToDoApp.Infrastructure.Identity.Models;

public class JwtConfiguration
{
    public string Section { get; set; } = "JwtSettings";
    public string? Key { get; set; }
    public string? Issuer { get; set; }
    public string? Audience { get; set; }
    public string? Expires { get; set; }
}
