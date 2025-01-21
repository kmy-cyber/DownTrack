

namespace DownTrack.Application.DTO.Paged;

public class GetPagedDto<T>
{
    public IEnumerable<T>? Items {get;set;}
    public int TotalCount {get;set;}
}