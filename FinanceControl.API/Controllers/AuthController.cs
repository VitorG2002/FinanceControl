using FinanceControl.FinanceControl.Application.DTOs.User;
using FinanceControl.FinanceControl.Application.Security;
using FinanceControl.FinanceControl.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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
            var users = await _userService.GetAllAsync();
            var existingUser = users.FirstOrDefault(u => u.Email == loginDto.Email);

            if (existingUser == null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, existingUser.Password))
                return Unauthorized("Email ou senha incorretos.");

            var token = _jwtService.GenerateToken(existingUser.Email, existingUser.Id.ToString());
            var refreshToken = _jwtService.GenerateRefreshToken();

            await _userService.UpdateRefreshTokenAsync(existingUser.Id, refreshToken, DateTime.UtcNow.AddDays(7));

            return Ok(new { Token = token, RefreshToken = refreshToken });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

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

        [Authorize]
        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenDto refreshTokenDto)
        {
            // Obtém o principal (usuário) a partir do token expirado
            var principal = _jwtService.GetPrincipalFromExpiredToken(refreshTokenDto.Token);
            var userId = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
                return BadRequest("Token inválido.");

            // Busca o usuário pelo ID
            var user = await _userService.GetByIdAsync(int.Parse(userId));
            if (user == null || user.RefreshToken != refreshTokenDto.RefreshToken || user.RefreshTokenExpiry <= DateTime.UtcNow)
                return BadRequest("Token de refresh inválido.");

            // Gera um novo token e um novo refresh token
            var newToken = _jwtService.GenerateToken(user.Email, user.Id.ToString());
            var newRefreshToken = _jwtService.GenerateRefreshToken();

            // Atualiza o refresh token do usuário
            await _userService.UpdateRefreshTokenAsync(user.Id, newRefreshToken, DateTime.UtcNow.AddDays(7));

            // Retorna o novo token e o novo refresh token
            return Ok(new { Token = newToken, RefreshToken = newRefreshToken });
        }
    }
}
