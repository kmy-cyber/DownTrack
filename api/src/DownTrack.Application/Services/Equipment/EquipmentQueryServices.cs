using System.Linq.Expressions;
using AutoMapper;
using DownTrack.Application.DTO;
using DownTrack.Application.DTO.Paged;
using DownTrack.Application.IServices;
using DownTrack.Application.IUnitOfWorkPattern;
using DownTrack.Domain.Entities;

namespace DownTrack.Application.Services;

public class EquipmentQueryServices : GenericQueryServices<Equipment, GetEquipmentDto>, 
                                      IEquipmentQueryServices
{
    private static readonly Expression<Func<Equipment, object>>[] includes = 
                            { d => d.Department,
                              d => d.Department.Section };
    public EquipmentQueryServices(IUnitOfWork unitOfWork, IMapper mapper)
        : base(unitOfWork, mapper)
    {

    }

    public override Expression<Func<Equipment, object>>[] GetIncludes()=> includes;


    public async Task<PagedResultDto<GetEquipmentDto>> GetPagedEquipmentsBySectionManagerIdAsync(PagedRequestDto paged,
                                                                                                int sectionManagerId)
    {
        //check sectionManagerId es valid

        var checkSectionManager = await _unitOfWork.GetRepository<Employee>()
                                             .GetByIdAsync(sectionManagerId);

        if (checkSectionManager.UserRole != "SectionManager")
            throw new Exception($"SectionManager with Id : {sectionManagerId} not exists");


        var queryEquipment = _unitOfWork.GetRepository<Equipment>()
                                         .GetAllByItems(e => e.Department.Section.SectionManagerId == sectionManagerId);
         
        return await GetPagedResultByQueryAsync(paged,queryEquipment);

    }

    public async Task<PagedResultDto<GetEquipmentDto>> GetPagedEquipmentsBySectionIdAsync(PagedRequestDto paged
                                                                                            , int sectionId)
    {
        //check section exist

        var checkSection = await _unitOfWork.GetRepository<Section>()
                                             .GetByIdAsync(sectionId);


        var queryEquipment = _unitOfWork.GetRepository<Equipment>()
                                         .GetAllByItems(e => e.Department.SectionId == sectionId);
                                         
        return await GetPagedResultByQueryAsync(paged,queryEquipment);
    }

    public async Task<PagedResultDto<GetEquipmentDto>> GetPagedEquipmentsByDepartmentIdAsync(PagedRequestDto paged,
                                                                                            int departmentId)
    {
        // check the department exist
        var checkDepartment = await _unitOfWork.DepartmentRepository
                                             .GetByIdAsync(departmentId);

        var queryEquipment = _unitOfWork.GetRepository<Equipment>()
                                         .GetAllByItems(e => e.DepartmentId == departmentId);
        
        return await GetPagedResultByQueryAsync(paged,queryEquipment);
    }

}