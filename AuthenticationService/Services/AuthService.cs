using AuthenticationService.IServices;
using AuthenticationService.Models;
using CommonLibrary.Configurations;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace AuthenticationService.Services;

public class AuthService : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly JwtSettings _jwtSettings;

    public AuthService(UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        IConfiguration configuration,
        IOptions<JwtSettings> jwtSettings)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _jwtSettings = jwtSettings.Value;
    }

    public async Task<AuthResponse> LoginAsync(LoginModel model)
    {
        var user = await _userManager.FindByEmailAsync(model.Email!);
        if (user is null)
            return new AuthResponse { Success = false, Message = "Invalid credentials!" };

        var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password!, false);
        if (!result.Succeeded)
            return new AuthResponse { Success = false, Message = "Invalid credentials!" };

        // update user last login date
        user.RefreshToken = GetRefreshToken();
        user.RefreshTokenExpiryTime = DateTime.Now.AddDays(7);

        await _userManager.UpdateAsync(user);

        // Generate token
        var authResponse = GenerateJwtTokenAuthResponse(user);
        return authResponse;
    }

    public async Task<RegisterResponse> RegisterAsync(RegisterModel model)
    {
        var user = new ApplicationUser { UserName = model.Email, Email = model.Email, FullName = model.FullName, Role = model.Role };

        var result = await _userManager.CreateAsync(user, model.Password!);

        if (!result.Succeeded) return new RegisterResponse { Success = false, Message = result.Errors };

        return new RegisterResponse { Success = true, Message = "User registered successfully!" };
    }

    public async Task<AuthResponse> RefreshTokenAsync(LoginModel model)
    {
        var user = await _userManager.FindByEmailAsync(model.Email!);
        if (user is null)
            return new AuthResponse { Success = false, Message = "Invalid credentials!" };

        var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password!, false);
        if (!result.Succeeded)
            return new AuthResponse { Success = false, Message = "Invalid credentials!" };

        // Generate token
        return GenerateJwtTokenAuthResponse(user);
    }

    public async Task<AuthResponse> LogoutAsync(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user is null)
            return new AuthResponse { Success = false, Message = "Invalid request!" };

        user.RefreshToken = null;
        user.RefreshTokenExpiryTime = DateTime.Now;
        await _userManager.UpdateAsync(user);

        return new AuthResponse { Success = false, Message = "User logged out successfully!" };
    }

    public async Task<AuthResponse> RefreshTokenAsync(RefreshTokenModel refresh)
    {
        var principal = GetPrincipalFromExpiredToken(refresh.Token!);

        if (principal is null)
            return new AuthResponse { Success = false, Message = "Invalid token!" };

        var userEmail = principal.Claims.FirstOrDefault((c) => c.Type == ClaimTypes.Email)!.Value;
        if (userEmail is null)
            return new AuthResponse { Success = false, Message = "Invalid token!" };

        var user = await _userManager.FindByEmailAsync(userEmail);

        if (user is null || user.RefreshToken != refresh.RefreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
            return new AuthResponse { Success = false, Message = "Invalid token!" };

        var authResponse = GenerateJwtTokenAuthResponse(user);
        var refreshToken = GetRefreshToken();

        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiryTime = DateTime.Now.AddDays(7);
        await _userManager.UpdateAsync(user);

        return authResponse;
    }

    private AuthResponse GenerateJwtTokenAuthResponse(ApplicationUser user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, user.UserName!),
                new Claim(ClaimTypes.Email, user.Email!),
                new Claim("Role", user.Role!),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            }),
            Expires = DateTime.Now.AddMinutes(Convert.ToInt32(_jwtSettings.ExpireMinutes)),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret!)),
                SecurityAlgorithms.HmacSha256Signature)
        };

        var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret!));

        var token = tokenHandler.CreateToken(tokenDescriptor);

        var authResponse = new AuthResponse()
        {
            Success = true,
            Token = tokenHandler.WriteToken(token),
            RefreshToken = GetRefreshToken(),
            Expiration = DateTime.Now.AddMinutes(Convert.ToInt32(_jwtSettings.ExpireMinutes))
        };
        return authResponse;
    }

    private string GetRefreshToken()
    {
        var randomNumber = new byte[32];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
    }

    private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false,
            ValidateIssuer = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret!)),
            ValidateLifetime = false
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);
        if (!(securityToken is JwtSecurityToken jwtSecurityToken) ||
            !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            throw new SecurityTokenException("Invalid token");

        return principal;
    }
}