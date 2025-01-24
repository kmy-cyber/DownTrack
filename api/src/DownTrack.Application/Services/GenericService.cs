
// using AutoMapper;
// using DownTrack.Application.DTO.Paged;
// using DownTrack.Application.IServices;
// using DownTrack.Application.IUnitOfWorkPattern;

// namespace DownTrack.Application.Services;


// public class GenericServices<TDto> :  IGenericService<TDto>
// {
//     private readonly IUnitOfWork _unitOfWork;
//     private readonly IMapper _mapper;

//     public GenericServices(IUnitOfWork unitOfWork, IMapper mapper)
//     {
//         _unitOfWork = unitOfWork;
//         _mapper = mapper;
//     }

//     public virtual async Task<TDto> CreateAsync(TDto dto)
//     {
//         throw new Exception("Not implemented");
//     }

//     public virtual async Task<TDto> UpdateAsync(TDto dto)
//     {
//         throw new Exception("Not implemented");
//     }

//     public virtual async Task DeleteAsync(int dto)
//     {
//         throw new Exception();
//     }

//     public virtual async Task<IEnumerable<TDto>> ListAsync()
//     {
//         throw new Exception();
//     }

//     public virtual async Task<PagedResultDto<TDto>> GetPagedResultAsync (PagedRequestDto paged)
//     {
//         throw new Exception();
//     }

//     public virtual async  Task<TDto> GetByIdAsync(int dto)
//     {
//         throw new Exception();
//     }
// }