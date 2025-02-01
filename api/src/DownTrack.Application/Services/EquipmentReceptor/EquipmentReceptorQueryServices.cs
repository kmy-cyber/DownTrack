using System.Linq.Expressions;
using AutoMapper;
using DownTrack.Application.DTO;
using DownTrack.Application.Interfaces;
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
    public EquipmentReceptorQueryServices(IUnitOfWork unitOfWork, IMapper mapper,
                                 IFilterService<EquipmentReceptor> filterService,
                                 ISortService<EquipmentReceptor> sortService,
                                 IPaginationService<EquipmentReceptor> paginationService)
        : base(unitOfWork, filterService,sortService,paginationService,mapper)
    {

    }

    public override Expression<Func<EquipmentReceptor, object>>[] GetIncludes()=> includes;



}