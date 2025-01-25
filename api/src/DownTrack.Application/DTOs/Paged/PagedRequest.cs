
using System.Collections;
using System.Linq.Expressions;
using System.Threading.Tasks.Dataflow;

namespace DownTrack.Application.DTO.Paged;

// formato de la solicitud GET que incluye listas

public class PagedRequestDto
{
    public int PageNumber {get;set;}
    public int PageSize {get;set;}
    public string? BaseUrl {get;set;}
}


public class Prueba <T>
{
    public PagedRequestDto paged {get;set;} = null!;
    public List<Expression<Func<T,bool>>> expressions {get;set;} = new ();
}