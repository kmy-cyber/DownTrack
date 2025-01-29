using System.Linq.Expressions;
using AutoMapper;
using DownTrack.Application.DTO;
using DownTrack.Application.IServices;
using DownTrack.Application.IUnitOfWorkPattern;
using DownTrack.Domain.Entities;
namespace DownTrack.Application.Services;

public class TechnicianQueryServices : GenericQueryServices<Technician,GetTechnicianDto>,
                                       ITechnicianQueryServices
{
    private static readonly Expression<Func<Technician, object>>[] includes = 
                            { d => d.User! };
    public TechnicianQueryServices(IUnitOfWork unitOfWork, IMapper mapper)
        : base(unitOfWork, mapper)
    {

    }
    
    public override Expression<Func<Technician, object>>[] GetIncludes()=> includes;

}