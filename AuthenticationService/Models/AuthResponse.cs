namespace AuthenticationService.Models;

public class AuthResponse()
{
    public bool Success { get; set; }
    public string? Token { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime Expiration { get; set; }
    public object? Message { get; set; }
}