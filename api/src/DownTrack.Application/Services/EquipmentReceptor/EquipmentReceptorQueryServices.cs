using System.Linq.Expressions;
using AutoMapper;
using DownTrack.Application.DTO;
using DownTrack.Application.IServices;
using DownTrack.Application.IUnitOfWorkPattern;
using DownTrack.Domain.Entities;


namespace DownTrack.Application.Services;

public class EquipmentReceptorQueryServices : GenericQueryServices<EquipmentReceptor,GetEquipmentReceptorDto>,
                                              IEquipmentReceptorQueryServices
{

    private static readonly Expression<Func<EquipmentReceptor, object>>[] includes = 
                            {er => er.User!,
                             er => er.Department,
                             er => er.Department.Section};
    public EquipmentReceptorQueryServices(IUnitOfWork unitOfWork, IMapper mapper)
        : base (unitOfWork,mapper)
    {

    }

    public override Expression<Func<EquipmentReceptor, object>>[] GetIncludes()=> includes;



}