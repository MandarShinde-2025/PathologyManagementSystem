using Microsoft.AspNetCore.Identity;

namespace AuthenticationService.Models;

public class ApplicationUser: IdentityUser
{
    public string? FullName { get; set; }
    public string? Role { get; set; } = "Patient"; // User can be Admin / Patient / Doctor / Collector
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiryTime { get; set; }
}