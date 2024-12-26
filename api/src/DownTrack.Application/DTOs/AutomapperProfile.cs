


using AutoMapper;
using DownTrack.Domain.Enitites;

namespace DownTrack.Application.DTO;

public class AutomapperProfile : Profile
{
    public AutomapperProfile()
    {
        CreateMap<TechnicianDto, Technician>();
        CreateMap<Technician,TechnicianDto>();
    }
}