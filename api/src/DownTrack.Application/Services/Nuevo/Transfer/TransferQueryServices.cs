using AutoMapper;
using DownTrack.Application.DTO;
using DownTrack.Application.DTO.Paged;
using DownTrack.Application.IServices;
using DownTrack.Application.IUnitOfWorkPattern;
using DownTrack.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DownTrack.Application.Services;

public class TransferQueryServices : ITransferQueryServices
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public TransferQueryServices(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<GetTransferDto>> ListAsync()
    {
        var transfer = await _unitOfWork.GetRepository<Transfer>()
                                        .GetAll()
                                        .Include(t => t.ShippingSupervisor)
                                        .Include(t => t.EquipmentReceptor)
                                        .ToListAsync();

        return transfer.Select(_mapper.Map<GetTransferDto>);
    }

    /// <summary>
    /// Retrieves a transfer by their ID
    /// </summary>
    /// <param name="transferDto">The transfer's ID to retrieve</param>
    /// <returns>A Task representing the asynchronous operation that fetches the transfer</returns>
    public async Task<GetTransferDto> GetByIdAsync(int transferDto)
    {
        var result = await _unitOfWork.GetRepository<Transfer>()
                                      .GetByIdAsync(transferDto, default,
                                                    t => t.ShippingSupervisor!,
                                                    t => t.EquipmentReceptor!);

        // and returns the updated transfer as a transferDto.
        return _mapper.Map<GetTransferDto>(result);

    }



    public async Task<PagedResultDto<GetTransferDto>> GetPagedResultAsync(PagedRequestDto paged)
    {
        //The queryable collection of entities to paginate
        IQueryable<Transfer> queryTransfer = _unitOfWork.GetRepository<Transfer>()
                                                        .GetAll()
                                                        .Include(t => t.ShippingSupervisor)
                                                        .Include(t => t.EquipmentReceptor);                               

        var totalCount = await queryTransfer.CountAsync();

        var items = await queryTransfer // Apply pagination to the query.
                        .Skip((paged.PageNumber - 1) * paged.PageSize) // Skip the appropriate number of items based on the current page
                        .Take(paged.PageSize) // Take only the number of items specified by the page size.
                        .ToListAsync(); // Convert the result to a list asynchronously.


        return new PagedResultDto<GetTransferDto>
        {
            Items = items?.Select(_mapper.Map<GetTransferDto>) ?? Enumerable.Empty<GetTransferDto>(),
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





