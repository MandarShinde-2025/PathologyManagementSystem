namespace AuthenticationService.Models;

public class RegisterModel
{
    public string? FullName { get; set; }
    public string? Email { get; set; }
    public string? Password { get; set; }
    public string? Role { get; set; } = "Patient"; // User can be Admin / Patient / Doctor / Collector
}
