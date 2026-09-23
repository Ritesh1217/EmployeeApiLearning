using System.Security.Cryptography;
using AutoMapper;
using EmployeeApiLearning.DTO;
using EmployeeApiLearning.Models;
using EmployeeApiLearning.Repositories;

namespace EmployeeApiLearning.Services
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _authRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IMapper _mapper;
        private readonly IJwtService _jwtService;

        public AuthService(
            IAuthRepository authRepository, 
            IEmployeeRepository employeeRepository, 
            IRefreshTokenRepository refreshTokenRepository,
            IMapper mapper,
            IJwtService jwtService)
        {
            _authRepository = authRepository;
            _employeeRepository = employeeRepository;
            _refreshTokenRepository = refreshTokenRepository;
            _mapper = mapper;
            _jwtService = jwtService;
        }

        public async Task<bool> Register(RegisterDto registerDto)
        {
            //checking user name already exist or not
            var existingUser = await _authRepository.GetUserByUsername(registerDto.Username);

            if (existingUser != null)
                return false;

            //checking employee is already exist or not
            var employee = await _employeeRepository.GetEmployeeByCode(registerDto.EmployeeCode);

            if (employee == null) 
                return false;

            //employee name must have to match username
            if (!employee.Name.Equals(registerDto.Username, StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            //checking employee has already userAccount
            var employeeUser = await _authRepository.GetUserByEmployeeCode(registerDto.EmployeeCode);

            if (employeeUser != null)
                return false;

            //convert RegisterDto into AppUser
            var user = _mapper.Map<AppUser>(registerDto);

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password);

            user.Role = "user";

            //save user in database
            await _authRepository.AddUser(user);

            return true;
        }

        public async Task<AuthResponseDto?> Login(LoginDto loginDto)
        {
            var user = await _authRepository.GetUserByUsername(loginDto.Username);

            if(user == null) 
                return null;

            bool passwordValid = BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash);

            if(!passwordValid)
                return null;

            var token = _jwtService.GenerateToken(
                loginDto.Username,
                user.Role,
                user.EmployeeCode);

            var refreshToken = await GenerateAndSaveRefreshToken(user.Username);
            return new AuthResponseDto
            {
                AccessToken = token,
                RefreshToken = refreshToken,
                Role = user.Role,
                EmployeeCode = user.EmployeeCode
            };
        }

        public async Task<string> GenerateAndSaveRefreshToken(string username)
        {
            var randomBytes = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomBytes);
            var tokenString = Convert.ToBase64String(randomBytes);

            var refreshToken = new RefreshToken
            {
                Token = tokenString,
                Username = username,
                ExpiresAt = DateTime.UtcNow.AddDays(7),
                IsRevoked = false,
                CreatedAt = DateTime.UtcNow
            };

            await _refreshTokenRepository.AddAsync(refreshToken);
            await _refreshTokenRepository.SaveChangesAsync();

            return tokenString;
        }
        public async Task<AuthResponseDto?> Refresh(string refreshToken)
        {
            // 1. DB se check karo ki token valid hai ya nahi (exist karta ho, revoked na ho, expire na hua ho)
            var storedToken = await _refreshTokenRepository.GetValidTokenAsync(refreshToken);
            if (storedToken == null)
                return null;

            var user = await _authRepository.GetUserByUsername(storedToken.Username);
            if (user == null)
                return null;

            // 2. Token Rotation: Purane token ko invalidate (revoked) mark karo taaki dobara use na ho
            storedToken.IsRevoked = true;
            await _refreshTokenRepository.UpdateAsync(storedToken);

            // 3. Naye tokens generate karo
            var newAccessToken = _jwtService.GenerateToken(
                user.Username,
                user.Role,
                user.EmployeeCode);

            var newRefreshToken = await GenerateAndSaveRefreshToken(user.Username);
            await _refreshTokenRepository.SaveChangesAsync();

            return new AuthResponseDto
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken,
                Role = user.Role,
                EmployeeCode = user.EmployeeCode
            };
        }
        public async Task<bool> Revoke(string refreshToken)
        {
            var storedToken = await _refreshTokenRepository.GetValidTokenAsync(refreshToken);
            if (storedToken == null)
                return false;

            // Token ko database me revoke (cancel) karo
            storedToken.IsRevoked = true;
            await _refreshTokenRepository.UpdateAsync(storedToken);
            await _refreshTokenRepository.SaveChangesAsync();

            return true;
        }
    }
}
