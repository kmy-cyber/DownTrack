
using DownTrack.Application.DTO;
using DownTrack.Application.IServices;
using DownTrack.Domain.Enitites;
using Microsoft.AspNetCore.Mvc;

namespace DownTrack.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserServices _userService;

        public UserController(IUserServices userServices)
        {
            _userService = userServices;
        }

        [HttpPost]
        [Route("POST")]

        public async Task<IActionResult> CreateUser(UserDto user)
        {
            await _userService.CreateAsync(user);

            return Ok("User added successfully");
        }

        [HttpGet]
        [Route("GET_ALL")]

        public async Task<ActionResult<IEnumerable<User>>> GetAllUser()
        {
            var results = await _userService.ListAsync();

            return Ok(results);

        }

        [HttpGet]
        [Route("GET")]

        public async Task<ActionResult<User>> GetUserById(int userId)
        {
            var result = await _userService.GetByIdAsync(userId);

            if (result == null)
                return NotFound($"User with ID {userId} not found");

            return Ok(result);

        }

        [HttpPut]
        [Route("PUT")]

        public async Task<IActionResult> UpdateUser(UserDto user)
        {
            var result = await _userService.UpdateAsync(user);
            return Ok(result);
        }

        [HttpDelete]
        [Route("DELETE")]

        public async Task<IActionResult> DeleteUser(int userId)
        {
            await _userService.DeleteAsync(userId);

            return Ok("User deleted successfully");
        }


    }

}