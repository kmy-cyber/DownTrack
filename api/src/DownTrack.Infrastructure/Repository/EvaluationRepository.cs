using DownTrack.Application.IRepository;
using DownTrack.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DownTrack.Infrastructure.Repository;

public class EvaluationRepository : GenericRepository<Evaluation>, IEvaluationRepository
{
    public EvaluationRepository(DownTrackContext context) : base(context) {}

}
