using AutoMapper;
using DownTrack.Application.DTO;
using DownTrack.Application.DTO.Paged;
using DownTrack.Application.IServices;
using DownTrack.Application.IUnitOfWorkPattern;
using DownTrack.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DownTrack.Application.Services;

public class EquipmentServices : IEquipmentServices
{
    //private readonly IEquipmentRepository _equipmentRepository;
    private readonly IMapper _mapper;

    private readonly IUnitOfWork _unitOfWork;

    public EquipmentServices(IUnitOfWork unitOfWork, IMapper mapper)
    {
        // _equipmentRepository = equipmentRepository;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<EquipmentDto> CreateAsync(EquipmentDto dto)
    {
        var equipment = _mapper.Map<Equipment>(dto);

        //await _equipmentRepository.CreateAsync(equipment);
        
        await _unitOfWork.GetRepository<Equipment>().CreateAsync(equipment);

        await _unitOfWork.CompleteAsync();

        return _mapper.Map<EquipmentDto>(equipment);
    }

    public async Task DeleteAsync(int dto)
    {
        await _unitOfWork.GetRepository<Equipment>().DeleteByIdAsync(dto);

        await _unitOfWork.CompleteAsync();
       
    }

    public async Task<IEnumerable<EquipmentDto>> ListAsync()
    {
        var equipment = await _unitOfWork.GetRepository<Equipment>().GetAll().ToListAsync();
        
        return equipment.Select(_mapper.Map<EquipmentDto>);
    }

    public async Task<EquipmentDto> UpdateAsync(EquipmentDto dto)
    {
        var equipment = await _unitOfWork.GetRepository<Equipment>().GetByIdAsync(dto.Id);
        
        _mapper.Map(dto, equipment);

        _unitOfWork.GetRepository<Equipment>().Update(equipment);

        await _unitOfWork.CompleteAsync();
       
        return _mapper.Map<EquipmentDto>(equipment);
    }

    /// <summary>
    /// Retrieves an equipment by their ID
    /// </summary>
    /// <param name="equipmentDto">The equipment's ID to retrieve</param>
    /// <returns>A Task representing the asynchronous operation that fetches the equipment</returns>
    public async Task<EquipmentDto> GetByIdAsync(int equipmentDto)
    {
        var result = await _unitOfWork.GetRepository<Equipment>().GetByIdAsync(equipmentDto);
        
        /// and returns the updated equipment as an EquipmentDto.
        return _mapper.Map<EquipmentDto>(result);

    }



    public async Task<PagedResultDto<EquipmentDto>> GetPagedResultAsync(PagedRequestDto paged)
    {
        //The queryable collection of entities to paginate
        IQueryable<Equipment> queryEquipment = _unitOfWork.GetRepository<Equipment>().GetAll();

        var totalCount = await queryEquipment.CountAsync();

        var items = await queryEquipment // Apply pagination to the query.
                        .Skip((paged.PageNumber - 1) * paged.PageSize) // Skip the appropriate number of items based on the current page
                        .Take(paged.PageSize) // Take only the number of items specified by the page size.
                        .ToListAsync(); // Convert the result to a list asynchronously.


        return new PagedResultDto<EquipmentDto>
        {
            Items = items?.Select(_mapper.Map<EquipmentDto>) ?? Enumerable.Empty<EquipmentDto>(),
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