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

        public async Task<Category> AddAsync(CategoryCreateDto dto, string userId)
        {
            // Verifica se já existe uma categoria com o mesmo nome para o usuário
            var existingCategory = await _rep.GetAllAsync(c => c.Name == dto.Name && c.UserId == int.Parse(userId));
            if (existingCategory.Any())
                throw new InvalidOperationException("Este nome já está em uso.");

            // Mapeia o DTO para a entidade e define o UserId
            var category = dto.MapTo<CategoryCreateDto, Category>();
            category.UserId = int.Parse(userId);

            return await _rep.AddAsync(category);
        }

        public async Task<CategoryReadDto> GetByIdAsync(int id, string userId)
        {
            // Busca a categoria pelo ID e UserId
            var category = await _rep.GetFirstOrDefaultAsync(c => c.Id == id && c.UserId == int.Parse(userId));

            if (category == null)
                throw new InvalidOperationException("Categoria não encontrada.");

            // Mapeia para DTO
            var dto = category.MapTo<Category, CategoryReadDto>();

            return dto;
        }

        public async Task<List<CategoryReadDto>> GetAllAsync(string userId)
        {
            // Filtra as categorias pelo UserId
            var categories = (await _rep.GetAllAsync(c => c.UserId == int.Parse(userId))).ToList();

            // Mapeia para DTO
            var listDtos = categories.MapTo<List<Category>, List<CategoryReadDto>>();

            return listDtos;
        }

        public async Task<bool> DeleteAsync(int id, string userId)
        {
            // Busca a categoria pelo ID e UserId
            var category = await _rep.GetAllAsync(c => c.Id == id && c.UserId == int.Parse(userId));
            var result = category.FirstOrDefault();

            if (result == null)
                throw new InvalidOperationException("Categoria não encontrada.");

            await _rep.DeleteAsync(result.Id);

            return true;
        }

        public async Task<Category> UpdateAsync(CategoryUpdateDto dto, string userId)
        {
            var category = await _rep.GetFirstOrDefaultAsync(c => c.Id == dto.Id && c.UserId == int.Parse(userId));

            if (category == null)
                throw new InvalidOperationException("Categoria não encontrada.");

            category = dto.MapTo(category);

            await _rep.UpdateAsync(category);

            return category;
        }
    }
}
