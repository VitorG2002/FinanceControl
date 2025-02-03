using FinanceControl.FinanceControl.Application.DTOs.User;
using FinanceControl.FinanceControl.Application.Security;
using FinanceControl.FinanceControl.Domain.Entities;
using FinanceControl.FinanceControl.Domain.Interfaces.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace FinanceControl.FinanceControl.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IRepository<User> _userRepository;
        private readonly JwtService _jwtService;

        public AuthController(IRepository<User> userRepository, JwtService jwtService)
        {
            _userRepository = userRepository;
            _jwtService = jwtService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var user = await _userRepository.GetAllAsync();
            var existingUser = user.FirstOrDefault(u => u.Email == loginDto.Email && u.Password == loginDto.Password);

            if (existingUser == null)
                return Unauthorized("Email ou senha incorretos.");

            var token = _jwtService.GenerateToken(existingUser.Email, existingUser.Id.ToString());

            return Ok(new { Token = token });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            var user = new User
            {
                Name = registerDto.Name,
                Email = registerDto.Email,
                Password = registerDto.Password,
                CreatedAt = DateTime.UtcNow
            };

            await _userRepository.AddAsync(user);

            var token = _jwtService.GenerateToken(user.Email, user.Id.ToString());

            return Ok(new { Token = token });
        }
    }
}
