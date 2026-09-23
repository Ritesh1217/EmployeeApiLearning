using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;

namespace EmployeeApiLearning.Services
{
    public class JwtService : IJwtService
    {
        private readonly IConfiguration _configuration;

        public JwtService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateToken(string username, string role, string employeeCode)
        {
            var key = _configuration["Jwt:Key"] ?? throw new InvalidOperationException("JWT key is missing");

            var issuer = _configuration["Jwt:Issuer"];
            var audience = _configuration["Jwt:Audience"];

            var expiryMinutes = int.Parse(_configuration["Jwt:ExpiryMinutes"] ?? "60");

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Role, role),
                new Claim("EmployeeCode", employeeCode)
            };

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer:  issuer,
                audience: audience,
                claims:  claims,
                expires: DateTime.UtcNow.AddMinutes(expiryMinutes),
                signingCredentials: credentials
             );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
