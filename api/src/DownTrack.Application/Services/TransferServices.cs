using System.Diagnostics;
using System.Linq.Expressions;
using AutoMapper;
using DownTrack.Application.DTO;
using DownTrack.Application.DTO.Paged;
using DownTrack.Application.IServices;
using DownTrack.Application.IUnitOfWorkPattern;
using DownTrack.Domain.Entities;
using DownTrack.Domain.Roles;
using Microsoft.EntityFrameworkCore;

namespace DownTrack.Application.Services;

public class TransferServices : ITransferServices
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public TransferServices(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<TransferDto> CreateAsync(TransferDto dto)
    {
        var transfer = _mapper.Map<Transfer>(dto);

        // if(transfer.ShippingSupervisorId == null || transfer.EquipmentReceptorId == null)
        //     throw new Exception("Shipping Supervisor and EquipmentReceptor is required");
        
        // var shippingSupervisor = await _unitOfWork.GetRepository<Employee>()
        //                                     .GetByIdAsync(transfer.ShippingSupervisorId);
        // if(shippingSupervisor == null)
        //     throw new Exception("");

        // if(shippingSupervisor.UserRole != UserRole.ShippingSupervisor.ToString())
        //     throw new Exception("");
        
        // var receptor = await _unitOfWork.GetRepository<EquipmentReceptor>()
        //                                     .GetByIdAsync(transfer.EquipmentReceptorId);
        // if(receptor == null)
        //     throw new Exception("");
        
        // var transferRequest = await _unitOfWork.GetRepository<TransferRequest>()
        //                                     .GetByIdAsync(transfer.EquipmentReceptorId);
        // if(receptor == null)
        //     throw new Exception("");

        // transfer.ShippingSupervisor = shippingSupervisor;
        // transfer.EquipmentReceptor = receptor;
        
        await _unitOfWork.GetRepository<Transfer>().CreateAsync(transfer);

        await _unitOfWork.CompleteAsync();

        return _mapper.Map<TransferDto>(transfer);
    }

    public async Task DeleteAsync(int dto)
    {
        await _unitOfWork.GetRepository<Transfer>().DeleteByIdAsync(dto);

        await _unitOfWork.CompleteAsync();
        //await _transferRepository.DeleteByIdAsync(dto);
    }

    public async Task<IEnumerable<TransferDto>> ListAsync()
    {
        var transfer = await _unitOfWork.GetRepository<Transfer>().GetAll().ToListAsync();
        //var transfer = await _transferRepository.ListAsync();
        return transfer.Select(_mapper.Map<TransferDto>);
    }

    public async Task<TransferDto> UpdateAsync(TransferDto dto)
    {
        var transfer = await _unitOfWork.GetRepository<Transfer>().GetByIdAsync(dto.RequestId);

        _mapper.Map(dto, transfer);

        _unitOfWork.GetRepository<Transfer>().Update(transfer);

        await _unitOfWork.CompleteAsync();

        return _mapper.Map<TransferDto>(transfer);
    }

    /// <summary>
    /// Retrieves a transfer by their ID
    /// </summary>
    /// <param name="transferDto">The transfer's ID to retrieve</param>
    /// <returns>A Task representing the asynchronous operation that fetches the transfer</returns>
    public async Task<TransferDto> GetByIdAsync(int transferDto)
    {
        var result = await _unitOfWork.GetRepository<Transfer>().GetByIdAsync(transferDto);

        // and returns the updated transfer as a transferDto.
        return _mapper.Map<TransferDto>(result);

    }



    public async Task<PagedResultDto<TransferDto>> GetPagedResultAsync(PagedRequestDto paged)
    {
        //The queryable collection of entities to paginate
        IQueryable<Transfer> queryTransfer = _unitOfWork.GetRepository<Transfer>().GetAll();

        var totalCount = await queryTransfer.CountAsync();

        var items = await queryTransfer // Apply pagination to the query.
                        .Skip((paged.PageNumber - 1) * paged.PageSize) // Skip the appropriate number of items based on the current page
                        .Take(paged.PageSize) // Take only the number of items specified by the page size.
                        .ToListAsync(); // Convert the result to a list asynchronously.


        return new PagedResultDto<TransferDto>
        {
            Items = items?.Select(_mapper.Map<TransferDto>) ?? Enumerable.Empty<TransferDto>(),
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





