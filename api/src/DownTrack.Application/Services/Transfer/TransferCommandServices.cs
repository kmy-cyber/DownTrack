using System.Linq.Expressions;
using AutoMapper;
using DownTrack.Application.DTO;
using DownTrack.Application.IServices;
using DownTrack.Application.IUnitOfWorkPattern;
using DownTrack.Domain.Entities;
using DownTrack.Domain.Enum;


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

        transfer.TransferRequest.Status = TransferRequestStatus.Accepted.ToString();

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
        // solo puedo modificar el shipping supervisor

        var transfer = await _unitOfWork.GetRepository<Transfer>()
                                        .GetByIdAsync(dto.Id);

        if (transfer.ShippingSupervisorId != dto.ShippingSupervisorId)
        {
            var expression = new Expression<Func<Employee, bool>>[2]
            {
                e=> e.Id == dto.Id,
                e=> e.UserRole == UserRole.ShippingSupervisor.ToString()
            };

            var includes = new Expression<Func<Employee, object>>[0];

            var supervisor = await _unitOfWork.GetRepository<Employee>()
                                              .GetByItems(expression, includes);

            if (supervisor == null)
                throw new Exception($"Shipping Supervisor with Id :{dto.ShippingSupervisorId} does not exist");

            
            _mapper.Map(dto, transfer);
            transfer.ShippingSupervisor = supervisor;
            
            _unitOfWork.GetRepository<Transfer>().Update(transfer);

            await _unitOfWork.CompleteAsync();

            return _mapper.Map<TransferDto>(transfer);
        }

        return dto; 


    }

}
