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
            return Ok(result);
        }
    }
}
