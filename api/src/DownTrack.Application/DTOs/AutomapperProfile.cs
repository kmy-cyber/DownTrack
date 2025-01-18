
using AutoMapper;
using DownTrack.Application.DTO.Authentication;
using DownTrack.Domain.Entities;

namespace DownTrack.Application.DTO;

public class AutomapperProfile : Profile
{
    public AutomapperProfile()
    {
        CreateMap<TechnicianDto, Technician>();
        CreateMap<Technician, TechnicianDto>();


        CreateMap<EmployeeDto, Employee>();
        CreateMap<Employee, EmployeeDto>();

        CreateMap<LoginUserDto, User>();
        CreateMap<RegisterUserDto, User>();

        CreateMap<RegisterUserDto, TechnicianDto>();
        CreateMap<RegisterUserDto, EmployeeDto>();


        CreateMap<EquipmentDto, Equipment>();
        CreateMap<Equipment, EquipmentDto>();
        CreateMap<SectionDto, Section>();
        CreateMap<Section, SectionDto>();
        CreateMap<MaintenanceDto, Maintenance>();
        CreateMap<Maintenance, MaintenanceDto>();

        CreateMap<DepartmentDto, Department>();
        CreateMap<Department, DepartmentDto>();

        CreateMap<TransferRequestDto, TransferRequest>();
        CreateMap<TransferRequest, TransferRequestDto>();

        CreateMap<TransferDto, Transfer>()
           .ForMember(dest => dest.RequestId, opt => opt.MapFrom(src => src.RequestId))
           .ForMember(dest => dest.ShippingSupervisorId, opt => opt.MapFrom(src => src.ShippingSupervisorId))
           .ForMember(dest => dest.EquipmentReceptorId, opt => opt.MapFrom(src => src.EquipmentReceptorId))
           .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.Date));

        CreateMap<Transfer, TransferDto>();
        CreateMap<Transfer, TransferDto>();

    }
}
