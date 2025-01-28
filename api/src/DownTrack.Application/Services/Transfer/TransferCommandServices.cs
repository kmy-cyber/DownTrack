using AutoMapper;
using DownTrack.Application.DTO;
using DownTrack.Application.IServices;
using DownTrack.Application.IUnitOfWorkPattern;
using DownTrack.Domain.Entities;


namespace DownTrack.Application.Services;

public class TransferCommandServices : ITransferCommandServices
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public TransferCommandServices(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<TransferDto> CreateAsync(TransferDto dto)
    {
        var transfer = _mapper.Map<Transfer>(dto);

        var shippingSupervisor = await _unitOfWork.GetRepository<Employee>()
                                                  .GetByIdAsync(transfer.ShippingSupervisorId);

        if (shippingSupervisor.UserRole != "ShippingSupervisor")
            throw new Exception("No es un responsable de envio");

        var receptor = await _unitOfWork.GetRepository<EquipmentReceptor>()
                                             .GetByIdAsync(transfer.EquipmentReceptorId);

        var transferRequest = await _unitOfWork.GetRepository<TransferRequest>()
                                               .GetByIdAsync(transfer.RequestId, default,
                                                            tr => tr.Equipment);

        if (receptor.DepartmentId != transferRequest.ArrivalDepartmentId)
            throw new Exception($"Equipment Receptor {dto.EquipmentReceptorId} not belong to Department: {transferRequest.ArrivalDepartmentId}");

        transfer.ShippingSupervisor = shippingSupervisor;
        transfer.EquipmentReceptor = receptor;
        transfer.TransferRequest = transferRequest;


        transfer.TransferRequest.Equipment.DepartmentId = transfer.TransferRequest.ArrivalDepartmentId;
        transfer.TransferRequest.Equipment.Department = transfer.TransferRequest.ArrivalDepartment;

        await _unitOfWork.GetRepository<Transfer>().CreateAsync(transfer);

        await _unitOfWork.CompleteAsync();

        return _mapper.Map<TransferDto>(transfer);
    }

    public async Task DeleteAsync(int dto)
    {
        await _unitOfWork.GetRepository<Transfer>().DeleteByIdAsync(dto);

        await _unitOfWork.CompleteAsync();
    }

    public async Task<TransferDto> UpdateAsync(TransferDto dto)
    {
        await DeleteAsync(dto.Id);
        await CreateAsync(dto);
        return dto;
        // var transfer = await _unitOfWork.GetRepository<Transfer>().GetByIdAsync(dto.RequestId);

        // var receptor2 = transfer.EquipmentReceptor;
        // var transfer2 = transfer.TransferRequest;

        // if (dto.ShippingSupervisorId != transfer.ShippingSupervisorId)
        // {
        //     var shippingSupervisor = await _unitOfWork.GetRepository<Employee>()
        //                                           .GetByIdAsync(transfer.ShippingSupervisorId);

        //     if (shippingSupervisor.UserRole != "ShippingSupervisor")
        //         throw new Exception("No es un responsable de envio");

        //     transfer.ShippingSupervisor = shippingSupervisor;
        // }


        // if (dto.EquipmentReceptorId != transfer.EquipmentReceptorId)
        // {
        //     var receptor = await _unitOfWork.GetRepository<EquipmentReceptor>()
        //                                      .GetByIdAsync(transfer.EquipmentReceptorId);

        //     receptor2 = receptor;
        //     transfer.EquipmentReceptor = receptor;
        // }

        // if (dto.RequestId != transfer.RequestId)
        // {

        //     var transferRequest = await _unitOfWork.GetRepository<TransferRequest>()
        //                                        .GetByIdAsync(transfer.RequestId, default,
        //                                                     tr => tr.Equipment);

        //     transfer2 = transferRequest;

        //     transfer.TransferRequest = transferRequest;
        //     transfer.TransferRequest.Equipment.DepartmentId = transfer.TransferRequest.ArrivalDepartmentId;
        //     transfer.TransferRequest.Equipment.Department = transfer.TransferRequest.ArrivalDepartment;

        // }

        // if (receptor2!.DepartmentId != transfer2.ArrivalDepartmentId)
        //     throw new Exception($"Equipment Receptor {dto.EquipmentReceptorId} not belong to Department: {transfer2.ArrivalDepartmentId}");


        // _mapper.Map(dto, transfer);

        // _unitOfWork.GetRepository<Transfer>().Update(transfer);

        // await _unitOfWork.CompleteAsync();

        // return _mapper.Map<TransferDto>(transfer);
    }

}
