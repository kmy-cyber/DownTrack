


using DownTrack.Application.DTO;
using DownTrack.Application.IServices;
using DownTrack.Application.IRepository;
using AutoMapper;
using DownTrack.Domain.Entities;

namespace DownTrack.Application.Services;

/// <summary>
/// Handle the business logic related to agency and work with DTOs 
/// to interact with the client , using the repository interface to access
/// the database 
/// </summary> 
public class TechnicianServices : ITechnicianServices
{

    private readonly ITechnicianRepository _technicianRepository;
    private readonly IMapper _mapper;

    public TechnicianServices(ITechnicianRepository technicianRepository, IMapper mapper)
    {
        _technicianRepository = technicianRepository;
        _mapper = mapper;
    }



    /// <summary>
    /// Creates a new technician based on the provided TechnicianDto.
    /// </summary>
    /// <param name="technicianDto">The DTO containing technician details to be created.</param>
    /// <returns>A Task representing the asynchronous operation, with an TechnicianDto as the result.</returns>
    public async Task<TechnicianDto> CreateAsync(TechnicianDto technicianDto)
    {
        // map the DTOs (technicianDto) to a domain entity (Technician) 
        var result = _mapper.Map<Technician>(technicianDto);

        // method of the repository is called to insert the Technician entity into the database
        await _technicianRepository.CreateAsync(result);

        // map the new created Technician entity to a TechnicianDTO
        return _mapper.Map<TechnicianDto>(result);
    }



    /// <summary>
    /// Updates an existing technician's information.
    /// </summary>
    /// <param name="technicianDto">The DTO containing updated technician details.</param>
    /// <returns>A Task representing the asynchronous operation, with an TechnicianDto as the result.</returns>
    public async Task<TechnicianDto> UpdateAsync(TechnicianDto technicianDto)
    {
        var result = _technicianRepository.GetById(technicianDto.Id);

        // Maps the provided TechnicianDto to the existing technician entity, updates the technician in the database, 
        _mapper.Map(technicianDto, result);

        await _technicianRepository.UpdateAsync(result);

        /// and returns the updated technician as a TechnicianDto.
        return _mapper.Map<TechnicianDto>(result);
    }



    /// <summary>
    /// Retrieves a list of all technicians.
    /// </summary>
    /// <returns>A Task representing the asynchronous operation, with a list of TechnicianDto as the result.</returns>
    public async Task<IEnumerable<TechnicianDto>> ListAsync()
    {
        //fetches all technicians from the repository
        var results = await _technicianRepository.ListAsync();

        //return them as a enumerable of TechnicanDto objects
        return results.Select(_mapper.Map<TechnicianDto>);

    }



    /// <summary>
    /// Deletes a technician by its ID.
    /// </summary>
    /// <param name="technicianDto">The technician's ID to be deleted.</param>
    /// <returns>A Task representing the asynchronous delete operation.</returns>
    public async Task DeleteAsync(int technicianDto)
    {
        await _technicianRepository.DeleteByIdAsync(technicianDto);
    }



}