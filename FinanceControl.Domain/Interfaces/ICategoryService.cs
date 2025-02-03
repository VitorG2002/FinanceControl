using FinanceControl.FinanceControl.Application.DTOs.Category;
using FinanceControl.FinanceControl.Domain.Entities;

namespace FinanceControl.FinanceControl.Domain.Interfaces
{
    public interface ICategoryService
    {
        Task<Category> AddAsync(CategoryCreateDto category);
        Task<CategoryReadDto> GetByIdAsync(int id);
        Task<List<CategoryReadDto>> GetAllAsync();
        Task<Category> UpdateAsync(CategoryUpdateDto dto); 
        Task<bool> DeleteAsync(int id);
    }
}
