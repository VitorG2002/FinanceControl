using FinanceControl.FinanceControl.Application.DTOs;
using FinanceControl.FinanceControl.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FinanceControl.FinanceControl.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _service;

        [HttpPost]
        public IActionResult Add([FromBody] CategoryCreateDto category)
        {
            if (category == null)
                return BadRequest("Invalid category data.");

            var categoryCreated = _service.AddAsync(category);

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
