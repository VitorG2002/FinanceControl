using FinanceControl.FinanceControl.Application.DTOs.Category;
using FinanceControl.FinanceControl.Application.Extensions;
using FinanceControl.FinanceControl.Domain.Entities;
using FinanceControl.FinanceControl.Domain.Interfaces.Repositories;
using FinanceControl.FinanceControl.Domain.Interfaces.Services;

namespace FinanceControl.FinanceControl.Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IRepository<Category> _rep;

        public CategoryService(IRepository<Category> rep)
        {
                _rep = rep;
        }

        public async Task<Category> AddAsync(CategoryCreateDto dto)
        {
            Category category = dto.MapTo<CategoryCreateDto, Category>();  

            return await _rep.AddAsync(category); 
        }

        public async Task<CategoryReadDto> GetByIdAsync(int id)
        {
            var category = await _rep.GetByIdAsync(id);

            var dto = category.MapTo<Category, CategoryReadDto>();

            return dto;
        }

        public async Task<List<CategoryReadDto>> GetAllAsync()
        {
            var categories = (await _rep.GetAllAsync()).ToList();

            var listDtos = categories.MapTo<List<Category>, List<CategoryReadDto>>();

            return listDtos;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            await _rep.DeleteAsync(id);

            return true;
        }

        public async Task<Category> UpdateAsync(CategoryUpdateDto dto)
        {
            var category = await _rep.GetByIdAsync(dto.Id);
            category = dto.MapTo(category);

            await _rep.UpdateAsync(category);

            return category;
        }
    }
}
