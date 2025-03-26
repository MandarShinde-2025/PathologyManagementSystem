using AuthenticationService.Models;

namespace AuthenticationService.IServices;

public interface IAuthService
{
    Task<AuthResponse> LoginAsync(LoginModel login);
    Task<RegisterResponse> RegisterAsync(RegisterModel register);
    Task<AuthResponse> RefreshTokenAsync(RefreshTokenModel refresh);
    Task<AuthResponse> LogoutAsync(string email);
}