
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
        CreateMap<Technician,GetTechnicianDto>()
            .ForMember(dest=> dest.UserName , opt=> opt.MapFrom(src=> src.User!.UserName));

        CreateMap<EmployeeDto, Employee>();
        CreateMap<Employee, EmployeeDto>();
        CreateMap<Employee, GetEmployeeDto>()
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User!.UserName))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.User!.Email));

        CreateMap<EquipmentReceptorDto, EquipmentReceptor>();
        CreateMap<EquipmentReceptor, EquipmentReceptorDto>()
            .ForMember(dest => dest.SectionId, opt=> opt.MapFrom(src => src.Department.SectionId));
        CreateMap<EquipmentReceptor, GetEquipmentReceptorDto>()
            .ForMember(dest => dest.SectionId, opt=> opt.MapFrom(src => src.Department.SectionId))
            .ForMember(dest=> dest.DepartmentName , opt=> opt.MapFrom(src=> src.Department.Name))
            .ForMember(dest=> dest.SectionName , opt=> opt.MapFrom(src=> src.Department.Section.Name))
            .ForMember(dest=> dest.UserName , opt=> opt.MapFrom(src=> src.User!.UserName));


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
        CreateMap<Equipment, GetEquipmentDto>()
            .ForMember(dest => dest.SectionId, opt=> opt.MapFrom(src => src.Department.SectionId))
            .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src=> src.Department.Name))
            .ForMember(dest => dest.SectionName, opt => opt.MapFrom(src=> src.Department.Section.Name));
        

        CreateMap<SectionDto, Section>();
        CreateMap<Section, SectionDto>();
        CreateMap<Section, GetSectionDto>()
            .ForMember(dest => dest.SectionManagerUserName, opt => opt.MapFrom(src=> src.SectionManager.User!.UserName));

        CreateMap<DoneMaintenanceDto, DoneMaintenance>();
        CreateMap<DoneMaintenance, DoneMaintenanceDto>();
        CreateMap<DoneMaintenance, GetDoneMaintenanceDto>()
            .ForMember(dest => dest.TechnicianUserName, opt => opt.MapFrom(src=> src.Technician!.User!.UserName));



        CreateMap<EvaluationDto, Evaluation>();
        CreateMap<Evaluation, EvaluationDto>();
        CreateMap<Evaluation,GetEvaluationDto>()
            .ForMember(dest => dest.SectionManagerUserName, opt => opt.MapFrom(src=> src.SectionManager!.User!.UserName))
            .ForMember(dest => dest.TechnicianUserName, opt => opt.MapFrom(src=> src.Technician.User!.UserName));


        CreateMap<TransferRequestDto, TransferRequest>();
        CreateMap<TransferRequest, TransferRequestDto>();
        CreateMap<TransferRequest,GetTransferRequestDto>()
            .ForMember(dest => dest.SectionManagerUserName, opt => opt.MapFrom(src=> src.SectionManager!.User!.UserName))
            .ForMember(dest => dest.ArrivalDepartmentName, opt => opt.MapFrom(src=> src.ArrivalDepartment.Name))
            .ForMember(dest => dest.ArrivalSectionId, opt => opt.MapFrom(src=> src.ArrivalDepartment.SectionId))
            .ForMember(dest => dest.ArrivalSectionName, opt => opt.MapFrom(src=> src.ArrivalDepartment.Section.Name));


        CreateMap<TransferDto, Transfer>();
        CreateMap<Transfer, TransferDto>();
        CreateMap<Transfer, GetTransferDto>()
            .ForMember(dest => dest.ShippingSupervisorName, opt => opt.MapFrom(src=> src.ShippingSupervisor!.Name))
            .ForMember(dest => dest.EquipmentReceptorUserName, opt => opt.MapFrom(src=> src.EquipmentReceptor!.User!.UserName));
        
        CreateMap<DepartmentDto, Department>();
        CreateMap<Department, DepartmentDto>()
            .ForMember(dest => dest.SectionName, opt=> opt.MapFrom(src => src.Section.Name));
        CreateMap<Department,GetDepartmentDto>()
            .ForMember(dest => dest.SectionName, opt=> opt.MapFrom(src => src.Section.Name));

        CreateMap<EquipmentDecommissioningDto,EquipmentDecommissioning>();
        CreateMap<EquipmentDecommissioning,EquipmentDecommissioningDto>();
        CreateMap<EquipmentDecommissioning,GetEquipmentDecommissioningDto>()
            .ForMember(dest => dest.TechnicianUserName, opt=> opt.MapFrom(src => src.Technician!.User!.UserName))
            .ForMember(dest => dest.ReceptorUserName, opt=> opt.MapFrom(src => src.Receptor!.User!.UserName));

    }
}
