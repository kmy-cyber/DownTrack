
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


        CreateMap<RegisterUserDto, Technician>();
        CreateMap<RegisterUserDto, Employee>();
        CreateMap<RegisterUserDto, EquipmentReceptor>();



        CreateMap<EquipmentDto, Equipment>();
        CreateMap<Equipment, EquipmentDto>();

        CreateMap<SectionDto, Section>();
        CreateMap<Section, SectionDto>();

        CreateMap<DoneMaintenanceDto, DoneMaintenance>();
        CreateMap<DoneMaintenance, DoneMaintenanceDto>();


        CreateMap<DepartmentDto, Department>();
        CreateMap<Department, DepartmentDto>();

        CreateMap<EvaluationDto, Evaluation>();
        CreateMap<Evaluation, EvaluationDto>();

        CreateMap<EquipmentReceptorDto, EquipmentReceptor>();
        CreateMap<EquipmentReceptor, EquipmentReceptorDto>();

        CreateMap<UpdateUserDto, Technician>();
        CreateMap<UpdateUserDto, Employee>();
        CreateMap<UpdateUserDto, EquipmentReceptor>();



        CreateMap<Employee, GetEmployeeDto>()
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User!.UserName))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.User!.Email))
            .ForMember(dest => dest.UserRole, opt => opt.MapFrom(src => src.UserRole));

        CreateMap<Department, DepartmentDto>()
            .ForMember(dest => dest.SectionName, opt=> opt.MapFrom(src => src.Section.Name));


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
