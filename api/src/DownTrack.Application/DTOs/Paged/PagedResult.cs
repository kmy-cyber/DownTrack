
namespace DownTrack.Application.DTO.Paged;

// formato de la solicitud de respuesta a la pagina

public class PagedResultDto<T>
{
    public IEnumerable<T>? Items {get;set;} 
    public int TotalCount {get;set;}
    public int PageNumber {get;set;}
    public int PageSize {get;set;}
    public string? NextPageUrl {get;set;}
    public string? PreviousPageUrl {get;set;}
}