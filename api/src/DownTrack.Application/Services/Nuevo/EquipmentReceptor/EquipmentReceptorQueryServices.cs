
using AutoMapper;
using DownTrack.Application.DTO;
using DownTrack.Application.DTO.Paged;
using DownTrack.Application.IServices;
using DownTrack.Application.IUnitOfWorkPattern;
using DownTrack.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DownTrack.Application.Services;

public class EquipmentReceptorQueryServices : IEquipmentReceptorQueryServices
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public EquipmentReceptorQueryServices(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<GetEquipmentReceptorDto>> ListAsync()
    {
        var equipmentReceptor = await _unitOfWork.GetRepository<EquipmentReceptor>()
                                                 .GetAll()
                                                 .Include(er => er.User)
                                                 .Include(er => er.Department)
                                                 .Include(er => er.Department.Section)
                                                 .ToListAsync();

        return equipmentReceptor.Select(_mapper.Map<GetEquipmentReceptorDto>);
    }

    /// <summary>
    /// Retrieves a equipmentReceptor by their ID
    /// </summary>
    /// <param name="equipmentReceptorDto">The equipmentReceptor's ID to retrieve</param>
    /// <returns>A Task representing the asynchronous operation that fetches the equipmentReceptor</returns>
    public async Task<GetEquipmentReceptorDto> GetByIdAsync(int equipmentReceptorDto)
    {

        var result = await _unitOfWork.GetRepository<EquipmentReceptor>()
                                        .GetByIdAsync(
                                            equipmentReceptorDto, default,
                                            er => er.Department,
                                            er => er.User!,
                                            er => er.Department.Section);

        return _mapper.Map<GetEquipmentReceptorDto>(result);

    }


    public async Task<PagedResultDto<GetEquipmentReceptorDto>> GetPagedResultAsync(PagedRequestDto paged)
    {
        //The queryable collection of entities to paginate
        IQueryable<EquipmentReceptor> queryEquipmentReceptor = _unitOfWork.GetRepository<EquipmentReceptor>()
                                                                          .GetAll()
                                                                          .Include(er => er.User)
                                                                          .Include(er => er.Department)
                                                                          .Include(er => er.Department.Section)
                                                                          .Include(er => er.Department);

        var totalCount = await queryEquipmentReceptor.CountAsync();


        var items = await queryEquipmentReceptor // Apply pagination to the query.
                        .Skip((paged.PageNumber - 1) * paged.PageSize) // Skip the appropriate number of items based on the current page
                        .Take(paged.PageSize) // Take only the number of items specified by the page size.
                        .ToListAsync(); // Convert the result to a list asynchronously.


        return new PagedResultDto<GetEquipmentReceptorDto>
        {
            Items = items?.Select(_mapper.Map<GetEquipmentReceptorDto>) ?? Enumerable.Empty<GetEquipmentReceptorDto>(),
            TotalCount = totalCount,
            PageNumber = paged.PageNumber,
            PageSize = paged.PageSize,
            NextPageUrl = paged.PageNumber * paged.PageSize < totalCount
                        ? $"{paged.BaseUrl}?pageNumber={paged.PageNumber + 1}&pageSize={paged.PageSize}"
                        : null,
            PreviousPageUrl = paged.PageNumber > 1
                        ? $"{paged.BaseUrl}?pageNumber={paged.PageNumber - 1}&pageSize={paged.PageSize}"
                        : null

        };
    }



}