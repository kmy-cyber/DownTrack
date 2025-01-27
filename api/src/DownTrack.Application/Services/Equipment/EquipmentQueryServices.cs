using AutoMapper;
using DownTrack.Application.DTO;
using DownTrack.Application.DTO.Paged;
using DownTrack.Application.IServices;
using DownTrack.Application.IUnitOfWorkPattern;
using DownTrack.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DownTrack.Application.Services;

public class EquipmentQueryServices : IEquipmentQueryServices
{
    private readonly IMapper _mapper;

    private readonly IUnitOfWork _unitOfWork;

    public EquipmentQueryServices(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<GetEquipmentDto>> ListAsync()
    {
        var equipment = await _unitOfWork.GetRepository<Equipment>()
                                         .GetAll()
                                         .Include(e=> e.Department)
                                         .Include(e=> e.Department.Section)
                                         .ToListAsync();
        
        return equipment.Select(_mapper.Map<GetEquipmentDto>);
    }

    /// <summary>
    /// Retrieves an equipment by their ID
    /// </summary>
    /// <param name="equipmentDto">The equipment's ID to retrieve</param>
    /// <returns>A Task representing the asynchronous operation that fetches the equipment</returns>
    public async Task<GetEquipmentDto> GetByIdAsync(int equipmentDto)
    {
        var result = await _unitOfWork.GetRepository<Equipment>()
                                      .GetByIdAsync(equipmentDto,default,
                                                    e=> e.Department,
                                                    e=> e.Department.Section);
        
        /// and returns the updated equipment as an EquipmentDto.
        return _mapper.Map<GetEquipmentDto>(result);

    }



    public async Task<PagedResultDto<GetEquipmentDto>> GetPagedResultAsync(PagedRequestDto paged)
    {
        //The queryable collection of entities to paginate
        IQueryable<Equipment> queryEquipment = _unitOfWork.GetRepository<Equipment>()
                                                          .GetAll()
                                                          .Include(e=> e.Department)
                                                          .Include(e=> e.Department.Section);

        var totalCount = await queryEquipment.CountAsync();

        var items = await queryEquipment // Apply pagination to the query.
                        .Skip((paged.PageNumber - 1) * paged.PageSize) // Skip the appropriate number of items based on the current page
                        .Take(paged.PageSize) // Take only the number of items specified by the page size.
                        .ToListAsync(); // Convert the result to a list asynchronously.


        return new PagedResultDto<GetEquipmentDto>
        {
            Items = items?.Select(_mapper.Map<GetEquipmentDto>) ?? Enumerable.Empty<GetEquipmentDto>(),
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