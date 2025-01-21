
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

            
    }
}
