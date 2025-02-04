using FinanceControl.FinanceControl.Application.DTOs.Category;
using FinanceControl.FinanceControl.Domain.Entities;

namespace FinanceControl.FinanceControl.Domain.Interfaces.Services
{
    public interface ICategoryService
    {
        Task<Category> AddAsync(CategoryCreateDto category, string userId);
        Task<CategoryReadDto> GetByIdAsync(int id, string userId);
        Task<List<CategoryReadDto>> GetAllAsync(string userId);
        Task<Category> UpdateAsync(CategoryUpdateDto dto, string userId);
        Task<bool> DeleteAsync(int id, string userId);
    }
}
