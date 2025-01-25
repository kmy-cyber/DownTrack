
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

        CreateMap<EquipmentReceptorDto, EquipmentReceptor>();
        CreateMap<EquipmentReceptor, EquipmentReceptorDto>()
            .ForMember(dest => dest.SectionId, opt=> opt.MapFrom(src => src.Department.SectionId));
  

        CreateMap<LoginUserDto, User>();
        CreateMap<RegisterUserDto, User>();

        CreateMap<RegisterUserDto, Technician>();
        CreateMap<RegisterUserDto, Employee>();
        CreateMap<RegisterUserDto, EquipmentReceptor>();

        CreateMap<UpdateUserDto, Technician>();
        CreateMap<UpdateUserDto, Employee>();
        CreateMap<UpdateUserDto, EquipmentReceptor>();

        CreateMap<EquipmentDto, Equipment>();
        CreateMap<Equipment, EquipmentDto>()
            .ForMember(dest => dest.SectionId, opt=> opt.MapFrom(src => src.Department.SectionId));
  

        CreateMap<SectionDto, Section>();
        CreateMap<Section, SectionDto>();

        CreateMap<DoneMaintenanceDto, DoneMaintenance>();
        CreateMap<DoneMaintenance, DoneMaintenanceDto>();

        CreateMap<EvaluationDto, Evaluation>();
        CreateMap<Evaluation, EvaluationDto>();

        CreateMap<TransferRequestDto, TransferRequest>();
        CreateMap<TransferRequest, TransferRequestDto>();

        CreateMap<TransferDto, Transfer>();
        CreateMap<Transfer, TransferDto>();
        
        CreateMap<Employee, GetEmployeeDto>()
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User!.UserName))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.User!.Email));

        CreateMap<DepartmentDto, Department>();
        CreateMap<Department, DepartmentDto>()
            .ForMember(dest => dest.SectionName, opt=> opt.MapFrom(src => src.Section.Name));
  

        CreateMap<EquipmentDecommissioningDto,EquipmentDecommissioning>();
        CreateMap<EquipmentDecommissioning,EquipmentDecommissioningDto>();

    }
}
