using FinanceControl.FinanceControl.Application.DTOs.Category;
using FinanceControl.FinanceControl.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FinanceControl.FinanceControl.API.Controllers
{
    /// <summary>
    /// Controlador para gerenciamento de categorias
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _service;

        public CategoryController(ICategoryService service)
        {
            _service = service;
        }

        /// <summary>
        /// Cria uma nova categoria
        /// </summary>
        /// <param name="category">Dados da categoria</param>
        /// <returns>Categoria criada</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Add([FromBody] CategoryCreateDto category)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized("Usuário não autenticado.");

            var categoryCreated = await _service.AddAsync(category, userId);
            return CreatedAtAction(nameof(GetAll), new { id = categoryCreated.Id }, categoryCreated);
        }

        /// <summary>
        /// Lista todas as categorias do usuário
        /// </summary>
        /// <returns>Lista de categorias</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetAll()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized("Usuário não autenticado.");

            var categories = await _service.GetAllAsync(userId);
            return Ok(categories);
        }

        /// <summary>
        /// Obtém uma categoria pelo ID
        /// </summary>
        /// <param name="id">ID da categoria</param>
        /// <returns>Categoria encontrada</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized("Usuário não autenticado.");

            var category = await _service.GetByIdAsync(id, userId);
            return Ok(category);
        }

        /// <summary>
        /// Atualiza uma categoria existente
        /// </summary>
        /// <param name="dto">Dados atualizados da categoria</param>
        /// <returns>Nenhum conteúdo</returns>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update([FromBody] CategoryUpdateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized("Usuário não autenticado.");

            var category = await _service.UpdateAsync(dto, userId);
            return NoContent();
        }

        /// <summary>
        /// Exclui uma categoria pelo ID
        /// </summary>
        /// <param name="id">ID da categoria</param>
        /// <returns>Nenhum conteúdo</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized("Usuário não autenticado.");

            var category = await _service.DeleteAsync(id, userId);
            return NoContent();
        }
    }
}