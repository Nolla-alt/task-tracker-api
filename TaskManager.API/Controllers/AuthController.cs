using Microsoft.AspNetCore.Mvc;
using TaskManager.API.DTOs;
using TaskManager.Business.Services.Interfaces;

namespace TaskManager.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;

        public AuthController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var user = await _userService.LoginAsync(dto.Email, dto.Password);
            if (user == null)
                return Unauthorized(new { error = "Invalid email or password." });

            return Ok(new UserDto { Id = user.Id, Name = user.Name, Email = user.Email });
        }
    }
}