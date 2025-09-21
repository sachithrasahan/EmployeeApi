using AutoMapper;
using EmployeeApi.Models;

namespace EmployeeApi.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Employee, EmployeeResponseDto>();
            CreateMap<EmployeeRequestDto, Employee>();
        }
    }
}
