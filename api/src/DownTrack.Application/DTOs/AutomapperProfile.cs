


using AutoMapper;
using DownTrack.Domain.Entities;

namespace DownTrack.Application.DTO;

public class AutomapperProfile : Profile
{
    public AutomapperProfile()
    {
        CreateMap<TechnicianDto, Technician>();
        CreateMap<Technician,TechnicianDto>();

        CreateMap<UserDto,User>();
        CreateMap<User,UserDto>();
    }
}