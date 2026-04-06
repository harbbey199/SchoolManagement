using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolManagement.Data;
using SchoolManagement.DTOs.Request;
using SchoolManagement.DTOs.Response;
using SchoolManagement.Services.Interfaces;

namespace SchoolManagement.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthenticationService _authService;
    private readonly ApplicationDbContext _context;
    public AuthController(IAuthenticationService authService, ApplicationDbContext context)
    {
        _authService = authService;
        _context = context;
    }

    /// <summary>
    /// Register a new user
    /// </summary>
    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<ActionResult<ApiResponse<AuthResponse>>> Register([FromBody] RegisterRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ApiResponse<AuthResponse>.ErrorResponse("Invalid input"));
        var result = await _authService.RegisterAsync(request);
        if (result == null)
            return BadRequest(ApiResponse<AuthResponse>.ErrorResponse("Email already exists"));

        return Ok(ApiResponse<AuthResponse>.SuccessResponse(result, "Registration successful"));
    }

    /// <summary>
    /// Login user with email and password
    /// </summary>
    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<ActionResult<ApiResponse<AuthResponse>>> Login([FromBody] LoginRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ApiResponse<AuthResponse>.ErrorResponse("Invalid input"));

        var result = await _authService.LoginAsync(request);
        if (result == null)
            return Unauthorized(ApiResponse<AuthResponse>.ErrorResponse("Invalid credentials"));

        return Ok(ApiResponse<AuthResponse>.SuccessResponse(result, "Login successful"));
    }
}
