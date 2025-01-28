using AutoMapper;
using DownTrack.Application.DTO;
using DownTrack.Application.DTO.Paged;
using DownTrack.Application.IServices;
using DownTrack.Application.IUnitOfWorkPattern;
using DownTrack.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DownTrack.Application.Services;

public class TransferRequestQueryServices : ITransferRequestQueryServices
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public TransferRequestQueryServices(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<GetTransferRequestDto>> ListAsync()
    {
        var transferRequest = await _unitOfWork.GetRepository<TransferRequest>()
                                               .GetAll()
                                               .Include(tr => tr.SectionManager!.User)
                                               .Include(tr=> tr.ArrivalDepartment)
                                               .Include(tr=> tr.Equipment)
                                               .Include(tr=> tr.ArrivalDepartment.Section)
                                               .ToListAsync();


        return transferRequest.Select(_mapper.Map<GetTransferRequestDto>);
    }
 
    /// <summary>
    /// Retrieves a transferRequest by their ID
    /// </summary>
    /// <param name="transferRequestDto">The transferRequest's ID to retrieve</param>
    /// <returns>A Task representing the asynchronous operation that fetches the transferRequest</returns>
    public async Task<GetTransferRequestDto> GetByIdAsync(int transferRequestDto)
    {
        var result = await _unitOfWork.GetRepository<TransferRequest>()
                                      .GetByIdAsync(transferRequestDto,default,
                                                    tr=> tr.Equipment,
                                                    tr=> tr.ArrivalDepartment,
                                                    tr=> tr.ArrivalDepartment.Section,
                                                    tr=> tr.SectionManager!.User!,
                                                    tr=> tr.Equipment.Department,
                                                    tr=> tr.Equipment.Department.Section);
        
        // and returns the updated transferRequest as a transferRequestDto.
        return _mapper.Map<GetTransferRequestDto>(result);

    }

    public async Task<PagedResultDto<GetTransferRequestDto>> GetPagedResultAsync(PagedRequestDto paged)
    {
        //The queryable collection of entities to paginate
        IQueryable<TransferRequest> queryTransferRequest = _unitOfWork.GetRepository<TransferRequest>()
                                                                      .GetAll()
                                                                      .Include(tr => tr.SectionManager!.User)
                                                                      .Include(tr=> tr.ArrivalDepartment)
                                                                      .Include(tr=> tr.Equipment)                                                                  
                                                                      .ThenInclude(e=> e.Department)
                                                                      .ThenInclude(d=> d.Section);

        var totalCount = await queryTransferRequest.CountAsync();

        var items = await queryTransferRequest // Apply pagination to the query.
                        .Skip((paged.PageNumber - 1) * paged.PageSize) // Skip the appropriate number of items based on the current page
                        .Take(paged.PageSize) // Take only the number of items specified by the page size.
                        .ToListAsync(); // Convert the result to a list asynchronously.


        return new PagedResultDto<GetTransferRequestDto>
        {
            Items = items?.Select(_mapper.Map<GetTransferRequestDto>) ?? Enumerable.Empty<GetTransferRequestDto>(),
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

    public async Task<PagedResultDto<GetTransferRequestDto>> GetPagedRequestsofArrivalDepartmentAsync(int arrivalDepartment, PagedRequestDto paged)
    {
        //The queryable collection of entities to paginate
        IQueryable<TransferRequest> queryTransferRequest = _unitOfWork.GetRepository<TransferRequest>()
                                                                      .GetAllByItems(tr=> tr.ArrivalDepartmentId == arrivalDepartment)
                                                                      .Include(tr => tr.SectionManager!.User)
                                                                      .Include(tr=> tr.ArrivalDepartment)
                                                                      .Include(tr=> tr.Equipment)                                                                  
                                                                      .ThenInclude(e=> e.Department)
                                                                      .ThenInclude(d=> d.Section);

        var totalCount = await queryTransferRequest.CountAsync();

        var items = await queryTransferRequest // Apply pagination to the query.
                        .Skip((paged.PageNumber - 1) * paged.PageSize) // Skip the appropriate number of items based on the current page
                        .Take(paged.PageSize) // Take only the number of items specified by the page size.
                        .ToListAsync(); // Convert the result to a list asynchronously.


        return new PagedResultDto<GetTransferRequestDto>
        {
            Items = items?.Select(_mapper.Map<GetTransferRequestDto>) ?? Enumerable.Empty<GetTransferRequestDto>(),
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




