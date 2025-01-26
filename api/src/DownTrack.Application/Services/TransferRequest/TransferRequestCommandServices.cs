using AutoMapper;
using DownTrack.Application.DTO;
using DownTrack.Application.IServices;
using DownTrack.Application.IUnitOfWorkPattern;
using DownTrack.Domain.Entities;

namespace DownTrack.Application.Services;

public class TransferRequestCommandServices : ITransferRequestCommandServices
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public TransferRequestCommandServices(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<TransferRequestDto> CreateAsync(TransferRequestDto dto)
    {
        var transferRequest = _mapper.Map<TransferRequest>(dto);

        var equipment = await _unitOfWork.GetRepository<Equipment>()
                                         .GetByIdAsync(transferRequest.EquipmentId);
        
        var sectionManager = await _unitOfWork.GetRepository<Employee>()
                                              .GetByIdAsync(transferRequest.SectionManagerId);

        if(sectionManager.UserRole != "SectionManager")
            throw new Exception("No es un jefe de seccion");

        var department = await _unitOfWork.GetRepository<Department>()
                                          .GetByIdAsync(transferRequest.ArrivalDepartmentId);
        
        transferRequest.SectionManager = sectionManager;
        transferRequest.Equipment = equipment;
        transferRequest.ArrivalDepartment = department;

        await _unitOfWork.GetRepository<TransferRequest>().CreateAsync(transferRequest);

        await _unitOfWork.CompleteAsync();

        return _mapper.Map<TransferRequestDto>(transferRequest);
    }

    public async Task DeleteAsync(int dto)
    {
        await _unitOfWork.GetRepository<TransferRequest>().DeleteByIdAsync(dto);

        await _unitOfWork.CompleteAsync();
    }

    public async Task<TransferRequestDto> UpdateAsync(TransferRequestDto dto)
    {
        var transferRequest = await _unitOfWork.GetRepository<TransferRequest>().GetByIdAsync(dto.Id);

        if(dto.EquipmentId != transferRequest.EquipmentId)
        {
            var equipment = await _unitOfWork.GetRepository<Equipment>()
                                         .GetByIdAsync(dto.EquipmentId);

            transferRequest.Equipment = equipment;
        }

        if(dto.SectionManagerId != transferRequest.SectionManagerId)
        {
            var sectionManager = await _unitOfWork.GetRepository<Employee>()
                                              .GetByIdAsync(dto.SectionManagerId);

            if(sectionManager.UserRole != "SectionManager")
                throw new Exception("No es un jefe de seccion");

            transferRequest.SectionManager = sectionManager;
        
        }

        if(dto.ArrivalDepartmentId != transferRequest.ArrivalDepartmentId)
        {
            var department = await _unitOfWork.GetRepository<Department>()
                                          .GetByIdAsync(dto.ArrivalDepartmentId);
        
        
            transferRequest.ArrivalDepartment = department;

        }
        
        
        _mapper.Map(dto, transferRequest);

        _unitOfWork.GetRepository<TransferRequest>().Update(transferRequest);

        await _unitOfWork.CompleteAsync();
        
        return _mapper.Map<TransferRequestDto>(transferRequest);
    }
 
}
