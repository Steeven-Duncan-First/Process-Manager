using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using GoldenSource.Mock.Services;
using GoldenSource.Mock.Models;

namespace GoldenSource.Mock.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IMockUserService _userService;

        public UserController(IMockUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(string id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
                return NotFound();
            return Ok(user);
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchUsers([FromQuery] string query)
        {
            var users = await _userService.SearchUsersAsync(query);
            return Ok(users);
        }

        [HttpGet("department/{department}")]
        public async Task<IActionResult> GetUsersByDepartment(string department)
        {
            var users = await _userService.GetUsersByDepartmentAsync(department);
            return Ok(users);
        }
    }
} 