using EmployeeApiLearning.DTO;
using EmployeeApiLearning.Services;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeApiLearning.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            this._authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            var success = await _authService.Register(registerDto);

            if (!success)
            {
                return BadRequest(new
                {
                    message = "registration failed"
                });
            }
            return Ok(new
            {
                message = "User registered successfully"
            });
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto login)
        {
            var result = await _authService.Login(login);

            if(result == null)
            {
                return Unauthorized(new
                {
                    message = "Invalid username or password"
                });
            }
            SetRefreshTokenCookie(result.RefreshToken);
            return Ok(result);
        }
        private void SetRefreshTokenCookie(string token)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true, // Kyunki aap https://localhost:7001 use kar rahe hain
                SameSite = SameSiteMode.Lax, // Localhost par Lax 100% save hota hai
                Expires = DateTimeOffset.UtcNow.AddDays(7),
                Path = "/" // Pure domain pe accessible banayein
            };

            Response.Cookies.Append("refreshToken", token, cookieOptions);
        }
    }
}
