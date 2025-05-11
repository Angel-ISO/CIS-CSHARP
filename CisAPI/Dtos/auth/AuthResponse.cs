using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CisAPI.Dtos.auth;
public class AuthResponse
{
    public string Username { get; set; } = default!;
    public string Message { get; set; } = default!;
    public string Jwt { get; set; } = default!;
    public bool? Status { get; set; }  
}
