
namespace DownTrack.Application.DTO.Paged;

// formato de la solicitud GET que incluye listas

public class PagedRequestDto
{
    public int PageNumber {get;set;}
    public int PageSize {get;set;}
    public string? BaseUrl {get;set;}
}