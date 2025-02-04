using FinanceControl.FinanceControl.Application.DTOs.User;
using FinanceControl.FinanceControl.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinanceControl.FinanceControl.API.Controllers
{

    /// <summary>
    /// Controlador para gerenciamento de usuários (apenas administradores)
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _service;

        public UserController(IUserService service)
        {
            _service = service;
        }

        /// <summary>
        /// Lista todos os usuários (requer privilégios de administrador)
        /// </summary>
        /// <returns>Lista de usuários</returns>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var users = await _service.GetAllAsync();

            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var user = await _service.GetByIdAsync(id);

            return Ok(user);
        }

        [HttpPut()]
        public async Task<IActionResult> Update([FromBody] UserUpdateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _service.UpdateAsync(dto);

            return NoContent();
        }
    }
}
