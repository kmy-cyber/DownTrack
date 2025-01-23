using DownTrack.Application.DTO;
using DownTrack.Application.DTO.Paged;
using DownTrack.Application.IRepository;
using DownTrack.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DownTrack.Infrastructure.Repository;

public class TechnicianRepository : GenericRepository<Technician>, ITechnicianRepository
{
    public TechnicianRepository(DownTrackContext context) : base(context){}


}