using AuthenticationService.IServices;
using AuthenticationService.Models;
using Microsoft.AspNetCore.Mvc;

namespace AuthenticationService.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync(LoginModel login)
        {
            var response = await _authService.LoginAsync(login);
            if (!response.Success) return Unauthorized(response);

            return Ok(response);
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync(RegisterModel register)
        {
            var response = await _authService.RegisterAsync(register);
            if (!response.Success) return BadRequest(response);

            return Ok(response);
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshTokenAsync(RefreshTokenModel refresh)
        {
            var response = await _authService.RefreshTokenAsync(refresh);
            if (!response.Success) return Unauthorized(response);

            return Ok(response);
        }

        [HttpPost("logout")]
        public async Task<IActionResult> LogoutAsync()
        {
            var email = User.Identity?.Name;
            if(email is null) return BadRequest("Invalid request!");

            var response = await _authService.LogoutAsync(email);

            if (!response.Success) return Unauthorized(response);

            return Ok(response);
        }
    }
}