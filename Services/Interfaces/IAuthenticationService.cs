using SchoolManagement.DTOs.Request;
using SchoolManagement.DTOs.Response;

namespace SchoolManagement.Services.Interfaces;

public interface IAuthenticationService
{
    Task<AuthResponse?> RegisterAsync(RegisterRequest request);
    Task<AuthResponse?> LoginAsync(LoginRequest request);
    Task<bool> ValidateTokenAsync(string token);
}
