
namespace DownTrack.Application.DTO.Paged;

public class PagedRequestDto
{
    public int PageNumber {get;set;}
    public int PageSize {get;set;}
    public string? BaseUrl {get;set;}
}