using AutoMapper;
using DownTrack.Application.DTO;
using DownTrack.Application.DTO.Paged;
using DownTrack.Application.IServices;
using DownTrack.Application.IUnitOfWorkPattern;
using DownTrack.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DownTrack.Application.Services;

public class DoneMaintenanceQueryServices : IDoneMaintenanceQueryServices
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public DoneMaintenanceQueryServices(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<GetDoneMaintenanceDto> GetByIdAsync(int dto)
    {
        var doneMaintenance = await _unitOfWork.GetRepository<DoneMaintenance>()
                                               .GetByIdAsync(dto,default,
                                                             dm=> dm.Equipment!,
                                                             dm=> dm.Technician!);

        return _mapper.Map<GetDoneMaintenanceDto>(doneMaintenance);
    }

    public async Task<IEnumerable<GetDoneMaintenanceDto>> ListAsync()
    {
        var doneMaintenance = await _unitOfWork.GetRepository<DoneMaintenance>()
                                               .GetAll()
                                               .Include(dm => dm.Equipment)
                                               .Include(dm => dm.Technician)
                                               .ToListAsync();

        return doneMaintenance.Select(_mapper.Map<GetDoneMaintenanceDto>);
    }

    public async Task<PagedResultDto<GetDoneMaintenanceDto>> GetPagedResultAsync(PagedRequestDto paged)
    {
        //The queryable collection of entities to paginate
        IQueryable<DoneMaintenance> queryDoneMaintenance = _unitOfWork.GetRepository<DoneMaintenance>()
                                                                      .GetAll()
                                                                      .Include(dm => dm.Equipment)
                                                                      .Include(dm => dm.Technician);

        var totalCount = await queryDoneMaintenance.CountAsync();

        var items = await queryDoneMaintenance // Apply pagination to the query.
                        .Skip((paged.PageNumber - 1) * paged.PageSize) // Skip the appropriate number of items based on the current page
                        .Take(paged.PageSize) // Take only the number of items specified by the page size.
                        .ToListAsync(); // Convert the result to a list asynchronously.


        return new PagedResultDto<GetDoneMaintenanceDto>
        {
            Items = items?.Select(_mapper.Map<GetDoneMaintenanceDto>) ?? Enumerable.Empty<GetDoneMaintenanceDto>(),
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

    public async Task<PagedResultDto<GetDoneMaintenanceDto>> GetByTechnicianIdAsync (PagedRequestDto paged, int technicianId)
    {
        IQueryable<DoneMaintenance> queryMaintenancesByTechnician = _unitOfWork.GetRepository<DoneMaintenance>()
                                                                                .GetAllByItems(dm=> dm.TechnicianId ==technicianId);

        var totalCount = await queryMaintenancesByTechnician.CountAsync();

        var items = await queryMaintenancesByTechnician // Apply pagination to the query.
                        .Skip((paged.PageNumber - 1) * paged.PageSize) // Skip the appropriate number of items based on the current page
                        .Take(paged.PageSize) // Take only the number of items specified by the page size.
                        .Include(dm=> dm.Technician!.User)
                        .ToListAsync(); // Convert the result to a list asynchronously.


        return new PagedResultDto<GetDoneMaintenanceDto>
        {
            Items = items?.Select(_mapper.Map<GetDoneMaintenanceDto>) ?? Enumerable.Empty<GetDoneMaintenanceDto>(),
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