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
        private readonly IMapper _mapper;
        private readonly IJwtService _jwtService;

        public AuthService(
            IAuthRepository authRepository, 
            IEmployeeRepository employeeRepository, 
            IMapper mapper,
            IJwtService jwtService)
        {
            _authRepository = authRepository;
            _employeeRepository = employeeRepository;
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
            
            return new AuthResponseDto
            {
                AccessToken = token,
                Role = user.Role,
                EmployeeCode = user.EmployeeCode
            };
        }
    }
}
