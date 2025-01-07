
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

        CreateMap<RegisterUserDto,Technician>();
        CreateMap<RegisterUserDto,Employee>();
        CreateMap<RegisterUserDto,EquipmentReceptor>();

        CreateMap<EquipmentDto, Equipment>();
        CreateMap<Equipment, EquipmentDto>();

        CreateMap<SectionDto, Section>();
        CreateMap<Section, SectionDto>();

        CreateMap<MaintenanceDto, Maintenance>();
        CreateMap<Maintenance, MaintenanceDto>();

        CreateMap<DepartmentDto, Department>();
        CreateMap<Department, DepartmentDto>();

        CreateMap<EvaluationDto,Evaluation>();
        CreateMap<Evaluation,EvaluationDto>();

        CreateMap<EquipmentReceptorDto,EquipmentReceptor>();
        CreateMap<EquipmentReceptor,EquipmentReceptorDto>();

    }
}
