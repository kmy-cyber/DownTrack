using System.Linq.Expressions;
using AutoMapper;
using DownTrack.Application.DTO;
using DownTrack.Application.DTO.Paged;
using DownTrack.Application.Interfaces;
using DownTrack.Application.IServices;
using DownTrack.Application.IUnitOfWorkPattern;
using DownTrack.Domain.Entities;

namespace DownTrack.Application.Services;

public class SectionQueryServices : GenericQueryServices<Section, GetSectionDto>,
                                    ISectionQueryServices
{
    private static readonly Expression<Func<Section, object>>[] includes =
                            { d => d.SectionManager.User! };

    public SectionQueryServices(IUnitOfWork unitOfWork, IMapper mapper,
                                 IFilterService<Section> filterService,
                                 ISortService<Section> sortService,
                                 IPaginationService<Section> paginationService)
        : base(unitOfWork, filterService,sortService,paginationService,mapper)
    {

    }

    public override Expression<Func<Section, object>>[] GetIncludes() => includes;


    public async Task<PagedResultDto<GetSectionDto>> GetSectionsByManagerAsync(PagedRequestDto paged, int sectionManagerId)
    {
        //check if a valid section
        var section = await _unitOfWork.GetRepository<Employee>().GetByIdAsync(sectionManagerId);

        if (section.UserRole != "SectionManager")
            throw new Exception($"This SectionManager with Id : {sectionManagerId} not exist");

        //The queryable collection of entities to paginate
        IQueryable<Section> querySection = _unitOfWork.GetRepository<Section>()
                                                      .GetAllByItems(s => s.SectionManagerId == sectionManagerId);

        return await GetPagedResultByQueryAsync(paged, querySection);
    }

   
    public async Task<GetSectionDto> GetSectionByNameAsync(string sectionName)
    {
        var expressions = new Expression<Func<Section, bool>>[]
        {
            e=> e.Name == sectionName
        };

        var includes = GetIncludes();

        var section = await _unitOfWork.GetRepository<Section>()
                                 .GetByItems(expressions, includes);


        if (section == null)
            throw new Exception($"No section found with the username '{sectionName}'.");

        return _mapper.Map<GetSectionDto>(section);

    }

}