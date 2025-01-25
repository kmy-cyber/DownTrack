using AutoMapper;
using DownTrack.Application.DTO;
using DownTrack.Application.DTO.Paged;
using DownTrack.Application.IServices;
using DownTrack.Application.IUnitOfWorkPattern;
using DownTrack.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DownTrack.Application.Services;

public class DoneMaintenanceServices : IDoneMaintenanceServices
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public DoneMaintenanceServices(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<DoneMaintenanceDto> CreateAsync(DoneMaintenanceDto dto)
    {
        if (dto.TechnicianId == null || dto.EquipmentId == null)
        {
            throw new ArgumentException("TechnicianId and EquipmentId must not be null.");
        }
        var doneMaintenance = _mapper.Map<DoneMaintenance>(dto);

        doneMaintenance.Technician = await _unitOfWork.GetRepository<Technician>()
                                        .GetByIdAsync(doneMaintenance.TechnicianId!.Value);

        doneMaintenance.Equipment = await _unitOfWork.GetRepository<Equipment>()
                                        .GetByIdAsync(doneMaintenance.EquipmentId!.Value);


        await _unitOfWork.GetRepository<DoneMaintenance>().CreateAsync(doneMaintenance);
        await _unitOfWork.CompleteAsync();

        return _mapper.Map<DoneMaintenanceDto>(doneMaintenance);
    }

    public async Task DeleteAsync(int dto)
    {
        await _unitOfWork.GetRepository<DoneMaintenance>().DeleteByIdAsync(dto);
        
        await _unitOfWork.CompleteAsync();
    }

    public async Task<DoneMaintenanceDto> GetByIdAsync(int dto)
    {
        var doneMaintenance = await _unitOfWork.GetRepository<DoneMaintenance>().GetByIdAsync(dto);

        return _mapper.Map<DoneMaintenanceDto>(doneMaintenance);
    }

    public async Task<IEnumerable<DoneMaintenanceDto>> ListAsync()
    {
        var doneMaintenance = await _unitOfWork.GetRepository<DoneMaintenance>().GetAll().ToListAsync();

        return doneMaintenance.Select(_mapper.Map<DoneMaintenanceDto>);
    }

    public async Task<DoneMaintenanceDto> UpdateAsync(DoneMaintenanceDto dto)
    {
        var doneMaintenance = await _unitOfWork.GetRepository<DoneMaintenance>().GetByIdAsync(dto.Id);

        _mapper.Map(dto, doneMaintenance);

        _unitOfWork.GetRepository<DoneMaintenance>().Update(doneMaintenance);

        await _unitOfWork.CompleteAsync();

        return _mapper.Map<DoneMaintenanceDto>(doneMaintenance);
    }



    public async Task<PagedResultDto<DoneMaintenanceDto>> GetPagedResultAsync(PagedRequestDto paged)
    {
        //The queryable collection of entities to paginate
        IQueryable<DoneMaintenance> queryDoneMaintenance = _unitOfWork.GetRepository<DoneMaintenance>().GetAll();

        var totalCount = await queryDoneMaintenance.CountAsync();

        var items = await queryDoneMaintenance // Apply pagination to the query.
                        .Skip((paged.PageNumber - 1) * paged.PageSize) // Skip the appropriate number of items based on the current page
                        .Take(paged.PageSize) // Take only the number of items specified by the page size.
                        .ToListAsync(); // Convert the result to a list asynchronously.


        return new PagedResultDto<DoneMaintenanceDto>
        {
            Items = items?.Select(_mapper.Map<DoneMaintenanceDto>) ?? Enumerable.Empty<DoneMaintenanceDto>(),
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