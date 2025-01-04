

using DownTrack.Application.IRepository;
using DownTrack.Domain.Entities;

namespace DownTrack.Application.IUnitOfWorkPattern;


public interface IUnitOfWork : IDisposable
{
    // ITechnicianRepository technicianRepository {get;}
    // IEmployeeRepository employeeRepository {get;}

    public IGenericRepository<T> GetRepository<T>() where T : GenericEntity;
    Task<int> CompleteAsync (CancellationToken cancellationToken = default);

}