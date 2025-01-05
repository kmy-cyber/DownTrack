
using DownTrack.Application.IRespository;
using DownTrack.Domain.Enitites;

namespace DownTrack.Application.IUnitOfWorkPattern;


public interface IUnitOfWork : IDisposable
{
    // ITechnicianRepository technicianRepository {get;}
    // IEmployeeRepository employeeRepository {get;}

    public IGenericRepository<T> GetRepository<T>() where T : GenericEntity;
    Task<int> CompleteAsync (CancellationToken cancellationToken = default);

}