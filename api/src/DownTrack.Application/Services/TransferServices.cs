using AutoMapper;
using DownTrack.Application.DTO;
using DownTrack.Application.IServices;
using DownTrack.Application.IUnitOfWorkPattern;
using DownTrack.Domain.Entities;
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
        var transfer = await _unitOfWork.GetRepository<Transfer>().GetAllAsync().ToListAsync();
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
}





