

// using DownTrack.Application.DTO;
// using DownTrack.Application.DTO.Paged;
// using DownTrack.Application.IServices;
// using Microsoft.AspNetCore.Mvc;

// [ApiController]
// [Route("api/[controller]")]
// public class GenericPagingController : ControllerBase
// {
//     private readonly IGenericPagingServices _pagingServices;

//     public GenericPagingController (IGenericPagingServices pagingServices)
//     {
//         _pagingServices = pagingServices;
//     }

//     [HttpPost("paged-data")]
//     public async Task<IActionResult> GetPagedData ([FromBody] PagedFilterRequestDto request)
//     {
//         var result = await _pagingServices.GetPagedDataAsync(request);

//         return Ok(result);
//     }

// }