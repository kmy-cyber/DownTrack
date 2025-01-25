using AutoMapper;
using DownTrack.Application.DTO;
using DownTrack.Application.DTO.Paged;
using DownTrack.Application.IServices;
using DownTrack.Application.IUnitOfWorkPattern;
using DownTrack.Domain.Entities;
using DownTrack.Domain.Enum;
using DownTrack.Domain.Status;
using Microsoft.EntityFrameworkCore;

namespace DownTrack.Application.Services;

public class EquipmentDecommissioningServices : IEquipmentDecommissioningServices
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    public EquipmentDecommissioningServices(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<EquipmentDecommissioningDto> CreateAsync(EquipmentDecommissioningDto dto)
    {
        if (dto.TechnicianId == null || dto.EquipmentId == null || dto.ReceptorId == null || string.IsNullOrEmpty(dto.Cause))
        {
            throw new ArgumentException("All fields must be non-null when creating a decommissioning.");
        }

        var equipmentDecommissioning = _mapper.Map<EquipmentDecommissioning>(dto);

        var equipment = await _unitOfWork.GetRepository<Equipment>().GetByIdAsync(equipmentDecommissioning.EquipmentId);

        if (equipment.Status == EquipmentStatus.Inactive.ToString())
        {
            throw new InvalidOperationException("The equipment is already inactive.");
        }

        var technician = await _unitOfWork.GetRepository<Technician>().GetByIdAsync(equipmentDecommissioning.TechnicianId);

        var receptor = await _unitOfWork.GetRepository<EquipmentReceptor>().GetByIdAsync(equipmentDecommissioning.ReceptorId);

        equipmentDecommissioning.Technician = technician;
        equipmentDecommissioning.Receptor = receptor;
        equipmentDecommissioning.Equipment = equipment;

        equipmentDecommissioning.Status = DecommissioningStatus.Pending.ToString();

        await _unitOfWork.GetRepository<EquipmentDecommissioning>().CreateAsync(equipmentDecommissioning);

        await _unitOfWork.CompleteAsync();

        return _mapper.Map<EquipmentDecommissioningDto>(equipmentDecommissioning);
    }

    public async Task DeleteAsync(int dto)
    {
        await _unitOfWork.GetRepository<EquipmentDecommissioning>().DeleteByIdAsync(dto);

        await _unitOfWork.CompleteAsync();
    }

    public async Task<EquipmentDecommissioningDto> GetByIdAsync(int dto)
    {
        var equipmentDecommissioning = await _unitOfWork.GetRepository<EquipmentDecommissioning>().GetByIdAsync(dto);
        return _mapper.Map<EquipmentDecommissioningDto>(equipmentDecommissioning);
    }

    public async Task<IEnumerable<EquipmentDecommissioningDto>> ListAsync()
    {
        var equipmentDecommissioning = await _unitOfWork.GetRepository<EquipmentDecommissioning>()
                                                        .GetAll()
                                                        .ToListAsync();

        return equipmentDecommissioning.Select(_mapper.Map<EquipmentDecommissioningDto>);
    }

    public async Task<EquipmentDecommissioningDto> UpdateAsync(EquipmentDecommissioningDto dto)
    {
        var equipmentDecommissioning = await _unitOfWork.GetRepository<EquipmentDecommissioning>().GetByIdAsync(dto.Id);

        if(equipmentDecommissioning.Status == DecommissioningStatus.Accepted.ToString())
            throw new Exception("The equipmentDecommissioning is already acepted ");


        var equipment = await _unitOfWork.GetRepository<Equipment>().GetByIdAsync(equipmentDecommissioning.EquipmentId);

        var receptor = await _unitOfWork.GetRepository<EquipmentReceptor>().GetByIdAsync(equipmentDecommissioning.ReceptorId);

        var technician = await _unitOfWork.GetRepository<Technician>().GetByIdAsync(equipmentDecommissioning.TechnicianId);

        if (equipment == null || receptor == null || technician == null)
            throw new Exception("CANDELAAAAAAAAAAAA");

        _mapper.Map(dto, equipmentDecommissioning);

        _unitOfWork.GetRepository<EquipmentDecommissioning>().Update(equipmentDecommissioning);

        await _unitOfWork.CompleteAsync();

        return _mapper.Map<EquipmentDecommissioningDto>(equipmentDecommissioning);
    }


    public async Task AcceptDecommissioningAsync(int dto)
    {
        var equipmentDecommissioning = await _unitOfWork.GetRepository<EquipmentDecommissioning>().GetByIdAsync(dto);

        if(equipmentDecommissioning.Status == DecommissioningStatus.Accepted.ToString())
            throw new Exception("The equipmentDecomissioning is already acepted");

        var equipment = await _unitOfWork.GetRepository<Equipment>().GetByIdAsync(equipmentDecommissioning.EquipmentId);

        equipment.Status = equipment.Status != EquipmentStatus.Inactive.ToString()
                            ? EquipmentStatus.Inactive.ToString()
                            : throw new Exception("The equipment is already inactive.");

        equipmentDecommissioning.Status = DecommissioningStatus.Accepted.ToString();

        _unitOfWork.GetRepository<EquipmentDecommissioning>().Update(equipmentDecommissioning);

        await _unitOfWork.CompleteAsync();

        // var allDecommissionings = await _unitOfWork.GetRepository<EquipmentDecommissioning>().GetAllAsync().ToListAsync();
        // foreach (var decommissioning in allDecommissionings)
        // {
        //     if (decommissioning.EquipmentId == equipmentDecommissioning.EquipmentId && decommissioning.Id != equipmentDecommissioning.Id)
        //     {
        //         await _unitOfWork.GetRepository<EquipmentDecommissioning>().DeleteByIdAsync(decommissioning.Id);
        //     }
        // }
        // equipmentDecommissioning.Status = Domain.Enum.DecommissioningStatus.Accepted;

        // _unitOfWork.GetRepository<Equipment>().Update(equipment);
        // await _unitOfWork.CompleteAsync();

        

    }

     public async Task<PagedResultDto<EquipmentDecommissioningDto>> GetPagedResultAsync(PagedRequestDto paged)
    {
        //The queryable collection of entities to paginate
        IQueryable<EquipmentDecommissioning> queryEquipmentDecommissioning = _unitOfWork.GetRepository<EquipmentDecommissioning>()
                                                                          .GetAll();

        var totalCount = await queryEquipmentDecommissioning.CountAsync();


        var items = await queryEquipmentDecommissioning // Apply pagination to the query.
                        .Skip((paged.PageNumber - 1) * paged.PageSize) // Skip the appropriate number of items based on the current page
                        .Take(paged.PageSize) // Take only the number of items specified by the page size.
                        .ToListAsync(); // Convert the result to a list asynchronously.


        return new PagedResultDto<EquipmentDecommissioningDto>
        {
            Items = items?.Select(_mapper.Map<EquipmentDecommissioningDto>) ?? Enumerable.Empty<EquipmentDecommissioningDto>(),
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