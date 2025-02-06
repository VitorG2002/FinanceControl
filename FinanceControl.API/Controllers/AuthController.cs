using FinanceControl.FinanceControl.Application.DTOs.User;
using FinanceControl.FinanceControl.Application.Security;
using FinanceControl.FinanceControl.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FinanceControl.FinanceControl.API.Controllers
{
    /// <summary>
    /// Controlador para autenticação de usuários
    /// </summary>
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

        /// <summary>
        /// Autentica um usuário e retorna tokens JWT
        /// </summary>
        /// <param name="loginDto">Dados de login (email e senha)</param>
        /// <returns>Token JWT e refresh token</returns>
        /// <response code="200">Retorna o token JWT e o refresh token</response>
        /// <response code="401">Se o login falhar devido a credenciais inválidas</response>
        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
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

        /// <summary>
        /// Registra um novo usuário
        /// </summary>
        /// <param name="registerDto">Dados de registro (nome, email e senha)</param>
        /// <returns>Token JWT</returns>
        /// <response code="200">Retorna o token JWT para o novo usuário</response>
        /// <response code="400">Se os dados de registro forem inválidos</response>
        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
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

        /// <summary>
        /// Renova o token JWT usando um refresh token válido
        /// </summary>
        /// <param name="refreshTokenDto">Token JWT expirado e refresh token</param>
        /// <returns>Novo token JWT e refresh token</returns>
        /// <response code="200">Retorna o novo token JWT e refresh token</response>
        /// <response code="400">Se o token ou refresh token forem inválidos</response>
        [Authorize]
        [HttpPost("refresh-token")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenDto refreshTokenDto)
        {
            var principal = _jwtService.GetPrincipalFromExpiredToken(refreshTokenDto.Token);
            var userId = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
                return BadRequest("Token inválido.");

            var user = await _userService.GetByIdAsync(int.Parse(userId));
            if (user == null || user.RefreshToken != refreshTokenDto.RefreshToken || user.RefreshTokenExpiry <= DateTime.UtcNow)
                return BadRequest("Token de refresh inválido.");

            var newToken = _jwtService.GenerateToken(user.Email, user.Id.ToString());
            var newRefreshToken = _jwtService.GenerateRefreshToken();

            await _userService.UpdateRefreshTokenAsync(user.Id, newRefreshToken, DateTime.UtcNow.AddDays(7));

            return Ok(new { Token = newToken, RefreshToken = newRefreshToken });
        }
    }
}
