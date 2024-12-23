


using DownTrack.Application.DTO;
using DownTrack.Application.IServices;
using DownTrack.Application.IRespository;

namespace DownTrack.Application.Services;

public class TechnicianServices : ITechnicianServices
{
    private readonly ITechnicianRepository _technicianRepository;

    public TechnicianServices(ITechnicianRepository technicianRepository)
    {
        _technicianRepository = technicianRepository;
    }

    public async Task<TechnicianDto> CreateAsync(TechnicianDto technicianDto)
    {
        // falta la logica 

        return technicianDto;
    }

    public async Task<TechnicianDto> UpdateAsync(TechnicianDto technicianDto)
    {
        // falta la logica 

        return technicianDto;
    }

    public async Task<IEnumerable<TechnicianDto>> ListAsync()
    {
        // falta la logica 
        List<TechnicianDto> list = new List<TechnicianDto>();

        return list;
    }

    public async Task DeleteAsync(int technicianDto)
    {
        // falta la logica 

    }

}