using Microsoft.AspNetCore.Mvc;
using TaskManager.API.DTOs;
using TaskManager.Business.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;   

namespace TaskManager.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ITokenService _tokenService;

        public AuthController(IUserService userService, ITokenService tokenService)
        {
            _userService = userService;
            _tokenService = tokenService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var user = await _userService.LoginAsync(dto.Email, dto.Password);
            if (user == null)
                return Unauthorized(new { error = "Invalid email or password." });

            var token = _tokenService.GenerateToken(user.Id, user.Name, user.Email);

            return Ok(new
            {
                token,
                user = new UserDto { Id = user.Id, Name = user.Name, Email = user.Email }
            });

        }
        [Authorize]
        [HttpGet("me")]
        public IActionResult Me()
        {
            return Ok(new
            {
                id = User.FindFirst("sub")?.Value,
                name = User.FindFirst("name")?.Value,
                email = User.FindFirst("email")?.Value
            });
        }
    }
}

/*using Microsoft.AspNetCore.Mvc;
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
}*/