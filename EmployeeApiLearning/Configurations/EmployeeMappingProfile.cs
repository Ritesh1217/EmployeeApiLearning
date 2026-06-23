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
        }
    }
}
