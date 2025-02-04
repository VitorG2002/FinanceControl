using FinanceControl.FinanceControl.Application.DTOs.Category;
using FinanceControl.FinanceControl.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace FinanceControl.FinanceControl.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _service;

        public CategoryController(ICategoryService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CategoryCreateDto category)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var categoryCreated = await _service.AddAsync(category);

            return CreatedAtAction(nameof(GetAll), new { id = categoryCreated.Id }, categoryCreated);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var categories = await _service.GetAllAsync();

            return Ok(categories);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var category = await _service.GetByIdAsync(id);

            return Ok(category);
        }

        [HttpPut()]
        public async Task<IActionResult> Update([FromBody] CategoryUpdateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var category = await _service.UpdateAsync(dto);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var category = await _service.DeleteAsync(id);

            return NoContent();
        }
    }
}
