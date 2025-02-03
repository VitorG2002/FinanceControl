using FinanceControl.FinanceControl.Application.DTOs.User;
using FinanceControl.FinanceControl.Application.Security;
using FinanceControl.FinanceControl.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace FinanceControl.FinanceControl.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly JwtService _jwtService;

        public AuthController(IUserService userService, JwtService jwtService)
        {
            _userService = userService;
            _jwtService = jwtService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var user = await _userService.GetAllAsync();
            var existingUser = user.FirstOrDefault(u => u.Email == loginDto.Email && u.Password == loginDto.Password);

            if (existingUser == null)
                return Unauthorized("Email ou senha incorretos.");

            var token = _jwtService.GenerateToken(existingUser.Email, existingUser.Id.ToString());

            return Ok(new { Token = token });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            var userDto = new UserCreateDto
            {
                Name = registerDto.Name,
                Email = registerDto.Email,
                Password = registerDto.Password,
            };

            var user = await _userService.AddAsync(userDto);

            var token = _jwtService.GenerateToken(user.Email, user.Id.ToString());

            return Ok(new { Token = token });
        }
    }
}
