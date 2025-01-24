using AutoMapper;
using DownTrack.Application.DTO;
using DownTrack.Application.DTO.Paged;
using DownTrack.Application.IServices;
using DownTrack.Application.IUnitOfWorkPattern;
using DownTrack.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DownTrack.Application.Services;

public class TransferRequestServices : ITransferRequestServices
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public TransferRequestServices(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<TransferRequestDto> CreateAsync(TransferRequestDto dto)
    {
        var transferRequest = _mapper.Map<TransferRequest>(dto);
        
        await _unitOfWork.GetRepository<TransferRequest>().CreateAsync(transferRequest);

        await _unitOfWork.CompleteAsync();

        return _mapper.Map<TransferRequestDto>(transferRequest);
    }

    public async Task DeleteAsync(int dto)
    {
        await _unitOfWork.GetRepository<TransferRequest>().DeleteByIdAsync(dto);

        await _unitOfWork.CompleteAsync();
        //await _transferRequestRepository.DeleteByIdAsync(dto);
    }

    public async Task<IEnumerable<TransferRequestDto>> ListAsync()
    {
        var transferRequest = await _unitOfWork.GetRepository<TransferRequest>().GetAll().ToListAsync();
        //var transferRequest = await _transferRequestRepository.ListAsync();
        return transferRequest.Select(_mapper.Map<TransferRequestDto>);
    }

    public async Task<TransferRequestDto> UpdateAsync(TransferRequestDto dto)
    {
        var transferRequest = await _unitOfWork.GetRepository<TransferRequest>().GetByIdAsync(dto.Id);

        _mapper.Map(dto, transferRequest);

        _unitOfWork.GetRepository<TransferRequest>().Update(transferRequest);

        await _unitOfWork.CompleteAsync();
        
        return _mapper.Map<TransferRequestDto>(transferRequest);
    }
 
    /// <summary>
    /// Retrieves a transferRequest by their ID
    /// </summary>
    /// <param name="transferRequestDto">The transferRequest's ID to retrieve</param>
    /// <returns>A Task representing the asynchronous operation that fetches the transferRequest</returns>
    public async Task<TransferRequestDto> GetByIdAsync(int transferRequestDto)
    {
        var result = await _unitOfWork.GetRepository<TransferRequest>().GetByIdAsync(transferRequestDto);
        
        // and returns the updated transferRequest as a transferRequestDto.
        return _mapper.Map<TransferRequestDto>(result);

    }


    public async Task<PagedResultDto<TransferRequestDto>> GetPagedResultAsync(PagedRequestDto paged)
    {
        //The queryable collection of entities to paginate
        IQueryable<TransferRequest> queryTransferRequest = _unitOfWork.GetRepository<TransferRequest>().GetAll();

        var totalCount = await queryTransferRequest.CountAsync();

        var items = await queryTransferRequest // Apply pagination to the query.
                        .Skip((paged.PageNumber - 1) * paged.PageSize) // Skip the appropriate number of items based on the current page
                        .Take(paged.PageSize) // Take only the number of items specified by the page size.
                        .ToListAsync(); // Convert the result to a list asynchronously.


        return new PagedResultDto<TransferRequestDto>
        {
            Items = items?.Select(_mapper.Map<TransferRequestDto>) ?? Enumerable.Empty<TransferRequestDto>(),
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




