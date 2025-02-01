
namespace DownTrack.Application.DTO.Paged;

// formato de la solicitud GET que incluye listas

public class PagedRequestDto
{
    public int PageNumber {get;set;}
    public int PageSize {get;set;}
    public string? BaseUrl {get;set;}
      // The name of the column by which the results should be sorted
    public string? SortColumn { get; set; }

    // Indicates whether the sorting should be descending (true) or ascending (false)
    public bool SortDescending { get; set; } = false;

    // Dynamic filters to apply to the query
    public Dictionary<string, object>? Filters { get; set; } 
}

