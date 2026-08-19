using AutoMapper;
using EmployeeApiLearning.DTO;
using EmployeeApiLearning.Models;

namespace EmployeeApiLearning.Configurations
{
    public class EmployeeMappingProfile : Profile
    {
        public EmployeeMappingProfile() 
        {
            CreateMap<Employee, EmployeeResponseDto>();

            CreateMap<EmployeeDto, Employee>();

            CreateMap<RegisterDto, AppUser>()
                .ForMember(
                    destination => destination.PasswordHash,
                    options => options.Ignore())
                .ForMember(
                    destination => destination.Role,
                    options => options.Ignore());
        }
    }
}
