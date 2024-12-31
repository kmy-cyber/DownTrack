


using AutoMapper;
using DownTrack.Application.DTO.Authentication;
using DownTrack.Domain.Entities;

namespace DownTrack.Application.DTO;

public class AutomapperProfile : Profile
{
    public AutomapperProfile()
    {
        CreateMap<TechnicianDto, Technician>();
        CreateMap<Technician,TechnicianDto>();

        CreateMap<EmployeeDto,Employee>();
        CreateMap<Employee,EmployeeDto>();

        CreateMap<LoginUserDto,User>();
        CreateMap<RegisterUserDto,User>();
        
    }
}