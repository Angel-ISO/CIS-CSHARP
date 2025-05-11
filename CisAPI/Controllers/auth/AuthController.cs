using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CisAPI.Dtos.auth;
using Microsoft.AspNetCore.Mvc;

namespace CisAPI.Controllers.auth;
public class AuthController: BaseApiController
{
 private readonly HttpClient _httpClient;
    private readonly string _javaAuthBaseUrl = "http://localhost:6969/api/auth"; 

    public AuthController(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient();
    }

    [HttpPost("signup")]
    public async Task<IActionResult> Signup([FromBody] AuthCreateUserRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync($"{_javaAuthBaseUrl}/signup", request);
        var content = await response.Content.ReadFromJsonAsync<AuthResponse>();

        return StatusCode((int)response.StatusCode, content);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] AuthLoginRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync($"{_javaAuthBaseUrl}/login", request);
        var content = await response.Content.ReadFromJsonAsync<AuthResponse>();

        return StatusCode((int)response.StatusCode, content);
    }
}

