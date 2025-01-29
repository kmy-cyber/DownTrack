using System.Linq.Expressions;
using AutoMapper;
using DownTrack.Application.DTO;
using DownTrack.Application.IServices;
using DownTrack.Application.IUnitOfWorkPattern;
using DownTrack.Domain.Entities;


namespace DownTrack.Application.Services;

public class EvaluationQueryServices : GenericQueryServices<Evaluation,GetEvaluationDto>,
                                       IEvaluationQueryServices
{
    private static readonly Expression<Func<Evaluation, object>>[] includes = 
                            { ev => ev.SectionManager!.User!,
                              ev => ev.Technician.User!};
    public EvaluationQueryServices(IUnitOfWork unitOfWork, IMapper mapper)
        : base (unitOfWork, mapper)
    {
    }


    public override Expression<Func<Evaluation, object>>[] GetIncludes()=> includes;



}