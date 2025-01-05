


using DownTrack.Application.DTO;
using DownTrack.Application.IServices;
using AutoMapper;
using DownTrack.Domain.Enitites;
using DownTrack.Application.IUnitOfWorkPattern;

namespace DownTrack.Application.Services;

public class TechnicianServices : ITechnicianServices
{
    //private readonly ITechnicianRepository _technicianRepository;
    private readonly IMapper _mapper;

    private readonly IUnitOfWork _unitOfWork;

    public TechnicianServices(IUnitOfWork unitOfWork, IMapper mapper)
    {
        // _technicianRepository = technicianRepository;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<TechnicianDto> CreateAsync(TechnicianDto dto)
    {
        var technician = _mapper.Map<Technician>(dto);

        //await _technicianRepository.CreateAsync(technician);
        
        await _unitOfWork.GetRepository<Technician>().CreateAsync(technician);

        return _mapper.Map<TechnicianDto>(technician);
    }

    public async Task DeleteAsync(int dto)
    {
        await _unitOfWork.GetRepository<Technician>().DeleteByIdAsync(dto);
        //await _technicianRepository.DeleteByIdAsync(dto);
    }

    public async Task<IEnumerable<TechnicianDto>> ListAsync()
    {
        var technician = await _unitOfWork.GetRepository<Technician>().ListAsync();
        //var technician = await _technicianRepository.ListAsync();
        return technician.Select(_mapper.Map<TechnicianDto>);
    }

    public async Task<TechnicianDto> UpdateAsync(TechnicianDto dto)
    {
        var technician = _unitOfWork.GetRepository<Technician>().GetById(dto.Id);


        //var technician = _technicianRepository.GetById(dto.Id);
        _mapper.Map(dto, technician);

        await _unitOfWork.GetRepository<Technician>().UpdateAsync(technician);

        //await _technicianRepository.UpdateAsync(technician);
        return _mapper.Map<TechnicianDto>(technician);
    }

    /// <summary>
    /// Retrieves a technician by their ID
    /// </summary>
    /// <param name="technicianDto">The technician's ID to retrieve</param>
    /// <returns>A Task representing the asynchronous operation that fetches the technician</returns>
    public async Task<TechnicianDto> GetByIdAsync(int technicianDto)
    {
        var result = await _unitOfWork.GetRepository<Technician>().GetByIdAsync(technicianDto);
        
        //var result = await _technicianRepository.GetByIdAsync(technicianDto);

        /// and returns the updated technician as a TechnicianDto.
        return _mapper.Map<TechnicianDto>(result);

    }
}